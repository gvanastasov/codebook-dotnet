using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodebookMenu
{
    class Program
    {
        static void Main(string[] args)
        {
            string chaptersPath = Path.Combine(Directory.GetCurrentDirectory(), "../chapters");

            int selectedGroupIndex = 0;
            while (true)
            {
                var groups = Directory.GetDirectories(chaptersPath)
                    .Select(dir => new
                    {
                        Name = Path.GetFileName(dir),
                        Path = dir
                    })
                    .OrderBy(group => group.Name)
                    .ToArray();

                Console.Clear();
                Console.WriteLine("Welcome to the C# Codebook");
                Console.WriteLine("Use Up/Down Arrow keys to navigate and Enter to select.\n");

                selectedGroupIndex = DisplayMenu("Select a group:", groups.Select(g => g.Name).Concat(new[] { "Exit" }).ToArray(), selectedGroupIndex);

                if (selectedGroupIndex == groups.Length)
                {
                    Console.WriteLine("Goodbye!");
                    return;
                }
                else if (selectedGroupIndex == -1)
                {
                    continue;
                }

                var selectedGroup = groups[selectedGroupIndex];
                var chapters = Directory.GetDirectories(selectedGroup.Path)
                    .Select(dir => new
                    {
                        Name = Path.GetFileName(dir),
                        Path = dir
                    })
                    .OrderBy(chapter => chapter.Name)
                    .ToArray();

                int selectedChapterIndex = 0;
                selectedChapterIndex = DisplayMenu($"Select a chapter from {selectedGroup.Name}:", chapters.Select(c => c.Name).Concat(new[] { "Back" }).ToArray(), selectedChapterIndex);

                if (selectedChapterIndex == chapters.Length + 2)
                {
                    continue;
                }
                else if (selectedChapterIndex == -1)
                {
                    continue;
                }

                RunChapter(chapters[selectedChapterIndex].Path);
            }
        }

        static int DisplayMenu(string prompt, string[] options, int selectedIndex = 0)
        {
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine(prompt);
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"> {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {options[i]}");
                    }
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
                }
                else if (key == ConsoleKey.Enter)
                {
                    return selectedIndex;
                }
            } while (true);
        }

        static void RunChapter(string chapterPath)
        {
            try
            {
                Console.Clear();
                Console.WriteLine($"Running chapter: {Path.GetFileName(chapterPath)}\n");

                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"run --project \"{chapterPath}\"",
                        UseShellExecute = true,
                        CreateNoWindow = true,
                    }
                };

                process.Start();

                using (StreamReader reader = process.StandardOutput)
                {
                    while (!reader.EndOfStream)
                    {
                        Console.WriteLine(reader.ReadLine());
                    }
                }

                process.WaitForExit();

                Console.WriteLine("\nChapter process has exited. Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}