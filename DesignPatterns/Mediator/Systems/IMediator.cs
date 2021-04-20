using DesignPatterns.Mediator.Model;

namespace DesignPatterns.Mediator.Systems
{
    public interface IMediator
    {
        void StartTransmission(Game game);
        void WelcomeCompleted();
        void Game_FirstHalfStarted();
        void UpdateDisplay(int minute);
        void ScoreGoal(int minute, Player player);
    }
}