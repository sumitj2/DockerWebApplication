using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using System.Threading.Tasks;

namespace DockerWebApplication.Models
{
    public class Repository : IRepository
    {
        private HttpClient _ihttpclient;

        public Repository(HttpClient httpClient)
        {
            _ihttpclient = httpClient;
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
        public async Task<List<Employee>> GetAllEmployee()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            Console.WriteLine(hostName);
            // Get the IP  
            string myIP = GetIPAddress();//Dns.GetHostByName(hostName).AddressList[0].ToString();




            List<Employee> model = null;
            var task = _ihttpclient.GetAsync($"https://{myIP}:5001/api/Employe")
          .ContinueWith((taskwithresponse) =>
          {
              var response = taskwithresponse.Result;
              var jsonString = response.Content.ReadAsStringAsync();
              jsonString.Wait();
              model = JsonConvert.DeserializeObject<List<Employee>>(jsonString.Result);
          });
            task.Wait();

            return model;
                    
        }

        public async Task<int> SaveEmployee(Employee employee)
        {

            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            Console.WriteLine(hostName);
            // Get the IP  
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            var jsonInString = JsonConvert.SerializeObject(employee);

            var result = await _ihttpclient.PostAsync($"https://{myIP}:5001/api/Employe", new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            if (result != null)
            {
                return 1;
            }
            return 0;

        }
    }
}
