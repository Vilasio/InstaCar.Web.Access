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
        /*public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        [HttpGet]
        [Route("api/rent/specrent/{rid}")]
        public RentContract Get(int rid)
        {
            try
            {
                RentContract result = (RentContract)Rent.GetSpecificRent(rid);
                return result;


            }
            catch (Exception ex)
            {
                WriteLog($"Error on GET: {ex.Message}");
                return new RentContract();
            };
        }

        [HttpGet]
        [Route("api/rent/rented/{cid}")]
        public List<RentContract> CustomerLogin(int cid)
        {
            try
            {
                List<RentContract> result = new List<RentContract>();
                Rent.GetCurrentRents(cid).ForEach(v => result.Add((RentContract)v));
                return result;
                

            }
            catch (Exception ex)
            {
                WriteLog($"Error on GET: {ex.Message}");
                return new List<RentContract>();
            }
        }

        [HttpGet]
        [Route("api/rent/Allrents/{cid}")]
        public List<RentContract> Allrents(int cid)
        {
            try
            {
                List<RentContract> result = new List<RentContract>();
                Rent.GetMyRents(cid).ForEach(v => result.Add((RentContract)v));
                return result;


            }
            catch (Exception ex)
            {
                WriteLog($"Error on GET: {ex.Message}");
                return new List<RentContract>();
            }
        }

        // POST: api/Rent
        public HttpResponseMessage Post([FromBody]RentContract value)
        {
            try
            {
                Rent rent = (Rent)value;
                rent.Save();
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/rent/rentend/{rid}")]
        public HttpResponseMessage RentEnd(int rid)
        {
            try
            {
                Rent rent = Rent.GetSpecificRent(rid);
                rent.Endrent();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/rent/rentbeginnow/")]
        public HttpResponseMessage RentBeginNow([FromBody]RentContract value)
        {
            try
            {
                Rent rent = (Rent)value;
                rent.StartRentNow();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/rent/rentbegin/")]
        public HttpResponseMessage RentBegin([FromBody]RentContract value)
        {
            try
            {
                Rent rent = (Rent)value;
                rent.Save();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
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
