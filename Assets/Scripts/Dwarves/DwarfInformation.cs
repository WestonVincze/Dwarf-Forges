using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DWARF_TYPE
{
    NONE,
    VOLATILE,
    DURABLE,
    LIGHT,
    STRONG
}

public class DwarfInformation : MonoBehaviour
{
    private DWARF_TYPE _dwarfType = DWARF_TYPE.DURABLE;

    public DWARF_TYPE dwarfType
    {
        get => _dwarfType;
        set
        {
            _dwarfType = value;
        }
    }

    public DWARF_TYPE GetDwarfType()
    {
        return dwarfType;
    }

    public void SetRandomDwarfType()
    {
        dwarfType = (DWARF_TYPE)Random.Range(1, (System.Enum.GetValues(typeof(DWARF_TYPE)).Length) - 1);
    }

    public void SetSpecificDwarfType(DWARF_TYPE type)
    {
        dwarfType = type;
    }
}
