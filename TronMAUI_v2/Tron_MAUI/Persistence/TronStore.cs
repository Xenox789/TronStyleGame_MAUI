using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tron.Persistence;

namespace Tron_MAUI.Persistence
{
    public class TronStore : IStore
    {
        public async Task<IEnumerable<String>> GetFilesAsync()
        {
            return await Task.Run(() => Directory.GetFiles(FileSystem.AppDataDirectory)
                .Select(Path.GetFileName)
                .Where(name => name?.EndsWith(".tbl") ?? false)
                .OfType<String>());
        }

        /// <summary>
        /// Módosítás idejének lekérdezése.
        /// </summary>
        /// <param name="name">A fájl neve.</param>
        /// <returns>Az utols módosítás ideje.</returns>
        public async Task<DateTime> GetModifiedTimeAsync(String name)
        {
            var info = new FileInfo(Path.Combine(FileSystem.AppDataDirectory, name));

            return await Task.Run(() => info.LastWriteTime);
        }
    }
}
