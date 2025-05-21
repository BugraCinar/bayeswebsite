using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace bayessoft.Models
{
    [Table("admin")]
    public class Admin
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public required string Username { get; set; }

        [Column("password_hash")]
        public required string PasswordHash { get; set; }
    }
}