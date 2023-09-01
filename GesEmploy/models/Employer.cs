using GesEmploy.models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        
        public Horaire? Horaire { get; set; }
        

        public int? HoraireId { get; set; }
        [JsonIgnore]
        public ICollection<GetIn>? getIns { get; } = new List<GetIn>();
        [JsonIgnore]
        public ICollection<GetOut>? getOuts { get; } = new List<GetOut>();
        [JsonIgnore]
        public ICollection<DayOffEmployer>? DayOffEmployer { get; } = new List<DayOffEmployer>();
        [JsonIgnore]
        public ICollection<DayOfDay>? DayOfDays { get; } = new List<DayOfDay>();




    }
}
