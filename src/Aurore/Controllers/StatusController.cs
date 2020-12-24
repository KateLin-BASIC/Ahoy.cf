using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aurore.Controllers
{
    public class StatusController : Controller
    {
        public ActionResult Index()
        {
            var id = Url.RequestContext.RouteData.Values["id"];

            var sCon = new MySqlConnection(ConfigurationManager.AppSettings["connectionString"]);
            sCon.Open();
            var sqlCom = new MySqlCommand();
            sqlCom.Connection = sCon;
            sqlCom.CommandText = "SELECT * FROM url WHERE shorted=@url";
            sqlCom.CommandType = CommandType.Text;
            sqlCom.Parameters.Add(new MySqlParameter("@url", id));
            MySqlDataReader reader = sqlCom.ExecuteReader();

            if (!reader.HasRows)
            {
                return HttpNotFound();
            }
            else
            {
                while (reader.Read())
                {
                    ViewBag.ID = reader["_id"];
                    ViewBag.StrID = reader["shorted"];
                    ViewBag.Date = reader["date"];
                    ViewBag.View = reader["view"];
                    ViewBag.UsePassword = !string.IsNullOrWhiteSpace(Convert.ToString(reader["password"]));
                    ViewBag.Shorted = $"http://{Request.Url.Authority}/{reader["shorted"]}";
                }
            }
            sCon.Close();

            return View();
        }
    }
}