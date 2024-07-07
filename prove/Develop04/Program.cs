using System;
using System.Threading;
using System.Collections.Generic;

namespace MindfulnessProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Mindfulness Program");
                Console.WriteLine("1. Breathing Activity");
                Console.WriteLine("2. Reflection Activity");
                Console.WriteLine("3. Listing Activity");
                Console.WriteLine("4. Exit");
                Console.Write("Select an activity (1-4): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        new BreathingActivity().Start();
                        break;
                    case "2":
                        new ReflectionActivity().Start();
                        break;
                    case "3":
                        new ListingActivity().Start();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please select again.");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }
    }
     public abstract class Activity
    {
        protected int duration;

        protected void DisplayStartingMessage()
        {
            Console.Clear();
            Console.WriteLine($"Starting {this.GetType().Name}...");
            Console.Write("Enter the duration in seconds: ");
            duration = int.Parse(Console.ReadLine());
            Console.WriteLine("Get ready to begin...");
            Thread.Sleep(3000);
        }

        protected void DisplayEndingMessage()
        {
            Console.WriteLine("Well done!");
            Console.WriteLine($"You have completed the {this.GetType().Name} for {duration} seconds.");
            Thread.Sleep(3000);
        }

        protected void DisplayCountdown(int seconds)
        {
            DateTime endTime = DateTime.Now.AddSeconds(seconds);

            while (DateTime.Now < endTime)
            {
                TimeSpan remainingTime = endTime - DateTime.Now;
                int secondsLeft = (int)Math.Ceiling(remainingTime.TotalSeconds);

                foreach (char c in @"/|\-")
                {
                    Console.Write($"\r{c} {secondsLeft} seconds remaining");
                    Thread.Sleep(250); // 250ms to complete a full cycle every second
                }
            }
            Console.Write("\rDone!                      \n"); // Clear the line after completion
        }
    }
     public class BreathingActivity : Activity
    {
        public void Start()
        {
            DisplayStartingMessage();

            for (int i = 0; i < duration; i += 2)
            {
                Console.WriteLine("Breathe in...");
                DisplayCountdown(2);

                Console.WriteLine("Breathe out...");
                DisplayCountdown(2);
            }

            DisplayEndingMessage();
        }
    }
    public class ReflectionActivity : Activity
    {
        private List<string> prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };

        private List<string> questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };

        public void Start()
        {
            DisplayStartingMessage();

            Random random = new Random();
            string prompt = prompts[random.Next(prompts.Count)];
            Console.WriteLine(prompt);
            Console.WriteLine("Press Enter when you are ready to move on.");
            Console.ReadLine();

            DateTime endTime = DateTime.Now.AddSeconds(duration);

            while (DateTime.Now < endTime)
            {
                string question = questions[random.Next(questions.Count)];
                Console.WriteLine(question);
                DisplayCountdown(10); // Adjust the time as needed
            }

            DisplayEndingMessage();
        }
    }
    public class ListingActivity : Activity
    {
        private List<string> prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };

        public void Start()
        {
            DisplayStartingMessage();

            Random random = new Random();
            string prompt = prompts[random.Next(prompts.Count)];
            Console.WriteLine(prompt);
            DisplayCountdown(5); // Time to think about the prompt

            DateTime endTime = DateTime.Now.AddSeconds(duration);
            List<string> items = new List<string>();

            while (DateTime.Now < endTime)
            {
                Console.Write("Enter an item: ");
                string item = Console.ReadLine();
                items.Add(item);
                DisplayCountdown(1); // Show spinner between inputs
            }

            Console.WriteLine($"You listed {items.Count} items.");
            DisplayEndingMessage();
        }
    }
}