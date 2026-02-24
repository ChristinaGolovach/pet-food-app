namespace PetFoodAnalyzer.Api.Models;

public class AnalysisResult
{
    public bool Success { get; set; }
    public string RawText { get; set; }
    public List<string> Ingredients { get; set; } = new();
    public string OverallRating { get; set; }
    public List<string> Warnings { get; set; } = new();
    public string Summary { get; set; }
}
