﻿namespace ubereats.Models
{
    public class User
    {
        public int Id { get; set; }
        public string loginname { get; set; } = "";
        public string password { get; set; } = "";
        public string email { get; set; } = "";
    }
}
