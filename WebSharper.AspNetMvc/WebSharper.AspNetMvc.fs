namespace WebSharper.AspNetMvc

open System.IO
open System.Runtime.CompilerServices
open System.Web
open System.Web.Mvc
open WebSharper

module ScriptManager =

    [<Literal>]
    let private smKey = "WebSharper.AspNetMvc.ScriptManager"

    /// <summary>
    /// Register a control so that its dependencies are rendered by the script manager.
    /// </summary>
    /// <param name="control">The control to render.</param>
    /// <returns>The code to insert the control in the page.</returns>
    let Register (control: Html.Client.IControl) =
        let items = HttpContext.Current.Items
        let sm =
            match items.[smKey] with
            | :? Web.ScriptManager as sm -> sm
            | _ ->
                let sm = new Web.ScriptManager()
                items.[smKey] <- sm
                sm
        let id = sm.Register (Some control.Id) control
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
            let c = new System.Web.UI.Control()
            MvcHtmlString(sw.ToString())
        | _ -> MvcHtmlString("")

type Filter(?siteletsOverrideMvc: bool, ?rpcModuleName: string, ?siteletsModuleName: string) =
    let rpcModuleName = defaultArg rpcModuleName "WebSharper.RemotingModule"
    let siteletsModuleName = defaultArg siteletsModuleName "WebSharper.Sitelets"
    let siteletsOverrideMvc = defaultArg siteletsOverrideMvc true

    interface IAuthorizationFilter with

        member this.OnAuthorization(filterCtx) =
            let httpCtx = filterCtx.HttpContext
            let tryRun (action: option<Async<unit>>) =
                action |> Option.map (fun run ->
                    filterCtx.Result <-
                        { new ActionResult() with
                            member this.ExecuteResult(_) =
                                Async.RunSynchronously run })
            let isRemoting =
                match httpCtx.ApplicationInstance.Modules.[rpcModuleName] with
                | :? Web.RpcModule as m ->
                    tryRun (m.TryProcessRequest httpCtx) |> Option.isSome
                | _ -> false
            if siteletsOverrideMvc && not isRemoting then
                match httpCtx.ApplicationInstance.Modules.[siteletsModuleName] with
                | :? Sitelets.HttpModule as m ->
                    tryRun (m.TryProcessRequest httpCtx) |> ignore
                | _ -> ()

    // Constructors intended for C#, which doesn't like F#'s optional attributes.

    /// <summary>
    /// Create a filter that enables WebSharper sitelets and remoting.
    /// </summary>
    new () =
        new Filter(?siteletsOverrideMvc = Some true)

    /// <summary>
    /// Create a filter that enables WebSharper sitelets and remoting.
    /// </summary>
    /// <param name="siteletsOverrideMvc">When both the sitelet and ASP.NET MVC accept a URL, the sitelet is served. (default: true)</param>
    new (siteletsOverrideMvc: bool) =
        new Filter(?siteletsOverrideMvc = Some siteletsOverrideMvc)

    /// <summary>
    /// Create a filter that enables WebSharper sitelets and remoting.
    /// </summary>
    /// <param name="rpcModuleName">The name of the WebSharper remoting module declared in Web.config. (default: WebSharper.RemotingModule)</param>
    /// <param name="siteletsModuleName">The name of the WebSharper sitelets module declared in Web.config. (default: WebSharper.Sitelets)</param>
    new (rpcModuleName: string, siteletsModuleName: string) =
        new Filter(
            ?rpcModuleName = Some rpcModuleName,
            ?siteletsModuleName = Some siteletsModuleName)

    /// <summary>
    /// Create a filter that enables WebSharper sitelets and remoting.
    /// </summary>
    /// <param name="siteletsOverrideMvc">When both the sitelet and ASP.NET MVC accept a URL, the sitelet is served. (default: true)</param>
    /// <param name="rpcModuleName">The name of the WebSharper remoting module declared in Web.config. (default: WebSharper.RemotingModule)</param>
    /// <param name="siteletsModuleName">The name of the WebSharper sitelets module declared in Web.config. (default: WebSharper.Sitelets)</param>
    new (siteletsOverrideMvc: bool, rpcModuleName: string, siteletsModuleName: string) =
        new Filter(
            ?siteletsOverrideMvc = Some siteletsOverrideMvc,
            ?rpcModuleName = Some rpcModuleName,
            ?siteletsModuleName = Some siteletsModuleName)
