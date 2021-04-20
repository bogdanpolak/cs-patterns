using DesignPatterns.Mediator.Model;
using DesignPatterns.Mediator.Systems;

namespace DesignPatterns.Mediator
{
    public class Mediator : IMediator
    {
        private readonly DisplaySystem _displaySystem;
        private readonly FieldSystem _fieldSystem;
        private readonly Scheduler _scheduler;

        public Mediator()
        {
            var mediator = this;
            _displaySystem = new DisplaySystem(mediator);
            _fieldSystem = new FieldSystem(mediator);
            _scheduler = new Scheduler(mediator);
        }

        public Scheduler GetScheduler() => _scheduler;
        public FieldSystem GetFieldSystem() => _fieldSystem;

        public void StartTransmission(Game game)
        {
            _displaySystem.ShowWelcomeScreen(game);
        }
        
        public void WelcomeCompleted()
        {
            
        }

        public void Game_FirstHalfStarted()
        {
            _displaySystem.SetTimer(0);
        }

        public void UpdateDisplay(int minute)
        {
            _displaySystem.ShowStatus(minute);
        }

        public void ScoreGoal(int minute, Player player)
        {
            _displaySystem.ScoreGoal(minute, player);
        }
    }
}