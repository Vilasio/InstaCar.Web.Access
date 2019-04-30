using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace InstaCar.Web.Access.Database
{
    public class ImageCar
    {
        //----------------------------------------------------------------------------------------------
        //Const
        //----------------------------------------------------------------------------------------------
        #region const
        private const string TABLE = "InstaCar.image";
        private const string COLUMN = "image_id, car_id, picture, kind, main, description, accident";
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
        public ImageCar()
        {

        }

        public ImageCar(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public ImageCar(NpgsqlConnection connection, Vehicle vehicle)
        {
            this.connection = connection;
            this.Vehicle = vehicle;
        }
        #endregion
        //----------------------------------------------------------------------------------------------
        //Property
        //----------------------------------------------------------------------------------------------
        #region property
        public long? ImageId { get; set; }
        public Image Image { get; set; }
        public Vehicle Vehicle { get; set; }
        public byte[] Picture { get; set; }
        public string Kind { get; set; }
        public bool Main { get; set; }
        public string Description { get; set; }
        public bool Accident { get; set; }
        #endregion
        //----------------------------------------------------------------------------------------------
        //Static
        //----------------------------------------------------------------------------------------------
        #region static


        public static List<ImageCar> GetList(NpgsqlConnection connection, Vehicle vehicle)
        {
            List<ImageCar> images = new List<ImageCar>();
            if (vehicle.CarId.HasValue)
            {
                NpgsqlCommand command = new NpgsqlCommand($"Select image_id, picture, kind, main, description, accident from {TABLE} where car_id = :cid", connection);
                command.Parameters.AddWithValue("cid", vehicle.CarId.Value);
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ImageCar imageCar = new ImageCar(connection, vehicle);

                    imageCar.ImageId = reader.GetInt64(0);
                    imageCar.Picture = reader.IsDBNull(1) ? null : (byte[])reader.GetValue(1);
                    imageCar.Kind = reader.IsDBNull(2) ? null : reader.GetString(2);
                    imageCar.Main = reader.IsDBNull(3) ? false : reader.GetBoolean(3);
                    imageCar.Description = reader.IsDBNull(4) ? null : reader.GetString(4);
                    imageCar.Accident = reader.IsDBNull(5) ? false : reader.GetBoolean(5);

                    MemoryStream memoryStream = new MemoryStream(imageCar.Picture);
                    imageCar.Image = Image.FromStream(memoryStream);
                    memoryStream.Close();

                    images.Add(imageCar);

                }
                reader.Close();
            }
            return images;
        }

        public static ImageCar GetMain(NpgsqlConnection connection, long? CarId)
        {
            ImageCar image = null;
            if (CarId.HasValue)
            {
                NpgsqlCommand command = new NpgsqlCommand($"Select image_id, picture, kind, main, description from {TABLE} where car_id = :cid and main = true", connection);
                command.Parameters.AddWithValue("cid", CarId.Value);
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    image = new ImageCar(connection);

                    image.ImageId = reader.GetInt64(0);
                    image.Picture = reader.IsDBNull(1) ? null : (byte[])reader.GetValue(1);
                    image.Kind = reader.IsDBNull(2) ? null : reader.GetString(2);
                    image.Main = reader.IsDBNull(3) ? false : reader.GetBoolean(3);
                    image.Description = reader.IsDBNull(4) ? null : reader.GetString(4);
                    image.Accident = reader.IsDBNull(5) ? false : reader.GetBoolean(5);


                    MemoryStream memoryStream = new MemoryStream(image.Picture);
                    image.Image = Image.FromStream(memoryStream);
                    memoryStream.Close();



                }
                reader.Close();
            }
            return image;
        }

        public static Image GetMainImage(NpgsqlConnection connection, long? CarId)
        {
            Image image = null;
            Byte[] pic = null;
            if (CarId.HasValue)
            {
                NpgsqlCommand command = new NpgsqlCommand($"Select picture from {TABLE} where car_id = :cid and main = true", connection);
                command.Parameters.AddWithValue("cid", CarId.Value);
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    pic = reader.IsDBNull(0) ? null : (byte[])reader.GetValue(0);
                    MemoryStream memoryStream = new MemoryStream(pic);
                    image = Image.FromStream(memoryStream);
                    memoryStream.Close();
                }

                reader.Close();
            }
            return image;
        }

        public static Byte[] GetMainImageByte(long? CarId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.AppSettings["Connection"]);
            connection.Open();
            Byte[] pic = null;
            if (CarId.HasValue)
            {
                NpgsqlCommand command = new NpgsqlCommand($"Select picture from {TABLE} where car_id = :cid and main = true", connection);
                command.Parameters.AddWithValue("cid", CarId.Value);
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    pic = reader.IsDBNull(0) ? null : (byte[])reader.GetValue(0);
                }

                reader.Close();
            }
            connection.Close();

            return pic;
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

            if (this.ImageId.HasValue)
            {
                command.CommandText =
                    $"update {TABLE} set picture = :pi, kind = :ki, main = :ma, description = :de";
            }
            else
            {
                command.CommandText = $"select nextval('{TABLE}_seq')";
                this.ImageId = (long?)command.ExecuteScalar();
                command.CommandText = $" insert into {TABLE} ({COLUMN})" +
                    $" values(:iid, :cid, :pi, :ki, :ma, :de, :ac)";
            }

            command.Parameters.AddWithValue("iid", this.ImageId.Value);
            command.Parameters.AddWithValue("cid", this.Vehicle.CarId.Value);
            command.Parameters.AddWithValue("pi", this.Picture == null ? (object)DBNull.Value : this.Picture);
            command.Parameters.AddWithValue("ki", String.IsNullOrEmpty(this.Kind) ? (object)DBNull.Value : this.Kind);
            command.Parameters.AddWithValue("ma", this.Main);
            command.Parameters.AddWithValue("de", String.IsNullOrEmpty(this.Description) ? (object)DBNull.Value : this.Description);
            command.Parameters.AddWithValue("ac", this.Accident);

            int result = command.ExecuteNonQuery();

            return result;
        }

        public int Delete()
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = this.connection;
            if (this.ImageId.HasValue)
            {
                int result = 0;

                command.CommandText = $"delete from {TABLE} where image_id = :iid";
                command.Parameters.AddWithValue("iid", this.ImageId.Value);
                result = command.ExecuteNonQuery();
                return result;
            }
            else
            {
                return 0;
            }
        }

        #endregion
        //----------------------------------------------------------------------------------------------
        //Private
        //----------------------------------------------------------------------------------------------
        #region private

        #endregion
    }
}