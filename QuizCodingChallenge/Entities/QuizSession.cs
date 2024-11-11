namespace QuizCodingChallenge.Entities;

public class QuizSession
{
    public string QuizId { get; set; }
    public List<User> Users { get; set; } = new();
    public Dictionary<string, int> Leaderboard { get; set; } = new(); // UserId to score mapping
}