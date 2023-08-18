using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;

public class DragAndDrop : MonoBehaviour
{
    //Enums
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

    //LayerMasks
    [Header("LayerMasks")]
    [SerializeField] private LayerMask playAreaLayerMask;
    [SerializeField] private LayerMask craftingLayerMask;
    [SerializeField] private LayerMask ignoredLayerMask;

    //Tags
    [SerializeField] private string playAreaTag;
    [SerializeField] private string craftingTag;

    private int previousLayer; //Stores the layer of the gameobject that is grabbed so that it can be changed back when object is dropped

    //Materials
    [Header("Materials")]
    [SerializeField] private Material highlightedMaterial;
    [SerializeField] private Material grabbedMaterial;

    private GameObject selectedGameObject;

    private Dictionary<Transform, Material> originalMaterials = new Dictionary<Transform, Material>();
    List<Transform> childObjects = new List<Transform>();

    void Update()
    {
        if (grabType != GRAB_TYPE.NONE)
        {
            //Create Raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100.0f);

            //Check Raycast

            //If The Player Is Empty Handed Check For Raycast To Highlight Objects
            if (grabState == GRAB_STATE.EMPTY_HANDED)
            {
                if (grabType == GRAB_TYPE.PLAY_AREA_GRAB) //Check Raycast With Play Area LayerMask
                {
                    if (Physics.Raycast(ray, out hit, 100.0f, playAreaLayerMask))
                    {
                        //Checks For Play Area Selectable Gameobjects
                        selectedGameObject = FindParentObject(hit.transform, playAreaTag).gameObject;
                        if(selectedGameObject != null)
                            HighlightObject(selectedGameObject.transform);
                    }
                }
                else if (grabType == GRAB_TYPE.CRAFTING_GRAB)
                {
                    if (Physics.Raycast(ray, out hit, 100.0f, craftingLayerMask))
                    {
                        //Checks For Play Area Selectable Gameobjects
                        selectedGameObject = FindParentObject(hit.transform, craftingTag).gameObject;
                        if (selectedGameObject != null)
                            HighlightObject(selectedGameObject.transform);
                    }
                }
            }
            else if (grabState == GRAB_STATE.HIGHLIGHTED)
            {
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

                    if (!_sameObject)
                    {
                        if(selectedGameObject)
                            UnHighlightObject(selectedGameObject.transform);
                    }
                }
            }
        }
    }

    void HighlightObject(Transform _parentObject)
    {
        //Find All Child Objects From The Parent Object
        childObjects = GetAllChildObjects(_parentObject);

        if (_parentObject.GetComponent<MeshRenderer>())
        {
            originalMaterials[_parentObject] = _parentObject.GetComponent<MeshRenderer>().material;
            _parentObject.GetComponent<MeshRenderer>().material = highlightedMaterial;
        }

        //Check Each Child For MeshRenderers
        foreach (Transform child in childObjects)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();

            if (meshRenderer)
            {
                //Store The Childs Original Material
                originalMaterials[child] = meshRenderer.material;

                //Change Childs Material To Highlight
                meshRenderer.material = highlightedMaterial;
            }
        }

        //Change Grab State
        grabState = GRAB_STATE.HIGHLIGHTED;
    }

    void UnHighlightObject(Transform _parentObject)
    {
        foreach (var kvp in originalMaterials)
        {
            MeshRenderer meshRenderer = kvp.Key.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = kvp.Value;
            }
        }

        originalMaterials.Clear();

        selectedGameObject = null;
        grabState = GRAB_STATE.EMPTY_HANDED;
    }

    //Functions To Select Every Object Connected To Selected Object (Child Objects) To Be Able To Change Materials
    Transform FindParentObject(Transform _childObject, string _tag)
    {
        //If the child object already has the parent tag, use child object
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

        // Iterate through each child of the current parent
        foreach (Transform child in parent)
        {
            childList.Add(child); // Add the child to the list
            childList.AddRange(GetAllChildObjects(child)); // Recursively get children of the child
        }

        return childList;
    }
}
