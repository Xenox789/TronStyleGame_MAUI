using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tron.Persistence
{
    public class TronFileDataAccess : ITronDataAccess
    {

        private String? _directory = String.Empty;

        public TronFileDataAccess(String? saveDirectory = null)
        {
            _directory = saveDirectory;
        }

        public async Task<TronTable> LoadAsync(string path)
        {
            if (!String.IsNullOrEmpty(_directory))
                path = Path.Combine(_directory, path);

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = await reader.ReadLineAsync() ?? string.Empty;
                    string[] datas = line.Split(' ');
                    int gridsize = int.Parse(datas[0]);
                    line = await reader.ReadLineAsync() ?? string.Empty;
                    datas = line.Split(' ');
                    int bluex = int.Parse(datas[0]);
                    int bluey = int.Parse(datas[1]);
                    Direction bluedirection = new Direction();
                    switch (datas[2])
                    {
                        case "Left":
                            bluedirection = Direction.Left;
                            break;
                        case "Right":
                            bluedirection = Direction.Right;
                            break;
                        case "Up":
                            bluedirection = Direction.Up;
                            break;
                        case "Down":
                            bluedirection = Direction.Down;
                            break;
                        default: throw new Exception("Wrong data!");
                    }

                    TronPlayer blue = new TronPlayer(1, bluex, bluey, bluedirection);

                    line = reader.ReadLine() ?? string.Empty;
                    datas = line.Split(' ');

                    int redx = int.Parse(datas[0]);
                    int redy = int.Parse(datas[1]);
                    Direction reddirection = new Direction();
                    switch (datas[2])
                    {
                        case "Left":
                            bluedirection = Direction.Left;
                            break;
                        case "Right":
                            bluedirection = Direction.Right;
                            break;
                        case "Up":
                            bluedirection = Direction.Up;
                            break;
                        case "Down":
                            bluedirection = Direction.Down;
                            break;
                        default: throw new Exception("Wrong data!");
                    }
                    TronPlayer red = new TronPlayer(2, redx, redy, reddirection);
                    
                    
                    int[,] grid = new int[gridsize, gridsize];

                    for (int i = 0; i < gridsize; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        datas = line.Split(' ');
                        for (int j = 0; j < gridsize; j++)
                        {
                            grid[i,j] = int.Parse(datas[j]);
                        }
                    }
                    TronTable _table = new TronTable(gridsize, grid, blue, red);
                    return _table;
                }
            }
            catch
            {
                throw new Exception();
            }
        }


        public async Task SaveAsync(string path, TronTable table)
        {
            if (!String.IsNullOrEmpty(_directory))
                path = Path.Combine(_directory, path);

            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine(table.GridSize);
                    await writer.WriteLineAsync(table.Blue.X + " " + table.Blue.Y + " " + table.Blue.Direction);
                    await writer.WriteLineAsync(table.Red.X + " " + table.Red.Y + " " + table.Red.Direction);
                    for (int i = 0; i < table.GridSize; i++)
                    {
                        for (int j = 0; j < table.GridSize; j++)
                        {
                            await writer.WriteAsync(table.Grid[i, j] + " ");
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new Exception("Something went wrong in saving");
            }
        }


    }
}
