using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebSharper;
using WebSharper.Sitelets;

namespace WebSharper.AspNetMvc.Tests
{
    public abstract class Sitelet
    {
        [EndPoint("/sitelet")]
        public class Index { }

        [EndPoint("/sitelet/{name}")]
        public class Named {
            public string name;
        }

        [Website]
        public static Sitelet<object> Website =>
            new SiteletBuilder()
                .With("/test", ctx =>
                    Content.Text("Sitelet with a plain string as URL")
                )
                .With<Index>((ctx, _) =>
                    Content.Text("Welcome to my sitelet!")
                )
                .With<Named>((ctx, ep) =>
                    Content.Text(String.Format("Hi there, {0}", ep.name))
                )
                .With("/Home/Index", ctx =>
                    Content.Text(
                        "This sitelet content overrides a URL that would otherwise be handled by the MVC application.<br/>" +
                        "Uncomment the 'OverrideHandler = false' line in Global.asax to change this behavior.")
                        .WithContentType("text/html")
                )
                .Install();
    }
}