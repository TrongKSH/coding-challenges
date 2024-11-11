namespace QuizCodingChallenge.Services.Interfaces;

public interface IQuizService
{
    Task JoinQuizAsync(string quizId, string userId, string userName);
    Task SubmitAnswerAsync(string quizId, string userId, int scoreIncrement);
}