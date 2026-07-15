using ImageGalleryLightBoxX.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ImageGallerryLightBoxX.Controllers
{
    public class GalleryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Gallery
        public ActionResult Index(int? categoryId)
        {
            var images = db.GalleryImages.Include(i => i.Category).Where(i => i.IsActive);

            if (categoryId.HasValue)
            {
                images = images.Where(i => i.CategoryId == categoryId.Value);
            }

            ViewBag.Categories = db.Categories.Where(c => c.IsActive).ToList();
            ViewBag.SelectedCategory = categoryId;

            return View(images.OrderByDescending(i => i.UploadedDate).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}