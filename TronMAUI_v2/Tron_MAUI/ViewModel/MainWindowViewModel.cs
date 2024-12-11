using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tron.Model;

namespace Tron_MAUI.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private TronGameModel _model;
        private int _tableSize;
        private BoardSizeViewModel _boardSize = null!;


        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand BlueLeftTurnCommand { get; private set; }
        public DelegateCommand BlueRightTurnCommand { get; private set; }
        public DelegateCommand PauseGameCommand { get; private set; }

        public DelegateCommand RedLeftTurnCommand { get; private set; }
        public DelegateCommand RedRightTurnCommand { get; private set; }


        public ObservableCollection<TronField> Fields { get; set; }
        public ObservableCollection<BoardSizeViewModel> BoardSizeLevels { get; set; }

        public BoardSizeViewModel BoardSize
        {
            get => _boardSize;
            set
            {
                _boardSize = value;
                _model.BoardSize = value.BoardSize;
                OnPropertyChanged();
            }
        }
        public int TableSize
        {
            get => _tableSize;
            set
            {
                _tableSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GameTableRows));
                OnPropertyChanged(nameof(GameTableColumns));
            }
        }

        public RowDefinitionCollection GameTableRows
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), TableSize).ToArray());
        }

        public ColumnDefinitionCollection GameTableColumns
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), TableSize).ToArray());
        }

        public event EventHandler? NewGame;
        public event EventHandler? LoadGame;
        public event EventHandler? SaveGame;
        public event EventHandler? ExitGame;
        public event EventHandler? PauseGame;

        public event EventHandler? BlueLeftTurn;
        public event EventHandler? BlueRightTurn;
        public event EventHandler? RedLeftTurn;
        public event EventHandler? RedRightTurn;

        public MainWindowViewModel(TronGameModel model)
        {
            _model = model;
            _model.GameAdvanced += new EventHandler<StepEventArgs>(Model_GameAdvanced);
            _model.GameCreated += new EventHandler<GameEventArgs>(Model_GameCreated);
            _model.GameOver += new EventHandler<TronEventArgs>(Model_GameOver);

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame()); 
            ExitCommand = new DelegateCommand(param => OnExitGame());
            PauseGameCommand = new DelegateCommand(param => OnPauseGame());
            BlueLeftTurnCommand = new DelegateCommand(param => OnBlueLeft());
            BlueRightTurnCommand = new DelegateCommand(param => OnBlueRight());
            RedLeftTurnCommand = new DelegateCommand(param => OnRedLeft());
            RedRightTurnCommand = new DelegateCommand(param => OnRedRight());

            BoardSizeLevels = new ObservableCollection<BoardSizeViewModel>
            {
                new BoardSizeViewModel {BoardSize = BoardSizes.Small},
                new BoardSizeViewModel {BoardSize = BoardSizes.Medium},
                new BoardSizeViewModel {BoardSize= BoardSizes.Large}
            };
            BoardSize = BoardSizeLevels[1];

            TableSize = _model.Table.GridSize;

            Fields = new ObservableCollection<TronField>();
            for (int i = 0; i < TableSize; i++)
            {
                for (int j = 0; j < TableSize; j++)
                {
                    Fields.Add(new TronField
                    {
                        X = i,
                        Y = j,
                        CellValue = 0
                    });
                }
            }
            RefreshFields();
        }

        private void RefreshFields()
        {
            foreach (var field in Fields)
            {
                field.CellValue = _model.Table.Grid[field.Y, field.X];
                OnPropertyChanged(nameof(field.CellValue));
            }
        }

        private void Model_GameOver(object? sender, TronEventArgs e)
        {
            foreach (TronField field in Fields)
            {
                field.CellValue = e.Winner;
                OnPropertyChanged(nameof(field.CellValue));
            }
        }

        private void Model_GameAdvanced(object? sender, StepEventArgs e)
        {
            RefreshFields();
        }

        private void Model_GameCreated(object? sender, GameEventArgs e)
        {
            TableSize = _model.Table.GridSize;
            Fields.Clear();
            for (int i = 0; i < TableSize; i++)
            {
                for (int j = 0; j < TableSize; j++)
                {
                    Fields.Add(new TronField
                    {
                        CellValue = _model.Table.Grid[i, j],
                        X = j,
                        Y = i,
                    }
                    );
                }
            }
            OnPropertyChanged(nameof(TableSize));
            RefreshFields();
        }

        private void OnPauseGame()
        {
            PauseGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnBlueLeft()
        {
            _model.BlueLeftTurn();
            BlueLeftTurn?.Invoke(this, EventArgs.Empty);
        }
        private void OnBlueRight()
        {
            _model.BlueRightTurn();
            BlueRightTurn?.Invoke(this, EventArgs.Empty);
        }
        private void OnRedRight()
        {

            _model.RedRightTurn();
            RedRightTurn?.Invoke(this, EventArgs.Empty);
        }

        private void OnRedLeft()
        {

            _model.RedLeftTurn();
            RedLeftTurn?.Invoke(this, EventArgs.Empty);
        }
    }
}
