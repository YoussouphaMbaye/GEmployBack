using Gemploy.models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GesEmploy.models
{
    public class Departement
    {
        [Key]
        public int IdDepartement { get; set; }
        public  string DepartementName  { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public ICollection<Employer>? Employers { get; } = new List<Employer>();
    }
}
