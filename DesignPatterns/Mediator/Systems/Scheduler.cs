using DesignPatterns.Mediator.Model;

namespace DesignPatterns.Mediator.Systems
{
    public class Scheduler
    {
        private readonly IMediator _mediator;

        public Scheduler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void StartTransmission(Game game)
        {
            _mediator.StartTransmission(game);
        }
    }
}