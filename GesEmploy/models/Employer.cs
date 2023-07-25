using System.ComponentModel.DataAnnotations;

namespace Gemploy.models
{
    public class Employer
    {
        [Key]
        public int IdEmp { get; set; }
        public string NameEmp { get; set; }
        public string EmailEmp { get; set; }
        public string PhoneEmp { get; set; }
        public DateTime BirthDay { get; set; }
        public bool Status { get; set; }
        public string Occupation { get; set; }
        public string CodeEmp { get; set; }


        public string? UrlPicture { get; set; }

        public string? UrlQrcode { get; set; }

        public Login? Login { get; set; }

        


    }
}
