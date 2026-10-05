using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalculateurAge.ViewModels;

/// <summary>
/// Classe mère de tous les ViewModels.
/// Implémente INotifyPropertyChanged pour notifier les changements aux vues.
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// L'événement : le moteur de binding s'y abonne.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Prévient la vue que cette propriété a changé.
    /// Si personne n'écoute, on ne fait rien (?.).
    /// </summary>
    protected void OnPropertyChanged(
        [CallerMemberName] string nom = null)
        => PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(nom));

    /// <summary>
    /// Affecte une valeur ET notifie, en une seule ligne.
    /// Renvoie true si la valeur a réellement changé.
    /// </summary>
    protected bool SetField<T>(ref T champ, T valeur,
        [CallerMemberName] string nom = null)
    {
        // Garde-fou : évite les notifications inutiles
        // et les boucles infinies en mode TwoWay.
        if (EqualityComparer<T>.Default.Equals(champ, valeur))
            return false;
        champ = valeur;
        OnPropertyChanged(nom);
        return true;
    }
}
