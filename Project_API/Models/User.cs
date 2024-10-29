using System;
using System.Collections.Generic;

namespace Project_API.Models
{
    public partial class User
    {
        public User()
        {
            Answers = new HashSet<Answer>();
            Questions = new HashSet<Question>();
        }

        public int UserId { get; set; }
        
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool? Gender { get; set; }
        public string? Avatar { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
