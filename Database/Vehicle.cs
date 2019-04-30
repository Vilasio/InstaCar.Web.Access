using InstaCar.Web.Access.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaCar.Web.Access.Database
{
    public class Vehicle
    {
        //----------------------------------------------------------------------------------------------
        //Const
        //----------------------------------------------------------------------------------------------
        #region const
        private const string TABLE = "InstaCar.car";
        private const string TABLERENT = "InstaCar.rent";
        private const string COLUMN = "car_id, location_id, modell, brand, hp, price, feature1, feature2, feature3, feature4, notavailable";
        #endregion

        //----------------------------------------------------------------------------------------------
        //PrivateMember
        //----------------------------------------------------------------------------------------------
        #region privateMember
        private NpgsqlConnection connection = null;
        #endregion

        //----------------------------------------------------------------------------------------------
        //Constructor
        //----------------------------------------------------------------------------------------------
        #region constructor
        public Vehicle()
        {
            this.connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
        }

        public Vehicle(NpgsqlConnection connection)
        {
            this.connection = connection;
        }
        #endregion
        //----------------------------------------------------------------------------------------------
        //Property
        //----------------------------------------------------------------------------------------------
        #region property

        enum enumFeatures
        {
            Kein = 0,
            Klimaanlage = 1,
            Multifunktionslenkrad = 2,
            Freispreckeinrichtung = 3,
            Tempomat = 4
        }

        public long? CarId { get; set; }
        public long? LocationId { get; set; }
        public string Modell { get; set; }
        public string Brand { get; set; }
        public long? HP { get; set; }
        public double? Price { get; set; }
        public long? Feature1 { get; set; }
        public long? Feature2 { get; set; }
        public long? Feature3 { get; set; }
        public long? Feature4 { get; set; }
        public bool NotAvailable { get; set; }
        public DateTime? Reserved { get; set; }
        public bool InUse { get; set; }
        public bool Locked { get; set; }

        private List<ImageCar> pictures = null;

        public List<ImageCar> Pictures
        {
            get
            {
                if (pictures == null)
                {
                    pictures = ImageCar.GetList(this.connection, this);
                }
                return pictures;
            }
            set { pictures = value; }
        }


        public string Feature1String
        {
            get { return Enum.GetName(typeof(enumFeatures), Feature1); }
            set { Feature1String = value; }
        }
        public string Feature2String
        {
            get { return Enum.GetName(typeof(enumFeatures), Feature2); }
            set { Feature2String = value; }
        }
        public string Feature3String
        {
            get { return Enum.GetName(typeof(enumFeatures), Feature3); }
            set { Feature3String = value; }
        }
        public string Feature4String
        {
            get { return Enum.GetName(typeof(enumFeatures), Feature4); }
            set { Feature4String = value; }
        }

        #endregion
        //----------------------------------------------------------------------------------------------
        //Static
        //----------------------------------------------------------------------------------------------
        #region static
        public static List<Vehicle> GetAllVehicles()
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
            connection.Open();
            List<Vehicle> allVehicles = new List<Vehicle>();
            Vehicle vehicle = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select * from {TABLE} where deleted = false;";

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                allVehicles.Add(
                    vehicle = new Vehicle(connection)
                    {
                        CarId = reader.GetInt64(0),
                        LocationId = reader.GetInt64(1),
                        Modell = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Brand = reader.IsDBNull(3) ? null : reader.GetString(3),
                        HP = reader.IsDBNull(4) ? 0 : reader.GetInt64(4),
                        Price = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                        Feature1 = reader.IsDBNull(6) ? 0 : reader.GetInt64(6),
                        Feature2 = reader.IsDBNull(7) ? 0 : reader.GetInt64(7),
                        Feature3 = reader.IsDBNull(8) ? 0 : reader.GetInt64(8),
                        Feature4 = reader.IsDBNull(9) ? 0 : reader.GetInt64(9),
                        NotAvailable = reader.IsDBNull(10) ? true : reader.GetBoolean(10),
                        Reserved = reader.IsDBNull(11) ? null : (DateTime?)reader.GetDateTime(11),
                        InUse = reader.IsDBNull(12) ? true : reader.GetBoolean(12),
                        Locked = reader.IsDBNull(13) ? true : reader.GetBoolean(13)
                    }
                );
            }
            reader.Close();
            connection.Close();
            return allVehicles;
        }

        public static Vehicle GetSpecificVehicles(NpgsqlConnection connection, int key)
        {

            Vehicle vehicle = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select * from {TABLE} where car_id = :id;";
            command.Parameters.AddWithValue("id", key);
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                vehicle = new Vehicle(connection)
                {
                    CarId = reader.GetInt64(0),
                    LocationId = reader.GetInt64(1),
                    Modell = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Brand = reader.IsDBNull(3) ? null : reader.GetString(3),
                    HP = reader.IsDBNull(4) ? 0 : reader.GetInt64(4),
                    Price = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Feature1 = reader.IsDBNull(6) ? 0 : reader.GetInt64(6),
                    Feature2 = reader.IsDBNull(7) ? 0 : reader.GetInt64(7),
                    Feature3 = reader.IsDBNull(8) ? 0 : reader.GetInt64(8),
                    Feature4 = reader.IsDBNull(9) ? 0 : reader.GetInt64(9),
                    NotAvailable = reader.IsDBNull(10) ? true : reader.GetBoolean(10),
                    Reserved = reader.IsDBNull(11) ? null : (DateTime?)reader.GetDateTime(11),
                    InUse = reader.IsDBNull(12) ? true : reader.GetBoolean(12),
                    Locked = reader.IsDBNull(13) ? true : reader.GetBoolean(13)
                };
            }
            reader.Close();
            return vehicle;
        }

        public static List<Vehicle> GetAvailableVehicles(NpgsqlConnection connection, DateTime time)
        {
            List<Vehicle> allVehicles = new List<Vehicle>();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select c.car_id, c.location_id, c.modell, c.brand, c.hp, c.price, c.feature1, c.feature2, c.feature3, c.feature4, " +
                $"c.notavailable, c.reserved, c.in_use, c.locked from {TABLE} as c left join {TABLERENT} as r on r.car_id = c.car_id " +
                $"where ((r.datebegin > :t or r.dateend < :t) or r.datebegin is null) and c.notavailable = false and c.deleted = false;";
            command.Parameters.AddWithValue("t", time);

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                allVehicles.Add(new Vehicle(connection)
                {
                    CarId = reader.GetInt64(0),
                    LocationId = reader.GetInt64(1),
                    Modell = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Brand = reader.IsDBNull(3) ? null : reader.GetString(3),
                    HP = reader.IsDBNull(4) ? 0 : reader.GetInt64(4),
                    Price = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Feature1 = reader.IsDBNull(6) ? 0 : reader.GetInt64(6),
                    Feature2 = reader.IsDBNull(7) ? 0 : reader.GetInt64(7),
                    Feature3 = reader.IsDBNull(8) ? 0 : reader.GetInt64(8),
                    Feature4 = reader.IsDBNull(9) ? 0 : reader.GetInt64(9),
                    NotAvailable = reader.IsDBNull(10) ? true : reader.GetBoolean(10),
                    Reserved = reader.IsDBNull(11) ? null : (DateTime?)reader.GetDateTime(11),
                    InUse = reader.IsDBNull(12) ? true : reader.GetBoolean(12),
                    Locked = reader.IsDBNull(13) ? true : reader.GetBoolean(13)
                });
            }
            reader.Close();
            return allVehicles;
        }

        public static double Getprice(NpgsqlConnection connection)
        {

            double result = 0;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select price from {TABLE}";
            NpgsqlDataReader reader = command.ExecuteReader();

            reader.Read();
            result = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);

            reader.Close();
            return result;
        }

        public static int Changeprice(NpgsqlConnection connection, double price)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            
                command.CommandText =
                $"update {TABLE} set price = :pr";
            
            command.Parameters.AddWithValue("pr", price);

            return command.ExecuteNonQuery();
        }
        #endregion
        //----------------------------------------------------------------------------------------------
        //Public
        //----------------------------------------------------------------------------------------------
        #region public
        public int Save()
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = this.connection;
            if (this.CarId.HasValue)
            {

                command.CommandText =
                $"update {TABLE} set location_id = :lid, modell = :mo, brand = :br, hp = :hp, price = :pr, feature1 = :f1, feature2 = :f2," +
                $"feature3 = :f3, feature4 = :f4, notavailable = :no where car_id = :cid";


            }
            else
            {
                command.CommandText = $"select nextval('{TABLE}_seq')";
                this.CarId = (long?)command.ExecuteScalar();
                command.CommandText = $" insert into {TABLE} ( car_id,location_id, modell , brand, hp, price, feature1, feature2,feature3, feature4, notavailable, in_use )" +
                    $" values(:cid,:lid, :mo, :br, :hp, :pr, :f1, :f2, :f3, :f4, :no, :iu)";
            }
            command.Parameters.AddWithValue("cid", this.CarId.Value);
            command.Parameters.AddWithValue("lid", this.LocationId.Value);
            command.Parameters.AddWithValue("mo", String.IsNullOrEmpty(this.Modell) ? (object)DBNull.Value : this.Modell);
            command.Parameters.AddWithValue("br", String.IsNullOrEmpty(this.Brand) ? (object)DBNull.Value : this.Brand);
            command.Parameters.AddWithValue("hp", this.HP.HasValue ? (object)this.HP.Value : 0);
            command.Parameters.AddWithValue("pr", this.Price.HasValue ? (object)this.Price.Value : 0);
            command.Parameters.AddWithValue("f1", this.Feature1.HasValue ? (object)this.Feature1.Value : 0);
            command.Parameters.AddWithValue("f2", this.Feature2.HasValue ? (object)this.Feature2.Value : 0);
            command.Parameters.AddWithValue("f3", this.Feature3.HasValue ? (object)this.Feature3.Value : 0);
            command.Parameters.AddWithValue("f4", this.Feature4.HasValue ? (object)this.Feature4.Value : 0);
            command.Parameters.AddWithValue("no", this.NotAvailable);
            command.Parameters.AddWithValue("iu", false);

            return command.ExecuteNonQuery();
        }

        /*public int Reserve()
        {
            int result = 0;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = this.connection;
            if (this.CarId.HasValue)
            {

                command.CommandText =
                $"update {TABLE} set reserved = current_timestamp;";


            }
           
            command.Parameters.AddWithValue("cid", this.CarId.Value);
            command.Parameters.AddWithValue("lid", this.LocationId.Value);



            return result;
        }*/

        public static explicit operator Vehicle(VehicleContract vehicleContract)
        {
            return vehicleContract == null ? null : new Vehicle()
            {
                CarId = vehicleContract.CarId,
                LocationId = vehicleContract.LocationId,
                Modell = vehicleContract.Modell,
                Brand = vehicleContract.Brand,
                HP = vehicleContract.HP,
                Price = vehicleContract.Price,
                Feature1 = vehicleContract.Feature1,
                Feature2 = vehicleContract.Feature2,
                Feature3 = vehicleContract.Feature3,
                Feature4 = vehicleContract.Feature4,
                NotAvailable = vehicleContract.NotAvailable,
                
               
            };
        }
        #endregion
        //----------------------------------------------------------------------------------------------
        //Private
        //----------------------------------------------------------------------------------------------
        #region private

        #endregion
    }
}
