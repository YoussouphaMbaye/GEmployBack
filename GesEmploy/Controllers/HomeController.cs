using Gemploy.models;
using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gemploy.Controllers
{
    public class HomeController : Controller
    {
        private readonly CatalogDbContext catalogDbContext;
        public HomeController(CatalogDbContext catalogDbContext)
        {
            this.catalogDbContext = catalogDbContext;
        }
        public IActionResult Index()
        {
            try
            {
                IEnumerable<Employer> emps = catalogDbContext.Employers;
                //string jjj = "kkkkk";
                return View(emps);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }
        //GET
        [HttpGet]
        public IActionResult Create()
        {
            Console.WriteLine("==++++++++++++++++++++++++++++==");
            return View();

        }

        [HttpPost]
        public IActionResult Create(Employer emp)
        {
            Console.WriteLine("-------------nnnnnnniiiipppp------------");
            double ConvertToUnixTimestamp(DateTime date)
            {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan diff = date.ToUniversalTime() - origin;
                return Math.Floor(diff.TotalSeconds);
            }
            string dayToday = ConvertToUnixTimestamp(DateTime.Now).ToString();
            GeneratedBarcode gBarcode = IronBarCode.BarcodeWriter.CreateBarcode("N"+dayToday, BarcodeEncoding.QRCode);
            gBarcode.SaveAsPng("/imgBarcode/"+emp.IdEmp.ToString() + ".png");
                emp.CodeEmp = "N"+ dayToday;
                Console.WriteLine(emp.NameEmp);
                catalogDbContext.Employers.Add(emp);
                catalogDbContext.SaveChanges();
                TempData["success"] = "Employer crée avec succés";
                return RedirectToAction("Index");
            
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public string Error()
        {
            return Activity.Current?.Id ?? HttpContext.TraceIdentifier ;
        }
    }
}
