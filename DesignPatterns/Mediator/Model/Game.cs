namespace DesignPatterns.Mediator.Model
{
    public class Game
    {
        public int ResultA { get; private set; }
        public int ResultB { get; private set; }
        public Team TeamA { get; set; }
        public Team TeamB { get; set; }

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
}