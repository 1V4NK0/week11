using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;

namespace MauiApp2
{
    public class AnimalViewModel : INotifyPropertyChanged
    {
        private string title;
        private List<Animal> animalList;
        private HttpClient httpClient;

        private ObservableCollection<Animal> _animals;
        public ObservableCollection<Animal> Animals { get => _animals; }


        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsNotBusy));
                }
            }
        }
        public bool IsNotBusy => !IsBusy;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Command GetAnimalsCommand { get; }
        public Command GoToDetailsCommand { get; }


        public AnimalViewModel()
        {
            httpClient = new HttpClient();
            animalList = new List<Animal>();
            _animals = new ObservableCollection<Animal>();
            GetAnimalsCommand = new Command(async () => await MakeCollection());
            GoToDetailsCommand = new Command<Animal>(async (animal) => await GoToDetails(animal));
        }

        async Task GoToDetails(Animal animal)
        {
            if (animal == null)
                return;
            await Shell.Current.GoToAsync(nameof(DetailsPage), new Dictionary<string, object>
            {
                { "animal", animal }
            });
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task GetAnimals()
        {
            try
            {
                var response = await httpClient.GetAsync("https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/MonkeysApp/monkeydata.json");
                if (response.IsSuccessStatusCode)
                {
                    string contents = await response.Content.ReadAsStringAsync();
                    animalList = JsonSerializer.Deserialize<List<Animal>>(contents);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error in loading monkeys", ex.Message, "OK");
            }
        }

        public async Task MakeCollection()
        {
            if (isBusy)
                return;
            IsBusy = true;
            try
            {
                await GetAnimals();
                _animals.Clear();
                foreach (var animal in animalList)
                {
                    _animals.Add(animal);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error in loading monkeys", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
