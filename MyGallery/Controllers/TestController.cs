using MyGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyGallery.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        private readonly IImageService _imageService = new ImageService();

        public ActionResult Index()
        {
            List<BaseImage> list = _imageService.GetImagesFromContainer();
            return View(list);
        }

    }
}
