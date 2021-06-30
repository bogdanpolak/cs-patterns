using System.Diagnostics.CodeAnalysis;

namespace DesignPatterns.Mediator
{
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
}