using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using System.Reflection.Metadata;
using Azure.Monitor.Query;
using Azure.Identity;

namespace azurepopchecker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }

        public string GetAFDPOP()
        {
            string XAzureRef = "";
            if (Request.Headers.TryGetValue("x-azure-ref", out var values))
            {
                XAzureRef = values.First();
            }

            string tenantID = "";
            string clientID = "";
            string clientSecret = "";



        }
    }
}