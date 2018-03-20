using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebSharper;

namespace WebSharper.AspNetMvc.Tests
{
    public class Rpc
    {
        [Remote]
        public static async Task<string> ServerFunc(string s)
        {
            return $"Hi from the server, {s}!";
        }
    }
}