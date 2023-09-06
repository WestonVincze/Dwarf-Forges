using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarInventory : MonoBehaviour
{
    [SerializeField] private Canvas _inventoryCanvas;
    private List<GameObject> _slots = new List<GameObject>();

    [SerializeField] private GameObject _testPrefab;
    [SerializeField] private GameObject _inventoryLayerGroupParent;
    [SerializeField] private GameObject _slotPrefab;

    [SerializeField] private LayerMask _uiLayerMask;

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    private bool _isOpened;
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
    }

    void AddInventorySlot(GameObject inventoryObject)
    {
        GameObject tempSlot = Instantiate(_slotPrefab, _inventoryLayerGroupParent.transform);
        tempSlot.GetComponent<InventorySlot>().SetUpSlot(inventoryObject);
        _slots.Add(tempSlot);
    }

    void Update()
    {
        CheckForRaycastOnSlot();

        if (Input.GetKeyDown(KeyCode.P))
        {
            AddInventorySlot(_testPrefab);
        }
    }

    void CheckForRaycastOnSlot()
    {
        print("Raycast");

        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position =
            Input.mousePosition; // use the position from controller as start of raycast instead of mousePosition.

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
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

            results.Clear();
        }
    }
}