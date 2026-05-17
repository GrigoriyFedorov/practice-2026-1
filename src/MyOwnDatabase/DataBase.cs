using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOwnDatabase
{
    public class DataBase
    {
        private readonly string _rootPath;
        public DataBase(string rootPath)
        {
            _rootPath = rootPath;

            if (!Directory.Exists(_rootPath))
                Directory.CreateDirectory(rootPath);
        }
    }
}
