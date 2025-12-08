using MudBlazor.Services;

namespace PizzaFrontend.Extensions
{
    public static class MudExtensions
    {
        public static IServiceCollection AddMud(this IServiceCollection services)
        {
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomLeft;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 200;
                config.SnackbarConfiguration.ShowTransitionDuration = 200;
            });

            return services;
        }
    }
}
