using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using InstaCar.Web.Access.Database;

namespace InstaCar.Web.Access.Models
{
    [DataContract(Name = "vehicle")]
    public class VehicleContract
    {
        public VehicleContract()
        {

        }

        [DataMember(Name = "carid")]
        public long? CarId { get; set; }
        [DataMember(Name = "locationid")]
        public long? LocationId { get; set; }
        [DataMember(Name = "modell")]
        public string Modell { get; set; }
        [DataMember(Name = "brand")]
        public string Brand { get; set; }
        [DataMember(Name = "hp")]
        public long? HP { get; set; }
        [DataMember(Name = "price")]
        public double? Price { get; set; }
        [DataMember(Name = "feature1")]
        public long? Feature1 { get; set; }
        [DataMember(Name = "feature2")]
        public long? Feature2 { get; set; }
        [DataMember(Name = "feature3")]
        public long? Feature3 { get; set; }
        [DataMember(Name = "feature4")]
        public long? Feature4 { get; set; }
        [DataMember(Name = "notavailable")]
        public bool NotAvailable { get; set; }
        [DataMember(Name = "feature1string")]
        public string Feature1String { get; set; }
        [DataMember(Name = "feature2string")]
        public string Feature2String { get; set; }
        [DataMember(Name = "feature3string")]
        public string Feature3String { get; set; }
        [DataMember(Name = "feature4string")]
        public string Feature4String { get; set; }
        [DataMember(Name = "mainimage")]
        public Byte[] MainImage { get; set; }
        [DataMember(Name = "mainimagestring")]
        public String MainImageString
        {
            get
            {
                if (MainImage != null)
                {
                    return Convert.ToBase64String(MainImage);
                } return null; } }



        public static explicit operator VehicleContract(Vehicle vehicle)
        {
            return vehicle == null ? null : new VehicleContract()
            {
                CarId = vehicle.CarId,
                LocationId = vehicle.LocationId,
                Modell = vehicle.Modell,
                Brand = vehicle.Brand,
                HP = vehicle.HP,
                Price = vehicle.Price,
                Feature1 = vehicle.Feature1,
                Feature2 = vehicle.Feature2,
                Feature3 = vehicle.Feature3,
                Feature4 = vehicle.Feature4,
                NotAvailable = vehicle.NotAvailable,
                Feature1String = vehicle.Feature1String,
                Feature2String = vehicle.Feature2String,
                Feature3String = vehicle.Feature3String,
                Feature4String = vehicle.Feature4String,
                MainImage = vehicle.MainImage
            };
        }
    }
}