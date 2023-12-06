using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

class Teacher
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Class { get; set; }
    public string Section { get; set; }
}

class TeacherManagement
{
    private string filePath;
    private string schoolName;

    public TeacherManagement(string filePath)
    {
        this.filePath = filePath;

        
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                schoolName = lines[0].Trim();
            }
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }

    public void AddTeacher()
    {
        Console.WriteLine("Enter Teacher Details:");

        
        Console.Write("ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Name: ");
        string name = Console.ReadLine();

        Console.Write("Class: ");
        string className = Console.ReadLine();

        Console.Write("Section: ");
        string section = Console.ReadLine();

       
        Teacher newTeacher = new Teacher
        {
            ID = id,
            Name = name,
            Class = className,
            Section = section
        };

        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine($"{newTeacher.ID} {newTeacher.Name} {newTeacher.Class} {newTeacher.Section}");
        }

        Console.WriteLine("Teacher added successfully!");
    }

    public void ViewTeachers()
    {
        if (File.Exists(filePath))
        {
            Console.WriteLine($"School Name: {schoolName}");
            Console.WriteLine("Teacher List:");

            
            bool headerSkipped = false;

            
            List<string[]> teacherDetails = new List<string[]>();

            foreach (string line in File.ReadLines(filePath))
            {
                if (!headerSkipped)
                {
                    headerSkipped = true;
                    continue; 
                }

               
                string[] fields = Regex.Split(line.Trim(), @"\s+");

                
                if (fields.Length >= 4)
                {
                    int id;
                    if (int.TryParse(fields[0], out id))
                    {
                        string name = fields[1];
                        string className = fields[2];
                        string section = fields[3];

                        
                        teacherDetails.Add(new string[] { id.ToString(), name, className, section });
                    }
                    
                }
                
            }

            
            PrintFormattedRow("ID", "Name", "Class", "Section");

            
            foreach (string[] details in teacherDetails)
            {
                PrintFormattedRow(details[0], details[1], details[2], details[3]);
            }
        }
        else
        {
            Console.WriteLine("No teachers found.");
        }
    }

    
    private void PrintFormattedRow(params string[] columns)
    {
        Console.WriteLine($"{columns[0],-5} {columns[1],-10} {columns[2],-10} {columns[3],-10}");
    }


    public void UpdateTeacher()
    {
        Console.Write("Enter the ID of the teacher to update: ");

        
        if (!int.TryParse(Console.ReadLine(), out int targetID))
        {
            Console.WriteLine("Invalid ID format. Please enter a valid integer.");
            return;
        }

        List<string> updatedTeachers = new List<string>();

       
        foreach (string line in File.ReadLines(filePath))
        {
            
            string[] fields = Regex.Split(line.Trim(), @"\s+");

            
            if (fields.Length >= 4 && int.TryParse(fields[0], out int currentID) && currentID == targetID)
            {
                Console.WriteLine("Enter new details for the teacher:");

                Console.Write("Name: ");
                fields[1] = Console.ReadLine();

                Console.Write("Class: ");
                fields[2] = Console.ReadLine();

                Console.Write("Section: ");
                fields[3] = Console.ReadLine();
            }

            updatedTeachers.Add(string.Join(" ", fields.Select(f => f.ToString())));
        }

        
        File.WriteAllLines(filePath, updatedTeachers);

        Console.WriteLine("Teacher updated successfully!");
    }

    static void Main()
    {
        string filePath = "C:\\Mphasis dot net\\Phase end Project 2\\Sort and Updating Teachers Name\\Teacherdata.txt";
        TeacherManagement teacherManagement = new TeacherManagement(filePath);

        // Display menu
        while (true)
        {
            Console.WriteLine("1. Add Teacher");
            Console.WriteLine("2. View Teachers");
            Console.WriteLine("3. Update Teacher");
            Console.WriteLine("4. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    teacherManagement.AddTeacher();
                    break;

                case "2":
                    teacherManagement.ViewTeachers();
                    break;

                case "3":
                    teacherManagement.UpdateTeacher();
                    break;

                case "4":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine();
        }
    }
}
