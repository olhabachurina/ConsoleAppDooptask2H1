// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString = "Server=DESKTOP-4PCU5RA\\SQLEXPRESS;Database=VegetablesAndFruits;Trusted_Connection=True;TrustServerCertificate=True";
    static bool isConnected = false;

    static void Main()
    {
        Console.WriteLine("Добро пожаловать в приложение \"VegetablesAndFruits\"!");

        Dictionary<string, Action> actions = new Dictionary<string, Action>
        {
            { "1", ConnectToDatabase },
            { "2", DisconnectFromDatabase },
            { "3", () => ExecuteIfConnected(DisplayVegetables) },
            { "4", () => ExecuteIfConnected(DisplayFruits) },
            { "5", () => ExecuteIfConnected(AddVegetable) },
            { "6", () => ExecuteIfConnected(AddFruit) },
            { "7", () => { Console.WriteLine("До свидания!"); Environment.Exit(0); } }
        };

        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Подключиться к базе данных");
            Console.WriteLine("2. Отключиться от базы данных");
            Console.WriteLine("3. Просмотреть таблицу Овощи");
            Console.WriteLine("4. Просмотреть таблицу Фрукты");
            Console.WriteLine("5. Добавить новый овощ");
            Console.WriteLine("6. Добавить новый фрукт");
            Console.WriteLine("7. Выйти из приложения");

            string choice = Console.ReadLine();

            if (actions.TryGetValue(choice, out var action))
            {
                action();
            }
            else
            {
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
            }
        }
    }

    static void ConnectToDatabase()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                isConnected = true;
                Console.WriteLine("Подключение к базе данных успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
            }
        }
    }

    static void DisconnectFromDatabase()
    {
        Console.WriteLine("Отключение от базы данных.");
        isConnected = false;
    }

    static void ExecuteIfConnected(Action action)
    {
        if (isConnected)
        {
            action();
        }
        else
        {
            Console.WriteLine("Необходимо подключиться к базе данных.");
        }
    }
    static void DisplayVegetables()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Vegetables";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("\nТаблица Овощи:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["VegetableId"]}: {reader["Name"]}, Цвет: {reader["Color"]}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отображении овощей: {ex.Message}");
        }
    }

    static void DisplayFruits()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Fruits";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("\nТаблица Фрукты:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["FruitId"]}: {reader["Name"]}, Вкус: {reader["Taste"]}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отображении фруктов: {ex.Message}");
        }
    }
    static void AddVegetable()
    {
        try
        {
            Console.Write("Введите имя овоща: ");
            string vegetableName = Console.ReadLine();
            Console.Write("Введите цвет овоща: ");
            string vegetableColor = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Vegetables (Name, Color) VALUES (@Name, @Color)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", vegetableName);
                    command.Parameters.AddWithValue("@Color", vegetableColor);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Овощ успешно добавлен!");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении овоща: {ex.Message}");
        }
    }

    static void AddFruit()
    {
        try
        {
            Console.Write("Введите имя фрукта: ");
            string fruitName = Console.ReadLine();
            Console.Write("Введите вкус фрукта: ");
            string fruitTaste = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Fruits (Name, Taste) VALUES (@Name, @Taste)";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", fruitName);
                    command.Parameters.AddWithValue("@Taste", fruitTaste);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Фрукт успешно добавлен!");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении фрукта: {ex.Message}");
        }
    }
}
