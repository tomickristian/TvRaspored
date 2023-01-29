using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TvRaspored.Models;

namespace TvRaspored.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;   
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    //ViewBag.ErrorMessage = "Pristup ";
                    break;
            }
            return View("AccessDenied");
            //return View("NotFound");
        }
        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View("AccessDenied");

            //var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //logger.LogError($"The path {exceptionDetails.Path} threw exception {exceptionDetails.Error}");
            //return View("Error");
        }
    }

}
