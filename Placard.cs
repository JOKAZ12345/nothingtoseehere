using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerDB
{
    public class Header
    {
        public DateTime timeStamp { get; set; }
        public bool responseSuccess { get; set; }
        public string version { get; set; }
    }

    public class EventPath
    {
        public int eventPathId { get; set; }
        public string eventPathDescription { get; set; } // Tipo desporto (Futebol)
        public int parentId { get; set; } // id da lista ligada anterior
    }

    public class Price
    {
        public double decimalPrice { get; set; }
    }

    public class Outcome
    {
        public int index { get; set; }
        public int outcomeId { get; set; }
        public string outcomeDescription { get; set; } // a equipa que fica com a odd (ou Empate), "Mais 2.5"
        // index 1 home, index 2 draw, index 3 away
        public double handicapValue { get; set; }
        public bool hidden { get; set; }
        public bool suspended { get; set; } // foi suspenso?
        public Price price { get; set; } // Odd
    }

    public class Market
    {
        // index 1 (TR), index 2 (INT), index 3 (DV), index 4 (HANDICAP)
        public int index { get; set; } // index para as possibilidades de aposta
        public int marketId { get; set; }
        public string marketStatus { get; set; }
        public DateTime retailSalesCloseDateTime { get; set; } // a que horas fecham as apostas para este evento ?
        public string promotionLevel { get; set; }
        public IList<Outcome> outcomes { get; set; }
        public int eventIndex { get; set; } // id do evento
    }

    public class Event
    {
        public int index { get; set; } // id do evento
        public IList<EventPath> eventPaths { get; set; }
        public DateTime eventStartDateTime { get; set; }
        public string homeOpponentDescription { get; set; } // nome equipa casa
        public string awayOpponentDescription { get; set; } // nome da equipa fora
        public bool fictional { get; set; } // ? não usado
        public IList<Market> markets { get; set; }
        public string sportCode { get; set; } // "FOOT"
        public string eventComments { get; set; } // NULL, ou algum comment que eles façam
        public string tvChannel { get; set; } // NULL, ou nome do canal
    }

    public class Data
    {
        public string status { get; set; }
        public DateTime programmeOpenDateTime { get; set; }
        public DateTime programmeCloseDateTime { get; set; }
        public IList<Event> exportedProgrammeEntries { get; set; }
    }

    public class Body
    {
        public string description { get; set; }
        public Data data { get; set; }
    }

    public class Placard
    {
        public Header header { get; set; }
        public Body body { get; set; }

        public List<Event> eventos;
    }

}
