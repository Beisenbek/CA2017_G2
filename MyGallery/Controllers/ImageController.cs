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
            BaseImage img = new BaseImage
            {
                URLcore = "https://kbtutest2.blob.core.windows.net/images/Chrysanthemum.jpg",
                URLcrop = "https://kbtutest2.blob.core.windows.net/images/prev_c.jpg",
                Name = "Chrysanthemum.jpg"
            };
            return View(img);
        }
    }
}
