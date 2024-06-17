using System;

class Program
{
    static void Main(string[] args)
    {
        Random randomGenerator = new Random();
        int magicNumber = randomGenerator.Next(1,101);

        int guess = -1;

        while (guess!= magicNumber)
        {
            Console.WriteLine("what is your guess?");
            guess = int.Parse(Console.ReadLine());

            if (magicNumber > guess)
            {
                Console.WriteLine("Too low!");
            }
            else if (magicNumber < guess)
            {
                Console.WriteLine("Too high!");
            }
            else
            {
                Console.WriteLine("You guessed it!");
            }
        }
        // Console.WriteLine("What is the magic number?");
        // string magic_number = Console.ReadLine();
        // int number = int.Parse(magic_number);
        // Console.WriteLine("What is your guess?");
        // string guess = Console.ReadLine();
        // int guess1 = int.Parse(guess);
        // while (guess1 != number)
        // {
        // if (guess1 == number)
        // {
        //     Console.WriteLine("That's correct!");       
        // }
        // else if (guess1 > number)
        // {
        //     Console.WriteLine("Too high!");
        // }
        // else if (guess1 < number)
        // {
        //     Console.WriteLine("Too low!");
        // }
        // }
    }
}