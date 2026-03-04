# Pet Food Analyzer

An application that analyzes pet food ingredient labels from photos using OCR and AI.

## Project Structure

```
pet-food-app/
├── src/
│   ├── frontend/          # Angular app
│   └── backend/           # .NET 8 Web API
├── infrastructure/        # Azure Bicep templates
└── README.md
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (LTS)
- [Angular CLI](https://angular.dev/)

## Getting Started

### Backend

```bash
cd src/backend/PetFoodAnalyzer.Api
dotnet run
```

The API will start at `http://localhost:5000`.

### Frontend

```bash
cd src/frontend
npm install
npx ng serve
```

The app will be available at `http://localhost:4200`.

## Infrastructure

Deploy Azure resources using Bicep:

```bash
az deployment group create \
  --resource-group <resource-group-name> \
  --template-file infrastructure/main.bicep \
  --parameters environment=dev
```
