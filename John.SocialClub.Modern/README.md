# John.SocialClub Modern (.NET 8)

Modernized 3-tier architecture of the legacy WinForms app:

- John.SocialClub.Domain: Entities and enums
- John.SocialClub.Application: Interfaces and business services
- John.SocialClub.Infrastructure: In-memory repository and DI
- John.SocialClub.Api: ASP.NET Core Web API (NET 8)
- John.SocialClub.Web: Blazor Server (NET 8)

Data is in-memory (no DB). Members CRUD is available.

How to run (requires .NET 8 SDK):

1. Open a terminal at this folder
2. Build solution (optional):
   dotnet build
3. Start API:
   cd John.SocialClub.Api
   dotnet run
   API will listen on http://localhost:5189 (or shown in console)
4. Start Blazor Server:
   Open a new terminal
   cd John.SocialClub.Web
   dotnet run
   App will open on http://localhost:5159 (or shown in console)

Blazor will call the API at http://localhost:5189 by default. Update BaseApiUrl in Web appsettings if ports differ.

Authentication is a simple in-memory username/password (admin / password). No JWT to keep it simple.
