using CalculateurAge.Services;

namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    private readonly IThemeService _themeService;

    // Champs privés : la vraie donnée.
    private string _nom = "";
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private bool _resultatVisible;
    private string _ageDetaille = "";
    private string _signeZodiaque = "";
    private string _message = "";
    private int _joursRestants;
    private bool _estThemeSombre;

    // Propriétés publiques : ce que le XAML voit.
    public string Nom
    {
        get => _nom;
        set
        {
            if (SetField(ref _nom, value))
                CalculerCommand.Rafraichir();
        }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set => SetField(ref _dateNaissance, value);
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set => SetField(ref _resultatVisible, value);
    }

    /// <summary>
    /// Fonctionnalité 1 : Âge détaillé (années, mois, jours)
    /// </summary>
    public string AgeDetaille
    {
        get => _ageDetaille;
        set => SetField(ref _ageDetaille, value);
    }

    /// <summary>
    /// Fonctionnalité 2 : Signe du zodiaque
    /// </summary>
    public string SigneZodiaque
    {
        get => _signeZodiaque;
        set => SetField(ref _signeZodiaque, value);
    }

    /// <summary>
    /// Message affichant "Majeur" ou "Mineur"
    /// </summary>
    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    /// <summary>
    /// Nombre de jours restants avant le prochain anniversaire
    /// </summary>
    public int JoursRestants
    {
        get => _joursRestants;
        set => SetField(ref _joursRestants, value);
    }

    /// <summary>
    /// Fonctionnalité 3 : Thème sombre activé
    /// </summary>
    public bool EstThemeSombre
    {
        get => _estThemeSombre;
        set => SetField(ref _estThemeSombre, value);
    }

    // Lié à Button.Command dans le XAML.
    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }
    public RelayCommand BasculerThemeCommand { get; }

    public CalculateurViewModel() : this(new ThemeService())
    {
    }

    public CalculateurViewModel(IThemeService themeService)
    {
        _themeService = themeService;
        EstThemeSombre = _themeService.EstThemeSombre();

        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom));

        EffacerCommand = new RelayCommand(Effacer);
        BasculerThemeCommand = new RelayCommand(BasculerTheme);
    }

    /// <summary>
    /// La logique métier : aucun contrôle d'interface ici.
    /// </summary>
    private void Calculer()
    {
        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date > DateTime.Today.AddYears(-age)) 
            age--;

        // Fonctionnalité 1 : Âge détaillé
        var ageDetailleText = CalculerAgeDetaille(DateNaissance);
        AgeDetaille = ageDetailleText;

        // Fonctionnalité 2 : Signe du zodiaque
        var signe = DeterminerSigneZodiaque(DateNaissance);
        SigneZodiaque = signe;

        // Message majeur/mineur
        Message = age >= 18 ? "Majeur" : "Mineur";

        // Nombre de jours restants avant anniversaire
        JoursRestants = CalculerJoursRestants(DateNaissance);

        Resultat = $"{Nom}, vous avez {age} ans";
        ResultatVisible = true;
    }

    /// <summary>
    /// Fonctionnalité 1 : Calcule l'âge en années, mois et jours.
    /// </summary>
    private string CalculerAgeDetaille(DateTime dateNaissance)
    {
        DateTime aujourd = DateTime.Today;
        int annees = aujourd.Year - dateNaissance.Year;
        int mois = aujourd.Month - dateNaissance.Month;
        int jours = aujourd.Day - dateNaissance.Day;

        // Ajustements si nécessaire
        if (jours < 0)
        {
            mois--;
            jours += DateTime.DaysInMonth(aujourd.Year, aujourd.Month);
        }

        if (mois < 0)
        {
            annees--;
            mois += 12;
        }

        return $"{annees} ans, {mois} mois et {jours} jours";
    }

    /// <summary>
    /// Fonctionnalité 2 : Détermine le signe du zodiaque.
    /// </summary>
    private string DeterminerSigneZodiaque(DateTime dateNaissance)
    {
        int jour = dateNaissance.Day;
        int mois = dateNaissance.Month;

        return (mois, jour) switch
        {
            (1, >= 20) or (2, <= 18) => "Verseau",
            (2, >= 19) or (3, <= 20) => "Poissons",
            (3, >= 21) or (4, <= 19) => "Bélier",
            (4, >= 20) or (5, <= 20) => "Taureau",
            (5, >= 21) or (6, <= 20) => "Gémeaux",
            (6, >= 21) or (7, <= 22) => "Cancer",
            (7, >= 23) or (8, <= 22) => "Lion",
            (8, >= 23) or (9, <= 22) => "Vierge",
            (9, >= 23) or (10, <= 22) => "Balance",
            (10, >= 23) or (11, <= 21) => "Scorpion",
            (11, >= 22) or (12, <= 21) => "Sagittaire",
            (12, >= 22) or (1, <= 19) => "Capricorne",
            _ => "Inconnu"
        };
    }

    /// <summary>
    /// Calcule les jours restants avant le prochain anniversaire.
    /// </summary>
    private int CalculerJoursRestants(DateTime dateNaissance)
    {
        var aujourd = DateTime.Today;
        var prochainAnniversaire = new DateTime(
            aujourd.Year, 
            dateNaissance.Month, 
            dateNaissance.Day);

        if (prochainAnniversaire < aujourd)
        {
            prochainAnniversaire = prochainAnniversaire.AddYears(1);
        }

        return (prochainAnniversaire - aujourd).Days;
    }

    /// <summary>
    /// Fonctionnalité 3 : Efface tous les champs.
    /// </summary>
    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        ResultatVisible = false;
        AgeDetaille = "";
        SigneZodiaque = "";
        Message = "";
        JoursRestants = 0;
        EstThemeSombre = false;
        _themeService.AppliquerTheme(false);
    }

    /// <summary>
    /// Fonctionnalité 3 : Bascule entre thème clair et sombre.
    /// </summary>
    private void BasculerTheme()
    {
        EstThemeSombre = !EstThemeSombre;
        _themeService.AppliquerTheme(EstThemeSombre);
    }
}
