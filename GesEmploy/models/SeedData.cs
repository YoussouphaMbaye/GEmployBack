
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace Gemploy.models
{
    public class SeedData
    {
        private readonly CatalogDbContext context;
        public SeedData(CatalogDbContext context)
        {
            this.context = context;
        }
        public void seed()
        {
            if (context.Employers.Any())
            {
                return;   // DB has been seeded
            }
            context.Employers.AddRange(
                new Employer
                {
                    //IdEmp=1,
                    NameEmp = "Youssou CISSE",
                    EmailEmp = "you@gmail.com",
                    UrlPicture = "http:\\hhhh",
                    CodeEmp = "002334894",
                    BirthDay = new DateTime(2008, 6, 11),
                    Occupation = "IT",
                    Status = true,
                    PhoneEmp = "779000212",


                },
                new Employer
                {
                    //IdEmp = 2,
                    NameEmp = "Laye CISSE",
                    EmailEmp = "laye@gmail.com",
                    UrlPicture = "http:\\hhhh",
                    CodeEmp = "004334894",
                    BirthDay = new DateTime(2008, 6, 11),
                    Occupation = "WEB develloppeur",
                    Status = true,
                    PhoneEmp = "779003212",
                },
                new Employer
                {
                    //IdEmp = 3,
                    NameEmp = "Sidi Fall",
                    EmailEmp = "sidi@gmail.com",
                    UrlPicture = "http:\\hhhh",
                    CodeEmp = "004334896",
                    BirthDay = new DateTime(2008, 6, 11),
                    Occupation = "WEB develloppeur",
                    Status = true,
                    PhoneEmp = "779003212",
                },
                new Employer
                {
                    //IdEmp = 3,
                    NameEmp = "Pape Fall",
                    EmailEmp = "Fall@gmail.com",
                    UrlPicture = "http:\\hhhh",
                    CodeEmp = "004334896",
                    BirthDay = new DateTime(2008, 6, 11),
                    Occupation = "WEB develloppeur",
                    Status = true,
                    PhoneEmp = "779003212",
                }


            );
            context.SaveChanges();

        }
        
    }
}
