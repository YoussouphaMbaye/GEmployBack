using Gemploy.models;

namespace GesEmploy.models
{
    public class EmpResult
    {
        public Employer employer { get; set; }

        public string hier { get; set; }
        public TimeOnly late { get; set; }
        public TimeOnly hourGetIn { get; set; }
        public TimeOnly hourGetOut { get; set; }
    }
}
