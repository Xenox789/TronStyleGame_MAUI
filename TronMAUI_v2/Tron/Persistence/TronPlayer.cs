using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tron.Persistence
{
    public enum Direction
    {
        Left, Right, Up, Down
    }
    public class TronPlayer
    {
        private int _id;
        private int _xPos;
        private int _yPos;
        private Direction _direction;

        public int X { get { return _xPos; } }
        public int Y { get { return _yPos; } }
        public Direction Direction { get { return _direction; } }
        public TronPlayer(int id, int x, int y, Direction dir)
        {
            _id = id;
            _xPos = x;
            _yPos = y;
            _direction = dir;

        }

        public void LeftTurn()
        {
            switch (_direction)
            {
                case Direction.Left:
                    _direction = Direction.Down;
                    break;
                case Direction.Right:
                    _direction = Direction.Up;
                    break;
                case Direction.Up:
                    _direction = Direction.Left;
                    break;
                case Direction.Down:
                    _direction = Direction.Right;
                    break;
                default:
                    throw new Exception("Bad data. (Player.LeftTurn())");
            }
        }

        public void RightTurn()
        {
            switch (_direction)
            {
                case Direction.Right:
                    _direction = Direction.Down;
                    break;
                case Direction.Left:
                    _direction = Direction.Up;
                    break;
                case Direction.Up:
                    _direction = Direction.Right;
                    break;
                case Direction.Down:
                    _direction = Direction.Left;
                    break;
                default: throw new Exception("Bad Data. (Player: RightTurn())");
            }
        }

        public void Move()
        {
            switch (_direction)
            {
                case Direction.Left:
                    _xPos--;
                    break;
                case Direction.Right:
                    _xPos++;
                    break;
                case Direction.Up:
                    _yPos--;
                    break;
                case Direction.Down:
                    _yPos++;
                    break;
                default: throw new Exception("Bad Data. (Player: ln83)");
            }
        }

    }
}
