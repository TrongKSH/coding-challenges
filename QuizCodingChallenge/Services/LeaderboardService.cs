using System.Collections.Concurrent;
using QuizCodingChallenge.Entities;
using QuizCodingChallenge.Services.Interfaces;

namespace QuizCodingChallenge.Services;

public class LeaderboardService : ILeaderboardService
{
    private static readonly ConcurrentDictionary<string, QuizSession> _quizSessions = new();

    public async Task UpdateLeaderboardAsync(string quizId, string userId, int score)
    {
        if (_quizSessions.TryGetValue(quizId, out var session))
        {
            session.Leaderboard[userId] = score;
        }
        else
        {
            var newSession = new QuizSession { QuizId = quizId };
            newSession.Leaderboard[userId] = score;
            _quizSessions[quizId] = newSession;
        }
    }

    public Dictionary<string, int> GetLeaderboard(string quizId)
    {
        return _quizSessions.TryGetValue(quizId, out var session) ? session.Leaderboard : new Dictionary<string, int>();
    }
}