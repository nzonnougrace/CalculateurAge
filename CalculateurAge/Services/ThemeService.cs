namespace CalculateurAge.Services;

/// <summary>
/// Interface pour gérer le thème de l'application (clair/sombre).
/// </summary>
public interface IThemeService
{
    void AppliquerTheme(bool estSombre);
    bool EstThemeSombre();
}

/// <summary>
/// Implémentation du service de thème.
/// </summary>
public class ThemeService : IThemeService
{
    public void AppliquerTheme(bool estSombre)
    {
        // Récupère l'application MAUI
        if (Application.Current == null)
            return;

        if (estSombre)
        {
            // Appliquer le thème sombre
            Application.Current.Resources.ApplyTheme(
                Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(d => d is AppShell) as ResourceDictionary ?? 
                new ResourceDictionary());
            Application.Current.UserAppTheme = AppTheme.Dark;
        }
        else
        {
            // Appliquer le thème clair
            Application.Current.UserAppTheme = AppTheme.Light;
        }
    }

    public bool EstThemeSombre()
    {
        return Application.Current?.UserAppTheme == AppTheme.Dark;
    }
}
