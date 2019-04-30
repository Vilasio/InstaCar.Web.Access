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
    public class Customer
    {
        //----------------------------------------------------------------------------------------------
        //Const
        //----------------------------------------------------------------------------------------------
        #region const
        private const string TABLE = "InstaCar.customer";
        private const string COLUMN = "customer_id, customer_no, name, familyname, street, housenr, postcode, city, email, telefon, iban, bic, nickname";
        private const string COLUMNPW = "customer_id, customer_no, name, familyname, street, housenr, postcode, city, email, telefon, iban, bic, password, nickname";
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
        public Customer()
        {
            this.connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
        }

        public Customer(NpgsqlConnection connection)
        {
            this.connection = connection;
        }
        #endregion
        //----------------------------------------------------------------------------------------------
        //Property
        //----------------------------------------------------------------------------------------------
        #region property
        public long? CustomerId { get; set; }
        public string CustomerNo { get; set; }
        public string Name { get; set; }
        public string Familyname { get; set; }
        public string Street { get; set; }
        public long? HouseNr { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }


        #endregion
        //----------------------------------------------------------------------------------------------
        //Static
        //----------------------------------------------------------------------------------------------
        #region static

        public static List<Customer> GetAllCustomer(NpgsqlConnection connection)
        {
            List<Customer> allCustomers = new List<Customer>();
            Customer customer = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select {COLUMN} from {TABLE};";

            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                allCustomers.Add(
                    customer = new Customer(connection)
                    {
                        CustomerId = reader.GetInt64(0),
                        CustomerNo = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Familyname = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Street = reader.IsDBNull(4) ? null : reader.GetString(4),
                        HouseNr = reader.IsDBNull(5) ? 0 : reader.GetInt64(5),
                        Postcode = reader.IsDBNull(6) ? null : reader.GetString(6),
                        City = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Email = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Telefon = reader.IsDBNull(9) ? null : reader.GetString(9),
                        Iban = reader.IsDBNull(10) ? null : reader.GetString(10),
                        Bic = reader.IsDBNull(11) ? null : reader.GetString(11),
                        Nickname = reader.IsDBNull(12) ? null : reader.GetString(12)

                    }
                );
            }
            reader.Close();
            return allCustomers;
        }

        static Customer GetSpecificCustomer(int key)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
            Customer customer = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select {COLUMN} from {TABLE} where customer_id = :id;";
            command.Parameters.AddWithValue("id", key);
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                customer = new Customer(connection)
                {
                    CustomerId = reader.GetInt64(0),
                    CustomerNo = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Familyname = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Street = reader.IsDBNull(4) ? null : reader.GetString(4),
                    HouseNr = reader.IsDBNull(5) ? 0 : reader.GetInt64(5),
                    Postcode = reader.IsDBNull(6) ? null : reader.GetString(6),
                    City = reader.IsDBNull(7) ? null : reader.GetString(7),
                    Email = reader.IsDBNull(8) ? null : reader.GetString(8),
                    Telefon = reader.IsDBNull(9) ? null : reader.GetString(9),
                    Iban = reader.IsDBNull(10) ? null : reader.GetString(10),
                    Bic = reader.IsDBNull(11) ? null : reader.GetString(11),
                    Nickname = reader.IsDBNull(12) ? null : reader.GetString(12)
                };
            }
            reader.Close();
            return customer;
        }

        public static Customer LoginCheck(string username, string password)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
            connection.Open();
            Customer customer = null;
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"Select {COLUMN} from {TABLE} where nickname = :us and password = crypt(:pa, password);";
            command.Parameters.AddWithValue(":us", String.IsNullOrEmpty(username) ? "" : username);
            command.Parameters.AddWithValue(":pa", String.IsNullOrEmpty(password) ? "" : password);
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    customer = new Customer(connection)
                    {
                        CustomerId = reader.GetInt64(0),
                        CustomerNo = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Familyname = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Street = reader.IsDBNull(4) ? null : reader.GetString(4),
                        HouseNr = reader.IsDBNull(5) ? 0 : reader.GetInt64(5),
                        Postcode = reader.IsDBNull(6) ? null : reader.GetString(6),
                        City = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Email = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Telefon = reader.IsDBNull(9) ? null : reader.GetString(9),
                        Iban = reader.IsDBNull(10) ? null : reader.GetString(10),
                        Bic = reader.IsDBNull(11) ? null : reader.GetString(11),
                        Nickname = reader.IsDBNull(12) ? null : reader.GetString(12)
                    };
                }
                reader.Close();
                
            }
            connection.Close();
            return customer;

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

            if (this.CustomerId.HasValue)
            {
                if (this.Password != null)
                {
                    command.CommandText =
                    $"update {TABLE} set  name = :na, familyname = :fn, street = :st, housenr = :hn, postcode = :pc, city = :ci, iban = :ib, bic = :bi, password = CRYPT(:pa, GEN_SALT('bf')), nickname = :ni where customer_id = :cid";

                }
                else
                {
                    command.CommandText =
                    $"update {TABLE} set  name = :na, familyname = :fn, street = :st, housenr = :hn, postcode = :pc, city = :ci, iban = :ib, bic = :bi, nickname = :ni where customer_id = :cid";
                }
            }
            else
            {
                command.CommandText = $"select nextval('{TABLE}_seq')";
                this.CustomerId = (long?)command.ExecuteScalar();
                command.CommandText = $" insert into {TABLE} ({COLUMNPW})" +
                    $" values(:cid, :cno, :na, :fn, :st, :hn, :pc, :ci, :em, :te, :ib, :bi, :pa, :ni)";
            }

            if (this.CustomerNo == null || this.CustomerNo == "")// CustomerNO generieren--------------------------
            {
                DateTime now = DateTime.Now;
                int year = now.Year;
                string number = this.CustomerId.ToString();
                number = number.PadLeft(6, '0');
                this.CustomerNo = $"{year}/{number}";
            }
            command.Parameters.AddWithValue("cid", this.CustomerId.Value);
            command.Parameters.AddWithValue("cno", this.CustomerNo);
            command.Parameters.AddWithValue("na", String.IsNullOrEmpty(this.Name) ? (object)DBNull.Value : this.Name);
            command.Parameters.AddWithValue("fn", String.IsNullOrEmpty(this.Familyname) ? (object)DBNull.Value : this.Familyname);
            command.Parameters.AddWithValue("st", String.IsNullOrEmpty(this.Street) ? (object)DBNull.Value : this.Street);
            command.Parameters.AddWithValue("hn", this.HouseNr.HasValue ? (object)this.HouseNr.Value : 3);
            command.Parameters.AddWithValue("pc", String.IsNullOrEmpty(this.Postcode) ? (object)DBNull.Value : this.Postcode);
            command.Parameters.AddWithValue("ci", String.IsNullOrEmpty(this.City) ? (object)DBNull.Value : this.City);
            command.Parameters.AddWithValue("ib", String.IsNullOrEmpty(this.Iban) ? (object)DBNull.Value : this.Iban);
            command.Parameters.AddWithValue("bi", String.IsNullOrEmpty(this.Bic) ? (object)DBNull.Value : this.Bic);
            command.Parameters.AddWithValue("em", String.IsNullOrEmpty(this.Email) ? (object)DBNull.Value : this.Email);
            command.Parameters.AddWithValue("te", String.IsNullOrEmpty(this.Telefon) ? (object)DBNull.Value : this.Telefon);
            command.Parameters.AddWithValue("pa", String.IsNullOrEmpty(this.Password) ? (object)DBNull.Value : this.Password);
            command.Parameters.AddWithValue("ni", String.IsNullOrEmpty(this.Nickname) ? (object)DBNull.Value : this.Nickname);

            return command.ExecuteNonQuery();
        }

        public int ChangePassword(string password, string newPassword)
        {
            int result = 0; 
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = this.connection;
            command.CommandText = $"Select {COLUMN} from {TABLE} where nickname = :ni and password = crypt(:pa, password);";
            command.Parameters.AddWithValue(":ni", String.IsNullOrEmpty(this.Nickname) ? "" : this.Nickname);
            command.Parameters.AddWithValue(":pa", String.IsNullOrEmpty(password) ? "" : password);
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows && newPassword != "")
            {
                reader.Close();
                command.CommandText = $"update {TABLE} set password = CRYPT(:npa, GEN_SALT('bf')) where customer_id = :cid";
                command.Parameters.AddWithValue("cid", this.CustomerId.Value);
                command.Parameters.AddWithValue("npa", newPassword);
                result = command.ExecuteNonQuery();
            }
            else
            {
                reader.Close();

            }
            return result;
        }

        public static explicit operator Customer(CustomerContract customerContract)
        {
            return customerContract == null ? null : new Customer()
            {
                CustomerId = customerContract.CustomerId,
                CustomerNo = customerContract.CustomerNo,
                Name = customerContract.Name,
                Familyname = customerContract.Familyname,
                Street = customerContract.Street,
                HouseNr = customerContract.HouseNr,
                Postcode = customerContract.Postcode,
                City = customerContract.City,
                Email = customerContract.Email,
                Telefon = customerContract.Telefon,
                Iban = customerContract.Iban,
                Bic = customerContract.Bic,
                Password = customerContract.Password,
                Nickname = customerContract.Nickname
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
