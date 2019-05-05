using Insta.Web.Access.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaCar.Web.Access.Database
{
    public class Rent
    {
        //----------------------------------------------------------------------------------------------
        //Const
        //----------------------------------------------------------------------------------------------
        #region const
        private const string TABLE = "InstaCar.rent";
        private const string TABLECar = "InstaCar.car";
        private const string TABLECus = "InstaCar.customer";
        private const string COLUMN = "rent_id, customer_id, car_id, cust_no, datebegin, dateend, sumprice, hours";
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
        public Rent()
        {
            this.connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
        }

        public Rent(int customerid, int vehicleid)
        {
            this.CustomerId = customerid;
            this.CarId = vehicleid;
            this.connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
        }

        public Rent(NpgsqlConnection connection)
        {
            this.connection = connection;
        }
        #endregion
        //----------------------------------------------------------------------------------------------
        //Property
        //----------------------------------------------------------------------------------------------
        #region property
        public long? RentId { get; set; }
        public long? CustomerId { get; set; }
        public long? CarId { get; set; }
        public string RentNo { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
        public double? PricePerHour { get; set; }
        public double? SumPrice { get; set; }
        public long? Hours { get; set; }
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string Modell { get; set; }
        public string Brand { get; set; }

        public Customer customer = null;
        public Vehicle Vehicle = null;
        #endregion
        //----------------------------------------------------------------------------------------------
        //Static
        //----------------------------------------------------------------------------------------------
        #region static
        static List<Rent> GetAllRents(NpgsqlConnection connection)
        {
            List<Rent> allRents = new List<Rent>();
            Rent Rent = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select * from {TABLE};";

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                allRents.Add(
                    Rent = new Rent(connection)
                    {
                        RentId = reader.GetInt64(0),
                        CustomerId = reader.GetInt64(1),
                        CarId = reader.GetInt64(2),
                        RentNo = reader.GetString(3),
                        Begin = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4),
                        End = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5),
                        SumPrice = reader.IsDBNull(6) ? 0 : (double?)reader.GetDouble(6),
                        Hours = reader.IsDBNull(7) ? 0 : (long?)reader.GetInt64(7),
                        Modell = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Brand = reader.IsDBNull(9) ? null : reader.GetString(9),
                        Name = reader.IsDBNull(10) ? null : reader.GetString(10),
                        FamilyName = reader.IsDBNull(11) ? null : reader.GetString(11),
                        PricePerHour = reader.IsDBNull(12) ? 0 : (double?)reader.GetDouble(12)
                    }
                );
            }
            reader.Close();
            return allRents;
        }

        public static List<Rent> GetCurrentRents(NpgsqlConnection connection)
        {
            List<Rent> currentRents = new List<Rent>();
            Rent Rent = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select r.rent_id, r.customer_id, r.car_id, r.rent_no, r.datebegin, r.dateend, r.sumprice, r.hours, v.modell, v.brand, c.name, c.familyname, r.priceperhour " +
                $"from {TABLE} as r " +
                $"inner join {TABLECar} as v on r.car_id = v.car_id " +
                $"inner join {TABLECus} as c on r.customer_id = c.customer_id" +
                $" where dateend > current_Timestamp or dateend is null;";

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                currentRents.Add(
                    Rent = new Rent(connection)
                    {
                        RentId = reader.GetInt64(0),
                        CustomerId = reader.GetInt64(1),
                        CarId = reader.GetInt64(2),
                        RentNo = reader.GetString(3),
                        Begin = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4),
                        End = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5),
                        SumPrice = reader.IsDBNull(6) ? 0 : (double?)reader.GetDouble(6),
                        Hours = reader.IsDBNull(7) ? 0 : (long?)reader.GetInt64(7),
                        Modell = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Brand = reader.IsDBNull(9) ? null : reader.GetString(9),
                        Name = reader.IsDBNull(10) ? null : reader.GetString(10),
                        FamilyName = reader.IsDBNull(11) ? null : reader.GetString(11),
                        PricePerHour = reader.IsDBNull(12) ? 0 : (double?)reader.GetDouble(12)

                    }
                );
            }
            reader.Close();
            return currentRents;
        }

        public static List<Rent> GetMyRents(int CustomerId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
            List<Rent> currentRents = new List<Rent>();
            Rent Rent = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select r.rent_id, r.customer_id, r.car_id, r.rent_no, r.datebegin, r.dateend, r.sumprice, r.hours, v.modell, v.brand, c.name, c.familyname, r.priceperhour " +
                $"from {TABLE} as r " +
                $"inner join {TABLECar} as v on r.car_id = v.car_id " +
                $"inner join {TABLECus} as c on r.customer_id = c.customer_id" +
                $" where r.customer_id = :cid;";

            command.Parameters.AddWithValue("cid", CustomerId);
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                currentRents.Add(
                    Rent = new Rent(connection)
                    {
                        RentId = reader.GetInt64(0),
                        CustomerId = reader.GetInt64(1),
                        CarId = reader.GetInt64(2),
                        RentNo = reader.GetString(3),
                        Begin = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4),
                        End = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5),
                        SumPrice = reader.IsDBNull(6) ? 0 : (double?)reader.GetDouble(6),
                        Hours = reader.IsDBNull(7) ? 0 : (long?)reader.GetInt64(7),
                        Modell = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Brand = reader.IsDBNull(9) ? null : reader.GetString(9),
                        Name = reader.IsDBNull(10) ? null : reader.GetString(10),
                        FamilyName = reader.IsDBNull(11) ? null : reader.GetString(11),
                        PricePerHour = reader.IsDBNull(12) ? 0 : (double?)reader.GetDouble(12)

                    }
                );
            }
            reader.Close();
            return currentRents;
        }

        public static Rent GetSpecificRent(int RentId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
            connection.Open();
            Rent rent = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select r.rent_id, r.customer_id, r.car_id, r.rent_no, r.datebegin, r.dateend, r.sumprice, r.hours, v.modell, v.brand, c.name, c.familyname, r.priceperhour " +
                $"from {TABLE} as r " +
                $"inner join {TABLECar} as v on r.car_id = v.car_id " +
                $"inner join {TABLECus} as c on r.customer_id = c.customer_id" +
                $" where r.rent_id = :rid and r.dateend is null;";

            command.Parameters.AddWithValue("rid", RentId);
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                rent = new Rent(connection)
                {
                    RentId = reader.GetInt64(0),
                    CustomerId = reader.GetInt64(1),
                    CarId = reader.GetInt64(2),
                    RentNo = reader.GetString(3),
                    Begin = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4),
                    End = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5),
                    SumPrice = reader.IsDBNull(6) ? 0 : (double?)reader.GetDouble(6),
                    Hours = reader.IsDBNull(7) ? 0 : (long?)reader.GetInt64(7),
                    Modell = reader.IsDBNull(8) ? null : reader.GetString(8),
                    Brand = reader.IsDBNull(9) ? null : reader.GetString(9),
                    Name = reader.IsDBNull(10) ? null : reader.GetString(10),
                    FamilyName = reader.IsDBNull(11) ? null : reader.GetString(11),
                    PricePerHour = reader.IsDBNull(12) ? 0 : (double?)reader.GetDouble(12)

                };
            }
            reader.Close();
            connection.Close();
            return rent;
        }

        public static List<Rent> GetCurrentRents(int CustomerId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
            connection.Open();
            List<Rent> rents = new List<Rent>();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select r.rent_id, r.customer_id, r.car_id, r.rent_no, r.datebegin, r.dateend, r.sumprice, r.hours, v.modell, v.brand, c.name, c.familyname, r.priceperhour " +
                $"from {TABLE} as r " +
                $"inner join {TABLECar} as v on r.car_id = v.car_id " +
                $"inner join {TABLECus} as c on r.customer_id = c.customer_id" +
                $" where r.customer_id = :cid and r.dateend is null;";

            command.Parameters.AddWithValue("cid", CustomerId);
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                rents.Add(new Rent(connection)
                {
                    RentId = reader.GetInt64(0),
                    CustomerId = reader.GetInt64(1),
                    CarId = reader.GetInt64(2),
                    RentNo = reader.GetString(3),
                    Begin = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4),
                    End = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5),
                    SumPrice = reader.IsDBNull(6) ? 0 : (double?)reader.GetDouble(6),
                    Hours = reader.IsDBNull(7) ? 0 : (long?)reader.GetInt64(7),
                    Modell = reader.IsDBNull(8) ? null : reader.GetString(8),
                    Brand = reader.IsDBNull(9) ? null : reader.GetString(9),
                    Name = reader.IsDBNull(10) ? null : reader.GetString(10),
                    FamilyName = reader.IsDBNull(11) ? null : reader.GetString(11),
                    PricePerHour = reader.IsDBNull(12) ? 0 : (double?)reader.GetDouble(12)

                });
            }
            reader.Close();
            connection.Close();
            return rents;
        }

        public static int SomethingRented(int customerId )
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);

            int result = 0;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select * where dateend is null and customer_id = :cid;";

            NpgsqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                result = 1;
            }

            reader.Close();
            return result;
        }
        #endregion
        //----------------------------------------------------------------------------------------------
        //Public
        //----------------------------------------------------------------------------------------------
        #region public
        public int Save()
        {
            if (!this.RentId.HasValue)
            {
                this.Vehicle = Vehicle.GetSpecificVehicles(this.connection, (int)this.CarId);
                this.Modell = this.Vehicle.Modell;
                this.Brand = this.Vehicle.Brand;
                this.customer = Customer.GetSpecificCustomer(this.connection, (int)this.CustomerId);
                this.Name = this.customer.Name;
                this.FamilyName = this.customer.Familyname;
                this.Begin = DateTime.Now;
                this.PricePerHour = this.Vehicle.Price;
            }
            
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = this.connection;
            connection.Open();
            if (this.RentId.HasValue)
            {

                command.CommandText =
                $"update {TABLE} set datebegin = :db, dateend = :de, sumprice = :sp, hours = :un where rent_id = :rid";


            }
            else
            {
                command.CommandText = $"select nextval('{TABLE}_seq')";
                this.RentId = (long?)command.ExecuteScalar();
                command.CommandText = $" insert into {TABLE} (rent_id, customer_id, car_id, rent_no, datebegin, dateend, sumprice, hours, deleted)" +
                    $" values(:rid, :cid, :caid, :rno, :db, :de, :sp, :un, :del)";
            }
            if (this.RentNo == null || this.RentNo == "")// CustomerNO generieren--------------------------
            {
                DateTime now = DateTime.Now;
                int year = now.Year;
                int month = now.Month;
                string number = this.RentId.ToString();
                number = number.PadLeft(6, '0');
                this.RentNo = $"{year}/{month}/{number}";
            }

            command.Parameters.AddWithValue("rid", this.RentId.Value);
            command.Parameters.AddWithValue("cid", this.CustomerId.Value);
            command.Parameters.AddWithValue("caid", this.CarId.Value);
            command.Parameters.AddWithValue("rno", this.RentNo);
            command.Parameters.AddWithValue("db", this.Begin.HasValue ? (object)this.Begin.Value : (object)DBNull.Value);
            command.Parameters.AddWithValue("de", this.End.HasValue ? (object)this.End.Value : (object)DBNull.Value);
            command.Parameters.AddWithValue("sp", this.SumPrice.HasValue ? (object)this.SumPrice.Value : 0);
            command.Parameters.AddWithValue("un", this.Hours.HasValue ? (object)this.Hours.Value : 0);
            command.Parameters.AddWithValue("del", false);

            connection.Close();
            return command.ExecuteNonQuery();
        }

        public int StartRent()
        {
            this.Vehicle = Vehicle.GetSpecificVehicles(this.connection, (int)this.CarId);
            this.Modell = this.Vehicle.Modell;
            this.Brand = this.Vehicle.Brand;
            this.customer = Customer.GetSpecificCustomer(this.connection, (int)this.CustomerId);
            this.Name = this.customer.Name;
            this.FamilyName = this.customer.Familyname;

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = this.connection;
            connection.Open();
            if (this.RentId.HasValue)
            {

                command.CommandText =
                $"update {TABLE} set datebegin = :db, dateend = :de, sumprice = :sp, hours = :un where rent_id = :rid";


            }
            else
            {
                command.CommandText = $"select nextval('{TABLE}_seq')";
                this.RentId = (long?)command.ExecuteScalar();
                command.CommandText = $" insert into {TABLE} (rent_id, customer_id, car_id, rent_no, datebegin, dateend, sumprice, priceperhour,  hours, deleted)" +
                    $" values(:rid, :cid, :caid, :rno, :db, :de, :sp, :pp, :un, :del)";
            }
            if (this.RentNo == null || this.RentNo == "")// CustomerNO generieren--------------------------
            {
                DateTime now = DateTime.Now;
                int year = now.Year;
                int month = now.Month;
                string number = this.RentId.ToString();
                number = number.PadLeft(6, '0');
                this.RentNo = $"{year}/{month}/{number}";
            }

            command.Parameters.AddWithValue("rid", this.RentId.Value);
            command.Parameters.AddWithValue("cid", this.CustomerId.Value);
            command.Parameters.AddWithValue("caid", this.CarId.Value);
            command.Parameters.AddWithValue("rno", this.RentNo);
            command.Parameters.AddWithValue("db", this.Begin.HasValue ? (object)this.Begin.Value : (object)DBNull.Value);
            command.Parameters.AddWithValue("de", this.End.HasValue ? (object)this.End.Value : (object)DBNull.Value);
            command.Parameters.AddWithValue("pp", this.PricePerHour.HasValue ? (object)this.PricePerHour.Value : 0);
            command.Parameters.AddWithValue("sp", this.SumPrice.HasValue ? (object)this.SumPrice.Value : 0);
            command.Parameters.AddWithValue("un", this.Hours.HasValue ? (object)this.Hours.Value : 0);
            command.Parameters.AddWithValue("del", false);

            connection.Close();
            return command.ExecuteNonQuery();
        }

        public int StartRentNow()
        {
            this.Vehicle = Vehicle.GetSpecificVehicles(this.connection, (int)this.CarId);
            this.Modell = this.Vehicle.Modell;
            this.Brand = this.Vehicle.Brand;
            this.customer = Customer.GetSpecificCustomer(this.connection, (int)this.CustomerId);
            this.Name = this.customer.Name;
            this.FamilyName = this.customer.Familyname;
            this.Begin = DateTime.Now;
            this.PricePerHour = this.Vehicle.Price;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = this.connection;
            connection.Open();
            if (this.RentId.HasValue)
            {

                command.CommandText =
                $"update {TABLE} set datebegin = :db, dateend = :de, sumprice = :sp, hours = :un where rent_id = :rid";


            }
            else
            {
                command.CommandText = $"select nextval('{TABLE}_seq')";
                this.RentId = (long?)command.ExecuteScalar();
                command.CommandText = $" insert into {TABLE} (rent_id, customer_id, car_id, rent_no, datebegin, dateend, sumprice, priceperhour,  hours, deleted)" +
                    $" values(:rid, :cid, :caid, :rno, :db, :de, :sp, :pp, :un, :del)";
            }
            if (this.RentNo == null || this.RentNo == "")// CustomerNO generieren--------------------------
            {
                DateTime now = DateTime.Now;
                int year = now.Year;
                int month = now.Month;
                string number = this.RentId.ToString();
                number = number.PadLeft(6, '0');
                this.RentNo = $"{year}/{month}/{number}";
            }

            command.Parameters.AddWithValue("rid", this.RentId.Value);
            command.Parameters.AddWithValue("cid", this.CustomerId.Value);
            command.Parameters.AddWithValue("caid", this.CarId.Value);
            command.Parameters.AddWithValue("rno", this.RentNo);
            command.Parameters.AddWithValue("db", this.Begin.HasValue ? (object)this.Begin.Value : (object)DBNull.Value);
            command.Parameters.AddWithValue("de", this.End.HasValue ? (object)this.End.Value : (object)DBNull.Value);
            command.Parameters.AddWithValue("pp", this.PricePerHour.HasValue ? (object)this.PricePerHour.Value : 0);
            command.Parameters.AddWithValue("sp", this.SumPrice.HasValue ? (object)this.SumPrice.Value : 0);
            command.Parameters.AddWithValue("un", this.Hours.HasValue ? (object)this.Hours.Value : 0);
            command.Parameters.AddWithValue("del", false);

            int result = command.ExecuteNonQuery();
            connection.Close();
            return result;
        }

        public int Endrent()
        {
            int result = 0;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = this.connection;
            connection.Open();
            if (!this.End.HasValue)
            {
                this.End = DateTime.Now;
            }
            if (this.RentId.HasValue)
            {
                this.End = DateTime.Now;
                TimeSpan difference = (DateTime)this.End - (DateTime)this.Begin;
                this.Hours = (long?)Math.Ceiling(difference.TotalHours);
                this.SumPrice = this.Hours * this.PricePerHour;


                command.CommandText =
                $"update {TABLE} set dateend = :de, sumprice = :sp, hours = :un where rent_id = :rid";

                command.Parameters.AddWithValue("rid", this.RentId.Value);
                command.Parameters.AddWithValue("cid", this.CustomerId.Value);
                command.Parameters.AddWithValue("caid", this.CarId.Value);
                command.Parameters.AddWithValue("rno", this.RentNo);
                command.Parameters.AddWithValue("db", this.Begin.HasValue ? (object)this.Begin.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("de", this.End.HasValue ? (object)this.End.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("sp", this.SumPrice.HasValue ? (object)this.SumPrice.Value : 0);
                command.Parameters.AddWithValue("un", this.Hours.HasValue ? (object)this.Hours.Value : 0);

                result = +command.ExecuteNonQuery();

                command.CommandText =
                  $"update {TABLECar} set in_use = false where car_id = :cid ";
                command.Parameters.AddWithValue("cid", this.CustomerId.Value);
                result = +command.ExecuteNonQuery();
            }
            connection.Close();
            return result;
        }



        public static explicit operator Rent(RentContract rentContract)
        {
            return rentContract == null ? null : new Rent()
            {
                RentId = rentContract.RentId,
                CustomerId = rentContract.CustomerId,
                CarId = rentContract.CarId,
                Begin = rentContract.Begin,
                End = rentContract.End,
                SumPrice = rentContract.SumPrice,
                Hours = rentContract.Hours,
                Name = rentContract.Name,
                FamilyName = rentContract.FamilyName,
                Modell = rentContract.Modell,
                Brand = rentContract.Brand,
                PricePerHour = rentContract.PricePerHour
                
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
