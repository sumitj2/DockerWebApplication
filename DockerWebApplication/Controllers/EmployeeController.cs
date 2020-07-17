using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DockerWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace DockerWebApplication.Controllers
{
    public class EmployeeController : Controller
    {
        IRepository _repo;
        public EmployeeController(IRepository Repo)
        {
            _repo = Repo;
        }
        public IActionResult Index()
        {
            List<Employee> employeelist = Task.Run(() => _repo.GetAllEmployee()).Result;

            return View(employeelist);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee e)
        {
            if (ModelState.IsValid)
            {
                var result = _repo.SaveEmployee(e);

                return RedirectToAction("index", "Employee");
            }

            return View();

        }
    }
}
