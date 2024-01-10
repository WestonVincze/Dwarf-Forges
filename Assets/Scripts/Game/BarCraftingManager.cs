using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BarCraftingManager : MonoBehaviour
{

    public static BarCraftingManager Instance { get; private set; }

    [SerializeField] private string[] recipes;
    [SerializeField] private GameObject[] craftedRecipes;
    [SerializeField] private GameObject volatileBar, durableBar, lightBar, strongBar;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject CraftBar(DWARF_TYPE _primaryAttribute, DWARF_TYPE _secondaryAttribute = DWARF_TYPE.NONE)
    {
        string currentRecipeString = _primaryAttribute.ToString().ToLower() + _secondaryAttribute.ToString().ToLower();

        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i] == currentRecipeString)
            {
                return craftedRecipes[i];
            }
        }

        switch (_primaryAttribute)
        {
            case DWARF_TYPE.VOLATILE:
                return volatileBar;
            case DWARF_TYPE.DURABLE:
                return durableBar;
            case DWARF_TYPE.LIGHT:
                return lightBar;
            case DWARF_TYPE.STRONG:
                return strongBar;
            default: return null;
        }
    }
}
