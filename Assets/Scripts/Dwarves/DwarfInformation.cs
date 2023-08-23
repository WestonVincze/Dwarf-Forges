using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class DwarfInformation : MonoBehaviour
{
    public enum DWARF_TYPE
    {
        NONE,
        VOLATILE,
        DURABLE,
        LIGHT,
        STRONG
    }

    private DWARF_TYPE dwarfType;

    void Awake()
    {
        dwarfType = (DWARF_TYPE)Random.Range(1, (System.Enum.GetValues(typeof(DWARF_TYPE)).Length) - 1);
    }

    public DWARF_TYPE GetDwarfType()
    {
        return dwarfType;
    }
}
