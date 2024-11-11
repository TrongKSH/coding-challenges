using QuizCodingChallenge.Entities;

namespace QuizCodingChallenge.Services.Interfaces;

public interface IUserService
{
    User GetUser(string userId);
    Task AddUserAsync(string quizId, User user);
}