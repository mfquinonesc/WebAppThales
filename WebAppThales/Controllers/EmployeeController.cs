using Microsoft.AspNetCore.Mvc;
using WebAppThales.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.AspNetCore.Http.Extensions;

namespace WebAppThales.Controllers
{
    public class EmployeeController : Controller
    {
        private List<EmployeeViewModel> GetEmployeesList()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://dummy.restapiexample.com/api/v1/");
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
            EmployeeViewModel employee;
            List<EmployeeViewModel> employees= GetEmployeesList();  
            employee = employees.Find(employee=> employee.Id == id);
            if(employee != null)
            {
                employee.Employee_anual_salary = employee.Employee_salary * 12;
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
            }   
            return View(employees);
        }


        [HttpGet]
        public IActionResult Search(string Id)
        {            
            int id = 0;
            if (int.TryParse(Id, out id))
            {

                return Redirect($"/Employee/Employee/{Id}");
            }
            else
            {
                return Redirect("/Employee/Employees");
            }        
        }
    }
}
