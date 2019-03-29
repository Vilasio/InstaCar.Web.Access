using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InstaCar.Web.Access.Controllers
{
    public class RentController : ApiController
    {
        // GET: api/Rent
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Rent/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Rent
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Rent/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Rent/5
        public void Delete(int id)
        {
        }
    }
}
