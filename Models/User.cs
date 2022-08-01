using System.Text.Json.Serialization;

namespace ubereats.Models
{
    public class User
    {
        public int Id { get; set; }
        public string loginname { get; set; } = "";
        [JsonIgnore]
        public string password { get; set; } = "";
        public string email { get; set; } = "";
    }
}
