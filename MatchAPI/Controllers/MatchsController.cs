using MatchAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Diagnostics;

namespace MatchAPI.Controllers
{
    [EnableCorsAttribute("*", "*", "*")]
    public class MatchsController : ApiController
    {
        // GET: api/Matchs
        public IEnumerable<Match> Get()
        {
            var repo = new MatchsRepository();
            List<Match> matchList = repo.Retrieve();

            return matchList;

        }

        // GET: api/Matchs?idpartido=id
        public IEnumerable<Match> GetId(int idpartido)
        {
            var repo = new MatchsRepository();
            List<Match> betList = repo.RetrieveById(idpartido);

            return betList;

        }

        // GET: api/Matchs/5
        public Match Get(int id)
        {

            return null;
        }

        // POST: api/Matchs
        public void Post([FromBody]Match bet)
        {
            //var repo = new MatchsRepository();
            //repo.Save(bet);
        }

        // PUT: api/Matchs/5
        public void Put(Boolean isOver, [FromBody]Market market)
        {
            var repo = new MatchsRepository();
            repo.InsertBet(isOver, market);
        }

        // DELETE: api/Matchs/5
        public void Delete(int id)
        {
        }
    }
}