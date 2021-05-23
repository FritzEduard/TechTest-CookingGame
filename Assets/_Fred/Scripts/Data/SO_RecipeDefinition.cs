using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a recipe for for a meal or another ingredient
/// </summary>
[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 2)]
public class SO_RecipeDefinition : ScriptableObject
{
    public SO_Ingredient Result;
    public List<SO_Ingredient> Ingredients;
    public float CookingTime;
}
