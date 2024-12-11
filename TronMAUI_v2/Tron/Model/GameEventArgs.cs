using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tron.Model
{
    public class GameEventArgs : EventArgs
    {
        public int gridSize;
         public GameEventArgs(int gridSize)
        {
            this.gridSize = gridSize;
        }
    }
}
