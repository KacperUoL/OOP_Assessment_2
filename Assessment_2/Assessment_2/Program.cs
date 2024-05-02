using System;
using System.Diagnostics;
using System.Linq;

internal class Die
{
    private static readonly Random _diceRoll = new Random();
    public int CurrentRoll { get; private set; }

    public int Roll()
    {
        CurrentRoll = _diceRoll.Next(1, 7);
        return CurrentRoll;
    }
}

internal class Game
{
    public Die[] Dice { get; private set; } = new Die[3];
    public int[] Scores { get; private set; } = new int[2];

    public Game()
    {
        for (int i = 0; i < 3; i++)
        {
            Dice[i] = new Die();
        }
    }

    public int DiceRoll()
    {
        int sum = 0;

        for (int i = 0; i < 3; i++)
        {
            int roll = Dice[i].Roll();
            Console.WriteLine($"Die {i + 1} rolled: {roll}");
            sum += roll;
        }

        Console.WriteLine("--------------------------------");
        Console.WriteLine($"Sum of the rolled dice is: {sum}");

        return sum;
    }

    public void PlaySevensOut(int currentPlayer)
    {
        int sum = DiceRoll();
        if (sum == 7)
        {
            Console.WriteLine($"Player {currentPlayer + 1} wins");
            Scores[currentPlayer]++;
        }
    }

    public void PlayThreeOrMore(int currentPlayer)
    {
        int sum = DiceRoll();
        Scores[currentPlayer] += sum;
        if (Scores[currentPlayer] >= 20)
        {
            Console.WriteLine($"Player {currentPlayer + 1} wins with a score of {Scores[currentPlayer]}!");
        }
    }
}

internal class Statistics
{
    private int[] _sevensOutPlays = new int[2];
    private int[] _threeOrMorePlays = new int[2];
    private int[] _sevensOutWins = new int[2];
    private int[] _threeOrMoreWins = new int[2];

    public void UpdateSevensOutStats(int player, bool isWin)
    {
        _sevensOutPlays[player]++;
        if (isWin)
            _sevensOutWins[player]++;
    }

    public void UpdateThreeOrMoreStats(int player, bool isWin)
    {
        _threeOrMorePlays[player]++;
        if (isWin)
            _threeOrMoreWins[player]++;
    }

    public void DisplayStats()
    {
        Console.WriteLine("-----Statistics-----");
        Console.WriteLine("Sevens Out:");
        Console.WriteLine($"Player 1: {_sevensOutPlays[0]} plays, {_sevensOutWins[0]} wins");
        Console.WriteLine($"Player 2: {_sevensOutPlays[1]} plays, {_sevensOutWins[1]} wins");
        Console.WriteLine("Three or More:");
        Console.WriteLine($"Player 1: {_threeOrMorePlays[0]} plays, {_threeOrMoreWins[0]} wins");
        Console.WriteLine($"Player 2: {_threeOrMorePlays[1]} plays, {_threeOrMoreWins[1]} wins");
    }
}

internal class Testing
{
    private Game _game = new Game();

    public void DebugProgram()
    {
        // Test Die.Roll() method
        foreach (var die in _game.Dice)
        {
            int roll = die.Roll();
            Debug.Assert(roll >= 1 && roll <= 6, "Dice roll out of range");
        }

        // Test Game.DiceRoll() method
        int expectedSum = _game.DiceRoll();

        Debug.Assert(expectedSum >= 3 && expectedSum <= 18, "Sum of die rolls out of range");
    }


}

internal class Program
{
    static void Main(string[] args)
    {
        Game[] games = new Game[2];
        for (int i = 0; i < 2; i++)
        {
            games[i] = new Game();
        }

        Statistics stats = new Statistics();
        Testing testing = new Testing();

        bool gameEnded = false;

        while (!gameEnded)
        {
            int currentPlayer = 0;

            while (!gameEnded)
            {
                Console.Clear(); // clear used for user readability

                Console.WriteLine("Welcome to the Dice Game");
                Console.WriteLine("---------------------------------");
                Console.WriteLine($"Player {currentPlayer + 1}'s turn:");
                Console.WriteLine("1. Play Sevens Out");
                Console.WriteLine("2. Play Three or More");
                Console.WriteLine("3. View Player Stats");
                Console.WriteLine("4. Testing");
                Console.WriteLine("5. Exit");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input enter a number");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    continue; // Continue to beginning of loop to prompt user 
                }

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        games[currentPlayer].PlaySevensOut(currentPlayer);
                        stats.UpdateSevensOutStats(currentPlayer, games[currentPlayer].Scores[currentPlayer] > 0);
                        if (games[currentPlayer].Scores[currentPlayer] > 0)
                        {
                            Console.WriteLine($"Player {currentPlayer + 1} wins ");
                            Console.WriteLine("Press any key to return to the menu");
                            Console.ReadKey();
                            gameEnded = true;
                        }
                        break;
                    case 2:
                        Console.Clear();
                        games[currentPlayer].PlayThreeOrMore(currentPlayer);
                        stats.UpdateThreeOrMoreStats(currentPlayer, games[currentPlayer].Scores[currentPlayer] >= 20);
                        if (games[currentPlayer].Scores[currentPlayer] >= 20)
                        {
                            Console.WriteLine($"Player {currentPlayer + 1} wins ");
                            Console.WriteLine("Press any key to return to the menu");
                            Console.ReadKey();
                            gameEnded = true;
                        }
                        break;
                    case 3:
                        Console.Clear();
                        stats.DisplayStats();
                        Console.WriteLine("Press any key to return to the menu");
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.Clear();
                        testing.DebugProgram();
                        Console.WriteLine("Testing completed");
                        Console.ReadKey();
                        break;
                    case 5:
                        gameEnded = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice enter a number between 1 and 5");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                }

                if (!gameEnded)
                {
                    currentPlayer = (currentPlayer + 1) % 2; // Switch players
                }
            }

            
            gameEnded = false;
        }
    }

}
