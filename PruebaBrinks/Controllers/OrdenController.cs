using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dominio;

namespace PruebaBrinks.Controllers
{
    public class OrdenController : Controller
    {
        // GET: Orden
        public ActionResult Index()
        {
            ServiceReference.Service1Client client = new ServiceReference.Service1Client();
            //List<Dominio.Orden> list = new List<Dominio.Orden>();
            List<Orden> list = new List<Orden>();
            var auxlist =client.ConsultarOrdenes().ToList();
            foreach (var item in auxlist)
            {
                Orden orden = new Orden
                {
                    Id = item.Id,
                    Cliente = item.Cliente,
                    Oficina = item.Oficina,
                    producto = item.producto,
                    FechaIngreso = item.FechaIngreso

                };
                list.Add(orden);
            }
            return View(list);
        }

        // GET: Orden/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Orden/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orden/Create
        [HttpPost]
        public ActionResult Create(Orden model)
        {
            try
            {
                ServiceReference.Service1Client client = new ServiceReference.Service1Client();
                //List<Dominio.Orden> list = new List<Dominio.Orden>();
                ServiceReference.Orden envio = new ServiceReference.Orden();
                    envio.Cliente = model.Cliente;
                envio.FechaIngreso = DateTime.Now;
                envio.Oficina = model.Oficina;
                envio.producto = model.producto;

                var i = client.CrearOrden(envio);
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Orden/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Orden/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Orden/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Orden/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
