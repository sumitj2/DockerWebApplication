using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<List<Employee>> GetAllEmployee()
        {
            //employees = await result.Content.ReadAsAsync < IList < Employee >>
            //var employeelist = await _ihttpclient.GetAsync("https://localhost:32774/api/Employe");
            //return employeelist;

            //employeelist.EnsureSuccessStatusCode();
            // var responseStream = await employeelist.Content.ReadAsStringAsync();

            List<Employee> model = null;
            var task = _ihttpclient.GetAsync("https://localhost:32774/api/Employe")
          .ContinueWith((taskwithresponse) =>
          {
              var response = taskwithresponse.Result;
              var jsonString = response.Content.ReadAsStringAsync();
              jsonString.Wait();
              model = JsonConvert.DeserializeObject<List<Employee>>(jsonString.Result);
          });
            task.Wait();

            return model;


            //IEnumerable<Employee> employees = null;

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://localhost:32774/api/Employe");

            //    var result = await client.GetAsync("employees/get");

            //    if (result.IsSuccessStatusCode)
            //    {
            //        employees = await result.Content.ReadAsAsync<List<Employee>>();
            //    }
            //    else
            //    {
            //        employees = Enumerable.Empty<Employee>();
            //        ModelState.AddModelError(string.Empty, "Server error try after some time.");
            //    }
            //}
            //return View(employees);
        }

        public async Task<int> SaveEmployee(Employee employee)
        {

            var jsonInString = JsonConvert.SerializeObject(employee);

            var result = await _ihttpclient.PostAsync("https://localhost:32774/api/Employe", new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            if (result != null)
            {
                return 1;
            }
            return 0;

        }
    }
}
