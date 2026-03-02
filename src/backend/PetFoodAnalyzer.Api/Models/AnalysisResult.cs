namespace PetFoodAnalyzer.Api.Models;

public class AnalysisResult
{
    public List<string> Ingredients { get; set; } = new();
    public int IngredientCount { get; set; }
	public List<string> Allergens { get; set; } = new();
    public int AllergenCount { get; set; }
    public double AllergenPercentage { get; set; }
}
