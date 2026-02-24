namespace PetFoodAnalyzer.Api.Services;

public interface IOcrService
{
    Task<string> ExtractTextAsync(Stream imageStream);
}
