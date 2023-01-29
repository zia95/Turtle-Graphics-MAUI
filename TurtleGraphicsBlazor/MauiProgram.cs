using Microsoft.AspNetCore.Components.WebView.Maui;
using Radzen;
using Sentry;
using TurtleGraphicsBlazor.Data;
using TurtleGraphicsBlazor.Services;

namespace TurtleGraphicsBlazor;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseSentry(options =>
            {
                options.Dsn = "https://e6d5f9688076418595b88aa7053304aa@o4504583321812992.ingest.sentry.io/4504587023286272";
            })
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		
		builder.Services.AddSingleton<DatabaseHelper>();
        builder.Services.AddSingleton<TurtleService>();

		//redzen
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<TooltipService>();
        builder.Services.AddScoped<ContextMenuService>();

        //sentry doesn't catch exception in blazor web view for some reason
        //this will help with that
        AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
        {
#if DEBUG
            string m = e.Exception.Message;
            System.Diagnostics.Debug.WriteLine($"FCE-> {m}");
#endif
            SentrySdk.CaptureException(e.Exception);
        };
        return builder.Build();
	}
}
