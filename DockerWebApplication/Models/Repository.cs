using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

using System.Threading.Tasks;

namespace DockerWebApplication.Models
{
    public class Repository : IRepository
    {
        private HttpClient _ihttpclient;

        private readonly string myIP;
        private readonly string port;

        public Repository(HttpClient httpClient)
        {
            _ihttpclient = httpClient;
          

            myIP = Environment.GetEnvironmentVariable("SERVICE-ADRESS");
            port = Environment.GetEnvironmentVariable("SERVICE-PORT");
        }

        static string GetIPAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> GetAllEmployee()
        {           
            List<Employee> model = null;
            using (var client = new HttpClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
               // request.RequestUri = new Uri($"http://{myIP}:{port}/api/Employe"); // ASP.NET 2.x
                request.RequestUri = new Uri($"http://{myIP}/api/Employe");
                var response = await client.SendAsync(request);
                var jsonString = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<List<Employee>>(jsonString);
            }
            return model;
        }

        public async Task<int> SaveEmployee(Employee employee)
        {
            var jsonInString = JsonConvert.SerializeObject(employee);
            using (var client = new HttpClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
               // request.RequestUri = new Uri($"http://{myIP}:{port}/api/Employe"); // ASP.NET 2.x
                request.RequestUri = new Uri($"http://{myIP}/api/Employe");
                var response = await client.PostAsync(request.RequestUri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
                if (response != null)
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}
