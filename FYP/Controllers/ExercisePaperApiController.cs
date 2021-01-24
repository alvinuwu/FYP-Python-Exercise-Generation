using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYP.Models;


namespace Lesson10.Controllers
{

    [Route("api/OEQuestion")]
    public class ExercisePaperApiController : Controller
    {
        private AppDbContext _dbContext;

        public ExercisePaperApiController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{topicId}")]
        public IActionResult AJAXquestionOE(int topicId)
        {
            DbSet<OEQuestion_Templates> oeQT = _dbContext.OEQuestion_Templates;
            List<OEQuestion_Templates> oeQTList = oeQT.Where(c => c.TopicId == topicId).ToList();

            List<OEQuestion_Templates> reformatted = new List<OEQuestion_Templates>();

            foreach (var item in oeQTList)
            {
                var figure = item.Figure.Replace("[nline]", "<br>");
                figure = figure.Replace("[comma]", ",");

                var question = item.Question.Replace("[nline]", "<br>");
                question = question.Replace("[comma]", ",");

                var answer = item.Answer.Replace("[nline]", "<br>");
                answer = answer.Replace("[comma]", ",");

                OEQuestion_Templates oeT = new OEQuestion_Templates();
                oeT.Figure = figure;
                oeT.Question = question;
                oeT.Answer = answer;
                oeT.FigureVar = item.FigureVar;
                oeT.QuestionVar = item.QuestionVar;
                oeT.AnswerVar = item.AnswerVar;
                oeT.TopicId = item.TopicId;
                oeT.Id = item.Id;

                reformatted.Add(oeT);
            }

            return Ok(reformatted);
            //return PartialView("_DisplayQn", model);
        }
    }
}