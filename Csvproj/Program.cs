using System;
using System.Collections.Generic;
using System.IO;

class FileHelper
{
    public static string FindFile(string folder, string name)
    {
        if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(name))
            return null;

        string path = Path.Combine(folder, name + ".csv");
        if (File.Exists(path))
            return path;

        Console.WriteLine("Файл не найден, пупсик. Попробуй другой путь");
        return null;
    }
}

class TableReader
{
    private string _filePath;
    private char _delimiter;

    public TableReader(string filePath, char delimiter = ',')
    {
        _filePath = filePath;
        _delimiter = delimiter;
    }

    public IEnumerable<string[]> ReadRows(int maxRows = 0)
    {
        if (_filePath == null) yield break;

        using (var reader = new StreamReader(_filePath))
        {
            string line;
            int count = 0;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                yield return line.Split(_delimiter);

                count++;
                if (maxRows > 0 && count >= maxRows) break;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        string folder = @""; // вставь сюда путь 
        string name = ""; // название файла. Если хотите открыть свой файл, то напишите название тут и в папек в качестве пути укажите .csv

        string filePath = FileHelper.FindFile(folder, name);
        if (filePath == null) return;

        TableReader reader = new TableReader(filePath);

        int rowNum = 0;
        foreach (var row in reader.ReadRows(100))
        {
            rowNum++;
            Console.WriteLine($"Строка {rowNum}: {string.Join(" | ", row)}");
        }

        Console.WriteLine("Готово!");
    }
}


