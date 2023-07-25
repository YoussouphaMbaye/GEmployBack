using System.ComponentModel.DataAnnotations;

namespace Gemploy.models
{
    public class Login
    {
        [Key]
        public int IdLog { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string RoleEmp { get; set; }
        public bool Status { get; set; }
        public Employer Employ { get; set; }
        
    }
}
