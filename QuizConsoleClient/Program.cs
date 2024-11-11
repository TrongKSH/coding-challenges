// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.SignalR.Client;

namespace QuizConsoleClient;

public class Program
{
    private static HubConnection connection;
    private static string quizId;
    private static string userId;
    private static string userName;

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Enter Quiz ID:");
        quizId = Console.ReadLine();
        Console.WriteLine("Enter Your User Name:");
        userName = Console.ReadLine();
        userId = $"{userName}-{DateTime.Now.Ticks}";

        // Initialize SignalR connection
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/quizHub")
            .Build();

        // Setup event handlers for real-time updates
        connection.On<string, int>("ReceiveScoreUpdate",
            (user, score) => { Console.WriteLine($"[Score Update] {user} - New Score: {score}"); });
        connection.On<Dictionary<string, int>>("UpdateLeaderboard", DisplayLeaderboard);

        await StartConnection();

        await JoinQuiz();
        await ShowMenu();
    }

    // Start SignalR connection
    private static async Task StartConnection()
    {
        try
        {
            await connection.StartAsync();
            Console.WriteLine("Connected to SignalR Hub.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection error: {ex.Message}");
        }
    }

    // Join quiz session
    private static async Task JoinQuiz()
    {
        await connection.InvokeAsync("JoinQuiz", quizId, userId, userName);
        Console.WriteLine($"Joined quiz: {quizId} as {userName}");
    }

    // Menu to interact with the quiz
    private static async Task ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Submit Answer (Increase Score)");
            Console.WriteLine("2. Exit");

            var choice = Console.ReadLine();
            if (choice == "1")
            {
                await SubmitAnswer();
            }
            else if (choice == "2")
            {
                await connection.DisposeAsync();
                Console.WriteLine("Disconnected.");
                break;
            }
        }
    }

    // Submit answer and trigger score update
    private static async Task SubmitAnswer()
    {
        const int scoreIncrement = 10;
        await connection.InvokeAsync("SubmitAnswer", quizId, userId, scoreIncrement);
        Console.WriteLine("Answer submitted! Score incremented by 10.");
    }

    // Display updated leaderboard
    private static void DisplayLeaderboard(Dictionary<string, int> leaderboard)
    {
        Console.WriteLine("\n[Leaderboard Update]");
        //Leaderboard ordering
        if (leaderboard.Count <= 0) return;
        foreach (var entry in leaderboard.OrderByDescending(x => x.Value))
            Console.WriteLine($"{entry.Key}: {entry.Value} points");
    }
}