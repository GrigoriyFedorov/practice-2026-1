using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public void Save<T> (T entity) where T : IEntity
        {
            string tablePath = Path.Combine(_rootPath, typeof(T).Name);
            Directory.CreateDirectory(tablePath);

            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            string filePath = Path.Combine(tablePath, $"{entity.Id}.json");

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(entity, options);
            File.WriteAllText(filePath, json);
        }
        public T Load<T> (Guid id) where T : IEntity
        {
            string filePath = Path.Combine(_rootPath, typeof(T).Name, $"{id}.json");

            if (!File.Exists(filePath))
                return default;

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }

        public List<T> LoadAll<T> () where T : IEntity
        {
            string tablePath = Path.Combine(_rootPath, typeof(T).Name);
            if (!Directory.Exists(tablePath))
                return new List<T>();

            List<T> result = new List<T>();
            string[] filePaths = Directory.GetFiles(tablePath, "*.json");
            
            foreach (var filePath in filePaths)
            {
                string json = File.ReadAllText(filePath);
                T entity = JsonSerializer.Deserialize<T>(json);
                if (entity != null)
                    result.Add(entity);
            }
            return result;
        }

        public void Delete<T>(Guid id) where T : IEntity
        {
            string filePath = Path.Combine(_rootPath, typeof(T).Name, $"{id}.json");

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public void DeleteAll<T>() where T : IEntity
        {
            string tablePath = Path.Combine(_rootPath, typeof(T).Name);

            if (Directory.Exists(tablePath))
                Directory.Delete(tablePath, true);
        }
    }
}
