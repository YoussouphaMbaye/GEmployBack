using Gemploy.models;
using IronBarCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Resources;
using static System.Net.WebRequestMethods;
using System.Threading;

namespace Gemploy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;
        public ApiController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;
        }
        [HttpGet(Name = "/GetEmployers")]
        public async Task<List<Employer>> GetEmployers()
        {
            //Timer timer = new Timer(this.startTransaction, null, 0, 100000);
            //int i= 0;
            
            string baseUrl = "http://localhost:5172/";

            var emps = catalogDbContext.Employers.ToList();
                //string jjj = "kkkkk";
                if(emps!=null && emps.Count() > 0)
                {
                   emps.ForEach(e => {
                       e.UrlPicture = baseUrl+ "Resources/Images/" + e.IdEmp + ".jpg";
                       e.UrlQrcode = baseUrl + "Resources/Images/" + e.CodeEmp + ".png";
                   }
                   ) ;

            }
            else
            { 
                return new List<Employer>();

            }
               
                return emps;
        }
        [HttpPost(Name = "/PostEmployer")]
        public Employer PostEmployer([FromBody]Employer emp)
        {
            double ConvertToUnixTimestamp(DateTime date)
            {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan diff = date.ToUniversalTime() - origin;
                return Math.Floor(diff.TotalSeconds);
            }
            string dayToday = ConvertToUnixTimestamp(DateTime.Now).ToString();
            GeneratedBarcode gBarcode = IronBarCode.BarcodeWriter.CreateBarcode("N" + dayToday, BarcodeEncoding.QRCode);
            gBarcode.SaveAsPng(".\\Resources\\Images\\" + "N" + dayToday + ".png");
            emp.CodeEmp = "N" + dayToday;
            catalogDbContext.Employers.Add(emp);
            catalogDbContext.SaveChanges();
            return emp;
        }
        
        [HttpPost("/UploadFile")]
        public IActionResult UploadFile(IFormFile formFile)
        {
            if(!Directory.Exists(this.getRootPath()+ "\\profiles\\")) {
                Directory.CreateDirectory(".\\profiles\\");
              
            }
            if (formFile!=null)
            {
                using (FileStream stream = System.IO.File.Create(".\\Resources\\Images\\" + formFile.FileName))
                {
                    formFile.CopyTo(stream);
                    stream.Flush();
                }
                    
            }
            
            Console.WriteLine(".\\Resources\\Images\\" + formFile.FileName);
            return Ok(formFile.FileName);
        }
        [NonAction]
        public string getRootPath()
        {
            return this._environnement.WebRootPath + ".\\Resources\\Images\\";
        }
        


    }
}
