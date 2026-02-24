using PetFoodAnalyzer.Api.Models;

namespace PetFoodAnalyzer.Api.Services;

public class IngredientAnalysisService : IIngredientAnalysisService
{
    public async Task<AnalysisResult> AnalyzeIngredientsAsync(string ingredientText)
    {
        // TODO: Implement ingredient analysis logic (e.g., Azure OpenAI)
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
