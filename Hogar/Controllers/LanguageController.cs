namespace Hogar.Web.Controllers
{
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;

    public class LanguageController : Controller
    {
        public IActionResult ToggleLanguage(string returnUrl)
        {
            var currentCulture = HttpContext.Request.Cookies["Culture"];
            var newCulture = currentCulture == "es-ES" ? "en-US" : "es-ES";

            // Guardar la nueva cultura en una cookie
            HttpContext.Response.Cookies.Append("Culture", newCulture);

            return LocalRedirect(returnUrl);
        }
    }

}
