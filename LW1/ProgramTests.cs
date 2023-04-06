using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using LW1;

namespace LW1Tests
{
    class ProgramTests
    {
        const string inputFileName = "InputTests.txt";
        static void Main(string[] args)
        {
            var path = Path.Combine(Environment.CurrentDirectory, inputFileName);
            string[] lines = File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                List<string> line = lines[i].Split(' ').ToList();
                string expected = line[line.Count - 1];
                line.RemoveAt(line.Count - 1);
                var arrayTesting = line.ToArray<string>();
                Program.Main(arrayTesting);
            }

            using (FileStream inputStream = File.OpenRead("InputTests.txt"))
            {

            }
        }
    }
}
