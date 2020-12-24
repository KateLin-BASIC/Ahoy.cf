using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Aurore.Class.TextClass;

namespace Aurore.Controllers
{
    public class PasswordController : Controller
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

            if (reader.HasRows)
            {
                ViewBag.Id = id;
                sCon.Close();
                return View();
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formValues)
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

            if (reader.HasRows)
            {
                ViewBag.Id = id;

                var sCon1 = new MySqlConnection(ConfigurationManager.AppSettings["connectionString"]);
                sCon1.Open();
                var sqlCom1 = new MySqlCommand();
                sqlCom1.Connection = sCon1;
                sqlCom1.CommandText = "SELECT * FROM url WHERE shorted=@url";
                sqlCom1.CommandType = CommandType.Text;
                sqlCom1.Parameters.Add(new MySqlParameter("@url", id));
                MySqlDataReader reader1 = sqlCom1.ExecuteReader();

                while (reader1.Read())
                {
                    if (SHA256Hash(formValues["password"]) == Convert.ToString(reader1["password"]))
                    {
                        var sCon2 = new MySqlConnection(ConfigurationManager.AppSettings["connectionString"]);
                        sCon2.Open();
                        var sqlCom2 = new MySqlCommand();
                        sqlCom2.Connection = sCon2;
                        sqlCom2.CommandText = "UPDATE url SET view = view + 1 WHERE shorted = @id;";
                        sqlCom2.CommandType = CommandType.Text;
                        sqlCom2.Parameters.Add(new MySqlParameter("@id", id));
                        sqlCom2.ExecuteNonQuery();

                        return Redirect(Convert.ToString(reader1["url"]));
                    }
                    else
                    {
                        return View();
                    }
                }
            }

            return HttpNotFound();
        }
    }
}