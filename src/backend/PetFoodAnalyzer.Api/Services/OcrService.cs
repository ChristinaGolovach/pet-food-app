using Azure;
using Azure.AI.Vision.ImageAnalysis;
using Azure.Identity;

namespace PetFoodAnalyzer.Api.Services;

public class OcrService : IOcrService
{
	private readonly ImageAnalysisClient _analysisClient;

	public OcrService(IConfiguration configuration)
	{
		var endpoint = configuration["AzureVision:Endpoint"]
			?? throw new InvalidOperationException("Azure Vision endpoint is not configured.");
		var apiKey = configuration["AzureVision:Key"]
			?? throw new InvalidOperationException("Azure Vision API key is not configured.");

		_analysisClient = new ImageAnalysisClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
	}

    public async Task<string> ExtractTextAsync(Stream imageStream, CancellationToken cancellationToken = default)
    {
	    var imageData = await BinaryData.FromStreamAsync(imageStream, cancellationToken);
	    var result = await _analysisClient.AnalyzeAsync(imageData, VisualFeatures.Read, default, cancellationToken);

	    if (result.Value.Read is null)
	    {
		    return String.Empty;
	    }

		var lines = result.Value.Read.Blocks
			.SelectMany(block => block.Lines)
			.Select(line => line.Text);

		return String.Join(Environment.NewLine, lines);
    }
}
