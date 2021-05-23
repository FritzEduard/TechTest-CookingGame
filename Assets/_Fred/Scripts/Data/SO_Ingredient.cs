using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines an ingredient for cooking a recipe
/// </summary>
[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
public class SO_Ingredient : ScriptableObject , IComparable<SO_Ingredient>
{
    public string Name;
    public GameObject Prefab;
    public bool IsCuttable;
    public GameObject CutPrefab;

    public int CompareTo(SO_Ingredient other)
    {
        if (other == null)
            return 1;
        else
            return this.Name.CompareTo(other.Name);
    }
}