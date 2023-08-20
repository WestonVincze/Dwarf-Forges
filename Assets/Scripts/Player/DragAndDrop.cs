using System.Collections.Generic;
using UnityEngine;

/*
 * Instructions On How Drag And Drop Works
 *
 * How To Set An Object To Be Draggable:
 * Step 1: Set the parent object's tag to either have the 'SelectableCraftingParent' or 'SelectablePlayAreaParent'
 * Step 2: Set the layers of the parent object and all child object's to draggable
 *
 * Controls:
 * Move cursor over object that you want to pick up and drag
 * When the item is highlighted you can click and hold down the left mouse button to pick it up and drag it around
 * To drop the grabbed item, release the left mouse button
 *
 * Grab Types:
 * If you want to pick up an object with the SelectableCraftingParent, the grab type has to be set to Crafting Mode
 * If you want to pick up an object with the SelectablePlayAreaParent, the grab type has to be set to Play Area Mode
 */

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
    [SerializeField] private LayerMask draggableLayerMask;
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
    [SerializeField] private bool usePIDController;

    private GameObject selectedGameObject;
    private Dictionary<Transform, Material> originalMaterials = new Dictionary<Transform, Material>();
    private Dictionary<Transform, int> originalLayers = new Dictionary<Transform, int>();
    private List<Transform> childObjects = new List<Transform>();

    private PIDController pidController;

    void Start()
    {
        GameManager.instance.AddEnterAction(GameManager.GameMode.Crafting, SetToCraftingMode);
        GameManager.instance.AddExitAction(GameManager.GameMode.Crafting, SetToPlayAreaMode);

        pidController = new PIDController();
    }

    private void Update()
    {
        if (grabType != GRAB_TYPE.NONE)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100.0f);

            //If the player hasn't highlighted or grabbed an object, check to see if they are over an object that can be selected
            if (grabState == GRAB_STATE.EMPTY_HANDED)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, draggableLayerMask)) //Raycast to find objects that can be dragged
                {
                    if (grabType == GRAB_TYPE.PLAY_AREA_GRAB) //Checks if the object is apart of the play area and if the player can grab from the play area
                    {
                        selectedGameObject = FindParentObject(hit.transform, playAreaTag)?.gameObject;

                        if (selectedGameObject)
                        {
                            HighlightObject(selectedGameObject.transform); //If the object is valid, highlight it
                        }
                    }
                    else if (grabType == GRAB_TYPE.CRAFTING_GRAB) //Checks if the object is apart of crafting and if the player is in crafting mode
                    {
                        selectedGameObject = FindParentObject(hit.transform, craftingTag)?.gameObject;

                        if (selectedGameObject)
                        {
                            HighlightObject(selectedGameObject.transform); //If the object is valid, highlight it
                        }
                    }
                }
            }
            else if (grabState == GRAB_STATE.HIGHLIGHTED) //If there is an object that is already highlighted, continue logic
            {
                if (Input.GetMouseButtonDown(0)) //TODO: CHANGE INPUT TO USE NEW INPUT SYSTEM
                {
                    PickUpItem();
                }

                if (Physics.Raycast(ray, out hit)) //Raycast to see if the player's cursor is still on the highlighted object or not
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
                        UnHighlightObject(selectedGameObject.transform); //If the highlighted object and the object that the cursor is on is not the same, unhighlight the highlighted object
                    }
                }
            }
            else if (grabState == GRAB_STATE.GRABBED) //If the player has an object picked up continue logic
            {
                if(!selectedGameObject.GetComponent<Rigidbody>() || !usePIDController)
                    MoveToCursor(); //Moves the object to the cursor position with a set height offset from wherever the raycast sits

                if (selectedGameObject.GetComponent<MobBrain>())
                {
                    CheckForForge();
                }

                if (Input.GetMouseButtonUp(0)) //TODO: CHANGE INPUT TO USE NEW INPUT SYSTEM
                {
                    DropItem(); //If they let go of the mouse button, drop the object
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(selectedGameObject)
            if(selectedGameObject.GetComponent<Rigidbody>() && usePIDController && grabState == GRAB_STATE.GRABBED)
                MoveToCursor();
    }

    void CheckForForge()
    {
        if (selectedGameObject)
        {
            Ray ray = new Ray(selectedGameObject.transform.position, Vector3.down);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100.0f);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoredLayerMask))
            {

                GameObject forge = FindParentObject(hit.transform, "Furnace")?.gameObject;
                if (forge)
                {
                    //TODO: Add Dropping Into Forge
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
            if(!usePIDController && selectedGameObject.GetComponent<Rigidbody>())
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
            if(!usePIDController && selectedGameObject.GetComponent<Rigidbody>()) 
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

            if (usePIDController && selectedGameObject.GetComponent<Rigidbody>())
            {
                pidController.targetPos = newPosition;

                print(pidController.targetPos);

                selectedGameObject.GetComponent<Rigidbody>().AddForce(pidController.Update(selectedGameObject.transform.position,
                    selectedGameObject.GetComponent<Rigidbody>().velocity), ForceMode.Acceleration);
            }
            else
            {
                selectedGameObject.transform.position = Vector3.Lerp(selectedGameObject.transform.position, newPosition, lerpAmount * Time.deltaTime);
            }
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
        string tagToIgnore;

        if (_tag == craftingTag)
            tagToIgnore = playAreaTag;
        else
            tagToIgnore = craftingTag;


        if (_childObject.tag == _tag)
        {
            return _childObject;
        }
        else if (_childObject.tag == tagToIgnore)
        {
            return null;
        }

        Transform parent = _childObject.parent;

        while (parent != null)
        {

            if (parent.CompareTag(_tag))
            {
                return parent;
            }
            else if (parent.CompareTag(tagToIgnore))
            {

                return null;
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

    void SetToCraftingMode()
    {
        if (grabState == GRAB_STATE.GRABBED) DropItem();

        grabType = GRAB_TYPE.CRAFTING_GRAB;
    }

    void SetToPlayAreaMode()
    {
        if (grabState == GRAB_STATE.GRABBED) DropItem();

        grabType = GRAB_TYPE.PLAY_AREA_GRAB;
    }
}
