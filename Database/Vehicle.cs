using InstaCar.Web.Access.Models;
using Npgsql;
using System;
using System.Collections.Generic;
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
        #endregion
        //----------------------------------------------------------------------------------------------
        //Static
        //----------------------------------------------------------------------------------------------
        #region static
        public static List<Vehicle> GetAllVehicles(NpgsqlConnection connection)
        {
            List<Vehicle> allVehicles = new List<Vehicle>();
            Vehicle vehicle = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select * from {TABLE};";

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
                        NotAvailable = reader.IsDBNull(10) ? true : reader.GetBoolean(10)
                    }
                );
            }
            reader.Close();
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
                    NotAvailable = reader.IsDBNull(10) ? true : reader.GetBoolean(10)
                };
            }
            reader.Close();
            return vehicle;
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
                command.CommandText = $" insert into {TABLE} ( car_id,location_id, modell , brand, hp, price, feature1, feature2,feature3, feature4, notavailable )" +
                    $" values(:cid,:lid, :mo, :br, :hp, :pr, :f1, :f2, :f3, :f4, :no)";
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

            return command.ExecuteNonQuery();
        }

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
                NotAvailable = vehicleContract.NotAvailable
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
