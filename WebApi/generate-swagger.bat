@echo off

echo Building project...
cd WebApi
dotnet build

echo Generating Swagger schema...
swagger tofile --output ..\swagger.json bin\Debug\net8.0\WebApi.dll v1

IF ERRORLEVEL 1 (
  echo Error generating Swagger schema
  pause
  exit /b 1
)

echo Swagger schema generated successfully: swagger.json
pause
