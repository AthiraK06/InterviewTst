using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region connection string
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Machinetest;Integrated Security=True";
        #endregion


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        #region ItemList
        public JsonResult ItemList()
        {
            DataSet dset = new DataSet();
            try
            {
                using SqlConnection sqlCon = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("CartItems", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDa = new SqlDataAdapter();
                sqlDa.SelectCommand = cmd;
                sqlDa.Fill(dset);
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //ViewBag.alert = msg;
                return Json(msg);
            }
            return Json(dset);
        }

        public JsonResult CartDetails(int Id)
        {
            DataSet dset = new DataSet();
            using SqlConnection sqlCon = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("[dbo].[CartDetails]", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sqlDa = new SqlDataAdapter();
            {
                sqlCon.Open();
                cmd.Connection = sqlCon;
                cmd.Parameters.AddWithValue("@ID", Id);
                sqlDa.SelectCommand = cmd;
                sqlDa.Fill(dset);
                sqlCon.Close();
                return Json(dset);
            }
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
