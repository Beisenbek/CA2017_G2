using MyGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyGallery.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/

        public ActionResult Index()
        {
            return View();
        }

        private readonly IImageService _imageService = new ImageService();

        [HttpPost]
        public async Task<ActionResult> Upload()
        {
            var model = new UploadedImage();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["uploadedFile"];
                model = await _imageService.CreateUploadedImage(file);
                await _imageService.AddImageToBlobStorageAsync(model);
            }
            return View("Index");
        }
    }
}
