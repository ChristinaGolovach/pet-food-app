using PetFoodAnalyzer.Api.Models;

namespace PetFoodAnalyzer.Api.Services;

public interface IIngredientAnalysisService
{
    Task<AnalysisResult> AnalyzeIngredientsAsync(string ingredientText);
}
