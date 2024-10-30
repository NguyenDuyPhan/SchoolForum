using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.DTO;
using Project_API.Models;

namespace Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {

        private static Project_PRN231Context _context;

        public AnswerController(Project_PRN231Context context)
        {
            _context = context;
        }

        [HttpGet("{questionId}")]
        public ActionResult GetAnswersByQuestionId(int questionId) {

            if (questionId <= 0) {
                return NotFound();
            }

            var answer = _context.Answers.Include(a => a.User)
                .Where(a => a.QuestionId == questionId)
                .Select(a => new
                {
                    Username = a.User.Username,
                    CreatedAt = GetTimeElapsed(a.CreatedAt),
                    Content = a.Content
                }).ToList();

            return Ok(answer);
        }

        [HttpPost]

        public IActionResult InsertAnswer(AnswerDTO answer)
        {
            if (answer == null)
            {
                return BadRequest();
            }
            Answer newAnswer = new Answer();
            newAnswer.UserId = answer.UserId;
            newAnswer.QuestionId = answer.QuestionId;
            newAnswer.Content = answer.Content;
            newAnswer.CreatedAt = DateTime.Now;
            _context.Answers.Add(newAnswer);
            _context.SaveChanges();
            return Ok(newAnswer);
        }

        [HttpPut("{answerId}")]
        public ActionResult UpdateAnswer(int answerId,[FromBody]AnswerDTO answer) {

            if (answer == null || answerId <= 0)
            {
                return BadRequest("Id or answer wrong format!");
            }

            var answerOld = _context.Answers.FirstOrDefault(a => a.AnswerId == answerId);
            if (answerOld != null)
            {

                answerOld.Content = answer.Content == null ? answerOld.Content : answer.Content;
                answerOld.UpdatedAt = DateTime.Now;
                _context.Answers.Update(answerOld);
                _context.SaveChanges();
                return Ok(answerOld);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{answerId}")]
        public IActionResult DeleteQuestion(int answerId)
        {
            if (ModelState.IsValid)
            {
                var answerOld = _context.Answers.FirstOrDefault(a => a.AnswerId == answerId);
                if (answerOld != null)
                {

                    _context.Answers.Remove(answerOld);
                    _context.SaveChanges();
                    return Ok(answerOld);
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
    }
}
