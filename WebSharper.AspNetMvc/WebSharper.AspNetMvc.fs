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
namespace WebSharper.AspNetMvc

open System
open System.IO
open System.Runtime.CompilerServices
open System.Web
open System.Web.Mvc
open WebSharper
open WebSharper.Web

module ScriptManager =

    [<Literal>]
    let private smKey = "WebSharper.AspNetMvc.ScriptManager"

    /// <summary>
    /// Register a control so that its dependencies are rendered by the script manager.
    /// </summary>
    /// <param name="control">The control to render.</param>
    /// <returns>The code to insert the control in the page.</returns>
    let Register (control: IControl) =
        let items = HttpContext.Current.Items
        let sm =
            match items.[smKey] with
            | :? Web.ScriptManager as sm -> sm
            | _ ->
                let sm = new Web.ScriptManager()
                items.[smKey] <- sm
                sm
        let id = sm.Register(Some control.Id, control, Shared.Metadata, Shared.Json)
        MvcHtmlString("<div id=\"" + id + "\"></div>")

    /// <summary>
    /// Render the head tags (style and script) required by the controls in the current page.
    /// </summary>
    /// <returns>The code to insert inside the page's <head> tag.</returns>
    let Head() =
        match HttpContext.Current.Items.[smKey] with
        | :? Web.ScriptManager as sm ->
            use sw = new StringWriter()
            use tw = new UI.HtmlTextWriter(sw)
            sm.RenderControl(tw)
            MvcHtmlString(sw.ToString())
        | _ -> MvcHtmlString("")

/// <summary>
/// Create a filter that enables WebSharper sitelets and remoting.
/// </summary>
[<Obsolete "Filter is not necessary anymore.">]
type Filter () =

    /// When both the sitelet and ASP.NET MVC accept a URL, the sitelet is served.
    /// Default: true
    [<Obsolete "SiteletsOverrideMvc is moved to WebSharper.Sitelets.HttpModule.OverrideHandler. Set it globally in Application_Start.">]
    member this.SiteletsOverrideMvc
        with get() = WebSharper.Sitelets.HttpModule.OverrideHandler
        and set v = WebSharper.Sitelets.HttpModule.OverrideHandler <- v

    /// The name of the WebSharper sitelets module declared in Web.config.
    /// Default: "WebSharper.Sitelets"
    member val SiteletsModuleName = "WebSharper.Sitelets" with get, set

    /// The name of the WebSharper remoting module declared in Web.config.
    /// Default: "WebSharper.RemotingModule"
    member val RemotingModuleName = "WebSharper.RemotingModule" with get, set

    interface IAuthorizationFilter with

        member this.OnAuthorization(_) = ()