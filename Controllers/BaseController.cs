using Microsoft.AspNetCore.Mvc;
using bayessoft.Services;
using bayessoft.Models;

namespace bayessoft.Controllers
{
    public class BaseController : Controller
    {
        protected readonly LayoutService _layoutService;

        public BaseController(LayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        protected LayoutViewModel HazirlaLayoutModeli()
        {
            
            var path = HttpContext.Request.Path.ToString().ToLower();
            string dil = path.StartsWith("/en") ? "en" : "tr";

            return _layoutService.GetLayoutData(dil);
        }
    }
}
