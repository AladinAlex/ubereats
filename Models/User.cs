using System.ComponentModel.DataAnnotations.Schema;

namespace ubereats.Models
{
    public class User
    {
        [Column("User_ID")]
        public int Id { get; set; }
        public string loginname { get; set; } = "";
        public string password { get; set; } = "";
        public string email { get; set; } = "";
    }
}
