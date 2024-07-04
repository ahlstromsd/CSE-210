using System;

class Program
{
    static void Main(string[] args)
    {
        Scripture scripture = new Scripture("John 3:16", "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life.");

        while (true)
        {
            Console.Clear();
            scripture.Display();
            if (scripture.AreAllWordsHidden())
            {
                break;
            }
            
            Console.WriteLine("\nPress Enter to hide words or type 'quit' to exit.");
            string input = Console.ReadLine();
            if (input.ToLower() == "quit")
            {
                break;
            }

            scripture.HideRandomWords(3); // Hide a few words at a time
        }
    }
}
public class Reference
{
    public string Book { get; private set; }
    public int Chapter { get; private set; }
    public int StartVerse { get; private set; }
    public int EndVerse { get; private set; }
    
    public Reference(string book, int chapter, int verse)
    {
        Book = book;
        Chapter = chapter;
        StartVerse = verse;
        EndVerse = verse;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        Book = book;
        Chapter = chapter;
        StartVerse = startVerse;
        EndVerse = endVerse;
    }

    public override string ToString()
    {
        if (StartVerse == EndVerse)
        {
            return $"{Book} {Chapter}:{StartVerse}";
        }
        else
        {
            return $"{Book} {Chapter}:{StartVerse}-{EndVerse}";
        }
    }
}
public class Word
{
    public string Text { get; private set; }
    public bool IsHidden { get; private set; }
    
    public Word(string text)
    {
        Text = text;
        IsHidden = false;
    }

    public void Hide()
    {
        IsHidden = true;
    }

    public void Show()
    {
        IsHidden = false;
    }

    public override string ToString()
    {
        return IsHidden ? "____" : Text;
    }
}
public class Scripture
{
    public Reference Reference { get; private set; }
    private List<Word> Words;

    public Scripture(string reference, string text)
    {
        var parts = reference.Split(' ');
        var verses = parts[1].Split(':');
        var chapter = int.Parse(verses[0]);
        var verseRange = verses[1].Split('-');
        if (verseRange.Length == 1)
        {
            Reference = new Reference(parts[0], chapter, int.Parse(verseRange[0]));
        }
        else
        {
            Reference = new Reference(parts[0], chapter, int.Parse(verseRange[0]), int.Parse(verseRange[1]));
        }

        Words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public void Display()
    {
        Console.WriteLine(Reference);
        Console.WriteLine(string.Join(" ", Words));
    }

    public void HideRandomWords(int count)
    {
        var rand = new Random();
        for (int i = 0; i < count; i++)
        {
            var index = rand.Next(Words.Count);
            Words[index].Hide();
        }
    }

    public bool AreAllWordsHidden()
    {
        return Words.All(word => word.IsHidden);
    }
}
