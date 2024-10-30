using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.DTO;
using Project_API.Models;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private static Project_PRN231Context _context;

        public QuestionController(Project_PRN231Context context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetAllQuestion")]
        public IActionResult GetAllQuestion([FromQuery] string? questionContent,
            [FromQuery] string? sort,
            [FromQuery] string? createdTime)
        {
            var questions = _context.Questions.Include(q => q.User).Include(q => q.Answers).AsQueryable();

            if (!string.IsNullOrEmpty(questionContent))
            {
                questions = questions.Where(q => q.Content.Contains(questionContent) || q.Title.Contains(questionContent) || q.User.Username.Contains(questionContent));
            }



            if (createdTime != null && createdTime.Length > 0)
            {
                DateTime currentTime = DateTime.Now;
                var timeConditions = new List<bool>();

                questions = questions.Where(q =>
                                            (createdTime.Contains('1') && q.CreatedDate >= currentTime.AddDays(-1)) ||
                                            (createdTime.Contains('7') && q.CreatedDate >= currentTime.AddDays(-7)) ||
                                            (createdTime.Contains("30") && q.CreatedDate >= currentTime.AddMonths(-1))
                                            );
            }

            if (sort == "newest")
            {
                questions = questions.OrderByDescending(q => q.CreatedDate);
            }
            else if (sort == "oldest")
            {
                questions = questions.OrderBy(q => q.CreatedDate);
            }

            var listQ = questions.Select(q => new
            {
                questionId = q.QuestionId,
                Avatar = q.User.Avatar,
                Title = q.Title,
                Content = q.Content,
                UserId = q.UserId,
                Author = q.User.Username,
                CreatedDate = GetTimeElapsed(q.CreatedDate),
                totalAnswer = q.Answers.Count
            }).ToList();




            return Ok(listQ);
        }
        [HttpGet("{questionId}")]
        public IActionResult getQuestionById(int questionId)
        {
            if(questionId <= 0)
            {
                return BadRequest("Wrong format!");
            }
            var question = _context.Questions
                .Include(q => q.User)
                .Include(q => q.Answers)
                .ThenInclude(a => a.User)
                .Where(q => q.QuestionId == questionId).FirstOrDefault();
            if(question == null)
            {
                return NotFound();
            }
            var result = new
            {
                questionId = question.QuestionId,
                Title = question.Title,
                Content = question.Content,
                Image = question.User.Avatar,
                userId = question.UserId,
                User =  question.User.Username,
                CreatedAt = GetTimeElapsed(question.CreatedDate),
                Status = question.Status,
                Answers = question.Answers.OrderByDescending(a => a.CreatedAt)
                .Select(a => new
                {
                    answerId = a.AnswerId,
                    userId = a.UserId,
                    User = a.User.Username,
                    Image = a.User.Avatar,
                    CreatedAt = GetTimeElapsed(a.CreatedAt),
                    Content = a.Content,
                    Updated = a.UpdatedAt == null ? null : a.UpdatedAt.ToString()
                })
            };
            return Ok(result);
        }
        private static string GetTimeElapsed(DateTime? createdDate)
        {
            if (createdDate == null)
            {
                return "";
            }
            TimeSpan timeElapsed = DateTime.Now - (DateTime)createdDate;

            if (timeElapsed.TotalDays >= 1)
            {
                return $"{(int)timeElapsed.TotalDays} day(s) ago";
            }
            else if (timeElapsed.TotalHours >= 1)
            {
                return $"{(int)timeElapsed.TotalHours} hour(s) ago";
            }
            else if (timeElapsed.TotalMinutes >= 1)
            {
                return $"{(int)timeElapsed.TotalMinutes} minute(s) ago";
            }
            else
            {
                return $"{(int)timeElapsed.TotalSeconds} second(s) ago";
            }
        }

        [HttpPost(Name = "Insert Question")]
        public IActionResult InsertQuestion([FromBody] QuestionDTO question)
        {
            if (ModelState.IsValid)
            {
                Question newQuestion = new Question();
                newQuestion.Title = question.Title;
                newQuestion.Content = question.Content;
                newQuestion.UserId = question.UserId;
                newQuestion.CreatedDate = DateTime.Now;
                newQuestion.Status = question.Status;
                _context.Questions.Add(newQuestion);
                _context.SaveChanges();
                return Ok(question);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{questionId}")]
        public IActionResult UpdateQuestion(int questionId, [FromBody] QuestionDTO question)
        {
            if (ModelState.IsValid)
            {
                var questionOld = _context.Questions.FirstOrDefault(q => q.QuestionId == questionId);
                if (questionOld != null)
                {

                    questionOld.Title = question.Title == null ? questionOld.Title : question.Title;
                    questionOld.Content = question.Content == null ? questionOld.Content : question.Content;
                    questionOld.Status = question.Status == null ? questionOld.Status : question.Status;
                    _context.Questions.Update(questionOld);
                    _context.SaveChanges();
                    return Ok(questionOld);
                }
                else
                {
                    return NotFound();
                }

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{questionId}")]
        public IActionResult DeleteQuestion(int questionId)
        {
            if (ModelState.IsValid)
            {
                var questionOld = _context.Questions.Include(q => q.Answers).FirstOrDefault(q => q.QuestionId == questionId);
                if (questionOld != null)
                {
                    if(questionOld.Answers != null)
                    {
                        
                        _context.Answers.RemoveRange(questionOld.Answers);
                    }
                    _context.Questions.Remove(questionOld);
                    _context.SaveChanges();
                    return Ok(questionOld);
                }
                else
                {
                    return NotFound();
                }

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
