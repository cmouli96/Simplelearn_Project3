using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce.Models;
using Microsoft.AspNetCore.Authorization;

namespace e_commerce.Controllers
{

    public class AdminController : Controller
    {
        EProductsDbContext context = new EProductsDbContext();

        // GET: AdminController
        public ActionResult Index()
        {
            return View(context.TblProducts);
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View(context.TblProducts.Single(x => x.Pid == id));
        }

        // GET: AdminController/Create
        [Authorize(Roles = "admin,seller")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TblProduct p)
        {
            try
            {
                context.TblProducts.Add(p);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "admin,seller")]
        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(context.TblProducts.Single(x => x.Pid == id));
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TblProduct p)
        {
            try
            {
                context.TblProducts.Update(p);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "admin,seller")]
        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(context.TblProducts.Single(x => x.Pid == id));
        }

        // POST: AdminController/Delete/5
        [Authorize(Roles = "admin,seller")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TblProduct p)
        {
            try
            {
                context.TblProducts.Remove(p);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
