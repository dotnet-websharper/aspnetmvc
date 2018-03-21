using System.Web;
using System.Web.Mvc;

namespace WebSharper.AspNetMvc.Tests
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
