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