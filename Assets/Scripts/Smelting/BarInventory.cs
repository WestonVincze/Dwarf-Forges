using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = System.Numerics.Vector3;

public class BarInventory : MonoBehaviour
{
    [SerializeField] private Canvas _inventoryCanvas = new Canvas();
    private Dictionary<string, int> _slots = new Dictionary<string, int>();

    [SerializeField] private GameObject[] _testPrefabs = null;
    [SerializeField] private GameObject _inventoryLayerGroupParent = null;
    [SerializeField] private GameObject _slotPrefab;

    [SerializeField] private LayerMask _uiLayerMask = 0;

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    private bool _isOpened;

    public bool isOpened
    {
        get => _isOpened;
        set
        {
            _isOpened = value;
            if (_isOpened) OpenInventory();
            else if (!_isOpened) CloseInventory();
        }
    }

    private GameObject _previousRaycastedUI;

    void Awake()
    {
        if (!_inventoryCanvas && GetComponent<Canvas>())
        {
            _inventoryCanvas = GetComponent<Canvas>();
        }

        // Get the GraphicRaycaster and EventSystem from the World Space Canvas
        raycaster = _inventoryCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = _inventoryCanvas.GetComponent<EventSystem>();

        isOpened = false;
    }

    public void AddInventorySlot(GameObject inventoryObject)
    {
        if (_slots.ContainsKey(inventoryObject.name))
        {
            int currentBarAmount = _slots[inventoryObject.name];
            _slots[inventoryObject.name] = ++currentBarAmount;
            print(_slots[inventoryObject.name]);
        }
        else
        {
            GameObject tempSlot = Instantiate(_slotPrefab, _inventoryLayerGroupParent.transform);
            tempSlot.GetComponent<InventorySlot>().SetUpSlot(inventoryObject);
            _slots[inventoryObject.name] = 1;
            print(_slots[inventoryObject.name]);
        }
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform.position);

        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpened = !isOpened;
        }

        CheckForRaycastOnSlot();

        if (Input.GetKeyDown(KeyCode.P))
        {
            int randomIndex = Random.Range(0, _testPrefabs.Length);
            AddInventorySlot(_testPrefabs[randomIndex]);
        }
    }

    void OpenInventory()
    {
        if(!_inventoryCanvas.enabled)
            _inventoryCanvas.enabled = true;
    }
    void CloseInventory()
    {
        if (_inventoryCanvas.enabled)
            _inventoryCanvas.enabled = false;
    }

    void CheckForRaycastOnSlot()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position =
            Input.mousePosition; // use the position from controller as start of raycast instead of mousePosition.

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            //Highlighting And Unhighlighting Slots
            if (results[0].gameObject != _previousRaycastedUI && _previousRaycastedUI)
            {
                if (_previousRaycastedUI.GetComponent<InventorySlot>())
                {
                    _previousRaycastedUI.GetComponent<InventorySlot>().UnHighlightSlot();
                }
            }

            if (results[0].gameObject.GetComponent<InventorySlot>() &&
                results[0].gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                results[0].gameObject.GetComponent<InventorySlot>().HighlightSlot();
                _previousRaycastedUI = results[0].gameObject;
            }

            //--------------------------------------------------------------------------

            if (Input.GetMouseButtonDown(0) && results[0].gameObject.TryGetComponent<InventorySlot>(out InventorySlot inventorySlot))
            {
                SpawnSlotPrefab(inventorySlot, results[0].worldPosition);
            }

            results.Clear();
        }
    }

    void SpawnSlotPrefab(InventorySlot slot, UnityEngine.Vector3 spawnLocation)
    {
        GameObject spawnedBar = Instantiate(slot.GetBar(), spawnLocation, Quaternion.identity);
        FindObjectOfType<DragAndDrop>().GetComponent<DragAndDrop>().PickUpItem(spawnedBar);

        int currentBarAmount = _slots[slot.GetBar().name];

        _slots[slot.GetBar().name] = --currentBarAmount;

        if (_slots[slot.GetBar().name] <= 0)
        {
            _slots.Remove(slot.GetBar().name);
            Destroy(slot.gameObject);
        }
    }
}