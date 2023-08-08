using MySql.Data.MySqlClient;
using School_Management_System.discipline;

namespace School_Management_System
{
    internal class Program
    {
        public static MySqlConnection connection = new MySqlConnection("server=localhost; port=4000; database=school_ms; uid=root; pwd=;");
        public static MySqlConnection connection2 = new MySqlConnection("server=localhost; port=4000; database=school_ms; uid=root; pwd=;");
        public static MySqlConnection connection3 = new MySqlConnection("server=localhost; port=4000; database=school_ms; uid=root; pwd=;");
        public static MySqlConnection connection4 = new MySqlConnection("server=localhost; port=4000; database=school_ms; uid=root; pwd=;");

        static void Main(string[] args)
        {
            connection.Open();
            connection2.Open();
            connection3.Open();
            connection4.Open();

            char option = ' ';

            while (true)
            {
                Console.WriteLine("**********Welcome to School Management System!**********");
                Console.WriteLine("1) Manage disciplines");
                Console.WriteLine("2) Manage teachers");
                Console.WriteLine("3) Manage classes");
                Console.WriteLine("4) Manage students");
                Console.WriteLine("X) Exit App");

                try
                {
                    Console.Write("Enter your option: ");
                    option = Console.ReadLine()[0];
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Wrong choice input!");
                }

                switch (option)
                {
                    // Manage Disciplines
                    case '1':
                        Console.Clear();
                        Discipline.Menu();
                        break;
                    // Manage Teachers
                    case '2':
                        Console.Clear();
                        Teacher.Menu();
                        break;
                    // Manage Classes
                    case '3':
                        Console.Clear();
                        Class.Menu();
                        break;
                    // Manage Students Info
                    case '4':
                        Console.Clear();
                        Student.Menu();
                        break;
                    // Exit App
                    case 'x':
                    case 'X':
                        Environment.Exit(0);
                        break;
                    // Default Case
                    default:
                        Console.WriteLine("Invalid option!");
                        break;

                }
            }
        }
    }
}