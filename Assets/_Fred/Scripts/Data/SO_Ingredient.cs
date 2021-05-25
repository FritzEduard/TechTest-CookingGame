using System;
using UnityEngine;

/// <summary>
/// Defines an ingredient for cooking a recipe
/// </summary>
[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
public class SO_Ingredient : ScriptableObject , IComparable<SO_Ingredient>
{
    public string Name; 
    public GameObject Prefab; //Prefab representation of the ingredient
    public bool IsCuttable; 
    public GameObject CutPrefab;

    /// <summary>
    /// Allows to sort Objects of this type in a list by name
    /// </summary>
    /// <param name="other">The other IngredientSO we want to compare to</param>
    /// <returns></returns>
    public int CompareTo(SO_Ingredient other)
    {
        if (other == null)
            return 1;
        else
            return this.Name.CompareTo(other.Name);
    }
}