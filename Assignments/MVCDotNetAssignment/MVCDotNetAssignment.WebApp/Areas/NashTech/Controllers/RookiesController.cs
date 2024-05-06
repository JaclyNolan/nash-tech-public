using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVCDotNetAssignment.WebApp.Areas.NashTech.Controllers
{
    [Area("NashTech")]
    public class RookiesController : Controller
    {
        // GET: RookiesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RookiesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RookiesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RookiesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RookiesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RookiesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RookiesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RookiesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
