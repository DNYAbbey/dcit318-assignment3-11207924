using System;
using System.Collections.Generic;
using System.IO;

// Custom Exceptions
public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message) : base(message) { }
}

public class MissingFieldException : Exception
{
    public MissingFieldException(string message) : base(message) { }
}

// Student Class (Primary Constructor)
public class Student(int id, string fullName, int score)
{
    public int Id { get; set; } = id;
    public string FullName { get; set; } = fullName;
    public int Score { get; set; } = score;

    public string GetGrade()
    {
        return Score switch
        {
            >= 80 and <= 100 => "A",
            >= 70 and <= 79 => "B",
            >= 60 and <= 69 => "C",
            >= 50 and <= 59 => "D",
            _ => "F"
        };
    }
}

// StudentResultProcessor Class
public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        var students = new List<Student>();

        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            string? line;
            int lineNumber = 1;

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');

                if (parts.Length != 3)
                {
                    throw new MissingFieldException($"Line {lineNumber}: Missing required fields.");
                }

                if (!int.TryParse(parts[0], out int id))
                {
                    throw new FormatException($"Line {lineNumber}: Invalid ID format.");
                }

                string fullName = parts[1].Trim();

                if (!int.TryParse(parts[2], out int score))
                {
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Score format is invalid.");
                }

                students.Add(new Student(id, fullName, score));
                lineNumber++;
            }
        }

        return students;
    }

    public void WriteReportToFile(List<Student> students, string outputFilePath)
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (var student in students)
            {
                writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
            }
        }
    }
}

// Main Program
class Program
{
    static void Main()
    {
        var processor = new StudentResultProcessor();
        string inputFile = Path.Combine(AppContext.BaseDirectory, "Data", "students.txt");

        string outputFile = Path.Combine(AppContext.BaseDirectory, "Data", "report.txt");


        try
        {
            var students = processor.ReadStudentsFromFile(inputFile);
            processor.WriteReportToFile(students, outputFile);
            Console.WriteLine($"Report generated successfully in '{outputFile}'");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Error: The input file was not found.");
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
