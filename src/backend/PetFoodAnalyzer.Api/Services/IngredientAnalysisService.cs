using System.ClientModel;
using System.Text.Json;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using PetFoodAnalyzer.Api.Models;
using PetFoodAnalyzer.Api.Prompts;

namespace PetFoodAnalyzer.Api.Services;

public class IngredientAnalysisService : IIngredientAnalysisService
{
	private readonly ChatClient _chatClient;

	public IngredientAnalysisService(IConfiguration configuration)
	{
		var endpoint = configuration["AzureOpenAI:Endpoint"]
			?? throw new InvalidOperationException("Azure OpenAI endpoint is not configured.");
		var apiKey = configuration["AzureOpenAI:Key"]
			?? throw new InvalidOperationException("Azure OpenAI API key is not configured.");
		var deploymentName = configuration["AzureOpenAI:DeploymentName"]
			?? throw new InvalidOperationException("Azure OpenAI deployment name is not configured.");

		var azureClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
		_chatClient = azureClient.GetChatClient(deploymentName);
	}

	public async Task<AnalysisResult> AnalyzeIngredientsAsync(string ingredientText, string[] allergens, CancellationToken cancellationToken = default)
	{
		var userMessage = $"""
			Raw label text:
			{ingredientText}

			Allergens to check for:
			{string.Join(", ", allergens)}
			""";
		
		ClientResult<ChatCompletion> response = null;

		try
		{
			response = await _chatClient.CompleteChatAsync([
				new SystemChatMessage(IngredientAnalysisPrompts.IngredientAnalyst),
				new UserChatMessage(userMessage)], cancellationToken: cancellationToken);
		}
		catch (RequestFailedException ex)
		{
			throw new InvalidOperationException(
				$"Azure OpenAI request failed. Status: {ex.Status}, Reason: {ex.ErrorCode}", ex);
		}

		var json = response?.Value.Content[0].Text;

		var result = JsonSerializer.Deserialize<AnalysisResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
			?? throw new InvalidOperationException("Failed to parse analysis result from OpenAI response.");

		return result;
	}
}
