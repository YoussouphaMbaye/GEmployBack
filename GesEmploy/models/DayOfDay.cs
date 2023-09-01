using Gemploy.models;
using System.ComponentModel.DataAnnotations;

namespace GesEmploy.models
{
    public class DayOfDay
    {
        [Key]
        public int IdDayOffDay { get; set; }
        public DateOnly dateStart { get; set; }
        public DateOnly dateEnd { get; set;}
        public bool active { get; set; }
        public int EmployerId { get; set; }
        public Employer? Employer { get; set; }

    }
}
