using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private bool _isHighlighted;
    private GameObject _barPrefab;
    private RawImage _slotImage;

    void Awake()
    {
        _slotImage = GetComponent<RawImage>();
    }

    public void SetUpSlot(GameObject barPrefab)
    {
        _barPrefab = barPrefab;
        ChangeSlotColor(barPrefab.GetComponent<MeshRenderer>().sharedMaterial.color);
    }

    void ChangeSlotColor(Color newColor)
    {
        _slotImage.color = new Color(newColor.r, newColor.g, newColor.b, 0.25f);
    }

    public void HighlightSlot()
    {
        print("HIGHLIGHTED " + name);
        _isHighlighted = true;
        _slotImage.color = new Color(_slotImage.color.r, _slotImage.color.g, _slotImage.color.b, 1.0f);
    }

    public void UnHighlightSlot()
    {
        _isHighlighted = false;
        _slotImage.color = new Color(_slotImage.color.r, _slotImage.color.g, _slotImage.color.b, 0.25f);
    }

    public RawImage GetSlotImage() { return _slotImage; }
    public bool GetIsHighlighted() { return _isHighlighted; }
}
