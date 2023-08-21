using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceManager : MonoBehaviour
{
    private enum MATERIAL_TYPE
    {
        VOLATILE,
        DURABLE,
        LIGHT,
        STRONG
    }

    [SerializeField] private int maxDwarvesInFurnace;
    [SerializeField] private float smeltingTime;
    [SerializeField] private GameObject materialPrefab;

    private List<GameObject> storedDwarves = new List<GameObject>();
    private Dictionary<MATERIAL_TYPE, List<float>> finishTimes = new Dictionary<MATERIAL_TYPE, List<float>>();
    private Queue<MATERIAL_TYPE> materialQueue = new Queue<MATERIAL_TYPE>();

    private float smeltingEndTime;

    void AddDwarf(GameObject _dwarfGameObject)
    {
        print("Added Dwarf");

        if (storedDwarves.Count < maxDwarvesInFurnace)
            storedDwarves.Add(_dwarfGameObject);

        if (storedDwarves.Count >= maxDwarvesInFurnace)
        {
            StartSmeltingMaterial(MATERIAL_TYPE.DURABLE, smeltingTime);
            storedDwarves.Clear();
        }
    }

    void StartSmeltingMaterial(MATERIAL_TYPE _type, float _smeltingTime)
    {
        print("Starting To Smelt");
        smeltingEndTime = Time.time + _smeltingTime;

        EnqueueEnumWithFinishTime(_type, smeltingEndTime);
    }

    public void UpdateQueue()
    {
        while (materialQueue.Count > 0)
        {
            MATERIAL_TYPE frontEnum = materialQueue.Peek();
            if (finishTimes.ContainsKey(frontEnum) && finishTimes[frontEnum].Count > 0 && Time.time >= finishTimes[frontEnum][0])
            {
                finishTimes[frontEnum].RemoveAt(0);
                if (finishTimes[frontEnum].Count == 0)
                {
                    finishTimes.Remove(frontEnum);
                }
                materialQueue.Dequeue();
                Debug.Log("Enum with finish time completed: " + frontEnum);
                Instantiate(materialPrefab,
                    new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z),
                    Quaternion.identity);
            }
            else
            {
                break; // Exit loop if no more items to process
            }
        }
    }

    void Update()
    {
        if (materialQueue.Count > 0)
        {
            UpdateQueue();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AddDwarf(new GameObject("DwarfTest"));
        }
    }

    // Enqueue an enum value with a specific finish time
    void EnqueueEnumWithFinishTime(MATERIAL_TYPE enumType, float finishTime)
    {
        materialQueue.Enqueue(enumType);

        if (!finishTimes.ContainsKey(enumType))
        {
            finishTimes[enumType] = new List<float>();
        }

        finishTimes[enumType].Add(finishTime);
    }
}
