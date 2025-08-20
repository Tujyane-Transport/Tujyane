using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tujyane;
using BlazorBootstrap;
using Tujyane.Services;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// build once
var host = builder.Build();

// get JS runtime
var js = host.Services.GetRequiredService<IJSRuntime>();

// initialize Appwrite
await js.InvokeVoidAsync("initAppwrite", "https://fra.cloud.appwrite.io/v1", "6898b288001e1235a86f");

// now run the app
await host.RunAsync();
