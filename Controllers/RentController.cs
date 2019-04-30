using Insta.Web.Access.Models;
using InstaCar.Web.Access.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
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

        [HttpGet]
        [Route("api/rent/rented/{cid}")]
        public RentContract CustomerLogin(int cid)
        {
            try
            {

                RentContract rented = (RentContract)Rent.GetCurrentRent(cid);
                if (rented.RentId.HasValue)
                {
                    return rented;
                }
                else
                {
                    return new RentContract();
                }

            }
            catch (Exception ex)
            {
                WriteLog($"Error on GET: {ex.Message}");
                return new RentContract();
            }
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

        private void WriteLog(string text)
        {

            string fileName = $"{HttpContext.Current.Server.MapPath("~/App_Data/debug.log")}";
            StreamWriter writer = new StreamWriter(fileName, true, Encoding.UTF8);
            writer.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}> {text}");
            writer.Close();
        }
    }
}
