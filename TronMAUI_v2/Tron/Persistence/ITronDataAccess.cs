using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tron.Persistence
{
    public interface ITronDataAccess
    {
        Task<TronTable> LoadAsync(String path);

        Task SaveAsync(String path, TronTable table);
    }
}
