using SpeakWithMe.Library.Models;
using SpeakWithMe.Library.Services.Implementation;
using SpeakWithMe.Library.Services.IServices;
using SpeakWithMe.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<ILogService, LogService>();
builder.Services
    .AddSingleton<IParcelBoxService, ParcelBoxService>();
builder.Services
    .AddSingleton<ISpeechService, SpeechService>();
builder.Services.AddSingleton<ITranslateService, TranslateService>();
builder.Services.AddSingleton<IChatGptService, ChatGptService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode().AddAdditionalAssemblies(typeof(ChatHistory).Assembly);

app.Run();
