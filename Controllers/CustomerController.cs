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
    public class CustomerController : ApiController
    {
        // GET: api/Customer
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Customer/5
        public CustomerContract Get(int id)
        {
            CustomerContract customer = (CustomerContract)Customer.LoginCheck("Tmueller", "1234");
            if (customer.CustomerId.HasValue)
            {
                return customer;
            }
            else
            {

                return new CustomerContract();
            }
            //return "value";
        }

        [HttpGet]
        [Route("api/customer/login/{nickname}/{password}")]
        public CustomerContract CustomerLogin(string nickname, string password)
        {
            try
            {
                
                CustomerContract customer = (CustomerContract) Customer.LoginCheck(nickname, password);
                if (customer.CustomerId.HasValue)
                {
                    return customer;
                }
                else
                {
                    return new CustomerContract();
                }
                
            }
            catch (Exception ex)
            {
                WriteLog($"Error on GET: {ex.Message}");
                return new CustomerContract();
            }
        }

        // POST: api/Customer
        public void Post([FromBody]string value)
        {
        }

        

        // PUT: api/Customer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Customer/5
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
