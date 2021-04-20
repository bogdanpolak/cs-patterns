using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Mediator.Model
{
    public class Team
    {
        public string Name { get; private set; }
        public string Abbreviation { get; private set; }
        private List<Player> Players { get; } = new List<Player>();
            
        public Team SetName(string name, string abbreviation) 
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
            Players.Add(player);
            return this;
        }

        public Player PlayerByNumber(int number) => Players.FirstOrDefault(pl => pl.Number == number);
    }
}