using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using John.SocialClub.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseApiUrl"] ?? "http://localhost:5189");
});

// Simple in-memory auth state to mimic legacy login
builder.Services.AddScoped<AuthState>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
