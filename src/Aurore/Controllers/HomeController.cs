using System;
using System.Net;
using System.Web;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static Aurore.Class.TextClass;

namespace Aurore.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var id = Url.RequestContext.RouteData.Values["id"];

            if (Convert.ToString(id) == string.Empty)
            {
                return View();
            }
            else
            {
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
                    while (reader.Read())
                    {
                        if (string.IsNullOrWhiteSpace(Convert.ToString(reader["password"])))
                        {
                            var sCon1 = new MySqlConnection(ConfigurationManager.AppSettings["connectionString"]);
                            sCon1.Open();
                            var sqlCom1 = new MySqlCommand();
                            sqlCom1.Connection = sCon1;
                            sqlCom1.CommandText = "UPDATE url SET view = view + 1 WHERE shorted = @id;";
                            sqlCom1.CommandType = CommandType.Text;
                            sqlCom1.Parameters.Add(new MySqlParameter("@id", id));
                            sqlCom1.ExecuteNonQuery();

                            return Redirect((string)reader["url"]);
                        }
                        else
                        {
                            return Redirect($"/pw/{(string)reader["shorted"]}");
                        }
                        
                    }
                }
                sCon.Close();
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formValues)
        {
            if (!string.IsNullOrWhiteSpace(formValues["url"]))
            {
                Uri uriResult = null;
                bool result = Uri.TryCreate(formValues["url"], UriKind.Absolute, out uriResult) && ((uriResult.Scheme ?? "") == (Uri.UriSchemeHttp ?? "") || (uriResult.Scheme ?? "") == (Uri.UriSchemeHttps ?? ""));

                if (result)
                {
                    if (!string.IsNullOrWhiteSpace(formValues["password"]))
                    {
                        return Redirect($"/stat/{AddNewUrl(formValues["url"], GenerateRandomString(5), SHA256Hash(formValues["password"]))}");
                    }
                    else
                    {
                        return Redirect($"/stat/{AddNewUrl(formValues["url"], GenerateRandomString(5), "")}");
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(formValues["password"]))
                    {
                        return Redirect($"/stat/{AddNewUrl("http://" + formValues["url"], GenerateRandomString(5), SHA256Hash(formValues["password"]))}");
                    }
                    else
                    {
                        return Redirect($"/stat/{AddNewUrl("http://" + formValues["url"], GenerateRandomString(5), "")}");
                    }
                }
            }

            return View();
        }

        public string AddNewUrl(string strUrl, string strShorted, string strPassword)
        {
            var sCon = new MySqlConnection(ConfigurationManager.AppSettings["connectionString"]);
            sCon.Open();
            var sqlCom = new MySqlCommand();
            sqlCom.Connection = sCon;
            sqlCom.CommandText = "INSERT INTO url (url, shorted, view, date, password) VALUES(@url, @short, 0, @date, @pw);";
            sqlCom.CommandType = CommandType.Text;
            sqlCom.Parameters.AddWithValue("@url", strUrl);
            sqlCom.Parameters.AddWithValue("@short", strShorted);
            sqlCom.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
            sqlCom.Parameters.AddWithValue("@pw", strPassword);
            sqlCom.ExecuteNonQuery();
            sCon.Close();

            return strShorted;
        }
    }
}