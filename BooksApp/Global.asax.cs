using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BooksApp.Infrastructure;

namespace BooksApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            ExecuteInitialBooksImport();
        }

        private void ExecuteInitialBooksImport()
        {
            string uploadsPath = Server.MapPath("~/Uploads");
            string backupsPath = Server.MapPath("~/Backups");

            foreach (string filePath in Directory.EnumerateFiles(uploadsPath, "*.xml"))
            {
                if (BooksHelper.ImportBooksFromXmlFile(filePath))
                {
                    // Either Books were successfully imported into the database, or the xml file can not be parsed correctly. Move the file to Backups.
                    File.Move(filePath, filePath.Replace(uploadsPath, backupsPath));
                }
            }
        }
    }
}