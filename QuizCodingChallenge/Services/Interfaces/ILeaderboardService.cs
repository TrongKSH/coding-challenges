namespace QuizCodingChallenge.Services.Interfaces;

public interface ILeaderboardService
{
    Task UpdateLeaderboardAsync(string quizId, string userId, int score);
    Dictionary<string, int> GetLeaderboard(string quizId);
}