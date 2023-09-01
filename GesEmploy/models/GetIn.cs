using Gemploy.models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GesEmploy.models
{
    public class GetIn
    {
        [Key]
        public int IdGetIn { get; set; }
        public int EmployerId { get; set; }
        public TimeOnly hour { get; set; }
        public DateOnly dateIn { get; set; }
        //public int? GetOutId { get; set; }
        public Employer? Employer { get; set; }
    }
}
