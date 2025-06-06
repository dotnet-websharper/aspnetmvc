// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
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
    [JavaScript]
    public class ClientControl : Web.Control
    {
        private string _init;

        [JavaScript(false)]
        public ClientControl(string init)
        {
            _init = init;
        }

        public override IControlBody Body
        {
            get
            {
                var v = Var.Create(_init);
                return div(
                    "Type your name: ",
                    input(v),
                    button("Send to server", async () =>
                    {
                        var s = await Rpc.ServerFunc(v.Value);
                        JS.Alert(s);
                    })
                );
            }
        }
    }
}