using Gemploy.models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GesEmploy.models
{
    public class GetOut: BaseEntity
    {
        [Key]
        public int IdGetOut { get; set; }
        public TimeOnly hour { get; set; }
 
        public int EmployerId { get; set; }
        public DateOnly dateOut { get; set; }
        public Employer? Employer { get; set; }


    }
}
