using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private enum GRAB_TYPE
    {
        NONE,
        PLAY_AREA_GRAB,
        CRAFTING_GRAB
    }

    private enum GRAB_STATE
    {
        EMPTY_HANDED,
        HIGHLIGHTED,
        GRABBED
    }

    [Header("Enums")]
    [SerializeField] private GRAB_TYPE grabType;
    [SerializeField] private GRAB_STATE grabState;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask playAreaLayerMask;
    [SerializeField] private LayerMask craftingLayerMask;
    [SerializeField] private LayerMask ignoredLayerMask;

    private int originalLayer;

    [SerializeField] private string playAreaTag;
    [SerializeField] private string craftingTag;

    private int previousLayer;

    [Header("Materials")]
    [SerializeField] private Material highlightedMaterial;
    [SerializeField] private Material grabbedMaterial;

    [Header("Movement")]
    [SerializeField] private float heightOffset;
    [SerializeField] private float lerpAmount;

    private GameObject selectedGameObject;
    private Dictionary<Transform, Material> originalMaterials = new Dictionary<Transform, Material>();
    private Dictionary<Transform, int> originalLayers = new Dictionary<Transform, int>();
    private List<Transform> childObjects = new List<Transform>();

    private void Update()
    {
        if (grabType != GRAB_TYPE.NONE)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100.0f);

            if (grabState == GRAB_STATE.EMPTY_HANDED)
            {
                if (grabType == GRAB_TYPE.PLAY_AREA_GRAB)
                {
                    if (Physics.Raycast(ray, out hit, 100.0f, playAreaLayerMask))
                    {
                        selectedGameObject = FindParentObject(hit.transform, playAreaTag).gameObject;
                        if (selectedGameObject != null)
                            HighlightObject(selectedGameObject.transform);
                    }
                }
                else if (grabType == GRAB_TYPE.CRAFTING_GRAB)
                {
                    if (Physics.Raycast(ray, out hit, 100.0f, craftingLayerMask))
                    {
                        selectedGameObject = FindParentObject(hit.transform, craftingTag).gameObject;
                        if (selectedGameObject != null)
                            HighlightObject(selectedGameObject.transform);
                    }
                }
            }
            else if (grabState == GRAB_STATE.HIGHLIGHTED)
            {
                if (Input.GetMouseButtonDown(0)) //TODO: CHANGE INPUT TO USE NEW INPUT SYSTEM
                {
                    PickUpItem();
                }

                if (Physics.Raycast(ray, out hit))
                {
                    bool _sameObject = false;

                    if (hit.transform.gameObject == selectedGameObject)
                    {
                        _sameObject = true;
                    }
                    else
                    {
                        foreach (Transform child in childObjects)
                        {
                            if (hit.transform == child)
                            {
                                _sameObject = true;
                                return;
                            }
                        }
                    }

                    if (!_sameObject && selectedGameObject)
                    {
                        UnHighlightObject(selectedGameObject.transform);
                    }
                }
            }
            else if (grabState == GRAB_STATE.GRABBED)
            {
                MoveToCursor();

                if (Input.GetMouseButtonUp(0)) //TODO: CHANGE INPUT TO USE NEW INPUT SYSTEM
                {
                    DropItem();
                }
            }
        }
    }

    private void HighlightObject(Transform _parentObject)
    {
        ChangeAllMaterialsOfParentObject(_parentObject, highlightedMaterial, true);
        grabState = GRAB_STATE.HIGHLIGHTED;
    }

    private void UnHighlightObject(Transform _parentObject)
    {
        RestoreOriginalMaterials();
        originalMaterials.Clear();
        selectedGameObject = null;
        grabState = GRAB_STATE.EMPTY_HANDED;
    }

    private void PickUpItem()
    {
        ChangeAllMaterialsOfParentObject(selectedGameObject.transform, grabbedMaterial, false);
        ChangeAllLayersOfParentObject(selectedGameObject.transform);
        if (selectedGameObject.GetComponent<Rigidbody>())
        {
            selectedGameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        grabState = GRAB_STATE.GRABBED;
    }

    private void DropItem()
    {
        RestoreOriginalMaterials();
        RestoreOriginalLayers();
        if (selectedGameObject.GetComponent<Rigidbody>())
        {
            selectedGameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        grabState = GRAB_STATE.EMPTY_HANDED;
    }

    private void MoveToCursor() //TODO: CHANGE THE MOVEMENT TO USE RIGIDBODY WITH PID CONTROLLER
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f, ~ignoredLayerMask))
        {
            Vector3 newPosition = new Vector3(hit.point.x, hit.point.y + heightOffset, hit.point.z);
            selectedGameObject.transform.position = Vector3.Lerp(selectedGameObject.transform.position, newPosition, lerpAmount * Time.deltaTime);
        }
    }

    private void ChangeAllMaterialsOfParentObject(Transform _parentObject, Material _material, bool _storeCurrentMaterial)
    {
        childObjects = GetAllChildObjects(_parentObject);

        if (_parentObject.GetComponent<MeshRenderer>())
        {
            if (_storeCurrentMaterial)
                originalMaterials[_parentObject] = _parentObject.GetComponent<MeshRenderer>().material;
            _parentObject.GetComponent<MeshRenderer>().material = _material;
        }

        foreach (Transform child in childObjects)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                if (_storeCurrentMaterial)
                    originalMaterials[child] = meshRenderer.material;
                meshRenderer.material = _material;
            }
        }
    }

    private void RestoreOriginalMaterials()
    {
        foreach (var kvp in originalMaterials)
        {
            MeshRenderer meshRenderer = kvp.Key.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = kvp.Value;
            }
        }
    }

    private void ChangeAllLayersOfParentObject(Transform _parentObject)
    {
        childObjects = GetAllChildObjects(_parentObject);
        int newLayer = (int)Mathf.Log(ignoredLayerMask.value, 2);

        if (_parentObject.GetComponent<Collider>())
        {
            originalLayers[_parentObject] = _parentObject.gameObject.layer;
            _parentObject.gameObject.layer = newLayer;
        }

        foreach (Transform child in childObjects)
        {
            Collider collider = child.GetComponent<Collider>();
            if (collider)
            {
                originalLayers[child] = child.gameObject.layer;
                child.gameObject.layer = newLayer;
            }
        }
    }

    private void RestoreOriginalLayers()
    {
        foreach (var kvp in originalLayers)
        {
            Collider collider = kvp.Key.GetComponent<Collider>();
            if (collider != null)
            {
                kvp.Key.gameObject.layer = kvp.Value;
            }
        }
    }

    private Transform FindParentObject(Transform _childObject, string _tag)
    {
        if (_childObject.tag == _tag)
            return _childObject;

        Transform parent = _childObject.parent;

        while (parent != null)
        {
            if (parent.CompareTag(_tag))
            {
                return parent;
            }
            parent = parent.parent;
        }

        return null;
    }

    private List<Transform> GetAllChildObjects(Transform parent)
    {
        List<Transform> childList = new List<Transform>();
        foreach (Transform child in parent)
        {
            childList.Add(child);
            childList.AddRange(GetAllChildObjects(child));
        }
        return childList;
    }
}
