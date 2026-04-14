using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YuyoDev.AdminPanel;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Ajustamos el puerto de tu API según el código que me pasaste antes (5212)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5212") });

// Agregamos los servicios de MudBlazor
builder.Services.AddMudServices();
// INYECTAMOS EL SERVICIO DEL CATÁLOGO
builder.Services.AddScoped<YuyoDev.AdminPanel.Services.CatalogApiService>();
// INYECTAMOS LOS SERVICIOS DEL FRONTEND
builder.Services.AddScoped<YuyoDev.AdminPanel.Services.CatalogApiService>();
builder.Services.AddScoped<YuyoDev.AdminPanel.Services.OrderApiService>();
builder.Services.AddScoped<YuyoDev.AdminPanel.Services.InventoryApiService>();
builder.Services.AddScoped<YuyoDev.AdminPanel.Services.SettingsApiService>();

await builder.Build().RunAsync();