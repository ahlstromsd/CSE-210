using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.IO;

namespace JournalProgram
{
    class JournalEntry
    {
        public string Date { get; set; }
        public string Prompt { get; set; }
        public string Response { get; set; }

        public JournalEntry(string prompt, string response)
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd");
            Prompt = prompt;
            Response = response;
        }

        public override string ToString()
        {
            return $"{Date} | {Prompt} | {Response}";
        }

        public string ToCsvString()
        {
            return $"{Date},{Prompt.Replace(",", ";")},{Response.Replace(",", ";")}";
        }
    }

    class Journal
    {
        private List<JournalEntry> entries;
        private List<string> prompts;

        public Journal()
        {
            entries = new List<JournalEntry>();
            prompts = new List<string>
            {
                "Who was the most interesting person I interacted with today?",
                "What was the best part of my day?",
                "How did I see the hand of the Lord in my life today?",
                "What was the strongest emotion I felt today?",
                "If I had one thing I could do over today, what would it be?"
            };
        }

        public void AddEntry()
        {
            Random random = new Random();
            string prompt = prompts[random.Next(prompts.Count)];
            Console.WriteLine(prompt);
            string response = Console.ReadLine();
            JournalEntry entry = new JournalEntry(prompt, response);
            entries.Add(entry);
        }

        public void DisplayEntries()
        {
            foreach (var entry in entries)
            {
                Console.WriteLine(entry.ToString());
            }
        }

        public void SaveToFile(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var entry in entries)
                {
                    writer.WriteLine(entry.ToCsvString());
                }
            }
        }

        public void LoadFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                entries.Clear();
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string date = parts[0];
                            string prompt = parts[1].Replace(";", ",");
                            string response = parts[2].Replace(";", ",");
                            JournalEntry entry = new JournalEntry(prompt, response) { Date = date };
                            entries.Add(entry);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Journal journal = new Journal();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Write a new entry");
                Console.WriteLine("2. Display the journal");
                Console.WriteLine("3. Save the journal to a file");
                Console.WriteLine("4. Load the journal from a file");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        journal.AddEntry();
                        break;
                    case "2":
                        journal.DisplayEntries();
                        break;
                    case "3":
                        Console.Write("Enter filename to save: ");
                        string saveFile = Console.ReadLine();
                        journal.SaveToFile(saveFile);
                        break;
                    case "4":
                        Console.Write("Enter filename to load: ");
                        string loadFile = Console.ReadLine();
                        journal.LoadFromFile(loadFile);
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}