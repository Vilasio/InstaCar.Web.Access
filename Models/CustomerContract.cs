using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using InstaCar.Web.Access.Database;

namespace InstaCar.Web.Access.Models
{
    [DataContract (Name = "customer")]
    public class CustomerContract
    {
        public CustomerContract()
        {

        }

        [DataMember(Name = "customerid")]
        public long? CustomerId { get; set; }

        [DataMember(Name = "customerno")]
        public string CustomerNo { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "familyname")]
        public string Familyname { get; set; }
        [DataMember(Name = "street")]
        public string Street { get; set; }
        [DataMember(Name = "housenr")]
        public long? HouseNr { get; set; }
        [DataMember(Name = "postcode")]
        public string Postcode { get; set; }
        [DataMember(Name = "city")]
        public string City { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "telefon")]
        public string Telefon { get; set; }
        [DataMember(Name = "iban")]
        public string Iban { get; set; }
        [DataMember(Name = "bic")]
        public string Bic { get; set; }
        [DataMember(Name = "password")]
        public string Password { get; set; }
        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }


        public static explicit operator CustomerContract(Customer customer)
        {
            return customer == null ? null : new CustomerContract()
            {
                CustomerId = customer.CustomerId,
                CustomerNo = customer.CustomerNo,
                Name = customer.Name,
                Familyname = customer.Familyname,
                Street = customer.Street,
                HouseNr = customer.HouseNr,
                Postcode = customer.Postcode,
                City = customer.City,
                Email = customer.Email,
                Telefon = customer.Telefon,
                Iban = customer.Iban,
                Bic = customer.Bic,
                Password = customer.Password,
                Nickname = customer.Nickname
            };
        }
    }
}