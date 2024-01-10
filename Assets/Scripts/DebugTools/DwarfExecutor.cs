using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfExecutor : MonoBehaviour
{
    MobSpawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        if (spawner == null)
        {
            spawner = FindObjectOfType<MobSpawner>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            KillAllDwarves();
        }
    }

    void KillAllDwarves()
    {
        Destructible[] dwarves = spawner.GetComponentsInChildren<Destructible>();
        Debug.Log("Kill " + dwarves.Length + " Dwarves");
        foreach (Destructible dwarf in dwarves)
        {
            dwarf.TakeDamage(dwarf.Health);
        }
    }
}
