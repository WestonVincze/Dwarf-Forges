using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Terrain;

public class FurnaceManager : MonoBehaviour
{
    public static FurnaceManager Instance { get; private set; }

    [SerializeField] private int maxDwarvesInFurnace = 2;
    [SerializeField] private float smeltingTime = 4;
    private TimerManager _timerManager;

    private List<DwarfInformation.DWARF_TYPE> storedDwarves = new List<DwarfInformation.DWARF_TYPE>();

    private float smeltingEndTime;

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

        _timerManager = GetComponent<TimerManager>();
    }

    void Update()
    {
        if(GameManager.instance.inDebugMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AddDwarf(DwarfInformation.DWARF_TYPE.DURABLE);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AddDwarf(DwarfInformation.DWARF_TYPE.LIGHT);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AddDwarf(DwarfInformation.DWARF_TYPE.STRONG);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AddDwarf(DwarfInformation.DWARF_TYPE.VOLATILE);
            }
        }
    }

    public bool AddDwarf(DwarfInformation.DWARF_TYPE _dwarfType)
    {
        if (storedDwarves.Count < maxDwarvesInFurnace)
        {
            storedDwarves.Add(_dwarfType);
            _timerManager.StartTimer(smeltingTime, FinishSmelting);
            print("Added Dwarf Of Type: " + _dwarfType);
            return true;
        }

        return false;
    }

    void FinishSmelting()
    {
        if(storedDwarves.Count <= 0)
            return;

        if (storedDwarves.Count == 1)
            FindAnyObjectByType<BarInventory>().GetComponent<BarInventory>().AddInventorySlot(BarCraftingManager.Instance.CraftBar(storedDwarves[0]));
        else if (storedDwarves.Count > 1)
        {
            FindAnyObjectByType<BarInventory>().GetComponent<BarInventory>().AddInventorySlot(BarCraftingManager.Instance.CraftBar(storedDwarves[0], storedDwarves[1]));
        }

        storedDwarves.Clear();
    }

    public bool IsFurnaceFull()
    {
        if (storedDwarves.Count >= maxDwarvesInFurnace)
            return true;
        return false;
    }
}
