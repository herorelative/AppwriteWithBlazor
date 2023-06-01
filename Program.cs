using AppwriteWithBlazor;
using AppwriteWithBlazor.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAntDesign();

builder.Services.AddHttpClient("APPWRITE", client =>
{
    client.BaseAddress = new Uri($"https://cloud.appwrite.io");
    client.DefaultRequestHeaders.Add("Host", "localhost");
    client.DefaultRequestHeaders.Add("X-Appwrite-Response-Format", "1.0.0");
    client.DefaultRequestHeaders.Add("X-Appwrite-Project", "646b65114782681cd3b7");
});

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IAppStates, AppState>();
builder.Services.AddScoped<SimpleAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<SimpleAuthStateProvider>());
builder.Services.AddScoped<IAuthProvider, AuthProvider>();

builder.Services.AddScoped<IProjectProvider>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>();
    var appState = sp.GetRequiredService<IAppStates>();
    var authState = sp.GetRequiredService<SimpleAuthStateProvider>();
    var databaseId = "646b657dbfceafca3af4";
    var collectionId = "6470e494014d63cd9659";
    return new ProjectProvider(httpClient, appState, databaseId, collectionId, authState);
});

builder.Services.AddScoped<ITodoProvider>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>();
    var appState = sp.GetRequiredService<IAppStates>();
    var authState = sp.GetRequiredService<SimpleAuthStateProvider>();
    var databaseId = "646b657dbfceafca3af4";
    var collectionId = "6470e4cfa9fee18f1096";
    return new TodoProvider(httpClient, appState, databaseId, collectionId, authState);
});

await builder.Build().RunAsync();
