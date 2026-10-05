using CalculateurAge.ViewModels;

namespace CalculateurAge;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        // Définir le ViewModel comme contexte de binding
        BindingContext = new CalculateurViewModel();
    }
}
