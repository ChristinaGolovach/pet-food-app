namespace PetFoodAnalyzer.Api.Services;

public class OcrService : IOcrService
{
    public async Task<string> ExtractTextAsync(Stream imageStream)
    {
        // TODO: Implement OCR logic (e.g., Azure AI Vision)
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}
