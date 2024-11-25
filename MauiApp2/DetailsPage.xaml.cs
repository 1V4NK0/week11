namespace MauiApp2;

[QueryProperty(nameof(Animal), "animal")]
public partial class DetailsPage : ContentPage
{
    Animal animal;
    public Animal Animal
    {
        get => animal;
        set
        {
            animal = value;
            OnPropertyChanged();
        }
    }
    public DetailsPage()
	{
		InitializeComponent();
        BindingContext = this; 
	}
}