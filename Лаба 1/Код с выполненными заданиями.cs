using System;
using System.Collections.Generic;

public struct Student
{
    public string LastName;
    public string FirstName;
    public string Group;
    public int Year;
}

class Program
{
    static void Main()
    {
       
        List<Student> students = new List<Student>();
        students.Add(new Student { LastName = "ВВВВААААА", FirstName = "Влад", Group = "24ВВВ3", Year = 2000 });
        students.Add(new Student { LastName = "ВВВКУКУКУ", FirstName = "Ваня", Group = "24ВВВ3", Year = 2001 });
        students.Add(new Student { LastName = "ывываыва", FirstName = "", Group = "24ВВВ3", Year = 2000 });

        
        Console.WriteLine("Введите фамилию для поиска:");
        string searchName = Console.ReadLine();

        
        bool found = false;
        foreach (Student s in students)
        {
            if (s.LastName.Contains(searchName))
            {
                Console.WriteLine("Найден: " + s.LastName + " " + s.FirstName + ", группа " + s.Group);
                found = true;
            }
        }

       
        if (!found)
        {
            Console.WriteLine("Студент не найден");
        }


        Random rnd = new Random();
        Console.WriteLine("Размер массива:");
         
        int cols = Convert.ToInt32(Console.ReadLine());
        int rows = Convert.ToInt32(Console.ReadLine());
        int[,] matrix = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = rnd.Next(-15, 15);
            }
        }

        Console.WriteLine("Двумерный массив:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
        int sum = 0;
        foreach (int s in matrix) { 
            if(s<=0) sum +=1;
        }
        Console.WriteLine("Кол-во: " + sum);

        Console.WriteLine("Размер массива:");
        int size = Convert.ToInt32(Console.ReadLine());
        int[,] matrix_v2 = new int[size, size];

       
        for (int i = 0; i < size; i++)
        {
            for (int j = i; j < size; j++)
            {
                if (i == j)
                {
                    
                    matrix_v2[i, j] = 1;
                }
                else
                {
                    
                    int value = rnd.Next(0, 2);
                    matrix_v2[i, j] = value;
                    
                    matrix_v2[j, i] = value;
                }
            }
        }

        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write(matrix_v2[i, j] + " ");
            }
            Console.WriteLine();
        }

            Console.WriteLine("Нажмите Enter для выхода");
        Console.ReadLine();
    }
}