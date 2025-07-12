using System.Security.Claims;

namespace EndPoint.Repositories
{
    public static class ClaimStore
    {

        public static List<Claim> AllClaims = new List<Claim>()
        {
        new Claim(ClaimTypeStore.EmployeeList,true.ToString()),
        new Claim(ClaimTypeStore.EmployeeDetails,true.ToString()),
        new Claim(ClaimTypeStore.EmployeeEdit,true.ToString()),
        new Claim(ClaimTypeStore.AddEmployee,true.ToString())
        };


    }
    public static class ClaimTypeStore
    {

        public const string EmployeeList = "EmployeeList";
        public const string EmployeeDetails = "EmployeeDetails";
        public const string EmployeeEdit = "EmployeeEdit";
        public const string AddEmployee = "AddEmployee";
    }
}