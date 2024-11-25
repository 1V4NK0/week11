namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        private AnimalViewModel animalViewModel;
    
        public MainPage()
        {
            InitializeComponent();
            animalViewModel = new AnimalViewModel();
            animalViewModel.Title = "Monkeys";
            BindingContext = animalViewModel;
        }


    }

}
