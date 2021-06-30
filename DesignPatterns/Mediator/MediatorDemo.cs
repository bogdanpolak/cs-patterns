using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Mediator
{
    public static class MediatorDemo
    {
        public static void RunDemo()
        {
            var teamItalian = TeamBuilder.ItalianTeam();
            var teamPolish = TeamBuilder.PolishTeam();
            var game = new Game { TeamA = teamItalian, TeamB = teamPolish}; 
            var playerLorenzoInsane = teamItalian.PlayerByNumber(10);
            var playerMatteoPessina = teamItalian.PlayerByNumber(8);
            var playerPiotrZieliński = teamPolish.PlayerByNumber(20);

            var mediator = new Mediator();
            var _ = new DisplaySystem(mediator, game);
            IFieldSystem fieldSystem = new FieldSystem(mediator, game);
            IScheduler scheduler = new Scheduler(mediator);
            
            //TODO: Extract code into MatchSimulator (mediator system)
            scheduler.StartTransmission();
            fieldSystem.StartFirstHalf();
            IntRange.Gen(1, 45).ForEach(minute => {
                fieldSystem.MatchMinute(minute);
                switch (minute)
                {
                    case (11): fieldSystem.ScoreGoal(playerLorenzoInsane); break;
                    case (26): fieldSystem.ScoreGoal(playerPiotrZieliński); break;
                    case (41): fieldSystem.ScoreGoal(playerMatteoPessina); break;
                }
            });
            fieldSystem.EndFirstHalf();
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
        public Team TeamA;
        public Team TeamB;
        private readonly List<Tuple<int, int>> _goalsTeamA = new List<Tuple<int, int>>();
        private readonly List<Tuple<int, int>> _goalsTeamB = new List<Tuple<int, int>>();

        public int ResultA => _goalsTeamA.Count;
        public int ResultB => _goalsTeamB.Count;

        public string GetResult => $"{ResultA} [{TeamA.Abbreviation}]"+
                                   $" [{TeamB.Abbreviation}] {ResultB}";

        public string GetGoals()
        {
            var goals = new List<string>();
            goals.AddRange(_goalsTeamA.Select(g =>
                $"{g.Item1:000} {TeamA.PlayerByNumber(g.Item2).LastName} ({g.Item1}\")")
            );
            goals.AddRange(_goalsTeamB.Select(g =>
                $"{g.Item1:000} {TeamB.PlayerByNumber(g.Item2).LastName} ({g.Item1}\")")
            );
            goals.Sort();
            return string.Join(", ", goals.Select(s => s.Substring(4)));
        } 

        public void ScoreGoal(int minute, Player player)
        {
            if (player.Team == TeamA)
            {
                _goalsTeamA.Add(new Tuple<int, int>(minute,player.Number));
            }
            else
            {
                _goalsTeamB.Add(new Tuple<int, int>(minute,player.Number));
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
        void StartTransmission();
        void Game_FirstHalfStarted();
        void UpdateDisplay(int minute);
        void ScoreGoal(int minute, Player player);
        void Game_FirstHalfEnded();
        void TransmissionStopped();
    }
    
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public interface IScheduler
    {
        void StartTransmission();
        bool IsActive();
        void ReadyToStopTransmission();
    }

    public interface IFieldSystem
    {
        void StartFirstHalf();
        void MatchMinute(int minute);
        void ScoreGoal(Player player);
        bool IsConnected();
        void EndFirstHalf();
    }

    public interface IDisplaySystem
    {
        void ShowWelcomeScreen();
        void SetTimer(int i);
        void ShowStatus(int minute);
        void ScoreGoal(int minute, Player player);
        void FirstHalfFinished();
        void TransmissionStopped();
        void TransmissionStarted();
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
        public void StartTransmission()
        {
            _mediator.StartTransmission();
        }

        public bool IsActive() => true;
        
        public void ReadyToStopTransmission()
        {
            _mediator.TransmissionStopped();
        }
    }

    public class FieldSystem : IFieldSystem
    {
        private readonly IMediator _mediator;
        private readonly Game _game;
        private int _matchMinute;

        public FieldSystem(IMediator mediator, Game game)
        {
            _mediator = mediator;
            _game = game;
            _mediator.RegisterFieldSystem(this);
        }

        public void StartFirstHalf()
        {
            _mediator.Game_FirstHalfStarted();
            _matchMinute = 0;
        }

        public void EndFirstHalf()
        {
            _mediator.Game_FirstHalfEnded();
        }
        
        //TODO: Add StartSecondHalf()

        public void MatchMinute(int minute)
        {
            _matchMinute = minute;
            _mediator.UpdateDisplay(minute);
        }

        public void ScoreGoal(Player player)
        {
            _game.ScoreGoal(_matchMinute, player);
            _mediator.ScoreGoal(_matchMinute, player);
        }

        public bool IsConnected() => true;
    }

    public class DisplaySystem : IDisplaySystem
    {
        private readonly Game _game;

        public DisplaySystem(IMediator mediator, Game game)
        {
            _game = game;
            mediator.RegisterDisplaySystem(this);
        }

        public void ShowWelcomeScreen()
        {
            Console.WriteLine($"Welcome! Match: {_game.TeamA.Name} - {_game.TeamB.Name}");
            Console.WriteLine($"Waiting for the game ...");
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
            Console.WriteLine($"   GOAL for {player.Team.Name}!!! {player.FirstName} {player.LastName} scoring {minute}:00 ");
        }

        public void FirstHalfFinished()
        {
            Console.WriteLine("First Half Finished."); 
            Console.WriteLine($"    Result: {_game.GetResult}");
            Console.WriteLine($"    Goals: {_game.GetGoals()}");
        }

        public void TransmissionStopped()
        {
            Console.WriteLine("Transmission stopped");
        }

        public void TransmissionStarted()
        {
            Console.WriteLine("Transmission started");
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

        private bool AllSchedulersAreActive() => _schedulers.All(scheduler => scheduler.IsActive());
        private bool AllFieldSystemsAreConnected() => _fieldSystems.All(fieldSystem => fieldSystem.IsConnected());

        public void StartTransmission()
        {
            if (!AllSchedulersAreActive())
                throw new ArgumentException("Schedulers are not active");
            if (!AllFieldSystemsAreConnected())
                throw new ArgumentException("Field Systems are connected");
            _displaySystems.ForEach(
                displaySystem => displaySystem.TransmissionStarted());
            _displaySystems.ForEach(
                displaySystem => displaySystem.ShowWelcomeScreen());
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

        public void Game_FirstHalfEnded()
        {
            _displaySystems.ForEach( 
                displaySystem => displaySystem.FirstHalfFinished());
            _schedulers.ForEach(
                scheduler => scheduler.ReadyToStopTransmission());
        }

        public void TransmissionStopped()
        {
            _displaySystems.ForEach(
                displaySystem=>displaySystem.TransmissionStopped());
        }
    }
}
