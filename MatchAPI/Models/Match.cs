using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatchAPI.Models
{
    public class Match
    {
        public Match(int idPartido, string equipoLoc, string equipoVis, List<Market> markets)
        {
            this.idPartido = idPartido;
            this.equipoLoc = equipoLoc;
            this.equipoVis = equipoVis;
            this.markets = markets;
        }

        public int idPartido { get; set; }
        public String equipoLoc { get; set; }
        public String equipoVis { get; set; }
        public List<Market> markets {get; set;}
    }
}