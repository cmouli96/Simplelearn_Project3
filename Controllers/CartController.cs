using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers
{
    public class CartController : Controller
    {
        EProductsDbContext context = new EProductsDbContext();


        

        public IActionResult Index()
        {
            int total = 0;
            string name = HttpContext.Session.GetString("uname");
            var rows = context.TblOrders.Where(x => x.Uname == name);
            foreach(var row in rows)
            {
                total += row.Cost;
            }
            ViewBag.total = total;
            
            return View(rows);
        }

        public IActionResult Details(int pid)
        {
            string name = HttpContext.Session.GetString("uname");
            return View(context.TblOrders.Single(x => x.Pid == pid && x.Uname == name));
        }

        public IActionResult AddToCart(int id)
        {
            return View(context.TblProducts.Single(x => x.Pid == id));
        }

        [HttpPost]
        public IActionResult AddToCart(int pid,int qty)
        {

            string name = HttpContext.Session.GetString("uname");

            TblProduct p = context.TblProducts.Single(x => x.Pid == pid);
            int available_qty = p.Qty;


            TblOrder checking = context.TblOrders.Find(name,pid);

            if(checking==null)
            {
                if (available_qty > qty)
                {
                    p.Qty = available_qty - qty;

                    TblOrder o = new TblOrder();




                    o.Uname = name;
                    o.Pid = p.Pid;
                    o.Pname = p.Pname;
                    o.Sname = p.Sname;
                    o.Qty = qty;
                    o.Cost = p.Price * qty;

                   


                    context.TblProducts.Update(p);

                    context.TblOrders.Add(o);
                    context.SaveChanges();

                    return RedirectToAction("index");
                }

                else
                {
                    ViewBag.msg = "Not enough items to Add";
                    return RedirectToAction("index");
                }

            }

            else
            {
                TblOrder o1 = context.TblOrders.Single(x => x.Pid == pid && x.Uname == name);

                if (o1 != null)
                {
                    return RedirectToAction("EditCart","Administration",pid);
                }

                return View();
            }
           

           
           
   
        }

        public IActionResult EditCart(int pid)
        {
            string name = HttpContext.Session.GetString("uname");
            return View(context.TblOrders.Single(x => x.Uname == name && x.Pid==pid));
        }

        [HttpPost]
        public IActionResult EditCart(int pid,int qty)
        {
            try
            {
                TblProduct p=context.TblProducts.Single(x=>x.Pid==pid);

                string name = HttpContext.Session.GetString("uname");
                TblOrder o = context.TblOrders.Single(x=>x.Uname==name&&x.Pid==pid);

                int price = p.Price;
                o.Uname = name;
                o.Pid = pid;
                o.Cost = o.Qty * price;
                context.TblOrders.Update(o);
                context.SaveChanges();
                return RedirectToAction("index");
            }
           
            catch
            {
                return RedirectToAction("index");
            }
        }

        public IActionResult DeleteItemCart(int pid)
        {
            string name = HttpContext.Session.GetString("uname");

            return View(context.TblOrders.Single(x => x.Uname == name && x.Pid == pid));
        }

        [HttpPost]
        public IActionResult DeleteItemCart(int pid,TblOrder o)
        {
            try
            {
                TblProduct p = context.TblProducts.Single(x => x.Pid == pid);
                p.Qty = p.Qty + o.Qty;

                context.TblProducts.Update(p);
                context.SaveChanges();
                context.TblOrders.Remove(o);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
           
        }

       
        
        public IActionResult CheckOut()
        {
            var name = HttpContext.Session.GetString("uname");


            IEnumerable<TblOrder> rows = context.TblOrders.Where(x=>x.Uname==name);
            foreach (TblOrder row in rows)
            {
                context.TblOrders.Remove(row);
            }
            context.SaveChanges();
            return View(rows);
        }
    }
}
