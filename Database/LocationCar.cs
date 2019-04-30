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
    public class LocationCar
    {
        //----------------------------------------------------------------------------------------------
        //Const
        //----------------------------------------------------------------------------------------------
        #region const
        private const string TABLE = "InstaCar.location";
        private const string COLUMN = "location_id, name, street, housenr, postcode, city";
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
        public LocationCar()
        {
            this.connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
        }

        public LocationCar(NpgsqlConnection connection)
        {
            this.connection = connection;
        }
        #endregion
        //----------------------------------------------------------------------------------------------
        //Property
        //----------------------------------------------------------------------------------------------
        #region property
        public long? LocationId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public long? HouseNr { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }



        #endregion
        //----------------------------------------------------------------------------------------------
        //Static
        //----------------------------------------------------------------------------------------------
        #region static
        public static List<LocationCar> GetAllLocation()
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);

            List<LocationCar> allLocations = new List<LocationCar>();
            LocationCar location = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select * from {TABLE};";

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                allLocations.Add(
                    location = new LocationCar(connection)
                    {
                        LocationId = reader.GetInt64(0),
                        Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Street = reader.IsDBNull(2) ? null : reader.GetString(2),
                        HouseNr = reader.IsDBNull(3) ? 0 : reader.GetInt64(3),
                        Postcode = reader.IsDBNull(4) ? null : reader.GetString(4),
                        City = reader.IsDBNull(5) ? null : reader.GetString(5)
                    }
                );
            }
            reader.Close();
            return allLocations;
        }

        static LocationCar GetSpecificLocation( int key)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);

            LocationCar location = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select * from {TABLE} where location_id = :id;";
            command.Parameters.AddWithValue("id", key);
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                location = new LocationCar(connection)
                {
                    LocationId = reader.GetInt64(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Street = reader.IsDBNull(2) ? null : reader.GetString(2),
                    HouseNr = reader.IsDBNull(3) ? 0 : reader.GetInt64(3),
                    Postcode = reader.IsDBNull(4) ? null : reader.GetString(4),
                    City = reader.IsDBNull(5) ? null : reader.GetString(5)
                };
            }
            reader.Close();
            return location;
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
            if (this.LocationId.HasValue)
            {

                command.CommandText =
                $"update {TABLE} set  name = :na, street = :st, housenr = :hn, postcode = :pc, city = :ci where location_id = :lid";


            }
            else
            {
                command.CommandText = $"select nextval('{TABLE}_seq')";
                this.LocationId = (long?)command.ExecuteScalar();
                command.CommandText = $" insert into {TABLE} (location_id, name, street, housenr, postcode, city)" +
                    $" values(:lid, :na, :st, :hn, :pc, :ci)";
            }
            command.Parameters.AddWithValue("lid", this.LocationId.Value);
            command.Parameters.AddWithValue("na", String.IsNullOrEmpty(this.Name) ? (object)DBNull.Value : this.Name);
            command.Parameters.AddWithValue("st", String.IsNullOrEmpty(this.Street) ? (object)DBNull.Value : this.Street);
            command.Parameters.AddWithValue("hn", this.HouseNr.HasValue ? (object)this.HouseNr.Value : 3);
            command.Parameters.AddWithValue("pc", String.IsNullOrEmpty(this.Postcode) ? (object)DBNull.Value : this.Postcode);
            command.Parameters.AddWithValue("ci", String.IsNullOrEmpty(this.City) ? (object)DBNull.Value : this.City);

            return command.ExecuteNonQuery();
        }

        public static explicit operator LocationCar(LocationCarContract locationCarContract)
        {
            return locationCarContract == null ? null : new LocationCar()
            {
                LocationId = locationCarContract.LocationId,
                Name = locationCarContract.Name,
                Street = locationCarContract.Street,
                HouseNr = locationCarContract.HouseNr,
                Postcode = locationCarContract.Postcode,
                City = locationCarContract.City
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
