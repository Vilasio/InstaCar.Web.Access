using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InstaCar.Web.Access.Controllers
{
    public class LocationCarController : ApiController
    {
        // GET: api/LocationCar
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LocationCar/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LocationCar
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LocationCar/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LocationCar/5
        public void Delete(int id)
        {
        }
    }
}
