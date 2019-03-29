using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using InstaCar.Web.Access.Database;

namespace InstaCar.Web.Access.Models
{
    [DataContract(Name = "LocationCar")]
    public class LocationCarContract
    {
        public LocationCarContract()
        {

        }

        [DataMember(Name = "locationid")]
        public long? LocationId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "street")]
        public string Street { get; set; }
        [DataMember(Name = "housenr")]
        public long? HouseNr { get; set; }
        [DataMember(Name = "postcode")]
        public string Postcode { get; set; }
        [DataMember(Name = "city")]
        public string City { get; set; }

        public static explicit operator LocationCarContract(LocationCar locationCar)
        {
            return locationCar == null ? null : new LocationCarContract()
            {
                LocationId = locationCar.LocationId,
                Name = locationCar.Name,
                Street = locationCar.Street,
                HouseNr = locationCar.HouseNr,
                Postcode = locationCar.Postcode,
                City = locationCar.City
            };
        }
    }
}