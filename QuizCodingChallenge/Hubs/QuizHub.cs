using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using QuizCodingChallenge.Entities;
using QuizCodingChallenge.Services.Interfaces;

namespace QuizCodingChallenge.Hubs;

public class QuizHub : Hub
{
    // In-memory store for quiz sessions
    private static ConcurrentDictionary<string, QuizSession> QuizSessions = new();
    private readonly IQuizService _quizService;

    public QuizHub(IQuizService quizService)
    {
        _quizService = quizService;
    }

    // Method to join a quiz session
    public async Task JoinQuiz(string quizId, string userId, string userName)
    {
        await _quizService.JoinQuizAsync(quizId, userId, userName);
        await Groups.AddToGroupAsync(Context.ConnectionId, quizId);
    }

    // Method to submit an answer and update score
    public async Task SubmitAnswer(string quizId, string userId, int scoreIncrement)
    {
        await _quizService.SubmitAnswerAsync(quizId, userId, scoreIncrement);
    }
}