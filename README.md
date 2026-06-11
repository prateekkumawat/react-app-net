# React + .NET Azure Sample

This workspace contains a simple React frontend and a .NET 9 Web API.

## Structure
- frontend/  - Vite React client
- backend/Api - ASP.NET Core API

## Run locally
1. Start the backend:
   dotnet run --project backend/Api/Api.csproj
2. Start the frontend:
   cd frontend
   npm install
   npm run dev

The frontend proxies /api requests to http://localhost:5000.

## Azure App Service
- Deploy the backend as a Web App.
- Deploy the frontend to a separate Web App or Static Web App.
- Set frontend API base URL to your deployed backend URL.
