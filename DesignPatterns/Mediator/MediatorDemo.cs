using System;
using System.Collections.Generic;
using DesignPatterns.Mediator.Model;

namespace DesignPatterns.Mediator
{
    public static class MediatorDemo
    {
        public static void RunDemo()
        {
            var teamItalian = BuildTeam_Itialian();
            var teamPolish = BuildTeam_Polish();
            var game = new Game { TeamA = teamItalian, TeamB = teamPolish}; 

            var mediator = new Mediator();
            var scheduler = mediator.GetScheduler();
            var fieldSystem = mediator.GetFieldSystem();

            scheduler.StartTransmission(game);
            fieldSystem.StartedFirstHalf();
            new List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13}
                .ForEach(minute => fieldSystem.UpdateMatchMinute(minute));
            
            fieldSystem.ScoreGoal(13,teamItalian.PlayerByNumber(10));
            
            new List<int>{14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25}
                .ForEach(minute => fieldSystem.UpdateMatchMinute(minute));

            Console.WriteLine();
        }
        
        private static Team BuildTeam_Itialian() => new Team()
            .SetName("Italy","ITA")
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

        private static Team BuildTeam_Polish() => new Team()
            .SetName("Poland", "POL");
        
    }
}