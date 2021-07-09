using CountriesAPI.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CountriesAPI.Services
{
    public class DataService
    {
        private SqliteConnection connection;

        private SqliteCommand command;

        private DialogService dialogService;

        public DataService()
        {
            dialogService = new DialogService();

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\Countries.sqlite";

            try
            {
                connection = new SqliteConnection("Data Source=" + path);
                connection.Open();

                string sqlcommand = "create table if not exists countries(name varchar(75), capital varchar(75), region varchar(75)," +
                    "subregion varchar(75), area real, flag varchar(350), alpha3Code varchar(10), population int, gini real)";

                command = new SqliteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }

        public void SaveData(List<Country> Countries)
        {
            try
            {
                foreach (var country in Countries)
                {
                    string sql = string.Format("insert into Countries (name, capital, region, subregion, area, flag, " +
                    "alpha3code, population, gini)values(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\")",
                        country.name, country.capital,
                        country.region, country.subregion, 
                        country.area, country.flag, country.alpha3code, 
                        country.population, country.gini);

                    command = new SqliteCommand(sql, connection);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
            }
        }

        public List<Country> GetData()
        {
            List<Country> countries = new List<Country>();

            try
            {
                string sql = "select name, capital, region, subregion, area, flag, alpha3code, population, gini from Countries";

                command = new SqliteCommand(sql, connection);

                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    countries.Add(new Country
                    {
                        name = (string) reader["name"],
                        capital = (string)reader["capital"],
                        region = (string)reader["region"],
                        subregion = (string)reader["subregion"],
                        area = (double)reader["area"],
                        flag = (string)reader["flag"],
                        alpha3code = (string)reader["alpha3code"],
                        population = (int)reader["population"],
                        gini = (double)reader["gini"]
                    }); 
                }
                connection.Close();
                return countries;

            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
                return null;
            }
        }
        public async Task DownloadImages(List<Country> countries)
        {
            try
            {
                WebClient client = new WebClient();

                string paste = "Flags";

                if (!Directory.Exists(paste))
                {
                    Directory.CreateDirectory(paste);
                }
                foreach (var country in countries)
                {
                    Uri url = new Uri(country.flag);
                    string fileName = Path.GetFileName(url.AbsolutePath);
                    string localFile = Environment.CurrentDirectory + "/Flags/";

                    await client.DownloadFileTaskAsync(url, localFile + fileName);
                }
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error", e.Message);
            }
        }
    }
}
