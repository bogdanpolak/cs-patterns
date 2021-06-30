using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DesignPatterns.Mediator
{
    public static class MediatorDemo
    {
        public static void RunDemo()
        {
            var italianTeam = TeamBuilder.ItalianTeam();
            var polishTeam =TeamBuilder.PolishTeam();
            var game = new Game { TeamA = italianTeam, TeamB = polishTeam}; 

            var mediator = new Mediator();
            var _ = new DisplaySystem(mediator);
            var fieldSystem = new FieldSystem(mediator);
            var scheduler = new Scheduler(mediator);
            
            scheduler.StartTransmission(game);
            fieldSystem.StartFirstHalf();
            var playerLorenzoInsane = italianTeam.PlayerByNumber(10);
            var playerPiotrZieliński = polishTeam.PlayerByNumber(20);
            IntRange.Gen(1, 45).ForEach(minute => { 
                switch (minute)
                {
                    case (10): fieldSystem.ScoreGoal(playerLorenzoInsane); break;
                    case (25): fieldSystem.ScoreGoal(playerPiotrZieliński); break;
                    default: fieldSystem.MatchMinute(minute); break;
                }
            });
        }
    }

    // --------------------------------------------------------------------
    // Model
    // --------------------------------------------------------------------

    public enum Position
    {
        Goalkeeper,
        Defence,
        MidField,
        Forward
    }
    
    public class Player
    {
        public int Number;
        public Position Position;
        public string FirstName;
        public string LastName;
        public Team Team;
    }

    public class Team
    {
        public string Name;
        public string Abbreviation;
        private readonly List<Player> _players = new List<Player>();
            
        public Team TeamName(string name, string abbreviation) 
        {
            Name = name;
            Abbreviation = abbreviation;
            return this;
        }
        public Team AddPlayer(int number, Position position, string firstName, string lastName)
        {
            var player = new Player
            {
                Number = number,
                Position = position,
                FirstName = firstName, LastName = lastName, 
                Team = this
            };
            _players.Add(player);
            return this;
        }

        public Player PlayerByNumber(int number) => _players.FirstOrDefault(pl => pl.Number == number);
    }

    public class Game
    {
        public int ResultA;
        public int ResultB;
        public Team TeamA;
        public Team TeamB;

        public void ScoreGoalForTeam(Team team)
        {
            if (team == TeamA)
            {
                ResultA++;
            }
            else
            {
                ResultB++;
            }
        }
    }
    // --------------------------------------------------------------------
    // Mediator Contracts
    // --------------------------------------------------------------------

    public interface IMediator
    {
        void RegisterScheduler(IScheduler scheduler);
        void RegisterFieldSystem(IFieldSystem fieldSystem);
        void RegisterDisplaySystem(IDisplaySystem displaySystem);
        void StartTransmission(Game game);
        void WelcomeCompleted();
        void Game_FirstHalfStarted();
        void UpdateDisplay(int minute);
        void ScoreGoal(int minute, Player player);
    }
    
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public interface IScheduler
    {
        void StartTransmission(Game game);
        bool IsActive();
    }

    public interface IFieldSystem
    {
        void StartFirstHalf();
        void MatchMinute(int minute);
        void ScoreGoal(Player player);
        bool IsConnected();
    }

    public interface IDisplaySystem
    {
        void ShowWelcomeScreen(Game game);
        void SetTimer(int i);
        void ShowStatus(int minute);
        void ScoreGoal(int minute, Player player);
    }

    // --------------------------------------------------------------------
    // Mediator Colleagues
    //   (systems which are communicating between themself using mediator)
    // --------------------------------------------------------------------
    
    public class Scheduler : IScheduler
    {
        private readonly IMediator _mediator;

        public Scheduler(IMediator mediator)
        {
            _mediator = mediator;
            _mediator.RegisterScheduler(this);
        }
        public void StartTransmission(Game game)
        {
            _mediator.StartTransmission(game);
        }

        public bool IsActive() => true;
    }

    public class FieldSystem : IFieldSystem
    {
        private readonly IMediator _mediator;
        private int _matchMinute;

        public FieldSystem(IMediator mediator)
        {
            _mediator = mediator;
            _mediator.RegisterFieldSystem(this);
        }

        public void StartFirstHalf()
        {
            _mediator.Game_FirstHalfStarted();
            _matchMinute = 0;
        }
        
        //TODO: Add EndFirstHalf()
        //TODO: Add StartSecondHalf()

        public void MatchMinute(int minute)
        {
            _matchMinute = minute;
            _mediator.UpdateDisplay(minute);
        }

        public void ScoreGoal(Player player)
        {
            _mediator.ScoreGoal(_matchMinute, player);
        }

        public bool IsConnected() => true;
    }

    public class DisplaySystem : IDisplaySystem
    {
        private readonly  IMediator _mediator;
        private Game _game;

        public DisplaySystem(IMediator mediator)
        {
            _mediator = mediator;
            _mediator.RegisterDisplaySystem(this);
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

    // --------------------------------------------------------------------
    // Concrete Mediator
    // --------------------------------------------------------------------

    public class Mediator : IMediator
    {
        private readonly List<IDisplaySystem> _displaySystems = new List<IDisplaySystem>();
        private readonly List<IFieldSystem> _fieldSystems = new List<IFieldSystem>();
        private readonly List<IScheduler> _schedulers = new List<IScheduler>();

        public void RegisterScheduler(IScheduler scheduler)
        {
            _schedulers.Add(scheduler);
        }

        public void RegisterFieldSystem(IFieldSystem fieldSystem)
        {
            _fieldSystems.Add(fieldSystem);
        }

        public void RegisterDisplaySystem(IDisplaySystem displaySystem)
        {
            _displaySystems.Add(displaySystem);
        }

        public bool HasAllSystemOnline()
            => _schedulers.All(scheduler => scheduler.IsActive())
                && _fieldSystems.All(fieldSystem => fieldSystem.IsConnected());
        
        
        public void StartTransmission(Game game)
        {
            _displaySystems.ForEach(
                displaySystem => displaySystem.ShowWelcomeScreen(game));
        }
        
        public void WelcomeCompleted()
        {
            
        }

        public void Game_FirstHalfStarted()
        {
            _displaySystems.ForEach(
                displaySystem => displaySystem.SetTimer(0));
        }

        public void UpdateDisplay(int minute)
        {
            _displaySystems.ForEach(
                displaySystem => displaySystem.ShowStatus(minute));
        }

        public void ScoreGoal(int minute, Player player)
        {
            _displaySystems.ForEach(
                displaySystem => displaySystem.ScoreGoal(minute, player));
        }
    }

}