using Gemploy.models;
using IronBarCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Resources;
using static System.Net.WebRequestMethods;
using System.Threading;
using GesEmploy.models;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Text.Json;
using System.Reflection;

namespace Gemploy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly CatalogDbContext catalogDbContext;
        private readonly IWebHostEnvironment _environnement;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        public ApiController(CatalogDbContext catalogDbContext, IWebHostEnvironment environnement, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager)
        {
            this.catalogDbContext = catalogDbContext;
            this._environnement = environnement;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }
        [HttpGet(Name = "/GetEmployers")]
        public async Task<List<Employer>> GetEmployers(string? codeEmp, string? departement)
        {
            //Timer timer = new Timer(this.startTransaction, null, 0, 100000);
            //int i= 0;

            string baseUrl = "http://localhost:5172/";
            string sql = "select * from employers";
            if (codeEmp != null || departement != null)
            {
                sql += " where";
            }
            if (codeEmp != null)
            {
                sql += " codeEmp='" + codeEmp + "'";
            }
            if (codeEmp != null && departement != null)
            {
                sql += " and";
            }
            if (departement != null)
            {
                sql += " occupation LIKE '" + departement + "%'";
            }

            //sql += ";";

            var emps = catalogDbContext.Employers.FromSqlRaw(sql).Include(e => e.Horaire).ToList();
            //string jjj = "kkkkk";
            if (emps != null && emps.Count() > 0)
            {
                emps.ForEach(e => {
                    e.UrlPicture = baseUrl + "Resources/Images/" + e.IdEmp + ".jpg";
                    e.UrlQrcode = baseUrl + "Resources/Images/" + e.CodeEmp + ".png";
                }
                );

            }
            else
            {
                return new List<Employer>();

            }

            return emps;
        }

        [HttpGet("/GetHoraires")]
        public async Task<List<Horaire>> GetHoraires()
        {
            //Timer timer = new Timer(this.startTransaction, null, 0, 100000);
            //int i= 0;

            string baseUrl = "http://localhost:5172/";

            var hors = catalogDbContext.Horaire.ToList();
            //string jjj = "kkkkk";
            if (hors != null && hors.Count() > 0)
            {
                return hors;

            }
            else
            {
                return new List<Horaire>();
            }


        }
        [HttpGet("/GetDayOfEmployer")]
        public async Task<List<DayOffEmployer>> GetDayOfEmployer(int idEmp)
        {
            //Timer timer = new Timer(this.startTransaction, null, 0, 100000);
            //int i= 0;

            string baseUrl = "http://localhost:5172/";

            var dys = await catalogDbContext.DayOffEmployer.Include(d => d.Employer).Include(d => d.DayOff).Where<DayOffEmployer>(d => d.IdEmp == idEmp).ToListAsync();
            //string jjj = "kkkkk";
            if (dys != null && dys.Count() > 0)
            {
                return dys;

            }
            else
            {
                return new List<DayOffEmployer>();
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Employer>> GetEmployer(int id)
        {
            //Timer timer = new Timer(this.startTransaction, null, 0, 100000);
            //int i= 0;

            string baseUrl = "http://localhost:5172/";

            var emp = await catalogDbContext.Employers.FindAsync(id);
            //string jjj = "kkkkk";
            if (emp != null)
            {

                emp.UrlPicture = baseUrl + "Resources/Images/" + emp.IdEmp + ".jpg";
                emp.UrlQrcode = baseUrl + "Resources/Images/" + emp.CodeEmp + ".png";

                return emp;
            }
            else
            {
                return NotFound();

            }


        }
        [HttpPut]
        public async Task<ActionResult> UpdateGetEmployer(Employer employer)
        {
            //Timer timer = new Timer(this.startTransaction, null, 0, 100000);
            //int i= 0;

            string baseUrl = "http://localhost:5172/";

            var emp = await catalogDbContext.Employers.FindAsync(employer.IdEmp);
            //string jjj = "kkkkk";
            if (emp != null)
            {
                try
                {
                    emp.EmailEmp = employer.EmailEmp;
                    emp.NameEmp = employer.NameEmp;
                    emp.BirthDay = employer.BirthDay;
                    emp.PhoneEmp = employer.PhoneEmp;
                    emp.EmailEmp = employer.EmailEmp;
                    emp.Occupation = employer.Occupation;
                    emp.HoraireId = employer.HoraireId;
                    catalogDbContext.Employers.Update(emp);
                    await catalogDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }




                return Ok();
            }
            else
            {
                return NotFound();

            }


        }
        [HttpPut("/DeactiveDayOfDay")]
        public async Task<ActionResult> DeactivateDayOfDay(int id)
        {
            //Timer timer = new Timer(this.startTransaction, null, 0, 100000);
            //int i= 0;

            string baseUrl = "http://localhost:5172/";
            DayOfDay d = null;

            d = await catalogDbContext.DayOfDay.FindAsync(id);

            //string jjj = "kkkkk";
            if (d != null)
            {
                try
                {
                    d.active = d.active ? false : true;

                    catalogDbContext.DayOfDay.Update(d);
                    await catalogDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }




                return Ok();
            }
            else
            {
                return NotFound();

            }


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGetEmployer(int id)
        {


            string baseUrl = "http://localhost:5172/";

            var emp = await catalogDbContext.Employers.FindAsync(id);
            //string jjj = "kkkkk";
            if (emp != null)
            {
                try
                {
                    catalogDbContext.Employers.Remove(emp);
                    await catalogDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }

                return Ok();
            }
            else
            {
                return NotFound();

            }


        }
        [HttpDelete("/publicHolidays/{id}")]
        public async Task<ActionResult> DeletePublicHoliday(int id)
        {


            string baseUrl = "http://localhost:5172/";

            var emp = await catalogDbContext.PublicHolidays.FindAsync(id);
            //string jjj = "kkkkk";
            if (emp != null)
            {
                try
                {
                    catalogDbContext.PublicHolidays.Remove(emp);
                    await catalogDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }

                return Ok();
            }
            else
            {
                return NotFound();

            }


        }
        [HttpDelete("/Horaires/{id}")]
        public async Task<ActionResult> DeleteHoraire(int id)
        {


            string baseUrl = "http://localhost:5172/";

            var emp = await catalogDbContext.Horaire.FindAsync(id);
            //string jjj = "kkkkk";
            if (emp != null)
            {
                try
                {
                    catalogDbContext.Horaire.Remove(emp);
                    await catalogDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }

                return Ok();
            }
            else
            {
                return NotFound();

            }


        }
        [HttpDelete("/deleteDayOffEmp")]
        public async Task<ActionResult> DeleteGetEmployer(int idEmp, int idDayOff)
        {


            string baseUrl = "http://localhost:5172/";

            var demp = await catalogDbContext.DayOffEmployer.Where(d => d.IdEmp == idEmp && d.IdDayOff == idDayOff).SingleOrDefaultAsync();
            //string jjj = "kkkkk";
            if (demp != null)
            {
                try
                {
                    catalogDbContext.DayOffEmployer.Remove(demp);
                    await catalogDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }

                return Ok();
            }
            else
            {
                return NotFound();

            }


        }
        [HttpPost(Name = "/PostEmployer")]
        public Employer PostEmployer([FromBody] Employer emp)
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
        [HttpPost("/PostHoraire")]
        public async Task<Horaire> PostHoraire([FromBody] Horaire horaire)
        {

            try {

                await catalogDbContext.Horaire.AddAsync(horaire);
                await catalogDbContext.SaveChangesAsync();

                return horaire;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("/PostDayOf")]
        public async Task<DayOff> PostDayOf([FromBody] DayOff dayOff)
        {

            try
            {

                await catalogDbContext.DayOffs.AddAsync(dayOff);
                await catalogDbContext.SaveChangesAsync();

                return dayOff;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("/PostDayOffEmployer")]
        public async Task<DayOffEmployer> DayOffEmployer([FromBody] DayOffEmployer dayOffEmployer)
        {

            try
            {
                //DayOffEmployer day=new DayOffEmployer();


                await catalogDbContext.DayOffEmployer.AddAsync(dayOffEmployer);
                await catalogDbContext.SaveChangesAsync();

                return dayOffEmployer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("/PostGetIn")]
        public async Task<GetIn> PostGetIn(int idEmp)
        {

            try
            {
                Employer emp = await catalogDbContext.Employers.Where(e => e.IdEmp == idEmp).SingleOrDefaultAsync();
                GetIn g = new GetIn();
                //Day off

                if (emp != null)
                {
                    DateOnly dd = DateOnly.FromDateTime(DateTime.Now);
                    GetIn getin = await catalogDbContext.GetIn.Where(e => e.dateIn == dd).SingleOrDefaultAsync();
                    if (getin == null)
                    {
                        g.EmployerId = idEmp;
                        g.hour = TimeOnly.FromDateTime(DateTime.Now);
                        g.dateIn = DateOnly.FromDateTime(DateTime.Now);

                        await catalogDbContext.GetIn.AddAsync(g);
                        await catalogDbContext.SaveChangesAsync();
                    }
                }

                return g;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        [HttpPost("/PostGetInQrCode")]
        public async Task<GetIn> PostGetInQrcode(string numQr)
        {

            try
            {
                Employer emp = await catalogDbContext.Employers.Where(e => e.CodeEmp == numQr).SingleOrDefaultAsync();
                GetIn g = new GetIn();
                //Day off
                Console.WriteLine(emp.NameEmp);
                var dd = DateTime.Now;
                Console.WriteLine("nnnnnn");
                if (emp != null)
                {
                    Console.WriteLine("Not null");
                    GetIn getin = await catalogDbContext.GetIn.Where(e => e.dateIn == DateOnly.FromDateTime(dd) && e.EmployerId==emp.IdEmp).SingleOrDefaultAsync();
                    Console.WriteLine(getin);
                   
                    if (getin == null)
                    {
                        Console.WriteLine("Dans");
                        g.EmployerId = emp.IdEmp;
                        g.hour = TimeOnly.FromDateTime(dd);
                        g.dateIn = DateOnly.FromDateTime(dd);

                        await catalogDbContext.GetIn.AddAsync(g);
                        await catalogDbContext.SaveChangesAsync();

                    }



                }
                return g;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        [HttpPost("/PostGetOutQrCode")]
        public async Task<GetOut> PostGetOutQrcode(string numQr)
        {

            try
            {
                Employer emp = await catalogDbContext.Employers.Where(e => e.CodeEmp == numQr).SingleOrDefaultAsync();
                GetOut g = new GetOut();
                //Day off
                Console.WriteLine(emp.NameEmp);
                var dd = DateTime.Now;
                //Console.WriteLine("nnnnnn");
                if (emp != null)
                {
                    //Si il est deja venu
                    GetIn getIn = await catalogDbContext.GetIn.Where(e => e.dateIn == DateOnly.FromDateTime(dd) && e.EmployerId == emp.IdEmp).SingleOrDefaultAsync();
                    if (getIn != null)
                    {
                        GetOut getOut = await catalogDbContext.GetOut.Where(e => e.dateOut == DateOnly.FromDateTime(dd) && e.EmployerId == emp.IdEmp).SingleOrDefaultAsync();


                        if (getOut == null)
                        {
                            Console.WriteLine("Dans");
                            g.EmployerId = emp.IdEmp;
                            g.hour = TimeOnly.FromDateTime(dd);
                            g.dateOut = DateOnly.FromDateTime(dd);

                            await catalogDbContext.GetOut.AddAsync(g);
                            await catalogDbContext.SaveChangesAsync();

                        }
                    }
                    else
                    {
                        Console.WriteLine("No getIn to getOut");
                    }



                }
                return g;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        [HttpPost("/PostGetOut")]
        public async Task<GetOut> PostGetOut(int idEmp)
        {

            try
            {
                GetOut g = new GetOut();
                g.EmployerId = idEmp;
                g.hour = TimeOnly.FromDateTime(DateTime.Now);
                g.dateOut = DateOnly.FromDateTime(DateTime.Now);
                await catalogDbContext.GetOut.AddAsync(g);
                await catalogDbContext.SaveChangesAsync();

                return g;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost("/PostDayOffDay")]
        public async Task<DayOfDay> PostDayOffDay([FromBody] DayOfDay DayOfDay)
        {

            try
            {

                await catalogDbContext.DayOfDay.AddAsync(DayOfDay);
                await catalogDbContext.SaveChangesAsync();

                return DayOfDay;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost("/PostPublicHoliday")]
        public async Task<PublicHolidays> PostPublicHoliday([FromBody] PublicHolidays PublicHoliday)
        {

            try
            {

                await catalogDbContext.PublicHolidays.AddAsync(PublicHoliday);
                await catalogDbContext.SaveChangesAsync();

                return PublicHoliday;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost("/GetGetOut/{id:int}")]
        public async Task<GetOut> GetGetOut(int id)
        {

            try
            {

                GetOut getOut = catalogDbContext.GetOut

                    .Include(e => e.Employer).Where(g => g.IdGetOut == id).Include(g => g.Employer).ToList().FirstOrDefault();
                //catalogDbContext.SaveChangesAsync();

                return getOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet("/missingOrHierDate")]
        public async Task<ActionResult> missingOrHierDate(DateTime theDate, string? codeEmp, string? HierOrMissing)
        {
            var nbHier = 0;
            var nbMissing = 0;
            try
            {
                DateTime gDayNow = theDate;
                DateOnly dateNow = DateOnly.FromDateTime(gDayNow);
                string query = $"Select * from employers";
                if (codeEmp != null)
                {
                    query += $" where codeEmp='{codeEmp}';";
                }
                Console.WriteLine(dateNow);
                var emps = await catalogDbContext.Employers.
                    FromSqlRaw(query).
                    ToListAsync();
                List<EmpResult> listEmpR = new List<EmpResult>();
                foreach (Employer g in emps)
                {
                    Console.WriteLine("==========================");
                    EmpResult empR = new EmpResult();
                    empR.employer = g;
                    Console.WriteLine(empR.employer.NameEmp);
                    GetIn gg = await catalogDbContext.GetIn.Include(e => e.Employer).Where(ge => ge.dateIn == dateNow && ge.EmployerId == g.IdEmp).SingleOrDefaultAsync();
                    
                        empR.hier = "Missing";
                    if (gg != null)
                    {
                        if (gg.Employer.HoraireId != null)
                        {

                            Console.WriteLine(gg.Employer.NameEmp);
                            empR.hier = "Hier";
                            nbHier += 1;
                            Horaire h = await catalogDbContext.Horaire.FirstAsync(e => e.IdHoraire == gg.Employer.HoraireId);
                            Console.WriteLine(gg.hour - h.TimeStart);
                            empR.late = TimeOnly.FromTimeSpan(gg.hour - h.TimeStart);
                            empR.hourGetIn = gg.hour;
                            GetOut getO = await catalogDbContext.GetOut.Include(e => e.Employer).Where(ge => ge.dateOut == dateNow && ge.EmployerId == g.IdEmp).SingleOrDefaultAsync();

                            if (getO != null)
                            {
                                empR.hourGetOut = getO.hour;
                            }
                        }
                    }

                    //en retard

                    //Day off
                    List<DayOffEmployer> demps = await catalogDbContext.DayOffEmployer.Include(d => d.DayOff).Where(d => d.IdEmp == g.IdEmp).ToListAsync();
                    if (demps != null)
                    {
                        foreach (DayOffEmployer demp in demps)
                        {
                            Console.WriteLine(demp.DayOff.WeeDay);
                            Console.WriteLine((int)gDayNow.DayOfWeek);
                            if (demp.DayOff.WeeDay == (int)gDayNow.DayOfWeek)
                            {
                                Console.WriteLine("Day off");
                                empR.hier = "Day off";
                            }

                        }
                    }
                    //Hoilidays
                    PublicHolidays ph = catalogDbContext.PublicHolidays.Where(p => p.date == DateOnly.FromDateTime(gDayNow)).SingleOrDefault();

                    if (ph != null)
                    {
                        //PublicHolidays ph = phs.First();
                        Console.WriteLine("public Holidays");
                        empR.hier = "Holiday";

                    }
                    //Day off days
                    DayOfDay dy = catalogDbContext.DayOfDay.Where(d => d.dateStart > DateOnly.FromDateTime(gDayNow) && d.dateEnd < DateOnly.FromDateTime(gDayNow) && d.active == true).SingleOrDefault();

                    if (dy != null)
                    {
                        Console.WriteLine("En congé");

                    }

                    if (HierOrMissing == null)
                    {
                        listEmpR.Add(empR);

                    } else if (HierOrMissing == empR.hier)
                    {
                        listEmpR.Add(empR);

                    }


                }
                //catalogDbContext.SaveChangesAsync();

                var res = JsonSerializer.Serialize(new { listEmp = listEmpR, nbHier = nbHier, nbMissing = nbMissing });
                return Ok(res); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        [HttpGet("/missingOrHier")]
        public async Task<ActionResult> missingOrHier()
        {
            var nbHier = 0;
            var nbMissing = 0;

            try
            { DateTime gDayNow = DateTime.Now;
                DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
                Console.WriteLine(dateNow);
                var emps = await catalogDbContext.Employers.ToListAsync();
                List<EmpResult> listEmpR = new List<EmpResult>();
                foreach (Employer g in emps)
                {
                    Console.WriteLine("==========================");
                    Console.WriteLine(dateNow);
                    EmpResult empR = new EmpResult();
                    empR.employer = g;
                    Console.WriteLine(empR.employer.NameEmp);
                    GetIn gg = await catalogDbContext.GetIn.Include(e => e.Employer).Where(ge => ge.dateIn == dateNow && ge.EmployerId == g.IdEmp).SingleOrDefaultAsync();
                    Console.WriteLine(gg);
                    empR.hier = "Missing";
                    if (gg != null)
                    {
                        Console.WriteLine("not nul"+gg);
                        if (gg.Employer.HoraireId != null)
                        {
                            Console.WriteLine("--------nnnnn----------"+gg.Employer.NameEmp);
                            empR.hier = "Hier";
                            nbHier += 1;
                            
                            Horaire h = await catalogDbContext.Horaire.FirstAsync(e => e.IdHoraire == gg.Employer.HoraireId);
                            Console.WriteLine(gg.hour - h.TimeStart);
                            empR.late = TimeOnly.FromTimeSpan(gg.hour - h.TimeStart);
                            empR.hourGetIn = gg.hour;
                            GetOut getO = await catalogDbContext.GetOut.Include(e => e.Employer).Where(ge => ge.dateOut == dateNow && ge.EmployerId == g.IdEmp).SingleOrDefaultAsync();

                            if (getO != null)
                            {
                                empR.hourGetOut = getO.hour;
                            }
                        }
                    }

                    //en retard

                    //Day off
                    List<DayOffEmployer> demps = await catalogDbContext.DayOffEmployer.Include(d => d.DayOff).Where(d => d.IdEmp == g.IdEmp).ToListAsync();
                    if (demps != null)
                    {
                        foreach (DayOffEmployer demp in demps)
                        {
                            Console.WriteLine(demp.DayOff.WeeDay);
                            Console.WriteLine((int)gDayNow.DayOfWeek);
                            if (demp.DayOff.WeeDay == (int)gDayNow.DayOfWeek)
                            {
                                Console.WriteLine("Day off");
                                empR.hier = "Day off";
                            }

                        }
                    }
                    //Hoilidays
                    PublicHolidays ph = catalogDbContext.PublicHolidays.Where(p => p.date == DateOnly.FromDateTime(gDayNow)).SingleOrDefault();

                    if (ph != null)
                    {
                        //PublicHolidays ph = phs.First();
                        Console.WriteLine("public Holidays");
                        empR.hier = "Holiday";

                    }
                    //Day off days
                    DayOfDay dy = catalogDbContext.DayOfDay.Where(d => d.dateStart > DateOnly.FromDateTime(gDayNow) && d.dateEnd < DateOnly.FromDateTime(gDayNow) && d.active == true).SingleOrDefault();

                    if (dy != null)
                    {
                        Console.WriteLine("En congé");

                    }
                    listEmpR.Add(empR);

                }
                //catalogDbContext.SaveChangesAsync();
                var res= JsonSerializer.Serialize(new { listEmp = listEmpR, nbHier = nbHier, nbMissing = nbMissing });
                return Ok(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost("/GetDayOffDay/{id:int}")]
        public async Task<DayOfDay> GetDayOffDay(int id)
        {

            try
            {

                DayOfDay getOut = catalogDbContext.DayOfDay

                    .Include(e => e.Employer).Where(g => g.IdDayOffDay == id).ToList().FirstOrDefault();
                //catalogDbContext.SaveChangesAsync();

                return getOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet("/GetDayOffDayByEmp/{id:int}")]
        public async Task<List<DayOfDay>> GetDayOffDayByEmp(int id)
        {

            try
            {

                List<DayOfDay> dayOfDays = catalogDbContext.DayOfDay

                    .Include(e => e.Employer).Where(g => g.EmployerId == id).ToList();
                //catalogDbContext.SaveChangesAsync();

                return dayOfDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost("/GetDayOff/{id:int}")]
        public async Task<DayOff> GetDayOff(int id)
        {

            try
            {

                DayOff getOut = catalogDbContext.DayOffs

                    .Where(g => g.IdDayOff == id).ToList().FirstOrDefault();
                //catalogDbContext.SaveChangesAsync();

                return getOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet("/GetDayOffs")]
        public async Task<List<DayOff>> GetDayOffs()
        {

            try
            {

                List<DayOff> dys = catalogDbContext.DayOffs

                    .ToList();
                //catalogDbContext.SaveChangesAsync();

                return dys;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet("/GetPublicHolidays")]
        public async Task<List<PublicHolidays>> GetPublicHolidays()
        {

            try
            {

                var publicHolidays = catalogDbContext.PublicHolidays.ToList();

                return publicHolidays;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost("/GetGetIn/{id:int}")]
        public async Task<GetIn> GetGetIn(int id)
        {

            try
            {

                GetIn getIn = catalogDbContext.GetIn.Where(g => g.IdGetIn == id)
                    .Include(g => g.Employer).ToList().FirstOrDefault();
                //catalogDbContext.SaveChangesAsync();

                return getIn;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost("/GetHoraire/{id:int}")]
        public async Task<Horaire> GetHoraire(int id)
        {

            try
            {

                Horaire horaire = await catalogDbContext.Horaire.FindAsync(id);
                catalogDbContext.SaveChangesAsync();

                return horaire;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //=======================
        [HttpPost("/Authcreate")]
        public async Task<IActionResult> CreateUser([FromBody] Login inboundUser)
        {
            try
            {
                var user = new IdentityUser { UserName = inboundUser.UserName,Email= inboundUser.UserName };

                bool userRoleExists = await _roleManager.RoleExistsAsync(RoleTypes.User.ToString());

                if (!userRoleExists)
                {
                    Console.WriteLine("role not exist"+RoleTypes.User.ToString());
                    await _roleManager.CreateAsync(new IdentityRole(RoleTypes.User.ToString()));
                }

                var result = await _userManager.CreateAsync(user, inboundUser.PassWord);
                Console.WriteLine("role exist" +result);
               

                var errors = result.Errors.Select(e => e.Description);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleTypes.User.ToString());
                    var token =await  BuildToken(inboundUser, new[] { RoleTypes.Admin, RoleTypes.User });
                    return Ok(token);
                }
                else
                {
                    return BadRequest(errors);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //=======================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.UserName, userInfo.PassWord,
                 isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = await BuildToken(userInfo, new[] {RoleTypes.User} );
                return Ok(token);
            }
            else
            {
                return BadRequest("Email or password invalid!");
            }
        }
        //======================
        [HttpGet("/test")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleTypes.User))]
        public IActionResult Health()
        {
            return Ok("Api is fine");
        }
        //======================

        [HttpGet("/app/{id:int}")]
        public IActionResult Get(int id)
        {
            Horaire item = new()
            {

                TimeStart = TimeOnly.FromDateTime(DateTime.UtcNow),
                TimeEnd = TimeOnly.FromDateTime(DateTime.UtcNow)
            };






            return Ok(item);
        }

        [HttpPost("/UploadFile")]
        public IActionResult UploadFile(IFormFile formFile)
        {
            if (!Directory.Exists(this.getRootPath() + "\\profiles\\")) {
                Directory.CreateDirectory(".\\profiles\\");

            }
            if (formFile != null)
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
        //=======================
        [NonAction]
        private async Task<string> BuildToken(Login userInfo, RoleTypes[] roleTypes)
        {
            var user = await _userManager.FindByEmailAsync(userInfo.UserName);
            if (user == null) return null;

            var claims = new List<Claim>() {
              new Claim(JwtRegisteredClaimNames.Email, userInfo.UserName),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roleTypes)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);
            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    



    //=======================



}
}
