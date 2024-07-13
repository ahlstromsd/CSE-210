using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// Base Goal class
public abstract class Goal
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Points { get; private set; }
    public bool IsComplete { get; private set; }

    public Goal(string name, string description, int points)
    {
        Name = name;
        Description = description;
        Points = points;
        IsComplete = false;
    }

    public virtual void RecordEvent(User user)
    {
        // This will be overridden in derived classes
    }

    public void MarkComplete()
    {
        IsComplete = true;
    }

    public override string ToString()
    {
        return $"{Name}: {Description} (Points: {Points}) - {(IsComplete ? "[X]" : "[ ]")}";
    }
}

// SimpleGoal class
public class SimpleGoal : Goal
{
    public SimpleGoal(string name, string description, int points)
        : base(name, description, points) { }

    public override void RecordEvent(User user)
    {
        if (!IsComplete)
        {
            user.UpdateScore(Points);
            MarkComplete();
        }
    }
}

// EternalGoal class
public class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points)
        : base(name, description, points) { }

    public override void RecordEvent(User user)
    {
        user.UpdateScore(Points);
    }
}

// ChecklistGoal class
public class ChecklistGoal : Goal
{
    public int RequiredCount { get; private set; }
    public int CurrentCount { get; private set; }
    public int BonusPoints { get; private set; }

    public ChecklistGoal(string name, string description, int points, int requiredCount, int bonusPoints)
        : base(name, description, points)
    {
        RequiredCount = requiredCount;
        CurrentCount = 0;
        BonusPoints = bonusPoints;
    }

    public override void RecordEvent(User user)
    {
        if (!IsComplete)
        {
            CurrentCount++;
            user.UpdateScore(Points);
            if (CurrentCount >= RequiredCount)
            {
                user.UpdateScore(BonusPoints);
                MarkComplete();
            }
        }
    }

    public string CheckProgress()
    {
        return $"{Name}: Completed {CurrentCount}/{RequiredCount} times";
    }

    public override string ToString()
    {
        return base.ToString() + $" - {CheckProgress()}";
    }
}

// User class
public class User
{
    public List<Goal> Goals { get; private set; }
    public int Score { get; private set; }
    public int Level { get; private set; }
    public int PointsForNextLevel { get; private set; }

    public User()
    {
        Goals = new List<Goal>();
        Score = 0;
        Level = 1;
        PointsForNextLevel = 1000;
    }

    public void AddGoal(Goal goal)
    {
        Goals.Add(goal);
    }

    public void RecordEvent(string goalName)
    {
        foreach (var goal in Goals)
        {
            if (goal.Name.Equals(goalName, StringComparison.OrdinalIgnoreCase))
            {
                goal.RecordEvent(this);
                CheckLevelUp();
                break;
            }
        }
    }

    public void DisplayGoals()
    {
        foreach (var goal in Goals)
        {
            Console.WriteLine(goal);
        }
    }

    public void SaveData(string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(this, options);
        File.WriteAllText(filePath, jsonString);
    }

    public static User LoadData(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<User>(jsonString);
    }

    public void CheckLevelUp()
    {
        if (Score >= PointsForNextLevel)
        {
            Level++;
            PointsForNextLevel += 1000; // Increase the threshold for the next level
            Console.WriteLine($"Congratulations! You've leveled up to Level {Level}!");
        }
    }

    public void UpdateScore(int points)
    {
        Score += points;
    }
}

// Main Program
class Program
{
    static void Main(string[] args)
    {
        User user = new User();
        string filePath = "user_data.json";
        
        if (File.Exists(filePath))
        {
            user = User.LoadData(filePath);
        }

        bool running = true;
        while (running)
        {
            Console.WriteLine("Eternal Quest Program");
            Console.WriteLine("1. Add New Goal");
            Console.WriteLine("2. Record Event");
            Console.WriteLine("3. Display Goals");
            Console.WriteLine("4. Save Data");
            Console.WriteLine("5. Load Data");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddNewGoal(user);
                    break;
                case "2":
                    RecordEvent(user);
                    break;
                case "3":
                    user.DisplayGoals();
                    break;
                case "4":
                    user.SaveData(filePath);
                    break;
                case "5":
                    user = User.LoadData(filePath);
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void AddNewGoal(User user)
    {
        Console.WriteLine("Select Goal Type:");
        Console.WriteLine("1. Simple Goal");
        Console.WriteLine("2. Eternal Goal");
        Console.WriteLine("3. Checklist Goal");
        string goalType = Console.ReadLine();

        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();
        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();
        Console.Write("Enter goal points: ");
        int points = int.Parse(Console.ReadLine());

        switch (goalType)
        {
            case "1":
                user.AddGoal(new SimpleGoal(name, description, points));
                break;
            case "2":
                user.AddGoal(new EternalGoal(name, description, points));
                break;
            case "3":
                Console.Write("Enter required count: ");
                int requiredCount = int.Parse(Console.ReadLine());
                Console.Write("Enter bonus points: ");
                int bonusPoints = int.Parse(Console.ReadLine());
                user.AddGoal(new ChecklistGoal(name, description, points, requiredCount, bonusPoints));
                break;
            default:
                Console.WriteLine("Invalid goal type. Please try again.");
                break;
        }
    }

    static void RecordEvent(User user)
    {
        Console.Write("Enter goal name to record event: ");
        string goalName = Console.ReadLine();
        user.RecordEvent(goalName);
    }
}