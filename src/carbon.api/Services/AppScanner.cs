using System;
using System.Linq;
using System.Reflection;

namespace carbon.api.Services
{
    public class AppScanner
    {
        public static Assembly[] GetCarbonAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                
            return  assemblies.Where(a => a.FullName.Contains("carbon")).ToArray();
        }

    }
}