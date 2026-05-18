using MyOwnDatabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }

    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
    internal class Test
    {
        static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyDataBase");

        static void ShowMenu()
        {
            Console.WriteLine("========MENU========");
            Console.WriteLine("1. Добавить объект в БД");
            Console.WriteLine("2. Найти объект");
            Console.WriteLine("3. Найти все объекты определенного типа");
            Console.WriteLine("4. Найти объекты, соответствующие условию");
            Console.WriteLine("5. Удалить объект");
            Console.WriteLine("6. Удалить все объекты определенного типа");
            Console.WriteLine("0. Выход");
            Console.WriteLine();
        }


        static void AddObject(DataBase db)
        {
            Console.WriteLine("\nВыберите тип добавляемого объекта:");
            Console.WriteLine("1. User (пользователь)");
            Console.WriteLine("2. Product (товар)");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var user = new User();
                    Console.Write("Введите имя: ");
                    user.Name = Console.ReadLine();
                    Console.Write("Введите возраст: ");
                    user.Age = int.Parse(Console.ReadLine());
                    Console.Write("Введите email: ");
                    user.Email = Console.ReadLine();
                    db.Save(user);
                    Console.WriteLine($"Пользователь сохранен. ID: {user.Id}");
                    break;

                case "2":
                    var product = new Product();
                    Console.Write("Введите название товара: ");
                    product.Title = Console.ReadLine();
                    Console.Write("Введите цену: ");
                    product.Price = decimal.Parse(Console.ReadLine());
                    Console.Write("Введите категорию: ");
                    product.Category = Console.ReadLine();
                    db.Save(product);
                    Console.WriteLine($"Товар сохранен. ID: {product.Id}");
                    break;

                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
        static void FindObject(DataBase db)
        {
            Console.WriteLine("\nВыберите тип объекта:");
            Console.WriteLine("1. User");
            Console.WriteLine("2. Product");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            Console.Write("Введите ID объекта (GUID): ");
            string idStr = Console.ReadLine();

            try
            {
                Guid id = Guid.Parse(idStr);

                switch (choice)
                {
                    case "1":
                        var user = db.Load<User>(id);
                        if (user == null)
                            Console.WriteLine("Пользователь не найден.");
                        else
                            Console.WriteLine($"Найден: {user.Name}, возраст: {user.Age}, email: {user.Email}");
                        break;

                    case "2":
                        var product = db.Load<Product>(id);
                        if (product == null)
                            Console.WriteLine("Товар не найден.");
                        else
                            Console.WriteLine($"Найден: {product.Title}, цена: {product.Price}, категория: {product.Category}");
                        break;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный формат GUID.");
            }
        }
        static void FindAllObjectsByType(DataBase db)
        {
            Console.WriteLine("\nВыберите тип объекта:");
            Console.WriteLine("1. User");
            Console.WriteLine("2. Product");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var users = db.LoadAll<User>();
                    Console.WriteLine($"\nВсего пользователей: {users.Count}");
                    foreach (var u in users)
                        Console.WriteLine($"  ID: {u.Id}\n  Имя: {u.Name}\n  Возраст: {u.Age}\n  Email: {u.Email}\n");
                    break;

                case "2":
                    var products = db.LoadAll<Product>();
                    Console.WriteLine($"\nВсего товаров: {products.Count}");
                    foreach (var p in products)
                        Console.WriteLine($"  ID: {p.Id}\n  Название: {p.Title}\n  Цена: {p.Price}\n  Категория: {p.Category}\n");
                    break;

                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
        static void FindByCondition(DataBase db)
        {
            Console.WriteLine("\nВыберите тип объекта:");
            Console.WriteLine("1. User");
            Console.WriteLine("2. Product");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите минимальный возраст для поиска: ");
                    int minAge = int.Parse(Console.ReadLine());
                    var adults = db.Find<User>(u => u.Age >= minAge);
                    Console.WriteLine($"\nНайдено пользователей: {adults.Count}");
                    foreach (var u in adults)
                        Console.WriteLine($"  {u.Name}, возраст: {u.Age}, email: {u.Email}");
                    break;

                case "2":
                    Console.Write("Введите максимальную цену для поиска: ");
                    decimal maxPrice = decimal.Parse(Console.ReadLine());
                    var cheapProducts = db.Find<Product>(p => p.Price <= maxPrice);
                    Console.WriteLine($"\nНайдено товаров: {cheapProducts.Count}");
                    foreach (var p in cheapProducts)
                        Console.WriteLine($"  {p.Title}, цена: {p.Price}, категория: {p.Category}");
                    break;

                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
        static void DeleteObject(DataBase db)
        {
            Console.WriteLine("\nВыберите тип объекта:");
            Console.WriteLine("1. User");
            Console.WriteLine("2. Product");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            Console.Write("Введите ID объекта для удаления: ");
            string idStr = Console.ReadLine();

            try
            {
                Guid id = Guid.Parse(idStr);

                switch (choice)
                {
                    case "1":
                        db.Delete<User>(id);
                        Console.WriteLine("Пользователь удален (если существовал).");
                        break;
                    case "2":
                        db.Delete<Product>(id);
                        Console.WriteLine("Товар удален (если существовал).");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный формат GUID.");
            }
        }

        static void DeleteAllObjectsByType(DataBase db)
        {
            Console.WriteLine("\nВыберите тип объекта для полной очистки:");
            Console.WriteLine("1. User");
            Console.WriteLine("2. Product");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            Console.Write("Вы уверены? (да/нет): ");
            string confirm = Console.ReadLine();

            if (confirm.ToLower() != "да")
            {
                Console.WriteLine("Операция отменена.");
                return;
            }

            switch (choice)
            {
                case "1":
                    db.DeleteAll<User>();
                    Console.WriteLine("Все пользователи удалены.");
                    break;
                case "2":
                    db.DeleteAll<Product>();
                    Console.WriteLine("Все товары удалены.");
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
        static void Main(string[] args)
        {
            DataBase db = new DataBase(dbPath);
          
            while (true)
            {
                ShowMenu();
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddObject(db); break;
                    case "2":
                        FindObject(db); break;
                    case "3":
                        FindAllObjectsByType(db); break;
                    case "4":
                        FindByCondition(db); break;
                    case "5":
                        DeleteObject(db); break;
                    case "6":
                        DeleteAllObjectsByType(db); break;
                    case "0":
                        Console.WriteLine("Выход");
                        return;
                    default:
                        Console.WriteLine("Введено некоректное значение");
                        break;
                }
                Console.WriteLine();
            }

        }
    }
}
