namespace PetFoodAnalyzer.Api.Models;

public class AnalysisRequest
{
    public IFormFile Photo { get; set; }
    public string Allergens { get; set; }
}
