using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace Repository
{
    public class AppSettingsReader
    {
        private static readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        private static JObject _settings;

        static AppSettingsReader()
        {
            try
            {
                string jsonContent = File.ReadAllText(_filePath);
                _settings = JObject.Parse(jsonContent);
            }
            catch (Exception e)
            {
                throw new Exception($"Error reading appsettings.json: {e.Message}");
            }
        }

        public static string GetString(string s1, string s2)
        {
            
            return _settings[s1]?[s2]?.ToString() ??
                throw new Exception("Fail to read appsetting file");

        }

        public static string GetAccount(string stringJson)
        {
            return _settings["Admin"]?[stringJson]?.ToString();
        }
        public static string GetConnectionString()
        {
            return _settings["ConnectionStrings"]?["DefaultConnection"]?.ToString() ??
                   throw new Exception("Connection string 'DefaultConnection' not found in appsettings.json.");
        }
        public static void display()
        {
            Debug.WriteLine("AppSettingsReader:");
            Debug.WriteLine($"Connection String: {GetConnectionString()}");
            Debug.WriteLine($"Admin Username: {GetAccount("Username")}");
            Debug.WriteLine($"Admin Password: {GetAccount("Password")}");
        }
    }
}
