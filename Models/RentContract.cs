using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using InstaCar.Web.Access.Database;

namespace Insta.Web.Access.Models
{
    [DataContract(Name = "Rent")]
    public class RentContract
    {
        public RentContract()
        {

        }

        [DataMember(Name = "rentid")]
        public long? RentId { get; set; }
        [DataMember(Name = "customerid")]
        public long? CustomerId { get; set; }
        [DataMember(Name = "carid")]
        public long? CarId { get; set; }
        [DataMember(Name = "rentno")]
        public string RentNo { get; set; }
        [DataMember(Name = "begin")]
        public DateTime? Begin { get; set; }
        [DataMember(Name = "end")]
        public DateTime? End { get; set; }
        [DataMember(Name = "sumprice")]
        public double? SumPrice { get; set; }
        [DataMember(Name = "priceperhour")]
        public double? PricePerHour { get; set; }
        [DataMember(Name = "hours")]
        public long? Hours { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "familyname")]
        public string FamilyName { get; set; }
        [DataMember(Name = "modell")]
        public string Modell { get; set; }
        [DataMember(Name = "brand")]
        public string Brand { get; set; }

    

        public static explicit operator RentContract(Rent rent)
        {
            return rent == null ? null : new RentContract()
            {
                RentId = rent.RentId,
                CustomerId = rent.CustomerId,
                CarId = rent.CarId,
                Begin = rent.Begin,
                End = rent.End,
                SumPrice = rent.SumPrice,
                Hours = rent.Hours,
                Name = rent.Name,
                FamilyName = rent.FamilyName,
                Modell = rent.Modell,
                Brand = rent.Brand,
                PricePerHour = rent.PricePerHour
            };
        }
    }
}