using Microsoft.AspNetCore.Mvc;
using WebAppThales.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace WebAppThales.Controllers
{
    public class EmployeeController : Controller
    {

        private const string API_EMPLOYEES = "http://dummy.restapiexample.com/api/v1/";

        /// <summary>
        /// This method is responsible for retrieving the employee with the specified Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmployeeViewModel GetEmployee(int id)
        {
            EmployeeViewModel employee = new EmployeeViewModel();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(API_EMPLOYEES);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseTask = client.GetAsync($"employee/{id}");
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        string jsonResult = result.Content.ReadAsStringAsync().Result;
                        var arrJSON = (JObject)JsonConvert.DeserializeObject(jsonResult);
                        var data = arrJSON.GetValue("data");
                        employee = data.ToObject<EmployeeViewModel>();
                    }
                }
            }
            catch(Exception ex)
            {
                employee = null;
            }
            return employee;
        }


        /// <summary>
        /// This method is responsible for retrieving the list of all employees.
        /// </summary>
        /// <returns></returns>
        public List<EmployeeViewModel> GetEmployeesList()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(API_EMPLOYEES);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseTask = client.GetAsync("employees");
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        string jsonResult = result.Content.ReadAsStringAsync().Result;
                        var arrJSON = (JObject)JsonConvert.DeserializeObject(jsonResult);
                        var data = arrJSON.GetValue("data");
                        employees = data.ToObject<List<EmployeeViewModel>>();
                    }                   
                }               
            }
            catch (Exception ex)
            {
                employees = null;
            }
            return employees;
        }


        [HttpGet]
        public IActionResult Employee(int id)
        {            
            EmployeeViewModel employee = this.GetEmployee(id);
            if(employee != null && employee.Id > 0)
            {
                employee.Employee_anual_salary = employee.Employee_salary * 12;
                ViewData["Title"] = employee.Employee_name;
                return View(employee);
            }
            else
            {
                return Redirect("/Employee/Employees");
            }            
        }     


        [HttpGet]
        public IActionResult Employees()
        {
            List<EmployeeViewModel> employees = this.GetEmployeesList(); ;
            if(employees != null)
            {
                if(employees.Count > 0) 
                {
                    foreach (EmployeeViewModel model in employees)
                    {
                        model.Employee_anual_salary = model.Employee_salary * 12;
                    }
                }
                else
                {
                   return Redirect("/Employee/Message");
                }
            }   
            return View(employees);
        }


        [HttpGet]
        public IActionResult Search(string Id)
        { 
            int id = 0;
            if (int.TryParse(Id, out id))
            {               
                return Redirect($"/Employee/Employee/{id}");
            }
            else
            {
                return Redirect("/Employee/Employees");
            }        
        }


        public IActionResult Message() 
        { 
            return View();
        }
    }
}
