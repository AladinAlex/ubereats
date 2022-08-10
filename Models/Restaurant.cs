using System.ComponentModel.DataAnnotations.Schema;

namespace ubereats.Models
{
    public class Restaurant
    {
        [Column("Restaurant_ID")]
        public int ID { get; set; }
        public string RestName { get; set; } = "";
        public string District { get; set; } = "";
        public string KitchenType { get; set; } = "";
        public int CookingTimeStart { get; set; }
        public int CookingTimeEnd { get; set; }
        public byte[] Image { get; set; } = null!;
        public bool isDeleted { get; set; }
    }
}
