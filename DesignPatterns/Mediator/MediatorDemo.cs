using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            fieldSystem.MatchMinute(1);
            fieldSystem.MatchMinute(2);
            fieldSystem.MatchMinute(3);
            fieldSystem.MatchMinute(4);
            fieldSystem.MatchMinute(5);
            fieldSystem.MatchMinute(6);
            fieldSystem.MatchMinute(7);
            fieldSystem.MatchMinute(8);
            fieldSystem.ScoreGoal(italianTeam.PlayerByNumber(10));
            new List<int>{9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25}
                .ForEach(minute => fieldSystem.MatchMinute(minute));
            fieldSystem.ScoreGoal(polishTeam.PlayerByNumber(20));
            new List<int>{26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43}
                .ForEach(minute => fieldSystem.MatchMinute(minute));
        }
    }

    // --------------------------------------------------------------------
    // Team builder
    // --------------------------------------------------------------------

    public static class TeamBuilder
    {
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static Team ItalianTeam() => new Team()
            .TeamName("Italy","ITA")
            .AddPlayer(1, Position.Goalkeeper, "Salvatore", "Sirigu")
            .AddPlayer(16, Position.Goalkeeper, "Alex", "Meret")
            .AddPlayer(21, Position.Goalkeeper, "Gianluigi", "Donnarumma")
            .AddPlayer(2, Position.Defence, "Giovanni", "Di Lorenzo")
            .AddPlayer(3, Position.Defence, "Alessandro", "Bastoni")
            .AddPlayer(4, Position.Defence, "Leonardo", "Spinazzola")
            .AddPlayer(6, Position.Defence, "Rafael", "Tolói")
            .AddPlayer(11, Position.Defence, "Manuel", "Lazzari")
            .AddPlayer(13, Position.Defence, "Emerson", "Palmieri")
            .AddPlayer(15, Position.Defence, "Francesco", "Acerbi")
            .AddPlayer(19, Position.Defence, "Leonardo", "Bonucci")
            .AddPlayer(23, Position.Defence, "Gianluca", "Mancini")
            .AddPlayer(5, Position.MidField, "Manuel", "Locatelli")
            .AddPlayer(7, Position.MidField, "Lorenzo", "Pellegrini")
            .AddPlayer(8, Position.MidField, "Matteo", "Pessina")
            .AddPlayer(12, Position.MidField, "Stefano", "Sensi")
            .AddPlayer(18, Position.MidField, "Nicolò", "Barella")
            .AddPlayer(9, Position.Forward, "Andrea", "Beloit")
            .AddPlayer(10, Position.Forward, "Lorenzo", "Insane")
            .AddPlayer(14, Position.Forward, "Federico", "Chess")
            .AddPlayer(17, Position.Forward, "Ciro", "Immobile")
            .AddPlayer(20, Position.Forward, "Federico", "Bernardeschi")
            .AddPlayer(22, Position.Forward, "Stephan", "El Shaarawy");

        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static Team PolishTeam() => new Team()
            .TeamName("Poland", "POL")
            .AddPlayer(1, Position.Goalkeeper, "Wojciech", "Szczęsny")
            .AddPlayer(12, Position.Goalkeeper, "Łukasz", "Skorupski") 
            .AddPlayer(22, Position.Goalkeeper, "Łukasz", "Fabiański")
            .AddPlayer(2, Position.Defence, "Kamil", "Piątkowski")
            .AddPlayer(3, Position.Defence, "Dawidowicz", "Paweł") 
            .AddPlayer(4, Position.Defence, "Tomasz", "Kędziora") 
            .AddPlayer(5, Position.Defence, "Jan", "Bednarek") 
            .AddPlayer(13, Position.Defence, "Maciej", "Rybus") 
            .AddPlayer(15, Position.Defence, "Kamil", "Glik") 
            .AddPlayer(18, Position.Defence, "Bartosz", "Bereszyński") 
            .AddPlayer(25, Position.Defence, "Michał", "Helik")
            .AddPlayer(26, Position.Defence, "Tymoteusz", "Puchacz")
            .AddPlayer(6, Position.MidField, "Kacper", "Kozłowski")
            .AddPlayer(8, Position.MidField, "Karol", "Linetty") 
            .AddPlayer(10, Position.MidField, "Grzegorz", "Krychowiak") 
            .AddPlayer(14, Position.MidField, "Mateusz", "Klich") 
            .AddPlayer(16, Position.MidField, "Jakub", "Moder") 
            .AddPlayer(17, Position.MidField, "Przemysław", "Płacheta") 
            .AddPlayer(19, Position.MidField, "Przemysław", "Frankowski") 
            .AddPlayer(20, Position.MidField, "Piotr", "Zieliński")
            .AddPlayer(21, Position.MidField, "Kamil", "Jóźwiak")
            .AddPlayer(7, Position.Forward, "Arkadiusz", "Milik")
            .AddPlayer( 9, Position.Forward, "Robert", "Lewandowski") 
            .AddPlayer(11, Position.Forward, "Karol", "Świderski") 
            .AddPlayer(23, Position.Forward, "Dawid", "Kownacki")
            .AddPlayer(24, Position.Forward, "Jakub", "Świerczok");
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