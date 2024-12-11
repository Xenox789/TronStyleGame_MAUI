using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tron.Model
{
    public class TronEventArgs
    {
        private int _winner;

        public int Winner { get { return _winner; } }

        public TronEventArgs(int? winner)
        {
            if(winner.HasValue)
            {
                _winner = winner.Value;
            }
        }
    }
}
