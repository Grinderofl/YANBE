using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Core.Domain;
using EFConvention;
using YANBE.Models;

namespace YANBE.Controllers
{
    public class TagsController : Controller
    {
        private DbContext _context;

        public TagsController(DbContext context)
        {
            _context = context;
        }

        public ActionResult Details(string id)
        {
            id = id.Replace("-", " ");
            var model = new TagsModel()
            {
                Posts = _context.Set<Post>().Where(x => x.Tags.Any(c => c.Name == id)).ToList(),
                Tag = id
            };
            
            return View(model);
        }
    }
}