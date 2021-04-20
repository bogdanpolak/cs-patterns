using System;
using System.Threading;
using DesignPatterns.Mediator.Model;

namespace DesignPatterns.Mediator.Systems
{
    public class DisplaySystem
    {
        private readonly  IMediator _mediator;
        private Game _game;

        public DisplaySystem(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void ShowWelcomeScreen(Game game)
        {
            _game = game;
            Console.WriteLine($"Soccer match: {_game.TeamA.Name} - {_game.TeamB.Name}");
            Console.WriteLine($"... Showing welcome screen");
            Thread.Sleep(100);
            _mediator.WelcomeCompleted();
        }

        public void SetTimer(int i)
        {
            ShowStatus(i);
        }

        public void ShowStatus(int minute)
        {
            if (minute%5 != 0) return;
            Console.WriteLine($"   {minute}:00 | " + 
                              $"{_game.ResultA}" + 
                              $" [{_game.TeamA.Abbreviation}]-[{_game.TeamB.Abbreviation}] " + 
                              $"{_game.ResultB}");
        }

        public void ScoreGoal(int minute, Player player)
        {
            _game.ScoreGoalForTeam(player.Team);
            Console.WriteLine($"   GOAL for {player.Team.Name}!!! {player.FirstName} {player.LastName} scoring {minute}:00 ");
        }
    }
}