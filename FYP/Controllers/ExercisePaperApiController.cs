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

                    var question = item.Question.Replace("[nline]", "<br>");

                    var answer = item.Answer.Replace("[nline]", "<br>");

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

                    var question = item.Question.Replace("[nline]", "<br>");

                    var answer = item.Answer.Replace("[nline]", "<br>");

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

                    var question = item.Question.Replace("[nline]", "<br>");

                    var answer = item.Answer.Replace("[nline]", "<br>");

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

                    var question = item.Question.Replace("[nline]", "<br>");

                    var answer = item.Answer.Replace("[nline]", "<br>");

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

        [HttpGet("3/{oeQid}/{exercisePaperId}")]
        public IActionResult EditOEQP(int oeQid, int exercisePaperId)
        {
            DbSet<OEQuestions> oeQ = _dbContext.OEQuestions;
            OEQuestions oeQItem = oeQ.Where(c => c.Id == oeQid).FirstOrDefault();

            DbSet<ExercisePaper> expp = _dbContext.ExercisePaper;
            ExercisePaper exppList = expp.Where(c => c.ExercisePaperId == exercisePaperId).FirstOrDefault();

            DbSet<OExPaperList> exP = _dbContext.OExPaperList;
            List<OExPaperList> exPList = exP.Where(c => c.OEQuestionId == oeQid).ToList();

            DbSet<OEQuestionsPaper> oeqP = _dbContext.OEQuestionsPaper;
            List<OEQuestionsPaper> oeqPList = oeqP.ToList();

            if (!(oeqPList.Where(c => c.Id == oeQid).Count() > 0))
            {
                OEQuestionsPaper oeqpS = new OEQuestionsPaper();
                oeqpS.Question = oeQItem.Question;
                oeqpS.Figure = oeQItem.Figure;
                oeqpS.Answer = oeQItem.Answer;
                oeqpS.Topic = oeQItem.Topic;

                if (exPList.Count() == 0)
                {
                    OExPaperList expItem = new OExPaperList();
                    expItem.ExercisePaperId = exercisePaperId;
                    expItem.OEQuestionId = oeQid;
                    _dbContext.OExPaperList.Add(expItem);
                }


                _dbContext.OEQuestionsPaper.Add(oeqpS);

            }
            else
            {
                OExPaperList expItem = new OExPaperList();
                expItem.ExercisePaperId = exercisePaperId;
                expItem.OEQuestionId = oeQid;
                _dbContext.OExPaperList.Add(expItem);
            }
            exppList.TotalQns = exppList.TotalQns + 1;

            _dbContext.ExercisePaper.Update(exppList);
            _dbContext.SaveChanges();

            var figure = oeQItem.Figure.Replace("[nline]", "<br>");

            var question = oeQItem.Question.Replace("[nline]", "<br>");

            var answer = oeQItem.Answer.Replace("[nline]", "<br>");

            OEQuestions oe = new OEQuestions();
            oe.Topic = oeQItem.Topic;
            oe.Figure = figure;
            oe.Question = question;
            oe.Answer = answer;
            oe.Id = oeQItem.Id;
            oe.UseCount = oeQItem.UseCount;



            return Ok(oe);
            //return PartialView("_DisplayQn", model);
        }

        [HttpGet("4/{id}")]
        public IActionResult AddQuestion(int id)
        {
            DbSet<OEQuestions> oeQT = _dbContext.OEQuestions;
            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> topicList = topics.ToList();
            var topic = topicList.Where(c => c.Id.Equals(id)).FirstOrDefault();
            List<OEQuestions> oeQList = oeQT.Where(c => c.Topic == topic.Name).ToList();
            List<OEQuestions> reformatted = new List<OEQuestions>();

            if (topic.Id == 0)
            {
                List<OEQuestions> allOEq = oeQT.ToList();

                foreach (var item in allOEq)
                {
                    var figure = item.Figure.Replace("[nline]", "<br>");

                    var question = item.Question.Replace("[nline]", "<br>");

                    var answer = item.Answer.Replace("[nline]", "<br>");

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

            foreach (var item in oeQList)
            {
                var figure = item.Figure.Replace("[nline]", "<br>");

                var question = item.Question.Replace("[nline]", "<br>");

                var answer = item.Answer.Replace("[nline]", "<br>");

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

        [HttpGet("5/{oeQid}/{exercisePaperId}")]
        public IActionResult DeleteOEQfromP(int oeQid, int exercisePaperId)
        {
            DbSet<ExercisePaper> expp = _dbContext.ExercisePaper;
            ExercisePaper exppList = expp.Where(c => c.ExercisePaperId == exercisePaperId).FirstOrDefault();

            DbSet<OExPaperList> exP = _dbContext.OExPaperList;
            OExPaperList expItem = exP.Where(c => c.ExercisePaperId == exercisePaperId && c.OEQuestionId == oeQid).FirstOrDefault();


            exppList.TotalQns = exppList.TotalQns - 1;

            _dbContext.ExercisePaper.Update(exppList);
            if (expItem != null)
            {
                _dbContext.OExPaperList.Remove(expItem);
            }
            _dbContext.SaveChanges();


            return Ok();
        }
    }
}