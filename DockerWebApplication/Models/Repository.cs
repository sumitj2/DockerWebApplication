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
            // Get the IP  
            // string myIP = GetIPAddress();

            List<Employee> model = null;
            await _ihttpclient.GetAsync($"http://{myIP}:{port}/api/Employe")
          .ContinueWith((taskwithresponse) =>
          {
              var response = taskwithresponse.Result;
              var jsonString = response.Content.ReadAsStringAsync();
              jsonString.Wait();
              model = JsonConvert.DeserializeObject<List<Employee>>(jsonString.Result);
          });

            return model;
        }

        public async Task<int> SaveEmployee(Employee employee)
        {
            var jsonInString = JsonConvert.SerializeObject(employee);

            var result = await _ihttpclient.PostAsync($"http://{myIP}:{port}/api/Employe", new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            if (result != null)
            {
                return 1;
            }
            return 0;
        }
    }
}
