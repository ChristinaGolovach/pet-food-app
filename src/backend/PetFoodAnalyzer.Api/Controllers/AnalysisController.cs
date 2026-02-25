using Microsoft.AspNetCore.Mvc;
using PetFoodAnalyzer.Api.Models;
using PetFoodAnalyzer.Api.Services;

namespace PetFoodAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly IOcrService _ocrService;
    private readonly IIngredientAnalysisService _analysisService;

    public AnalysisController(IOcrService ocrService, IIngredientAnalysisService analysisService)
    {
        _ocrService = ocrService;
        _analysisService = analysisService;
    }

    [HttpPost]
    public async Task<ActionResult<AnalysisResult>> Analyze([FromForm] AnalysisRequest request)
    {
        if (request.Photo is null || request.Photo.Length == 0)
        {
            return BadRequest("A photo is required.");
        }

        if (request.Allergens is null || request.Allergens.Length == 0)
        {
			return BadRequest("User preferences are required.");
        }

        using var stream = request.Photo.OpenReadStream();
        var extractedText = await _ocrService.ExtractTextAsync(stream);
        var result = await _analysisService.AnalyzeIngredientsAsync(extractedText);

        return Ok(result);
    }
}
