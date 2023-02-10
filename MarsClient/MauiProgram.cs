using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace MarsClient;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		Routing.RegisterRoute("Move", typeof(MovePage));
		Routing.RegisterRoute("Waiting", typeof(WaitingPage));

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddSingleton<MovePage>();
		builder.Services.AddSingleton<MoveViewModel>();

		builder.Services.AddSingleton<INetService, MauiNetService>();
		builder.Services.AddSingleton<INavService, MauiNavService>();

		builder.Services.AddSingleton<WaitingPage>();
		builder.Services.AddSingleton<MapService>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
