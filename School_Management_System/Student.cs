using MySql.Data.MySqlClient;
using School_Management_System.discipline;

namespace School_Management_System
{
    internal class Student
    {
        public static void Menu()
        {
            char option = ' ';
            int id;
            bool run = true;

            while (run)
            {
                Console.WriteLine("1) Add student");
                Console.WriteLine("2) Show students");
                Console.WriteLine("3) Update student");
                Console.WriteLine("4) Delete student");
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
                        Read("SELECT * FROM students_info ORDER BY surname");
                        break;
                    case '3':
                        // Update
                        do
                        {
                            Console.Write("Enter the name of the student you want to modify: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter the surname of the student you want to modify: ");
                            string surname = Console.ReadLine();

                            int disciplineID = 0;
                            do
                            {
                                Console.Write("Enter the name of the discipline studied by the student you want to modify: ");
                                string discipline = Console.ReadLine();
                                disciplineID = Discipline.Read("SELECT * FROM disciplines WHERE name = @value", discipline);
                            } while (disciplineID == 0);

                            id = Read("SELECT * FROM students_info WHERE name = @value1 AND surname = @value2 AND discipline_id = @value3",
                                            name, surname, disciplineID);

                        } while (id == 0);
                        Update(id);
                        break;
                    case '4':
                        // Delete
                        do
                        {
                            Console.Write("Enter the name of the student you want to modify: ");
                            string name = Console.ReadLine();

                            Console.Write("Enter the surname of the student you want to modify: ");
                            string surname = Console.ReadLine();

                            int disciplineID = 0;
                            do
                            {
                                Console.Write("Enter the name of the discipline studied by the student you want to modify: ");
                                string discipline = Console.ReadLine();
                                disciplineID = Discipline.Read("SELECT * FROM disciplines WHERE name = @value", discipline);
                            } while (disciplineID == 0);

                            id = Read("SELECT * FROM students_info WHERE name = @value1 AND surname = @value2 AND discipline_id = @value3",
                                            name, surname, disciplineID);

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
            Console.WriteLine("Insert data for student:");

            Console.Write("Student's name: ");
            string name = Console.ReadLine();

            Console.Write("Student's surname: ");
            string surname = Console.ReadLine();

            Console.Write("Student's gender(m/f): ");
            string stringGender = Console.ReadLine();
            bool gender = stringGender == "m" ? true : false;

            Console.Write("Student's age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Student's average grade: ");
            double averageDegree = Convert.ToDouble(Console.ReadLine());

            int classID = 0;
            do
            {
                Console.Write("Class name: ");
                string className = Console.ReadLine();
                classID = Class.Read("SELECT * FROM classes WHERE group_name = @value", className);
            } while (classID == 0);

            int disciplineID = 0;
            do
            {
                Console.Write("Discipline studied by student: ");
                string discipline = Console.ReadLine();
                disciplineID = Discipline.Read("SELECT * FROM disciplines WHERE name = @value", discipline);
            } while (disciplineID == 0);

            string sql = "INSERT INTO students_info(name, surname, gender, age, discipline_average_grade, class_id, " +
                         "discipline_id) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7)";
            MySqlCommand insert = new MySqlCommand(sql, Program.connection);

            insert.Parameters.AddWithValue("@value1", name);
            insert.Parameters.AddWithValue("@value2", surname);
            insert.Parameters.AddWithValue("@value3", gender);
            insert.Parameters.AddWithValue("@value4", age);
            insert.Parameters.AddWithValue("@value5", averageDegree);
            insert.Parameters.AddWithValue("@value6", classID);
            insert.Parameters.AddWithValue("@value7", disciplineID);

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
                    Console.WriteLine($"Student name: {reader["name"].ToString()}");
                    Console.WriteLine($"Student surname: {reader["surname"].ToString()}");
                    if (Convert.ToBoolean(reader["gender"].ToString()) == true)
                    {
                        Console.WriteLine("Student gender: male");
                    }
                    else
                    {
                        Console.WriteLine("Student gender: female");
                    }
                    Console.WriteLine($"Student age: {reader["age"].ToString()}");
                    Console.WriteLine("Student's class:");
                    Class.Read("SELECT * FROM classes WHERE class_id = @value",
                               Convert.ToInt32(reader["class_id"].ToString()));
                    Console.WriteLine("Discipline studied by student:");
                    Discipline.Read("SELECT * FROM disciplines WHERE discipline_id = @value",
                               Convert.ToInt32(reader["discipline_id"].ToString()));
                    Console.WriteLine($"Student average grade for this discipline: {reader["discipline_average_grade"].ToString()}");
                    Console.WriteLine();
                }
            }
        }
        public static int Read(string sql, string name, string surname, int discipline_id)
        {
            MySqlCommand readStudent = new MySqlCommand(sql, Program.connection);
            readStudent.Parameters.AddWithValue("@value1", name);
            readStudent.Parameters.AddWithValue("@value2", surname);
            readStudent.Parameters.AddWithValue("@value3", discipline_id);

            using (MySqlDataReader stuentReader = readStudent.ExecuteReader())
            {
                if (stuentReader.Read())
                {
                    return Convert.ToInt32(stuentReader["student_id"].ToString());
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
                "average grade",
                "class",
                "discipline"
            };

            while (run)
            {
                string sql = "SELECT * FROM students_info";
                bool genderChanged = false;
                bool gender = false;
                int age = 0;
                double averageDegree = 0;
                int classID = 0;
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
                        Console.Write("Enter new student's name: ");
                        response = Console.ReadLine();
                        sql = $"UPDATE students_info SET name = @value WHERE student_id = {id}";
                        break;
                    case '2':
                        Console.Write("Enter new student's surname: ");
                        response = Console.ReadLine();
                        sql = $"UPDATE students_info SET surname = @value WHERE student_id = {id}";
                        break;
                    case '3':
                        genderChanged = true;
                        Console.Write("Enter new student's gender(m/f): ");
                        response = Console.ReadLine();
                        gender = response == "m" ? true : false;
                        sql = $"UPDATE students_info SET gender = @value WHERE student_id = {id}";
                        break;
                    case '4':
                        Console.Write("Enter new student's age: ");
                        age = Convert.ToInt32(Console.ReadLine());
                        sql = $"UPDATE students_info SET age = @value WHERE student_id = {id}";
                        break;
                    case '5':
                        Console.Write("Enter new student's average grade for their discipline: ");
                        averageDegree = Convert.ToDouble(Console.ReadLine());
                        sql = $"UPDATE students_info SET discipline_average_grade = @value WHERE student_id = {id}";
                        break;
                    case '6':
                        do
                        {
                            Console.Write("Enter new student's class name: ");
                            string className = Console.ReadLine();
                            classID = Class.Read("SELECT * FROM classes WHERE group_name = @value", className);
                        } while (classID == 0);
                        sql = $"UPDATE students_info SET class_id = @value WHERE student_id = {id}";
                        break;
                    case '7':
                        do
                        {
                            Console.Write("Enter new discipline studied by student: ");
                            string discipline = Console.ReadLine();
                            disciplineID = Discipline.Read("SELECT * FROM disciplines WHERE name = @value", discipline);
                        } while (disciplineID == 0);
                        sql = $"UPDATE students_info SET discipline_id = @value WHERE student_id = {id}";
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
                else if (age != 0)
                {
                    update.Parameters.AddWithValue("@value", age);
                }
                else if (averageDegree != 0)
                {
                    update.Parameters.AddWithValue("@value", averageDegree);
                }
                else if (classID != 0)
                {
                    update.Parameters.AddWithValue("@value", classID);
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
            string sql = $"DELETE FROM students_info WHERE student_id = {id}";

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
