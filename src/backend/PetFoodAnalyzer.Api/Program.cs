using Azure.Identity;
using PetFoodAnalyzer.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var keyBuilder = builder.Configuration["KeyVault:Uri"]
    ?? throw new InvalidOperationException("Key Vault URI is not configured.");

builder.Configuration.AddAzureKeyVault(new Uri(keyBuilder), new DefaultAzureCredential());

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services
builder.Services.AddScoped<IOcrService, OcrService>();
builder.Services.AddScoped<IIngredientAnalysisService, IngredientAnalysisService>();

// Configure CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();

        policy.WithOrigins("https://red-water-07921b90f.1.azurestaticapps.net")
	        .AllowAnyHeader()
	        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();
