using System;
using System.Collections.Generic;
using System.IO;

class FileHelper
{
    public static string FindFile(string folder, string name, out char delimiter)
    {
        delimiter = ',';

        if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(name))
            return null;

        string csvPath = Path.Combine(folder, name + ".csv");
        string tsvPath = Path.Combine(folder, name + ".tsv");

        if (File.Exists(csvPath))
        {
            delimiter = ',';
            return csvPath;
        }

        if (File.Exists(tsvPath))
        {
            delimiter = '\t';
            return tsvPath;
        }

        Console.WriteLine("Файл не найден, пупсик, проверь путь и имя файла");
        return null;
    }
}

class TableReader
{
    private string _filePath;
    private char _delimiter;

    public TableReader(string filePath, char delimiter)
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
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                yield return line.Split(_delimiter);

                count++;
                if (maxRows > 0 && count >= maxRows)
                    break;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Введи путь к папке с файлом:");
        string folder = Console.ReadLine();

        Console.WriteLine("Введи имя файла без .csv или .tsv:");
        string name = Console.ReadLine();

        char delimiter;
        string filePath = FileHelper.FindFile(folder, name, out delimiter);

        if (filePath == null)
            return;

        Console.WriteLine($"Файл найден: {filePath}");
        Console.WriteLine("Чтение данных...\n");

        TableReader reader = new TableReader(filePath, delimiter);

        int rowNum = 0;
        foreach (var row in reader.ReadRows(100))
        {
            rowNum++;
            Console.WriteLine($"Строка {rowNum}: {string.Join(" | ", row)}");
        }

        Console.WriteLine("\nГотово, брят");
    }
}