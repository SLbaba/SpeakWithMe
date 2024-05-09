using Microsoft.Extensions.Logging;
using SpeakWithMe.Library.Services.Implementation;
using SpeakWithMe.Library.Services.IServices;

namespace SpeakWithMe
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<ILogService, LogService>();
            builder.Services
                .AddSingleton<IParcelBoxService, ParcelBoxService>();
            builder.Services
                .AddSingleton<ISpeechService, SpeechService>();
            builder.Services.AddSingleton<ITranslateService, TranslateService>();
            builder.Services.AddSingleton<IChatGptService, ChatGptService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
