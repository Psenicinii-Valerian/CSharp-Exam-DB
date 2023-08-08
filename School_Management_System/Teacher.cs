using MySql.Data.MySqlClient;
using School_Management_System.discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_Management_System
{
    internal class Teacher
    {
        public static void Menu()
        {
            char option = ' ';
            int id;
            bool run = true;

            while (run)
            {
                Console.WriteLine("1) Add teacher");
                Console.WriteLine("2) Show teachers");
                Console.WriteLine("3) Update teacher");
                Console.WriteLine("4) Delete teacher");
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
                        Read("SELECT * FROM teachers");
                        break;
                    case '3':
                        // Update
                        do
                        {
                            Console.Write("Enter the name of the teacher you want to modify: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter the surname of the teacher you want to modify: ");
                            string surname = Console.ReadLine();

                            id = Read("SELECT * FROM teachers WHERE name = @value1 AND surname = @value2",
                                      name, surname);

                        } while (id == 0);
                        Update(id);
                        break;
                    case '4':
                        // Delete
                        do
                        {
                            Console.Write("Enter the name of the class master you want to delete: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter the surname of the class master you want to delete: ");
                            string surname = Console.ReadLine();

                            id = Read("SELECT * FROM teachers WHERE name = @value1 AND surname = @value2",
                                            name, surname);

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
            Console.WriteLine("Insert data for teacher:");

            Console.Write("Teacher's name: ");
            string name = Console.ReadLine();

            Console.Write("Teacher's surname: ");
            string surname = Console.ReadLine();

            Console.Write("Teacher's gender(m/f): ");
            string stringGender = Console.ReadLine();
            bool gender = stringGender == "m"? true: false;

            Console.Write("Teacher's age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Teacher's experience(years): ");
            int experience = Convert.ToInt32(Console.ReadLine());

            Console.Write("Teacher's phone number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Teacher's salary($): ");
            double salary = Convert.ToDouble(Console.ReadLine());

            int disciplineID = 0;
            do
            {
                Console.Write("Discipline taught by teacher: ");
                string discipline = Console.ReadLine();
                disciplineID = Discipline.Read("SELECT * FROM disciplines WHERE name = @value", discipline);
            } while (disciplineID == 0);

            string sql = "INSERT INTO teachers(name, surname, gender, age, experience, phone_number, salary, discipline_id)" +
                         " VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8)";
            MySqlCommand insert = new MySqlCommand(sql, Program.connection);

            insert.Parameters.AddWithValue("@value1", name);
            insert.Parameters.AddWithValue("@value2", surname);
            insert.Parameters.AddWithValue("@value3", gender);
            insert.Parameters.AddWithValue("@value4", age);
            insert.Parameters.AddWithValue("@value5", experience);
            insert.Parameters.AddWithValue("@value6", phoneNumber);
            insert.Parameters.AddWithValue("@value7", salary);
            insert.Parameters.AddWithValue("@value8", disciplineID);

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
                    Console.WriteLine($"Teacher's name: {reader["name"].ToString()}");
                    Console.WriteLine($"Teacher's surname: {reader["surname"].ToString()}");
                    if (Convert.ToBoolean(reader["gender"].ToString()) == true)
                    {
                        Console.WriteLine("Teacher's gender: male");
                    }
                    else
                    {
                        Console.WriteLine("Teacher's gender: female");
                    }
                    Console.WriteLine($"Teacher's age: {reader["age"].ToString()}");
                    Console.WriteLine($"Teacher's experience: {reader["experience"].ToString()}");
                    Console.WriteLine($"Teacher's phone number: {reader["phone_number"].ToString()}");
                    Console.WriteLine($"Teacher's salary: {reader["salary"].ToString()}$");
                    Console.WriteLine($"Discipline taught by teacher:");

                    int discipline_id = Convert.ToInt32(reader["discipline_id"].ToString());
                    Discipline.Read("SELECT * FROM disciplines WHERE discipline_id = @value", 
                                    discipline_id);
                    Console.WriteLine();
                }
            }
        }

        public static void Read(string sql, int id)
        {
            MySqlCommand readTeacher = new MySqlCommand(sql, Program.connection4);
            readTeacher.Parameters.AddWithValue("@value", id);

            using (MySqlDataReader teacherReader = readTeacher.ExecuteReader())
            {
                while (teacherReader.Read())
                {
                    Console.WriteLine($"Teacher's name: {teacherReader["name"].ToString()}");
                    Console.WriteLine($"Teacher's surname: {teacherReader["surname"].ToString()}");
                    //if (Convert.ToBoolean(teacherReader["gender"].ToString()) == true)
                    //{
                    //    Console.WriteLine("Teacher's gender: male");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Teacher's gender: female");
                    //}
                    //Console.WriteLine($"Teacher's age: {teacherReader["age"].ToString()}");
                    //Console.WriteLine($"Teacher's experience: {teacherReader["experience"].ToString()}");
                    //Console.WriteLine($"Teacher's phone number: {teacherReader["phone_number"].ToString()}");
                    //Console.WriteLine($"Teacher's salary: {teacherReader["salary"].ToString()}$");
                    Console.WriteLine($"Discipline taught by teacher:");
                    Discipline.Read("SELECT * FROM disciplines WHERE discipline_id = @value", 
                                    Convert.ToInt32(teacherReader["discipline_id"].ToString()));
                }
            }
        }

        public static int Read(string sql, string name, string surname)
        {
            MySqlCommand readTeacher = new MySqlCommand(sql, Program.connection);
            readTeacher.Parameters.AddWithValue("@value1", name);
            readTeacher.Parameters.AddWithValue("@value2", surname);

            using (MySqlDataReader teacherReader = readTeacher.ExecuteReader())
            {
                if (teacherReader.Read())
                {
                    return Convert.ToInt32(teacherReader["teacher_id"].ToString());
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
                "surname",
                "gender",
                "age",
                "experience",
                "phone_number",
                "salary",
                "discipline"
            };

            while (run)
            {
                string sql = "SELECT * FROM teachers";
                bool genderChanged = false;
                bool gender = false;
                int age = 0;
                int experience = -1;
                int disciplineID = 0;

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
                        Console.Write("Enter new teacher's name: ");
                        response = Console.ReadLine();
                        sql = $"UPDATE teachers SET name = @value WHERE teacher_id = {id}";
                        break;
                    case '2':
                        Console.Write("Enter new teacher's surname: ");
                        response = Console.ReadLine();
                        sql = $"UPDATE teachers SET surname = @value WHERE teacher_id = {id}";
                        break;
                    case '3':
                        genderChanged = true;
                        Console.Write("Enter new teacher's gender(m/f): ");
                        response = Console.ReadLine();  
                        gender = response == "m" ? true: false;
                        sql = $"UPDATE teachers SET gender = @value WHERE teacher_id = {id}";
                        break;
                    case '4':
                        Console.Write("Enter new teacher's age: ");
                        age = Convert.ToInt32(Console.ReadLine());
                        sql = $"UPDATE teachers SET age = @value WHERE teacher_id = {id}";
                        break;
                    case '5':
                        Console.Write("Enter new teacher's experience(years): ");
                        experience = Convert.ToInt32(Console.ReadLine());
                        sql = $"UPDATE teachers SET experience = @value WHERE teacher_id = {id}";
                        break;
                    case '6':
                        Console.Write("Enter new teacher's phone number: ");
                        response = Console.ReadLine();
                        sql = $"UPDATE teachers SET phone_number = @value WHERE teacher_id = {id}";
                        break;
                    case '7':
                        Console.Write("Enter new teacher's salary($): ");
                        response = Console.ReadLine();
                        sql = $"UPDATE teachers SET salary = @value WHERE teacher_id = {id}";
                        break;
                    case '8':
                        do
                        {
                            Console.Write("Enter new discipline taught by teacher: ");
                            string discipline = Console.ReadLine();
                            disciplineID = Discipline.Read("SELECT * FROM disciplines WHERE name = @value", discipline);
                        } while (disciplineID == 0);
                        sql = $"UPDATE teachers SET discipline_id = @value WHERE teacher_id = {id}";
                        break;
                    case 'x':
                    case 'X':
                        run = false;
                        break;
                }

                MySqlCommand update = new MySqlCommand(sql, Program.connection);

                if (genderChanged)
                {
                    update.Parameters.AddWithValue("@value", gender);
                }
                else if(age != 0)
                {
                    update.Parameters.AddWithValue("@value", age);
                }
                else if (experience != -1)
                {
                    update.Parameters.AddWithValue("@value", experience);
                }
                else if (disciplineID != 0)
                {
                    update.Parameters.AddWithValue("@value", disciplineID);
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
            string sql = $"DELETE FROM teachers WHERE teacher_id = {id}";

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
