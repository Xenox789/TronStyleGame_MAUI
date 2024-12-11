using Tron.Model;
using Tron.Persistence;
using Tron_MAUI.Persistence;
using Tron_MAUI.ViewModel;

namespace Tron_MAUI
{
    public partial class App : Application
    {
        private const string SuspendedGameSavePath = "SuspendedGame";


        private readonly AppShell _appShell;
        private readonly ITronDataAccess _tronDataAccess;
        private readonly TronGameModel _gameModel;
        private readonly IStore _store;
        private readonly MainWindowViewModel _viewModel;
        

        public App()
        {
            InitializeComponent();

            _store = new TronStore();
            _tronDataAccess = new TronFileDataAccess(FileSystem.AppDataDirectory);

            _gameModel = new TronGameModel(_tronDataAccess);
            _viewModel = new MainWindowViewModel(_gameModel);

            _appShell = new AppShell(_store, _tronDataAccess, _gameModel, _viewModel)
            {
                BindingContext = _viewModel
            };

            MainPage = _appShell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);

            // az alkalmazás indításakor
            window.Created += (s, e) =>
            {
                // új játékot indítunk
                _gameModel.NewGame();
            };

            // amikor az alkalmazás fókuszba kerül
            window.Activated += (s, e) =>
            {
                if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
                    return;

                Task.Run(async () =>
                {
                    // betöltjük a felfüggesztett játékot, amennyiben van
                    try
                    {
                        await _gameModel.LoadGameAsync(SuspendedGameSavePath);

                        // csak akkor indul az időzítő, ha sikerült betölteni a játékot
                        _appShell.StartTimer();
                    }
                    catch
                    {
                    }
                });
            };

            // amikor az alkalmazás fókuszt veszt
            window.Deactivated += (s, e) =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        // elmentjük a jelenleg folyó játékot
                        _appShell.StopTimer();
                        await _gameModel.SaveGameAsync(SuspendedGameSavePath);
                    }
                    catch
                    {
                    }
                });
            };

            return window;
        }
    }
}
