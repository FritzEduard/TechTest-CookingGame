using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class CookingController : MonoBehaviour
{
    public event Action OnStartCooking;
    public event Action OnCookingDone;

    [Header("References")]
    [SerializeField] private SO_Ingredient defaultResult;
    [SerializeField] private List<SO_RecipeDefinition> recipes;
    [SerializeField] private Image progressBar;
    [SerializeField] private List<FoodPosition> foodPositions;

    [Header("Settings")]
    [SerializeField] private int maxIngredients = 5;
    [SerializeField] private float defaultRecipeCookingDuration;

    private List<SO_Ingredient> currentIngredients = new List<SO_Ingredient>();
    private List<Transform> ingredientsTransform = new List<Transform>();
    private SO_RecipeDefinition currentRecipe;
    private bool isValidRecipe = false;
    private bool isCooking = false;

    private void Awake()
    {
        progressBar.transform.parent.gameObject.SetActive(false);
    }
    /// <summary>
    /// Adds new ingredient to the cooking pan
    /// </summary>
    /// <param name="ingredient">The ingredient to add</param>
    private void AddIngredient(Ingredient ingredient)
    {
        Destroy(ingredient.gameObject.GetComponent<XRGrabInteractable>());

        if (currentIngredients.Count == maxIngredients) return;
        if (!ingredientsTransform.Contains(ingredient.transform))
        {
            currentIngredients.Add(ingredient.IngredientSO);
            OnStartCooking += ingredient.Cook;


            ingredientsTransform.Add(ingredient.transform);

            ingredient.transform.SetParent(transform);
            ingredient.transform.localScale = Vector3.one / 2;
            ingredient.transform.position = GetFoodPosition().position;

            if (ingredient.Rigidbody != null)
            {
                ingredient.Rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
        }

        CheckRecipeValidity();
    }
    /// <summary>
    /// Removes ingredient from the cooking pan
    /// </summary>
    /// <param name="ingredient">The ingredient to remove</param>
    private void RemoveIngredient (Ingredient ingredient)
    {
        if (currentIngredients.Contains(ingredient.IngredientSO))
        {
            currentIngredients.Remove(ingredient.IngredientSO);
            OnStartCooking -= ingredient.Cook;
        } 

        if (ingredientsTransform.Contains(ingredient.transform))
        {
            ingredientsTransform.Remove(ingredient.transform);
            ingredient.transform.SetParent(null);
            ingredient.transform.localScale = Vector3.one;

            if (ingredient.Rigidbody != null)
            {
                ingredient.Rigidbody.constraints = RigidbodyConstraints.None;
            }
        }

        CheckRecipeValidity();
    }

    /// <summary>
    /// Checks if a recipe can be produce from the current elements that are in the Cooking Pan
    /// </summary>
    private void CheckRecipeValidity()
    {
        currentIngredients.Sort();
        currentRecipe = null;

        foreach (var recipe in recipes)
        {
            recipe.Ingredients.Sort();

            if (currentIngredients.Count != recipe.Ingredients.Count) continue;

            for (int i = 0; i < currentIngredients.Count; i++)
            {
                if (currentIngredients[i].Name != recipe.Ingredients[i].Name)
                {
                    isValidRecipe = false;
                    break; //This is not the correct recipe . We stop checking for this recipe and go to the next one
                }
                
                Debug.Log("Ingredient Match!");
                isValidRecipe = true;
            }

            if(isValidRecipe)
            {
                Debug.Log("Valid Recipe!");
                currentRecipe = recipe;
                break;
            }
        }
    }
    /// <summary>
    /// Cooks the ingredients
    /// </summary>
    [ContextMenu("Cook now!")]
    public void Cook()
    {
        if (isCooking) return;

        isCooking = true;
        StartCoroutine(_CookingCoroutine());
    }
    /// <summary>
    /// Cooks the ingredients
    /// </summary>
    private IEnumerator _CookingCoroutine ()
    {
        float duration;
        float curentTime = 0.0f;
        SO_Ingredient currentResult;
        if (currentRecipe == null)
        {
            duration = defaultRecipeCookingDuration;
            currentResult = defaultResult;
        }
        else
        {
            duration = currentRecipe.CookingTime;
            currentResult = currentRecipe.Result;
        }

        OnStartCooking.Invoke();

        progressBar.transform.parent.gameObject.SetActive(true);

        while (curentTime < duration)
        {
            curentTime += Time.deltaTime;
            progressBar.fillAmount = curentTime / duration;
            yield return new WaitForEndOfFrame();
        }

        OnCookingDone.Invoke();
    }

    /// <summary>
    /// Clears the cooking pan and adds the result of the cooking when the cooking is done
    /// </summary>
    private void CookingDone()
    {
        isCooking = false;
        //Hide the progress bar
        progressBar.transform.parent.gameObject.SetActive(false);

        //We destroy all the ingredient that were used for the cooking 
        foreach (Transform item in ingredientsTransform) 
        {
            Destroy(item.gameObject);
        }
        //We clear the lists 
        ingredientsTransform.Clear();
        currentIngredients.Clear();
        foreach (FoodPosition item in foodPositions)
        {
            item.IsUsed = false;
        }

        //Instantiates the result of the cooking
        Instantiate(currentRecipe.Result.Prefab , transform.position, Quaternion.identity , null);
    }

    /// <summary>
    /// Interate trough a list of predefined position and returns a position that is not occupied
    /// </summary>
    /// <returns></returns>
    private Transform GetFoodPosition()
    {
        for (int i = 0; i < foodPositions.Count; i++)
        {
            if (!foodPositions[i].IsUsed)
            {
                foodPositions[i].IsUsed = true;
                return foodPositions[i].Position;
            }
        }
        return foodPositions[0].Position;
    }

    //---------------UNITY EVENTS--------------//
    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.gameObject.GetComponentInParent<Ingredient>();
        if (ingredient == null) return;

       AddIngredient(ingredient);
    }
    private void OnTriggerExit(Collider other)
    {
        Ingredient ingredient = other.gameObject.GetComponentInParent<Ingredient>();
        if (ingredient == null) return;        
        
        RemoveIngredient(ingredient);
    }

    private void OnEnable()
    {
        OnCookingDone += CookingDone;
    }

    private void OnDisable()
    {
        OnCookingDone -= CookingDone;
    }
}

[Serializable]
public class FoodPosition
{
    public Transform Position;
    public bool IsUsed;
}