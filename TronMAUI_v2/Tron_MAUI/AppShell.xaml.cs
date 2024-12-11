using Tron.Model;
using Tron.Persistence;
using Tron_MAUI.View;
using Tron_MAUI.ViewModel;

namespace Tron_MAUI
{
    public partial class AppShell : Shell
    {
        private readonly ITronDataAccess _tronDataAccess;
        private readonly TronGameModel _tronGameModel;
        private readonly MainWindowViewModel _mainWindowViewModel;

        private readonly IDispatcherTimer _timer;

        private readonly IStore _store;
        private readonly StoredGameBrowserModel _storedGameBrowserModel;
        private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

        public AppShell(IStore store, ITronDataAccess dataAccess, TronGameModel tronGameModel, MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            
            _store = store;
            _tronDataAccess = dataAccess;
            _tronGameModel = tronGameModel;
            _mainWindowViewModel = mainWindowViewModel;

            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(800);
            _timer.Tick += new EventHandler(Timer_tick);

            _tronGameModel.GameOver += GameModel_GameOver;

            _mainWindowViewModel.NewGame += ViewModel_NewGame;
            _mainWindowViewModel.LoadGame += ViewModel_LoadGame;
            _mainWindowViewModel.SaveGame += ViewModel_SaveGame;
            _mainWindowViewModel.ExitGame += ViewModel_ExitGame;
            _mainWindowViewModel.PauseGame += ViewModel_PauseGame;

            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
            _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
        }

        

        private void ViewModel_PauseGame(object? sender, EventArgs e)
        {
            if (_timer.IsRunning)
            {
                _timer.Stop();
            } else
            {
                _timer.Start();
            }
        }
        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _tronGameModel.NewGame();
            StartTimer();
        }

        private async void ViewModel_LoadGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await Navigation.PushAsync(new LoadGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await Navigation.PushAsync(new SaveGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private async void ViewModel_ExitGame(object? sender, EventArgs e)
        {
            StopTimer();
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = _mainWindowViewModel
            });
        }

        private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); // visszanavigálunk

            // betöltjük az elmentett játékot, amennyiben van
            try
            {
                await _tronGameModel.LoadGameAsync(e.Name);

                // sikeres betöltés
                await Navigation.PopAsync(); // visszanavigálunk a játék táblára
                await DisplayAlert("Tron", "Sikeres betöltés.", "OK");

                // csak akkor indul az időzítő, ha sikerült betölteni a játékot
                StartTimer();
            }
            catch
            {
                await DisplayAlert("Tron", "Sikertelen betöltés.", "OK");
            }
        }


        private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); // visszanavigálunk
            StopTimer();

            try
            {
                // elmentjük a játékot
                await _tronGameModel.SaveGameAsync(e.Name);
                await DisplayAlert("Tron", "Sikeres mentés.", "OK");
            }
            catch
            {
                await DisplayAlert("Tron", "Sikertelen mentés.", "OK");
            }
        }

        private void Timer_tick(object? sender, EventArgs e)
        {
            _tronGameModel.AdvanceGame();
        }

        internal void StartTimer() => _timer.Start();
        internal void StopTimer() => _timer.Stop();

        private async void GameModel_GameOver(object? sender, TronEventArgs e)
        {
            _timer.Stop();
            switch (e.Winner)
            {
                case 1:
                    await DisplayAlert("Tron","Game Over! Blue Won!", "OK");
                    break;
                case 2:
                    await DisplayAlert("Tron", "Game Over! Red Won!", "OK");
                    break;
                default: throw new Exception("Bad Data");
            }
        }
    }
}
