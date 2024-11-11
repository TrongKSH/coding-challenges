using Microsoft.AspNetCore.SignalR;
using QuizCodingChallenge.Entities;
using QuizCodingChallenge.Enums;
using QuizCodingChallenge.Hubs;
using QuizCodingChallenge.Services.Interfaces;

namespace QuizCodingChallenge.Services;

public class QuizService : IQuizService
{
    private readonly IHubContext<QuizHub> _hubContext;
    private readonly ILeaderboardService _leaderboardService;
    private readonly IUserService _userService;

    public QuizService(IUserService userService, ILeaderboardService leaderboardService,
        IHubContext<QuizHub> hubContext)
    {
        _userService = userService;
        _leaderboardService = leaderboardService;
        _hubContext = hubContext;
    }

    public async Task JoinQuizAsync(string quizId, string userId, string userName)
    {
        var user = new User { UserId = userId, Name = userName, Score = 0 };
        await _userService.AddUserAsync(quizId, user);

        // Notify when new usser join
        var leaderboard = _leaderboardService.GetLeaderboard(quizId);
        await _hubContext.Clients.Group(quizId).SendAsync(EventNames.UpdateLeaderboard, leaderboard);
    }

    public async Task SubmitAnswerAsync(string quizId, string userId, int scoreIncrement)
    {
        var user = _userService.GetUser(userId);
        if (user != null)
        {
            user.Score += scoreIncrement;
            await _leaderboardService.UpdateLeaderboardAsync(quizId, userId, user.Score);

            // Notify when leaderboard got updated
            await _hubContext.Clients.Group(quizId).SendAsync(EventNames.ReceiveScoreUpdate, userId, user.Score);
            var leaderboard = _leaderboardService.GetLeaderboard(quizId);
            await _hubContext.Clients.Group(quizId).SendAsync(EventNames.UpdateLeaderboard, leaderboard);
        }
    }
}