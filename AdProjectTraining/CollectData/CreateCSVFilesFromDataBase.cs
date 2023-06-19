using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectData
{
    internal static class CreateCSVFilesFromDataBase
    {
        public static void CreateFile()
        {
            string connectionString = "Server=.;Database=AdvertisementsDB;Integrated Security=True;";
            string query = @"SELECT TOP (10)  Advertisements.Area, Advertisements.BuildYear, Advertisements.Rooms, Advertisements.Floor, Advertisements.Elevator, Advertisements.Parking, Advertisements.Storage,  Locations.Name AS LocationName, Advertisements.TotalPrice
                           FROM     Advertisements INNER JOIN
                           Locations ON Advertisements.LocationId = Locations.Id
                           ORDER BY Advertisements.Id DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        using (StreamWriter writer = new StreamWriter("output.csv", false, Encoding.UTF8))
                        {
                            using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                            {
                                DataTable schemaTable = reader.GetSchemaTable();
                                foreach (DataRow row in schemaTable.Rows)
                                {
                                    csvWriter.WriteField(row["ColumnName"].ToString());

                                }
                                csvWriter.NextRecord();

                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        csvWriter.WriteField(reader[i]);
                                    }
                                    csvWriter.NextRecord();
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}
