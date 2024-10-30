using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Project_API.Models
{
    public partial class Answer
    {
        public Answer()
        {
            InverseParentAnswerNavigation = new HashSet<Answer>();
        }

        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int? ParentAnswer { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Answer? ParentAnswerNavigation { get; set; }
        [JsonIgnore]
        public virtual Question Question { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Answer> InverseParentAnswerNavigation { get; set; }
    }
}
