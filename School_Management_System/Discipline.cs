using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace School_Management_System.discipline
{
    internal class Discipline
    {
        public static void Menu()
        {
            char option = ' ';
            int id;
            bool run = true;

            while (run)
            {
                Console.WriteLine("1) Add discipline");
                Console.WriteLine("2) Show disciplines");
                Console.WriteLine("3) Update discipline");
                Console.WriteLine("4) Delete discipline");
                Console.WriteLine("X) Back");

                try
                {
                    Console.Write("Enter your option: ");
                    option = Console.ReadLine()[0];
                    Console.WriteLine("-------------------------------------------");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Wrong choice input!");
                }

                switch (option)
                {
                    case '1':
                        // Create
                        Create();
                        break;
                    case '2':
                        // Read
                        Read("SELECT * FROM disciplines");
                        break;
                    case '3':
                        // Update
                        do
                        {
                            Console.Write("Enter the name of the discipline you want to modify: ");
                            string discipline = Console.ReadLine();
                            id = Read("SELECT * FROM disciplines WHERE name = @value", discipline);
                        } while (id == 0);
                        Update(id);
                        break;
                    case '4':
                        // Delete
                        do
                        {
                            Console.Write("Enter the name of the discipline you want to delete: ");
                            string discipline = Console.ReadLine();
                            id = Read("SELECT * FROM disciplines WHERE name = @value", discipline);
                        } while (id == 0);
                        Delete(id);
                        break;
                    case 'x':
                    case 'X':
                        // Go Back To School Management
                        run = false;
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }

                if (run)
                {
                    Console.Write("\nPress any key to continue: ");
                    string cont = Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        public static void Create()
        {
            Console.WriteLine("Insert data for school's discipline:");

            Console.Write("Discipline's name: ");
            string name = Console.ReadLine();

            Console.Write("Discipline's description: ");
            string description = Console.ReadLine();

            string sql = "INSERT INTO disciplines(name, description) VALUES (@value1, @value2)";
            MySqlCommand insert = new MySqlCommand(sql, Program.connection);

            insert.Parameters.AddWithValue("@value1", name);
            insert.Parameters.AddWithValue("@value2", description);

            int rowsAffected = insert.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Something went wrong!");
            }
        }

        public static void Read(string sql)
        {
            MySqlCommand c1 = new MySqlCommand(sql, Program.connection);

            using (MySqlDataReader reader = c1.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Discipline's name: {reader["name"].ToString()}");
                    Console.WriteLine($"Discipline's description: {reader["description"].ToString()}");
                    Console.WriteLine();
                }
            }
        }

        public static void Read(string sql, int id)
        {
            MySqlCommand readDiscipline = new MySqlCommand(sql, Program.connection2);
            readDiscipline.Parameters.AddWithValue("@value", id);

            using (MySqlDataReader disciplineReader = readDiscipline.ExecuteReader())
            {
                while (disciplineReader.Read())
                {
                    Console.WriteLine($"Discipline's name: {disciplineReader["name"].ToString()}");
                    //Console.WriteLine($"Discipline's description: {disciplineReader["description"].ToString()}");
                }
            }
        }

        public static int Read(string sql, string name)
        {
            MySqlCommand readDiscipline = new MySqlCommand(sql, Program.connection);
            readDiscipline.Parameters.AddWithValue("@value", name);

            using (MySqlDataReader disciplineReader = readDiscipline.ExecuteReader())
            {
                if (disciplineReader.Read())
                {
                    return Convert.ToInt32(disciplineReader["discipline_id"].ToString());
                }
            }

            return 0;
        }

        public static void Update(int id)
        {
            bool run = true;
            string response = "";
            string wait = "";
            char option = ' ';

            List<string> valuesAvailable = new List<string>() {
                "name",
                "description"
            };

            while (run)
            {
                string sql = "SELECT * FROM disciplines";

                Console.WriteLine("Enter what column(value) you want to modify: ");
                int i = 1;
                foreach (var value in valuesAvailable)
                {
                    Console.WriteLine($"{i++}. {value}");
                }
                Console.WriteLine("X. Stop modifying");
                Console.Write("Enter your option: ");
                try
                {
                    option = Console.ReadLine()[0];
                } 
                catch (Exception ex)
                {
                    Console.WriteLine("Wrong choice input!");
                }

                switch (option)
                {
                    case '1':
                        Console.Write("Enter new discipline's name: ");
                        response = Console.ReadLine();
                        sql = $"UPDATE disciplines SET name = @value WHERE discipline_id = {id}";
                        break;
                    case '2':
                        Console.Write("Enter new discipline's description: ");
                        response = Console.ReadLine();
                        sql = $"UPDATE disciplines SET description = @value WHERE discipline_id  = {id}";
                        break;
                    case 'x':
                    case 'X':
                        run = false;
                        break;
                }

                MySqlCommand update = new MySqlCommand(sql, Program.connection);

                update.Parameters.AddWithValue("@value", response);

                int rowsAffected = update.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Success");
                }
                else
                {
                    Console.WriteLine("Something went wrong or just nothing has been updated!");
                }

                if (run)
                {
                    Console.Write("Press any key to continue: ");
                    wait = Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        public static void Delete(int id)
        {
            string sql = $"DELETE FROM disciplines WHERE discipline_id = {id}";

            MySqlCommand delete = new MySqlCommand(sql, Program.connection);

            int rowsAffected = delete.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Something went wrong!");
            }
        }
    }
}
