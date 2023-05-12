using WebAppThales.Controllers;
using WebAppThales.Models;

namespace WebAppThalesTest
{
    [TestClass]
    public class UnitTestWebAppThales
    {
        /// <summary>
        /// Here is the test for the method that is eable to get the list of employees from the API
        /// This method checks whether or not the total of elements in the list is 24. 
        /// It also checks whether the total of elements is cero or is null  
        /// </summary>
        [TestMethod]
        public void TestMethod_GetEmployeesList()
        {            
            EmployeeController controller = new EmployeeController();
            List <EmployeeViewModel> employees = controller.GetEmployeesList();
            if(employees == null)
            {
                Assert.AreEqual(null, employees);                
            }
            else if(employees.Count == 0)
            {
                Assert.AreEqual(0, employees.Count);
            }
            else
            {
                Assert.AreEqual(24, employees.Count);
            }                  
        }
    }
}