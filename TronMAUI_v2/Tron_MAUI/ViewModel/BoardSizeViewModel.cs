using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tron.Model;

namespace Tron_MAUI.ViewModel
{
    public class BoardSizeViewModel : ViewModelBase
    {
        private BoardSizes _boardSize;

        public BoardSizes BoardSize 
        {
            get { return _boardSize; }
            set
            {
                _boardSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SizeText));
            }
        }

        public string SizeText => _boardSize.ToString();
    }
}
