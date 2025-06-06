using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSharper.AspNetCore;

namespace WebSharper.AspNetMvc.Tests.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebSharperContentService _webSharperContentService;

        public IndexModel(ILogger<IndexModel> logger, IWebSharperContentService webSharperContentService)
        {
            _logger = logger;
            _webSharperContentService = webSharperContentService;
        }

        public void OnGet()
        {

        }

        public IHtmlContent ClientControl =>
            _webSharperContentService.Render(                
                WebSharper.Sitelets.Content.Bundle("index", new ClientControl("Adam")));
    }
}
