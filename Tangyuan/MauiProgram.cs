using CommunityToolkit.Maui;

namespace Tangyuan;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("HiraginoSansGB-W3.ttf", "OpenSansRegular");
				fonts.AddFont("HiraginoSansGB-W6.ttf", "HiraginoW6");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}
