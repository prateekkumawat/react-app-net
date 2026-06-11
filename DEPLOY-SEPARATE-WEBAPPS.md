# Separate Azure Web App Deployment

Use this flow when the React frontend and .NET API are deployed to different Azure Web Apps.

## Required GitHub Secrets
Create these repository secrets in GitHub:
- AZURE_WEBAPP_BACKEND_PUBLISH_PROFILE
- AZURE_WEBAPP_FRONTEND_PUBLISH_PROFILE

## Replace placeholders
In `.github/workflows/azure-webapp-separate.yml`, replace:
- YOUR_BACKEND_WEBAPP_NAME
- YOUR_FRONTEND_WEBAPP_NAME

## What happens
1. Backend publishes from `backend/Api` and deploys to the backend Web App.
2. Frontend builds with `VITE_API_BASE_URL` pointing to the backend app.
3. Frontend publishes `frontend/dist` to the frontend Web App.

## Notes
- The backend must expose the API at `/api/...`.
- The frontend should use the deployed backend URL in its environment file.
