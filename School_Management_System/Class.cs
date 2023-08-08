using MySql.Data.MySqlClient;
using School_Management_System.discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management_System
{
    internal class Class
    {
        public static void Menu()
        {
            char option = ' ';
            int id;
            bool run = true;

            while (run)
            {
                Console.WriteLine("1) Add class");
                Console.WriteLine("2) Show classes");
                Console.WriteLine("3) Update class");
                Console.WriteLine("4) Delete class");
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
                        Read("SELECT * FROM classes");
                        break;
                    case '3':
                        // Update
                        do
                        {
                            Console.Write("Enter the name of the class you want to modify: ");
                            string className = Console.ReadLine();
                            id = Read("SELECT * FROM classes WHERE group_name = @value", className);
                        } while (id == 0);
                        Update(id);
                        break;
                    case '4':
                        // Delete
                        do
                        {
                            Console.Write("Enter the name of the class you want to delete: ");
                            string className = Console.ReadLine();
                            id = Read("SELECT * FROM classes WHERE group_name = @value", className);
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
            Console.WriteLine("Insert data for class:");

            Console.Write("Class name: ");
            string groupName = Console.ReadLine();

            Console.Write("Class formation year: ");
            int formationYear = Convert.ToInt32(Console.ReadLine());

            Console.Write("Class study language: ");
            string studyLanguage = Console.ReadLine();

            Console.Write("Class study profile: ");
            string studyProfile = Console.ReadLine();

            int classMasterID = 0;
            do
            {
                Console.Write("Class master name: ");
                string name = Console.ReadLine();

                Console.Write("Class master surname: ");
                string surname = Console.ReadLine();

                classMasterID = Teacher.Read("SELECT * FROM teachers WHERE name = @value1 AND surname = @value2",
                                name, surname);

            } while (classMasterID == 0);
            

            string sql = "INSERT INTO classes(group_name, formation_year, study_language, study_profile, class_master_id) " +
                         "VALUES (@value1, @value2, @value3, @value4, @value5)";
            MySqlCommand insert = new MySqlCommand(sql, Program.connection);

            insert.Parameters.AddWithValue("@value1", groupName);
            insert.Parameters.AddWithValue("@value2", formationYear);
            insert.Parameters.AddWithValue("@value3", studyLanguage);
            insert.Parameters.AddWithValue("@value4", studyProfile);
            insert.Parameters.AddWithValue("@value5", classMasterID);

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
                    Console.WriteLine($"Class name: {reader["group_name"].ToString()}");
                    Console.WriteLine($"Class formation year: {reader["formation_year"].ToString()}");
                    Console.WriteLine($"Class study language: {reader["study_language"].ToString()}");
                    Console.WriteLine($"Class study profile: {reader["study_profile"].ToString()}");
                    Console.WriteLine("Class master:");
                    Teacher.Read($"SELECT * FROM teachers WHERE teacher_id = @value", 
                                 Convert.ToInt32(reader["class_master_id"].ToString()));
                    Console.WriteLine();
                }
            }
        }

        public static void Read(string sql, int id)
        {
            MySqlCommand readClass = new MySqlCommand(sql, Program.connection3);
            readClass.Parameters.AddWithValue("@value", id);

            using (MySqlDataReader classReader = readClass.ExecuteReader())
            {
                while (classReader.Read())
                {
                    Console.WriteLine($"Class name: {classReader["group_name"].ToString()}");
                    //Console.WriteLine($"Class formation year: {classReader["formation_year"].ToString()}");
                    //Console.WriteLine($"Class study language: {classReader["study_language"].ToString()}");
                    //Console.WriteLine($"Class study profile: {classReader["study_profile"].ToString()}");
                    //Console.WriteLine("Class master:");
                    Teacher.Read($"SELECT * FROM teachers WHERE teacher_id = @value", 
                                 Convert.ToInt32(classReader["class_master_id"].ToString()));
                }
            }
        }

        public static int Read(string sql, string name)
        {
            MySqlCommand readClass = new MySqlCommand(sql, Program.connection);
            readClass.Parameters.AddWithValue("@value", name);

            using (MySqlDataReader classReader = readClass.ExecuteReader())
            {
                if (classReader.Read())
                {
                    return Convert.ToInt32(classReader["class_id"].ToString());
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
                "formation year",
                "study language",
                "study profile",
                "master"
            };

            while (run)
            {
                string sql = "SELECT * FROM classes";
                int formationYear = 0;
                int masterID = 0;

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
                        Console.Write("Enter new class name: ");
                        response = Console.ReadLine();
                        sql = $"UPDATE classes SET group_name = @value WHERE class_id = {id}";
                        break;
                    case '2':
                        Console.Write("Enter new class formation year: ");
                        formationYear = Convert.ToInt32(Console.ReadLine());
                        sql = $"UPDATE classes SET formation_year = @value WHERE class_id = {id}";
                        break;
                    case '3':
                        Console.Write("Enter new class study language: ");
                        response = Console.ReadLine();
                        sql = $"UPDATE classes SET study_language = @value WHERE class_id = {id}";
                        break;
                    case '4':
                        Console.Write("Enter new class study profile: ");
                        response = Console.ReadLine();
                        sql = $"UPDATE classes SET study_profile = @value WHERE class_id = {id}";
                        break;
                    case '5':
                        do
                        {
                            Console.Write("Enter new class master name: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter new class master surname: ");
                            string surname = Console.ReadLine();

                            masterID = Teacher.Read("SELECT * FROM teachers WHERE name = @value1 AND surname = @value2",
                                                    name, surname);

                        } while (masterID == 0);
                        sql = $"UPDATE classes SET class_master_id = @value WHERE class_id = {id}";
                        break;
                    case 'x':
                    case 'X':
                        run = false;
                        break;
                }

                MySqlCommand update = new MySqlCommand(sql, Program.connection);

                if (formationYear != 0)
                {
                    update.Parameters.AddWithValue("@value", formationYear);
                }
                else if (masterID != 0)
                {
                    update.Parameters.AddWithValue("@value", masterID);
                }
                else
                {
                    update.Parameters.AddWithValue("@value", response);
                }

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
            string sql = $"DELETE FROM classes WHERE class_id = {id}";

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
