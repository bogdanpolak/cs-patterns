using DesignPatterns.Mediator.Model;

namespace DesignPatterns.Mediator.Systems
{
    public class FieldSystem
    {
        private readonly IMediator _mediator;

        public FieldSystem(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void StartedFirstHalf()
        {
            _mediator.Game_FirstHalfStarted();
        }

        public void UpdateMatchMinute(int minute)
        {
            _mediator.UpdateDisplay(minute);
        }

        public void ScoreGoal(int minute, Player player)
        {
            _mediator.ScoreGoal(minute, player);
        }
    }
}