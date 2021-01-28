using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYP.Models;


namespace FYP.Controllers
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
            List<OEQuestion_Templates> reformatted = new List<OEQuestion_Templates>();

            if (topicId == 0)
            {
                List<OEQuestion_Templates> allOEqT = oeQT.ToList();
                foreach (var item in allOEqT)
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
            }
            else
            {
                List<OEQuestion_Templates> oeQTList = oeQT.Where(c => c.TopicId == topicId).ToList();

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
            }


            return Ok(reformatted);
            //return PartialView("_DisplayQn", model);
        }

        [HttpGet("2/{topicId}")]
        public IActionResult AJAXquestionOEq(int topicId)
        {
            DbSet<OEQuestions> oeQ = _dbContext.OEQuestions;
            List<OEQuestions> reformatted = new List<OEQuestions>();

            if (topicId == 0)
            {
                List<OEQuestions> allOEq = oeQ.ToList();

                foreach (var item in allOEq)
                {
                    var figure = item.Figure.Replace("[nline]", "<br>");
                    figure = figure.Replace("[comma]", ",");

                    var question = item.Question.Replace("[nline]", "<br>");
                    question = question.Replace("[comma]", ",");

                    var answer = item.Answer.Replace("[nline]", "<br>");
                    answer = answer.Replace("[comma]", ",");

                    OEQuestions oe = new OEQuestions();
                    oe.Topic = item.Topic;
                    oe.Figure = figure;
                    oe.Question = question;
                    oe.Answer = answer;
                    oe.Id = item.Id;
                    oe.UseCount = item.UseCount;

                    reformatted.Add(oe);
                }

                return Ok(reformatted);
            }
            else
            {
                DbSet<Topics> topics = _dbContext.Topics;
                List<Topics> topicList = topics.ToList();
                var topic = topicList.Where(c => c.Id.Equals(topicId)).FirstOrDefault();
                List<OEQuestions> oeQList = oeQ.Where(c => c.Topic == topic.Name).ToList();
                foreach (var item in oeQList)
                {
                    var figure = item.Figure.Replace("[nline]", "<br>");
                    figure = figure.Replace("[comma]", ",");

                    var question = item.Question.Replace("[nline]", "<br>");
                    question = question.Replace("[comma]", ",");

                    var answer = item.Answer.Replace("[nline]", "<br>");
                    answer = answer.Replace("[comma]", ",");

                    OEQuestions oe = new OEQuestions();
                    oe.Topic = item.Topic;
                    oe.Figure = figure;
                    oe.Question = question;
                    oe.Answer = answer;
                    oe.Id = item.Id;
                    oe.UseCount = item.UseCount;

                    reformatted.Add(oe);
                }
            }


            return Ok(reformatted);
            //return PartialView("_DisplayQn", model);
        }
    }
}