using System;
using System.Collections.Generic;
using System.Linq;
using WebSharper;
using WebSharper.Core;
using WebSharper.JavaScript;
using WebSharper.Web;
using WebSharper.UI;
using WebSharper.UI.Client;
using static WebSharper.UI.Client.Html;

namespace WebSharper.AspNetMvc.Tests
{
    [Serializable]
    public class Client : Web.Control
    {
        [JavaScript]
        public override IControlBody Body
        {
            get
            {
                var v = Var.Create("");
                return div(
                    "Type your name: ",
                    input(v),
                    button("Send to server", async () =>
                    {
                        var s = await Rpc.ServerFunc(v.Value);
                        JSModule.Alert(s);
                    })
                );
            }
        }
    }
}