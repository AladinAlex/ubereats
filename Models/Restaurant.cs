﻿namespace ubereats.Models
{
    public class Restaurant
    {
        public int ID { get; set; }
        public string RestName { get; set; } = "";
        public string District { get; set; } = "";
        public string KitchenType { get; set; } = "";
        public TimeSpan CookingTime { get; set; }
        public byte[] Image { get; set; } = null!;
        public bool isDeleted { get; set; }
    }
}