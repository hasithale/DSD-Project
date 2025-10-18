# DSD SaaS - MVP (Step 0)

## Run API (dev)
1. Ensure dotnet SDK and dotnet-ef are installed
2. From repository run:
dotnet build
dotnet ef database update --project DSD.Data --startup-project DSD.API
dotnet run --project DSD.API

3. Open `http://localhost:5000/swagger` (or the listed URL) and `GET /api/ping`

## Notes
- Development DB uses LocalDB: `Server=(localdb)\mssqllocaldb;Database=DSD_Db;...`
- JWT secret: update `DSD.API/appsettings.Development.json` before production