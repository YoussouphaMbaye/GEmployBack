using System.ComponentModel.DataAnnotations;

namespace GesEmploy.models
{
    public class PublicHolidays: BaseEntity
    {
        [Key]
        public int IdPublicHolidaysayOff { get; set; }
        public DateOnly date { get; set; }

    }
}
