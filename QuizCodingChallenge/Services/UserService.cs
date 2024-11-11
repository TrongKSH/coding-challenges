using System.Collections.Concurrent;
using QuizCodingChallenge.Entities;
using QuizCodingChallenge.Services.Interfaces;

namespace QuizCodingChallenge.Services;

public class UserService : IUserService
{
    private static readonly ConcurrentDictionary<string, QuizSession> _quizSessions = new();

    public User GetUser(string userId)
    {
        return _quizSessions.Values.SelectMany(s => s.Users).FirstOrDefault(u => u.UserId == userId);
    }

    public async Task AddUserAsync(string quizId, User user)
    {
        var session = _quizSessions.GetOrAdd(quizId, new QuizSession { QuizId = quizId });
        if (!session.Users.Any(u => u.UserId == user.UserId))
        {
            session.Users.Add(user);
            session.Leaderboard[user.UserId] = 0;
        }
    }
}