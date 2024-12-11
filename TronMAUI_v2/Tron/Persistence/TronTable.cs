using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tron.Persistence
{
    public class TronTable
    {
        private int _gridSize;
        private int[,] _grid;
        private TronPlayer _blue;
        private TronPlayer _red;

        public int GridSize { get { return _gridSize; } }
        public int[,] Grid { get { return _grid; } }
        public TronPlayer Blue { get {  return _blue; } }
        public TronPlayer Red { get { return _red; } }
        public TronTable(int gridSize)
        {
            this._gridSize = gridSize;
            _grid = new int[_gridSize, _gridSize];
            for (int i = 0; i < _gridSize - 1; i++)
            {
                for (int j = 0; j < _gridSize - 1; j++)
                {
                    _grid[i, j] = 0;
                }
            }

            _blue = new TronPlayer(1, 0, _gridSize / 2 - 1, Direction.Right);
            _red = new TronPlayer(2, _gridSize - 1, _gridSize / 2 - 1, Direction.Left);
            setPlayers();
        }

        public TronTable(int gridSize, int[,] grid, TronPlayer blue, TronPlayer red)
        {
            _gridSize = gridSize;
            _grid = grid;
            _blue = blue;
            _red = red;
            setPlayers();
        }

        public bool IsEmpty(int x, int y)
        {
            return _grid[y, x] == 0;
        }
        
        public void setPlayers()
        {
            _grid[_blue.Y, _blue.X] = 1;
            _grid[_red.Y, _red.X] = 2;
        }

        public void MovePlayers()
        {
            _blue.Move();
            _red.Move();
            
            
        }

        public int Hit()
        {
            if (_blue.X < 0 || _blue.Y < 0 || _blue.X >= _gridSize || _blue.Y >= _gridSize || !IsEmpty(_blue.X, _blue.Y))
            {
                return 2;
            }
            if (_red.X < 0 || _red.Y < 0 || _red.X >= _gridSize || _red.Y >= _gridSize || !IsEmpty(_red.X, _red.Y))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        
    }
}
