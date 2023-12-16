using ASPDotnetFC.Models;
using ASPDotnetFC.Data.Context;


namespace ASPDotnetFC
{
    public class Seed
    {
        private readonly FootballContext footballContext;
        public Seed(FootballContext context)
        {
            footballContext = context;
        }
        public void SeedDataContext()
        {
            if (!footballContext.ClubCompetitions.Any())
            {
                var clubCompetition = new List<ClubCompetition>()
                {
                    new ClubCompetition()
                    {
                        Club = new Club()
                        {
                            FullName = "São Paulo Futebol Clube",
                            Name = "São Paulo",
                            Abbreviation = "SPFC",
                            UrlImage = "https://logodetimes.com/times/sao-paulo/logo-sao-paulo-256.png",
                            Mascot = "Santo Paulo",
                            OfficialSite = "http://www.saopaulofc.net/spfc",
                            Country = "Brazil",
                            Stadium = new Stadium()
                            {
                                Name = "Estádio do Morumbi",
                                ConstructionYear = "1960",
                                Capacity = 66795,
                            },
                            
                            TopScorer = new Player()
                            {
                                Fullname = "Sérgio Bernardino",
                                Nickname = "Sérginho Chulapa",
                                Birth = "23/10/1953",
                                TeamGoals = 242,
                                TeamMatches = 397,
                                Position = "Atacante",
                                Heigth = 1.94,
                                Country = "Brazil",
                                BestFoot = "Canhota",
                                UrlImage = "https://s3p.sofifa.net/f31d606e6f3fb6bb08d3e3b8d0ee0fa259ef696f.png"
                            },

                            FoundationYear = 1930,
                        },
                        Competition = new Competition()
                        {
                            CompetitionName = "Brasileirão Séria A",
                            Country = "Brazil",
                            NumberOfTeams = 20,
                            FirstEdition = "1971",
                            IsContinental = false,
                            IsWorldwide = false,
                        }
                    },

                    new ClubCompetition()
                    {
                        Club = new Club()
                        {
                            FullName = "Sport Club Corinthians Paulista",
                            Name = "Corinthians",
                            UrlImage = "https://logodetimes.com/times/corinthians/logo-do-corinthians-256.png",
                            Abbreviation = "SCCP",
                            Mascot = "Mosqueteiro",
                            OfficialSite = "https://www.corinthians.com.br/",
                            Country = "Brazil",
                            Stadium = new Stadium()
                            {
                                Name = "Neo Química Arena",
                                ConstructionYear = "2014",
                                Capacity = 47605
                            },
                            TopScorer = new Player()
                            {
                                Fullname = "Claudio Christovan de Pinho",
                                Nickname = "Cláudio",
                                Birth = "17/07/1922",
                                TeamGoals = 305,
                                TeamMatches = 550,
                                Position = "Meia ofensivo",
                                Heigth = 1.62,
                                Country = "Brazil",
                                BestFoot = "Canhota",
                                UrlImage = "https://cdn.meutimao.com.br/_upload/idolos-do-corinthians/claudio.jpg"
                            },
                            FoundationYear = 1910,
                        },
                        Competition = new Competition()
                        {
                            CompetitionName = "Copa do Brasil",
                            Country = "Brazil",
                            NumberOfTeams = 91,
                            FirstEdition = "1989",
                            IsContinental = false,
                            IsWorldwide = false,
                        }
                    }
                };

                footballContext.ClubCompetitions.AddRange(clubCompetition);
                footballContext.SaveChanges();
            }
        }
    }
}