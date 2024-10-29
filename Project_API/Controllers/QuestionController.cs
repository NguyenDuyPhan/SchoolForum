using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult GetAllQuestion()
        {
            var questions = _context.Questions.ToList();
            return Ok(questions);
        }

        [HttpPost(Name = "Insert Question")]
        public IActionResult InsertQuestion([FromBody] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Questions.Add(question);
                _context.SaveChanges();
                return Ok(question);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{questionId}")]
        public IActionResult UpdateQuestion(int questionId ,[FromBody] Question question)
        {
            if (ModelState.IsValid)
            {
                var questionOld = _context.Questions.FirstOrDefault(q => q.QuestionId == questionId);
                if (questionOld != null)
                {

                    questionOld.Title = question.Title == null ? questionOld.Title : question.Title;
                    questionOld.Content = question.Content == null ? questionOld.Content : question.Content;
                    _context.Questions.Update(questionOld);
                    _context.SaveChanges();
                    return Ok(questionOld);
                } else
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
                var questionOld = _context.Questions.FirstOrDefault(q => q.QuestionId == questionId);
                if (questionOld != null)
                {
                    
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
