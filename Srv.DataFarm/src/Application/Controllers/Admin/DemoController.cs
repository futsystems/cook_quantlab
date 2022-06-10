
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCryptoLab.Models;
using UniCryptoLab.Web.Framework;


namespace UniCryptoLab.Web.API.App
{

    /// <summary>
    /// Demo接口
    /// </summary>
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class DemoController : AdminController
    {
        private static readonly NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();



        
        [HttpPost]
        [ActionName("demo")]
        public IActionResult Demo()
        {
            return Json(SuccessResult("hello world"));
        }
    }
}
