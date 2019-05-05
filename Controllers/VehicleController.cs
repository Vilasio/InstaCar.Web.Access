using InstaCar.Web.Access.Database;
using InstaCar.Web.Access.Models;
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
    public class VehicleController : ApiController
    {
        // GET: api/Vehicle
        public IEnumerable<VehicleContract> Get()
        {
            try
            {
                List<VehicleContract> result = new List<VehicleContract>();
                Vehicle.GetAllVehicles().ForEach(v => result.Add((VehicleContract)v));
                return result;
            }
            catch (Exception ex)
            {
                WriteLog($"Error on GET: {ex.Message}");
                return null;
            }
            
        }

        [HttpGet]
        [Route("api/vehicle/available")]
        public List<VehicleContract> AvailableVehicles()
        {
            try
            {
                List<VehicleContract> result = new List<VehicleContract>();
                Vehicle.GetAvailableVehicles().ForEach(v => result.Add((VehicleContract)v));
                return result;
            }
            catch (Exception ex)
            {
                WriteLog($"Error on GET: {ex.Message}");
                return null;
            }
        }

        // GET: api/Vehicle/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Vehicle
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Vehicle/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Vehicle/5
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
