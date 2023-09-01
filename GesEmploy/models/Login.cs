using System.ComponentModel.DataAnnotations;

namespace Gemploy.models
{
    public class Login
    {
        [Key]
        public int IdLog { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public Employer ?Employ { get; set; }
        
    }
}
