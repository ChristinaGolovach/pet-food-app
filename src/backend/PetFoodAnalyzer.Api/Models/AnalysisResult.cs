namespace PetFoodAnalyzer.Api.Models;

public class AnalysisResult
{
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<string> Allergens { get; set; } = new();
    public int IngredientCount => Ingredients.Count;
    public int AllergenCount => Allergens.Count;
    public double AllergenPercentage => IngredientCount == 0
        ? 0
        : Math.Round((double)AllergenCount / IngredientCount * 100, 2);
}
