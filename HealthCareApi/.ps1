# Script to run the HealthCareApi with Swagger enabled
Write-Host "=====================================" -ForegroundColor Green
Write-Host "HealthCareApi - Swagger Setup Complete" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""
Write-Host "To access your API and Swagger UI:" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Build the project in Visual Studio (Build > Build Solution)" -ForegroundColor Yellow
Write-Host "2. Start the project (F5 or Debug > Start Debugging)" -ForegroundColor Yellow
Write-Host ""
Write-Host "Once the application is running, open your browser to:" -ForegroundColor Cyan
Write-Host ""
Write-Host "   Swagger UI:     http://localhost:PORT/swagger/ui/index" -ForegroundColor Magenta
Write-Host "   Swagger JSON:   http://localhost:PORT/swagger/docs/v1" -ForegroundColor Magenta
Write-Host ""
Write-Host "Where PORT is typically 50000+ for IIS Express" -ForegroundColor Yellow
Write-Host ""
Write-Host "Available Endpoints:" -ForegroundColor Cyan
Write-Host "   GET  /api/doctors     - Get all doctors" -ForegroundColor Green
Write-Host ""
Write-Host "=====================================" -ForegroundColor Green
