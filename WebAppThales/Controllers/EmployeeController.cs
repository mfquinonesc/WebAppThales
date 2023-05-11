using Microsoft.AspNetCore.Mvc;
using WebAppThales.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace WebAppThales.Controllers
{
    public class EmployeeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            List<EmployeeViewModel> employees= new List<EmployeeViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://dummy.restapiexample.com/api/v1/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseTask = client.GetAsync("employees");
                    responseTask.Wait();
                    //HttpResponseMessage getData = await client.GetAsync("employees");
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        string jsonResult = result.Content.ReadAsStringAsync().Result;
                        var arrJSON = (JObject)JsonConvert.DeserializeObject(jsonResult);
                        var data = arrJSON.GetValue("data");
                        employees = data.ToObject<List<EmployeeViewModel>>();
                    }
                    else
                    {
                        Console.WriteLine("Too Many Requests");
                    }
                }
            }
            catch (Exception ex)
            {

            }




           
            return View();

            }
     
        
    }
}
