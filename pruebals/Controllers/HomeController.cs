using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using pruebals.Permisos;
using System.Configuration;
using pruebals.Models;
using System.Web.UI;

namespace pruebals.Controllers
{

    [ValidarSesion]
    public class HomeController : Controller
    {
        static string cadena = ConfigurationManager.AppSettings["Conexion"];
        public ActionResult Index()
        {
            List<Producto> LstProducto = new List<Producto>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarProductos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Producto producto = new Producto();
                        producto.id_producto = reader.GetInt32(0);
                        producto.codigo = reader.GetString(1);
                        producto.nombre = reader.GetString(2);
                        producto.precio = reader.GetDecimal(3);
                        producto.stock = reader.GetInt32(4);
                        producto.status = reader.GetBoolean(5);
                        producto.descripcion = reader.GetString(6);
                        LstProducto.Add(producto);
                    }
                }
                else
                {
                    //Console.WriteLine("No rows found.");
                }


            }



            return View(LstProducto);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Usuario usuario = new Usuario();
            usuario = (Usuario)Session["usuario"];
            ViewBag.Message = "Your contact page.";
            List<Carrito> LstCarrito = new List<Carrito>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarCarrito", cn);
                cmd.Parameters.AddWithValue("id_usuario", usuario.IdUsuario);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Carrito carrito = new Carrito();
                        carrito.nombre = reader.GetString(0);
                        carrito.cantidad = reader.GetInt32(1);
                        carrito.valor = reader.GetDecimal(2);
                        carrito.total = reader.GetDecimal(3);
                        LstCarrito.Add(carrito);
                    }
                }
                else
                {
                    //Console.WriteLine("No rows found.");
                }



            }



            return View(LstCarrito);
        }

        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Acceso");
        }

        [HttpPost]
        public bool AgregarCarrito(Producto producto)
        {
            Usuario usuario = new Usuario();



            using (SqlConnection cn = new SqlConnection(cadena))
            {
                usuario = (Usuario)Session["usuario"];
                SqlCommand cmd = new SqlCommand("sp_AgregarCarrito", cn);
                cmd.Parameters.AddWithValue("id_producto", producto.id_producto);
                cmd.Parameters.AddWithValue("id_usuario", usuario.IdUsuario);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                producto.id_producto = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }

            if (producto.id_producto != 0)
            {
                ViewData["Mensaje"] = "Producto agregado al carrito";
                return true;
            }
            else
            {
                ViewData["Mensaje"] = "Error";
                return false;
            }


        }

    }
}