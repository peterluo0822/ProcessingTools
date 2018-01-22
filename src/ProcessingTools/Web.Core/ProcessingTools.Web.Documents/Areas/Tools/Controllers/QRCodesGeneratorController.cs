﻿namespace ProcessingTools.Web.Documents.Areas.Tools.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ProcessingTools.Web.Documents.Constants;

    [Area(AreaNames.Tools)]
    public class QRCodesGeneratorController : Controller
    {
        public const string ControllerName = "QRCodesGenerator";
        public const string IndexActionName = nameof(Index);

        [ActionName(IndexActionName)]
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
