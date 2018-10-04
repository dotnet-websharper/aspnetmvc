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