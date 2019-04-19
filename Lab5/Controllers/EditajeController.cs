using Lab5.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab5.Controllers
{
    public class EditajeController : Controller
    {
        // GET: Editaje
        public ActionResult Index()
        {
            foreach (var item in Datos.ins.Diccio)
            {
                var d = (Estampa)item.Value;
                var objeto = new Estampa { nombre = item.Key, cantidad = d.cantidad, dispo = d.dispo };
                Datos.ins.mostrar.Add(objeto);
            }
            Datos.ins.GuardarArchivo(Datos.ins.mostrar);
            return View(Datos.ins.mostrar);
        }
        [HttpGet]
        public ActionResult SubirArchivo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubirArchivo(HttpPostedFileBase File)
        {
            try
            {
                if (File.ContentLength > 0 && File.FileName.EndsWith(".csv"))
                {
                    Datos.ins.Diccio.Clear();
                    Datos.ins.Diccio2.Clear();
                    Datos.ins.mostrar.Clear();
                    string _FileName = Path.GetFileName(File.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Uploads"), _FileName);
                    File.SaveAs(_path);
                    ViewBag.Message = "El album se ha cargado exitosamente";
                    Datos.ins.path = _path;

                    using (var Reader = new StreamReader(_path))
                    {
                        char[] Linea = Reader.ReadLine().ToCharArray();
                        while (!Reader.EndOfStream)
                        {
                            Linea = Reader.ReadLine().ToCharArray();
                            var info = Datos.Retornar(Linea);

                            string estado = (Convert.ToInt32(info[2]) > 1) ? "Posible intercambio" : ((Convert.ToInt32(info[2]) < 1) ? "No la tiene" : "La tiene");

                            var objeto = new Estampa { cantidad = Convert.ToInt32(info[2]), dispo = estado };

                            string nombre = $"{info[0]}_{info[1]}";
                            bool disp;
                            disp = (Convert.ToInt32(info[2]) == 0) ? false : true;

                            Datos.ins.Diccio.Add(nombre, objeto);
                            Datos.ins.Diccio2.Add(nombre, disp);
                        }
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "ERROR: carga de archivo";
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Agregar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Agregar(FormCollection collection)
        {
            try
            {
                if (Datos.ins.Diccio.ContainsKey(collection["nombre"]))
                {
                    Datos.ins.mostrar.Clear();
                    var objeto = (Estampa)Datos.ins.Diccio[collection["nombre"]];
                    objeto.cantidad = objeto.cantidad + Convert.ToInt32(collection["cantidad"]);
                    objeto.dispo = (objeto.cantidad > 1) ? "Posbile intercambio" : ((objeto.cantidad < 1) ? "No la tiene" : "La tiene");
                    Datos.ins.Diccio[collection["nombre"]] = objeto;
                    Datos.ins.Diccio2[collection["nombre"]] = true;

                }
                else
                {
                    ViewBag.Message = "No es valido el dato";
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Eliminar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Eliminar(FormCollection collection)
        {
            try
            {
                if (Datos.ins.Diccio.ContainsKey(collection["nombre"]))
                {
                    Datos.ins.mostrar.Clear();
                    var objeto = (Estampa)Datos.ins.Diccio[collection["nombre"]];
                    if (objeto.cantidad >= Convert.ToInt32(collection["cantidad"]))
                    {
                        objeto.cantidad -= Convert.ToInt32(collection["cantidad"]);
                        objeto.dispo = (objeto.cantidad > 1) ? "Posible intercambio" : ((objeto.cantidad < 1) ? "No la tiene" : "La tiene");
                        Datos.ins.Diccio[collection["nombre"]] = objeto;
                        if (objeto.cantidad == 0)
                        {
                            Datos.ins.Diccio2[collection["nombre"]] = false;
                        }
                    }


                }
                else
                {
                    ViewBag.Message = "No has ingresado cantidad";
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}