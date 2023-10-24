using Gemploy.models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GesEmploy.models
{
    public class Horaire: BaseEntity
    {
        [Key]
        public int IdHoraire { get; set; }
        public TimeOnly TimeStart { get; set; }
        public TimeOnly TimeEnd { get; set; }
        public string type { get; set; }
        [JsonIgnore]
        public ICollection<Employer>? Employers { get; } = new List<Employer>();
    }
}
