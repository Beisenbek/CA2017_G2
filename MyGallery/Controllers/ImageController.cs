using MyGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyGallery.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/

        public ActionResult Index()
        {
            BaseImage image = new BaseImage();
            image.name = "KBTU";
            image.url = @"http://www.kbtu.kz/Content/Kbtu/images/slides/main_id9.png";
            return View(image);
        }

        

    }
}
