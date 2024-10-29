using System;
using System.Collections.Generic;

namespace Project_API.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Status { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
