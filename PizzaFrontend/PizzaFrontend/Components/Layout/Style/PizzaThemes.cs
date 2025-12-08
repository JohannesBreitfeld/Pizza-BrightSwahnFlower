using MudBlazor;

namespace PizzaFrontend.Components.Layout.Style
{
    public static class PizzaThemes
    {
        public static MudTheme Default => new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#2A3520",     
                Secondary = "#7A1F1F",
                Background = "#3A4A29",    
                Surface = "#3A4A29",
                AppbarBackground = "#2A3520",
                TextPrimary = "#FFFFFF",
                TextSecondary = "#F8F3E7"
            }
        };
    }
}
