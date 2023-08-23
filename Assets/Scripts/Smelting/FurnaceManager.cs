using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Terrain;

public class FurnaceManager : MonoBehaviour
{
    public static FurnaceManager Instance { get; private set; }

    [SerializeField] private int maxDwarvesInFurnace;
    [SerializeField] private float smeltingTime;

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
    }

    public bool AddDwarf(DwarfInformation.DWARF_TYPE _dwarfType)
    {
        if (storedDwarves.Count < maxDwarvesInFurnace)
        {
            storedDwarves.Add(_dwarfType);
            smeltingEndTime = Time.time + smeltingTime;
            print("Added Dwarf Of Type: " + _dwarfType);
            return true;
        }

        return false;
    }

    void Update()
    {
        if (storedDwarves.Count > 0)
        {
            if(Time.time >= smeltingEndTime)
                FinishSmelting();
        }

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

    void FinishSmelting()
    {
        if (storedDwarves.Count == 1)
            Instantiate(BarCraftingManager.Instance.CraftBar(storedDwarves[0]),
                new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z),
                Quaternion.identity);
        else if (storedDwarves.Count > 1)
            Instantiate(BarCraftingManager.Instance.CraftBar(storedDwarves[0], storedDwarves[1]),
                new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z),
                Quaternion.identity);

        storedDwarves.Clear();
    }
}
