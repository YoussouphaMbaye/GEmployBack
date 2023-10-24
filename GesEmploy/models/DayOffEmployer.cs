using Gemploy.models;

namespace GesEmploy.models
{
    public class DayOffEmployer: BaseEntity
    {
        public int IdEmp { get; set; }
        public Employer ? Employer { get; set; }
        public int IdDayOff { get; set; }
        public DayOff ? DayOff { get; set; }

    }
}
