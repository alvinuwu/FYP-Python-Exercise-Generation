using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using FYP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Rotativa.AspNetCore;

namespace FYP.Controllers
{
    public class ExercisePaperController : Controller
    {
        private AppDbContext _dbContext;
        public ExercisePaperController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult ViewTopics()
        {
            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.ToList();

            return View(model);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult AddTopic()
        {
            return View();
        }

        [Authorize(Roles = "Lecturer,Admin")]
        [HttpPost]
        public IActionResult AddTopic(Topics topics)
        {
            DbSet<Topics> dbTopics = _dbContext.Topics;
            List<Topics> topicList = dbTopics.ToList();

            if (topicList.Where(c => c.Name.Equals(topics.Name)).Count() > 0)
            {
                TempData["Msg"] = "Failed to update database! Topic name already exists.";
                TempData["MsgType"] = "danger";

                return RedirectToAction("ViewTopics");
            }
            else
            {
                Topics t = new Topics();
                t.Id = topicList.Count();
                t.Name = topics.Name;

                _dbContext.Topics.Add(t);
                _dbContext.SaveChanges();

                TempData["Msg"] = "New Topic added!";
                TempData["MsgType"] = "success";

                return RedirectToAction("ViewTopics");
            }
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult EditOEQuestionsTemplate(int id)
        {
            DbSet<OEQuestion_Templates> dboeT = _dbContext.OEQuestion_Templates;
            OEQuestion_Templates oeT = dboeT.Where(c => c.Id == id).FirstOrDefault();

            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.Where(c => c.Id != 0).ToList();
            ViewData["topics"] = new SelectList(model, "Id", "Name");

            return View(oeT);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        [HttpPost]
        public IActionResult EditOEQuestionsTemplate(IFormCollection form)
        {
            var id = Int32.Parse(form["Id"]);
            var topicId = Int32.Parse(form["TopicId"]);
            var figure = form["Figure"];
            var question = form["Question"];
            var answer = form["Answer"];

            List<string> figureVarList = new List<string>();
            List<string> questionVarList = new List<string>();
            List<string> answerVarList = new List<string>();

            for (int i = 0; i < form.Count(); i++)
            {
                var figureVar = form["FigureVar[" + i + "]"].ToString();
                if (figureVar == "")
                {
                    figureVar = "[=]";
                }
                var questionVar = form["QuestionVar[" + i + "]"].ToString();
                if (questionVar == "")
                {
                    questionVar = "[=]";
                }
                var answerVar = form["AnswerVar[" + i + "]"].ToString();
                if (answerVar == "")
                {
                    answerVar = "[=]";
                }

                figureVarList.Add(figureVar);
                questionVarList.Add(questionVar);
                answerVarList.Add(answerVar);
            }
            for (int i = 0; i < form.Count(); i++)
            {
                var figureVars = form["FigureVars[" + i + "]"].ToString();
                if (figureVars == "")
                {
                    figureVars = "[=]";
                }

                var questionVars = form["QuestionVars[" + i + "]"].ToString();
                if (questionVars == "")
                {
                    questionVars = "[=]";
                }

                var answerVars = form["AnswerVars[" + i + "]"].ToString();
                if (answerVars == "")
                {
                    answerVars = "[=]";
                }

                figureVarList.Add(figureVars);
                questionVarList.Add(questionVars);
                answerVarList.Add(answerVars);
            }
            int counter = 0;
            int totalCount = figureVarList.Count();
            for (int i = 0; i < totalCount; i++)
            {
                if (figureVarList[counter].Equals("[=]") && questionVarList[counter].Equals("[=]") && answerVarList[counter].Equals("[=]"))
                {
                    figureVarList.Remove(figureVarList[counter]);
                    questionVarList.Remove(questionVarList[counter]);
                    answerVarList.Remove(answerVarList[counter]);
                }
                else
                {
                    counter++;
                }
            }

            //var countF = figureVarList.Count();
            //var countQ = questionVarList.Count();
            //var countA = answerVarList.Count();

            //var countList = new List<int>();
            //countList.Add(countF);
            //countList.Add(countQ);
            //countList.Add(countA);

            //var maxCount = 0;
            //foreach (var obj in countList)
            //{
            //    if (obj > maxCount)
            //    {
            //        maxCount = obj;
            //    }
            //}

            //var emptyString = "";
            //for (int i = 0; i < maxCount; i++)
            //{
            //    if (figureVarList.Count() < maxCount)
            //    {
            //        figureVarList.Add(emptyString);
            //    }
            //}
            //for (int i = 0; i < maxCount; i++)
            //{
            //    if (questionVarList.Count() < maxCount)
            //    {
            //        questionVarList.Add(emptyString);
            //    }
            //}
            //for (int i = 0; i < maxCount; i++)
            //{
            //    if (answerVarList.Count() < maxCount)
            //    {
            //        answerVarList.Add(emptyString);
            //    }
            //}


            var splitFigures = "";
            var fCount = 1;

            foreach (var item in figureVarList)
            {
                if (figureVarList.Count() == fCount)
                {
                    splitFigures += item.Replace(Environment.NewLine, "[nline]");
                }
                else if (figureVarList.Count() > fCount)
                {
                    splitFigures += item.Replace(Environment.NewLine, "[nline]") + "[/]";
                }
                fCount++;
            }

            var splitQuestions = "";
            var qCount = 1;

            foreach (var item in questionVarList)
            {
                if (questionVarList.Count() == qCount)
                {
                    splitQuestions += item.Replace(Environment.NewLine, "[nline]");
                }
                else if (questionVarList.Count() > qCount)
                {
                    splitQuestions += item.Replace(Environment.NewLine, "[nline]") + "[/]";
                }
                qCount++;
            }

            var splitAnswers = "";
            var aCount = 1;

            foreach (var item in answerVarList)
            {
                if (answerVarList.Count() == aCount)
                {
                    splitAnswers += item.Replace(Environment.NewLine, "[nline]");
                }
                else if (answerVarList.Count() > aCount)
                {
                    splitAnswers += item.Replace(Environment.NewLine, "[nline]") + "[/]";
                }
                aCount++;
            }

            splitFigures = splitFigures.Replace("[=]", "");
            splitQuestions = splitQuestions.Replace("[=]", "");
            splitAnswers = splitAnswers.Replace("[=]", "");



            DbSet<OEQuestion_Templates> ex = _dbContext.OEQuestion_Templates;
            OEQuestion_Templates oeTItem = ex.Where(o => o.TopicId == topicId).FirstOrDefault();
            oeTItem.Figure = figure;
            oeTItem.Question = question;
            oeTItem.Answer = answer;
            oeTItem.FigureVar = splitFigures;
            oeTItem.QuestionVar = splitQuestions;
            oeTItem.AnswerVar = splitAnswers;

            if (_dbContext.SaveChanges() == 1)
            {
                TempData["Msg"] = "Open-Ended Template updated!";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Msg"] = "Failed to update database!";
            }

            return RedirectToAction("AutoGenerateQn");
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult DeleteOEQuestionsTemplate(int id)
        {
            DbSet<OEQuestion_Templates> ex = _dbContext.OEQuestion_Templates;
            OEQuestion_Templates oe = ex.Where(c => c.Id == id).FirstOrDefault();

            if (oe != null)
            {
                ex.Remove(oe);
                if (_dbContext.SaveChanges() == 1)
                {
                    TempData["Msg"] = "Open Ended Template deleted!";
                    TempData["MsgType"] = "warning";
                }
                else
                {
                    TempData["Msg"] = "Failed to delete Template!";
                }
            }
            else
            {
                TempData["Msg"] = "Question Template not found!";
            }
            return RedirectToAction("AutoGenerateQn");
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult AutoGenerateQn()
        {
            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.ToList();
            ViewData["topics"] = new SelectList(model, "Id", "Name");
            DbSet<OEQuestion_Templates> oeQT = _dbContext.OEQuestion_Templates;
            List<OEQuestion_Templates> oeQTList = oeQT.ToList();

            return View(oeQTList);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult AutoGenerateQnPt2(int id, int topicId)
        {
            DbSet<OEQuestion_Templates> oeQT = _dbContext.OEQuestion_Templates;
            var model = oeQT.Where(c => c.Id == id).FirstOrDefault();
            TempData["Topic"] = topicId;


            return View(model);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        [HttpPost]
        public IActionResult AutoGenerateQnPt2(IFormCollection form)
        {
            var topicId = Int32.Parse(TempData["Topic"].ToString());
            DbSet<Topics> dbTopic = _dbContext.Topics;
            var topic = dbTopic.Where(c => c.Id == topicId).FirstOrDefault();

            var figure = form["figure"];
            var question = form["question"];
            var answer = form["answer"];
            var checkCount = form["useCount"];
            var checkDifficulty = form["difficulty"];
            var useCount = 0;
            if (!checkCount.Equals(""))
            {
                useCount = Int32.Parse(form["useCount"]);
            }
            var difficulty = 0;
            if (!checkCount.Equals(""))
            {
                difficulty = Int32.Parse(form["difficulty"]);
            }
            OEQuestions oeQuestions = new OEQuestions();
            oeQuestions.Figure = figure;
            oeQuestions.Question = question;
            oeQuestions.Answer = answer;
            oeQuestions.Topic = topic.Name;
            if (useCount != 0)
            {
                oeQuestions.UseCount = useCount;
            }
            else
            {
                oeQuestions.UseCount = null;
            }
            if (difficulty != 0)
            {
                oeQuestions.Difficulty = difficulty;
            }
            else
            {
                oeQuestions.Difficulty = 3;
            }
            _dbContext.OEQuestions.Add(oeQuestions);
            _dbContext.SaveChanges();

            TempData["Msg"] = "New Open-Ended Question added!";
            TempData["MsgType"] = "success";

            return RedirectToAction("AutoGenerateQn");
        }

        public IActionResult RandomiseOEF(int id, int rand)
        {
            DbSet<OEQuestion_Templates> dbs = _dbContext.OEQuestion_Templates;
            var oeList = dbs.Where(l => l.Id == id).FirstOrDefault();

            Random r = new Random();
            var figureSplit = oeList.FigureVar.Split("[/]");

            var figure = figureSplit[rand];

            if (figure == "")
            {
                figure = oeList.Figure;


            }

            return PartialView("_DisplayF", figure);
        }

        public IActionResult RandomiseOEQ(int id, int rand)
        {
            DbSet<OEQuestion_Templates> dbs = _dbContext.OEQuestion_Templates;
            var oeList = dbs.Where(l => l.Id == id).FirstOrDefault();

            Random r = new Random();
            var questionSplit = oeList.QuestionVar.Split("[/]");

            var question = questionSplit[rand];


            if (question == "")
            {
                question = oeList.Question;

            }


            return PartialView("_DisplayQ", question);
        }

        public IActionResult RandomiseOEA(int id, int rand)
        {
            DbSet<OEQuestion_Templates> dbs = _dbContext.OEQuestion_Templates;
            var oeList = dbs.Where(l => l.Id == id).FirstOrDefault();

            Random r = new Random();
            var answerSplit = oeList.AnswerVar.Split("[/]");

            var answer = answerSplit[rand];


            if (answer == "")
            {
                answer = oeList.Answer;

            }


            return PartialView("_DisplayA", answer);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult ViewOEQuestions()
        {
            DbSet<OEQuestions> oeQ = _dbContext.OEQuestions;
            List<OEQuestions> oeList = oeQ.ToList();

            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.Where(c => c.Id != 0).ToList();
            ViewData["topics"] = new SelectList(model, "Id", "Name");

            return View(oeList);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult EditOEQuestions(int id)
        {
            DbSet<OEQuestions> oeQ = _dbContext.OEQuestions;
            OEQuestions oe = oeQ.Where(c => c.Id == id).FirstOrDefault();

            return View(oe);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        [HttpPost]
        public IActionResult EditOEQuestions(OEQuestions item)
        {
            DbSet<OEQuestions> oeQ = _dbContext.OEQuestions;
            OEQuestions oe = oeQ.Where(c => c.Id == item.Id).FirstOrDefault();

            var question = item.Question.Replace(Environment.NewLine, "[nline]");
            var figure = item.Figure.Replace(Environment.NewLine, "[nline]");
            var answer = item.Answer.Replace(Environment.NewLine, "[nline]");

            oe.Id = item.Id;
            oe.Figure = figure;
            oe.Question = question;
            oe.Answer = figure;
            oe.UseCount = item.UseCount;
            oe.Topic = item.Topic;
            oe.Difficulty = item.Difficulty;

            if (_dbContext.SaveChanges() == 1)
            {
                TempData["Msg"] = "Open-Ended Question updated!";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Msg"] = "Failed to update database!";
            }

            return RedirectToAction("ViewOEQuestions");
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult DeleteOEQuestions(int id)
        {
            DbSet<OEQuestions> oeQ = _dbContext.OEQuestions;
            OEQuestions oe = oeQ.Where(c => c.Id == id).FirstOrDefault();

            if (oe != null)
            {
                oeQ.Remove(oe);
                if (_dbContext.SaveChanges() == 1)
                {
                    TempData["Msg"] = "Open Ended Question deleted!";
                    TempData["MsgType"] = "warning";
                }
                else
                {
                    TempData["Msg"] = "Failed to delete Question!";
                }
            }
            else
            {
                TempData["Msg"] = "Question not found!";
            }
            return RedirectToAction("ViewOEQuestions");
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult ViewOEPapers()
        {
            DbSet<ExercisePaper> dbs = _dbContext.ExercisePaper;
            List<ExercisePaper> model = dbs.ToList();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            DbSet<ExercisePaper> dbsExercisePaper = _dbContext.ExercisePaper;
            DbSet<AppUser> dbsUser = _dbContext.AppUser;

            ViewData["ExercisePaper"] = dbsExercisePaper.ToList<ExercisePaper>();
            ViewData["users"] = dbsUser.ToList<AppUser>();

            return View(model);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult EditOEQuestionsPaper(int id)
        {
            ViewData["ExercisePaperId"] = id;
            DbSet<OExPaperList> exPl = _dbContext.OExPaperList;
            List<OExPaperList> exl = exPl.Where(c => c.ExercisePaperId == id).ToList();

            DbSet<OEQuestionsPaper> exP = _dbContext.OEQuestionsPaper;
            List<OEQuestionsPaper> newList = new List<OEQuestionsPaper>();

            foreach (var item in exl)
            {
                var exQid = item.OEQuestionId;
                OEQuestionsPaper ex = exP.Where(c => c.Id == exQid).FirstOrDefault();
                newList.Add(ex);
            }

            DbSet<ExercisePaper> dbsExercisePaper = _dbContext.ExercisePaper;
            List<ExercisePaper> exercisePaperList = dbsExercisePaper.ToList();
            var filterExPaper = exercisePaperList.Where(o => o.ExercisePaperId == id).ToList();
            ViewData["ExercisePaperName"] = filterExPaper[0].Name;

            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.ToList();
            ViewData["topics"] = new SelectList(model, "Id", "Name");

            return View(newList);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult DeleteOEQuestionsPaper(int id)
        {
            DbSet<ExercisePaper> ex = _dbContext.ExercisePaper;
            ExercisePaper oe = ex.Where(c => c.Id == id).FirstOrDefault();

            if (oe != null)
            {
                ex.Remove(oe);
                if (_dbContext.SaveChanges() == 1)
                {
                    TempData["Msg"] = "Open Ended Paper deleted!";
                    TempData["MsgType"] = "warning";
                }
                else
                {
                    TempData["Msg"] = "Failed to delete Paper!";
                }
            }
            else
            {
                TempData["Msg"] = "Question not found!";
            }
            return RedirectToAction("ViewOEPapers");
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult GenerateOE()
        {
            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.Where(c => c.Id != 0).ToList();
            ViewData["topics"] = new SelectList(model, "Id", "Name");

            return View();
        }

        [Authorize(Roles = "Lecturer,Admin")]
        [HttpPost]
        public IActionResult GenerateOE(CreateExQuestion createExQuestion)
        {
            //CURRENTLY the variants are fixed at only 3 for every question,
            //will need to use jQuery to make the page more dynamic, so the
            //user can decide how many variants they want


            if (ModelState.IsValid)
            {
                //setting values to be stored in the database
                //"Environment.NewLine" detects if the string has '\r\n', to be able to replace to different value
                var topicId = createExQuestion.Topics;
                var question = createExQuestion.Question.Replace(Environment.NewLine, "[nline]");
                var figure = createExQuestion.Figure.Replace(Environment.NewLine, "[nline]");
                var answer = createExQuestion.Answer.Replace(Environment.NewLine, "[nline]");

                //set variable for the Variants to be stored as a single string to be stored in the database
                int questionCount = 1;

                if (createExQuestion.QuestionVar != null)
                {
                    string questionVar = "";
                    var questionNullRemove = new List<string>();     //Though the other 2 variants can be empty, the string might be saved as "example, "

                    foreach (var item in createExQuestion.QuestionVar)
                    {
                        if (item != null)
                        {
                            questionNullRemove.Add(item);
                        }
                    }
                    foreach (var item in questionNullRemove)
                    {
                        if (item != null)
                        {
                            if (questionNullRemove.Count() == questionCount)
                            {
                                questionVar += item.Replace(Environment.NewLine, "[nline]");
                            }
                            else if (questionNullRemove.Count() > questionCount)
                            {
                                questionVar += item.Replace(Environment.NewLine, "[nline]") + "[/]";
                            }
                            questionCount++;
                        }

                    }

                    int figureCount = 1;
                    string figureVar = "";
                    var figureNullRemove = new List<string>();
                    foreach (var item in createExQuestion.FigureVar)
                    {
                        if (item != null)
                        {
                            figureNullRemove.Add(item);
                        }
                    }
                    foreach (var item in figureNullRemove)
                    {
                        if (item != null)
                        {
                            if (figureNullRemove.Count() == figureCount)
                            {
                                figureVar += item.Replace(Environment.NewLine, "[nline]");
                            }
                            else if (figureNullRemove.Count() > figureCount)
                            {
                                figureVar += item.Replace(Environment.NewLine, "[nline]") + "[/]";
                            }
                            figureCount++;
                        }
                    }

                    int answerCount = 1;
                    string answerVar = "";
                    var answerNullRemove = new List<string>();
                    foreach (var item in createExQuestion.AnswerVar)
                    {
                        if (item != null)
                        {
                            answerNullRemove.Add(item);
                        }
                    }
                    foreach (var item in answerNullRemove)
                    {
                        if (item != null)
                        {
                            if (answerNullRemove.Count() == answerCount)
                            {
                                answerVar += item.Replace(Environment.NewLine, "[nline]");
                            }
                            else if (answerNullRemove.Count() > answerCount)
                            {
                                answerVar += item.Replace(Environment.NewLine, "[nline]") + "[/]";
                            }
                            answerCount++;
                        }
                    }

                    OEQuestion_Templates qT = new OEQuestion_Templates();
                    qT.TopicId = topicId;
                    qT.Figure = figure;
                    qT.Question = question;
                    qT.Answer = answer;
                    qT.FigureVar = figureVar;
                    qT.QuestionVar = questionVar;
                    qT.AnswerVar = answerVar;
                    _dbContext.OEQuestion_Templates.Add(qT);
                    _dbContext.SaveChanges();
                }
                else
                {
                    OEQuestion_Templates qT = new OEQuestion_Templates();
                    qT.TopicId = topicId;
                    qT.Figure = figure;
                    qT.Question = question;
                    qT.Answer = answer;
                    qT.FigureVar = "";
                    qT.QuestionVar = "";
                    qT.AnswerVar = "";
                    _dbContext.OEQuestion_Templates.Add(qT);
                    _dbContext.SaveChanges();
                }

                TempData["Msg"] = "New Question added!";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
                TempData["MsgType"] = "danger";
            }


            return RedirectToAction("AutoGenerateQn");
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult Create()
        {
            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.Where(c => c.Id != 0).ToList();
            ViewData["topics"] = new SelectList(model, "Id", "Name");

            return View();
        }

        [Authorize(Roles = "Lecturer,Admin")]
        [HttpPost]
        public IActionResult Create(CreateExPaper createExPaper)
        {

            //CREATE POST - GENERATION OF PAPER
            if (ModelState.IsValid)
            {
                //initialise exercise paper
                DbSet<ExercisePaper> exercisePapers = _dbContext.ExercisePaper;
                List<ExercisePaper> papersList = exercisePapers.ToList();
                DbSet<Topics> lstOfTopics = _dbContext.Topics;
                List<Topics> topicsList = lstOfTopics.ToList();
                int totalQns = createExPaper.OETotalQns;
                int actualTotalQns = 0;
                int idCounter = 0;
                int? time = 30;
                int? difficulty = 0;
                if (createExPaper.Time != null)
                {
                    time = createExPaper.Time;
                }

                if (papersList.Count() > 0)
                {
                    idCounter = papersList[papersList.Count() - 1].Id + 1;  //Sets the exercisepaperId later on
                }
                var topicList = "";
                var itemString = "";
                var i = 1;
                List<int> lstTopic = new List<int>();
                foreach (var item in createExPaper.Topics)
                {
                    lstTopic.Add(item);
                }

                //Formats string of topics
                foreach (var item in lstTopic)
                {
                    foreach (var x in topicsList)
                    {
                        if (item == x.Id)
                        {
                            itemString = x.Name;
                        }
                    }

                    if (createExPaper.Topics.Count() == i)
                    {
                        topicList = String.Format(topicList + itemString);
                    }
                    else if (createExPaper.Topics.Count() > i)
                    {
                        topicList = String.Format(topicList + itemString + ", ");
                    }
                    i++;
                }



                #region QUESTION TEMPALTES

                //initialise random
                Random r = new Random();

                //initialise list
                List<string> selectedTopics = new List<string>();

                //loop through all topic Ids
                foreach (var item in createExPaper.Topics)
                {
                    var topicString = "";
                    foreach (var obj in topicsList)
                    {
                        if (item == obj.Id)
                        {
                            topicString = obj.Name;
                        }
                    }
                    selectedTopics.Add(topicString);
                }

                //determine number of questions per topic
                int qnPerTopic = totalQns / createExPaper.Topics.Count();
                int qnLeftOver = totalQns % createExPaper.Topics.Count(); //Takes leftover if division has remainder, can also use totalQns - qnPerTopic to get same result


                //Extra questions selected by random topic
                List<string> randomTopicSelector = new List<string>();
                for (int qnCount = 0; qnCount < qnLeftOver; qnCount++)
                {
                    var item = r.Next(1, createExPaper.Topics.Count());
                    var topicString = "";
                    foreach (var obj in topicsList)
                    {
                        if (item == obj.Id)
                        {
                            topicString = obj.Name;
                        }
                    }
                    randomTopicSelector.Add(topicString);
                }

                DbSet<OEQuestions> oeQT = _dbContext.OEQuestions;

                //For each topic, add in questions
                foreach (var item in selectedTopics)
                {
                    List<OEQuestions> oeQList = oeQT.Where(c => c.Topic == item && c.UseCount > 0).ToList();
                    List<OEQuestions> chosen = new List<OEQuestions>();
                    List<int> topicTotalQns = new List<int>();
                    int noOfQns = oeQList.Count();
                    for (int x = 0; x < noOfQns; x++)
                    {
                        topicTotalQns.Add(x + 1);
                    }

                    //For each topic, include random questions in the topic.
                    for (int qnCount = 1; qnCount <= qnPerTopic; qnCount++)
                    {
                        if (topicTotalQns.Count() > 0)
                        {
                            int rand = r.Next(0, topicTotalQns.Count() - 1);
                            chosen.Add(oeQList[rand]);
                            oeQList.Remove(oeQList[rand]);
                            topicTotalQns.Remove(topicTotalQns[rand]);
                        }

                    }

                    //For extra questions, add in more questions if topic is selected from random, based on the number of times it was chosen
                    foreach (var countOfExtraQn in randomTopicSelector)
                    {
                        if (randomTopicSelector.Contains(item))
                        {
                            if (topicTotalQns.Count() > 0)
                            {
                                int rand = r.Next(0, topicTotalQns.Count() - 1);
                                chosen.Add(oeQList[rand]);
                                topicTotalQns.Remove(topicTotalQns[rand]);
                            }
                        }
                    }

                    foreach (var obj in chosen)
                    {
                        //Add difficulty
                        difficulty += obj.Difficulty;

                        //NEED TO GROUP Figure + FigureVar, Question + QuestionVar, Answer + AnswerVar
                        #region Combine figure, question, and answer
                        var figure_ = obj.Figure;
                        //var figureVariants = obj.FigureVar;
                        //var combineFigure = figure_ + "|||" + figureVariants;

                        //var figureSplit = combineFigure.Split("|||");

                        var question_ = obj.Question;
                        //var questionVariants = obj.QuestionVar;
                        //var combineQuestion = question_ + "|||" + questionVariants;

                        //var questionSplit = combineQuestion.Split("|||");

                        var answer_ = obj.Answer;
                        //var answerVariants = obj.AnswerVar;
                        //var combineAnswer = answer_ + "|||" + answerVariants;

                        //var answerSplit = combineAnswer.Split("|||");
                        #endregion

                        //Choosing of question
                        //int variant_num = r.Next(0, figureSplit.Length - 1);               //Random using figure's list
                        //var figureFinal = figureSplit[variant_num];                        //Figure based on random
                        //var questionFinal = questionSplit[variant_num];                    //Question based on random
                        //var answerFinal = answerSplit[variant_num];                        //Answer based on random
                        var topicName = item;                                                //Topic
                        var topic = topicsList.Where(m => m.Name.Equals(topicName)).FirstOrDefault();

                        //Set questionID inside exercise paper db
                        DbSet<OEQuestionsPaper> oeQuestions = _dbContext.OEQuestionsPaper;
                        List<OEQuestionsPaper> oeQuestionsList = oeQuestions.ToList();

                        //Insert values into type of OEQuestionsPaper
                        OEQuestionsPaper oeQ = new OEQuestionsPaper();                               //Type OEQuestionsPaper to store data of the same type
                        oeQ.Id = 1;
                        if (oeQuestionsList.Count() > 0)
                        {
                            oeQ.Id = oeQuestionsList[oeQuestionsList.Count() - 1].Id + 1;
                        }
                        oeQ.Figure = figure_;
                        oeQ.Question = question_;
                        oeQ.Answer = answer_;
                        oeQ.Topic = topic.Name.ToString();

                        
                        obj.UseCount = obj.UseCount - 1;

                        //Insert into db
                        _dbContext.OEQuestions.Update(obj);
                        _dbContext.OEQuestionsPaper.Add(oeQ);
                        _dbContext.SaveChanges();

                        var questionId = 1;
                        if (oeQuestionsList.Count() > 0)
                        {
                            questionId = oeQuestionsList[oeQuestionsList.Count() - 1].Id + 1; //checks for the last item in db (most recent added)
                        }
                        //initialise exercise paper list
                        OExPaperList exercisePaper = new OExPaperList();
                        exercisePaper.ExercisePaperId = idCounter;
                        exercisePaper.OEQuestionId = questionId;
                        _dbContext.OExPaperList.Add(exercisePaper);
                        _dbContext.SaveChanges();
                        actualTotalQns++;
                    }
                }
                #endregion

                //Insert into ExercisePaper db
                ExercisePaper eP = new ExercisePaper();
                eP.Name = createExPaper.Name;
                eP.Topics = topicList;
                eP.TotalQns = actualTotalQns;
                eP.ExercisePaperId = idCounter;
                eP.Time = time;
                eP.Difficulty = difficulty;
                _dbContext.ExercisePaper.Add(eP);
                _dbContext.SaveChanges();

                TempData["Msg"] = "Exercise Paper [" + createExPaper.Name + "] added!";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
                TempData["MsgType"] = "danger";
            }


            return RedirectToAction("ViewOEPapers");
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult Preview(int id)
        {
            DbSet<OEQuestionsPaper> dbsOEQuestions = _dbContext.OEQuestionsPaper;
            List<OEQuestionsPaper> questionList = dbsOEQuestions.ToList();
            DbSet<OExPaperList> dbsOExPaperList = _dbContext.OExPaperList;
            List<OExPaperList> paperList = dbsOExPaperList.ToList();
            DbSet<ExercisePaper> dbsExercisePaper = _dbContext.ExercisePaper;
            List<ExercisePaper> exercisePaperList = dbsExercisePaper.ToList();

            List<OEQuestionsPaper> model = new List<OEQuestionsPaper>();
            List<int> questionNums = new List<int>();
            var filteredList = paperList.Where(c => c.ExercisePaperId == id).ToList();

            foreach (var item in filteredList)
            {
                questionNums.Add(item.OEQuestionId);
            }

            foreach (var item in questionNums)
            {
                foreach (var x in questionList)
                {
                    if (x.Id == item)
                    {
                        OEQuestionsPaper oeQ = new OEQuestionsPaper();
                        oeQ.Id = x.Id;
                        oeQ.Question = x.Question;
                        oeQ.Figure = x.Figure;
                        oeQ.Answer = x.Answer;
                        oeQ.Topic = x.Topic;
                        model.Add(oeQ);
                    }
                }
            }
            var filterExPaper = exercisePaperList.Where(o => o.ExercisePaperId == id).ToList();
            ViewData["exercisePaperId"] = id;
            ViewData["ExercisePaperName"] = filterExPaper[0].Name;
            ViewData["TopicsList"] = filterExPaper[0].Topics;

            return View(model);
        }

        [Authorize]
        public IActionResult GenerateExercisePaper(int id)
        {
            DbSet<OEQuestionsPaper> dbsOEQuestions = _dbContext.OEQuestionsPaper;
            List<OEQuestionsPaper> questionList = dbsOEQuestions.ToList();
            DbSet<OExPaperList> dbsOExPaperList = _dbContext.OExPaperList;
            List<OExPaperList> paperList = dbsOExPaperList.ToList();
            DbSet<ExercisePaper> dbsExercisePaper = _dbContext.ExercisePaper;
            List<ExercisePaper> exercisePaperList = dbsExercisePaper.ToList();

            //var PaperQuestionList = questionList.Join(paperList,
            //                                          q => q.Id,
            //                                          p => p.OEQuestionId,
            //                                          (q, p) => new
            //                                          {
            //                                              ExercisePaperId = p.ExercisePaperId,
            //                                              QuestionId = p.OEQuestionId,
            //                                              //TopicId = q.TopicId,
            //                                              //Question = q.Question,
            //                                              //Figure = q.Figure,
            //                                              //Answer = q.Answer
            //                                          }).ToList();


            //var filteredList = PaperQuestionList.Where(c => c.ExercisePaperId == id).ToList();

            List<OEQuestionsPaper> model = new List<OEQuestionsPaper>();
            List<int> questionNums = new List<int>();
            var filteredList = paperList.Where(c => c.ExercisePaperId == id).ToList();

            foreach (var item in filteredList)
            {
                questionNums.Add(item.OEQuestionId);
            }

            foreach (var item in questionNums)
            {
                foreach (var x in questionList)
                {
                    if (x.Id == item)
                    {
                        OEQuestionsPaper oeQ = new OEQuestionsPaper();
                        oeQ.Id = x.Id;
                        oeQ.Question = x.Question;
                        oeQ.Figure = x.Figure;
                        oeQ.Answer = x.Answer;
                        oeQ.Topic = x.Topic;
                        model.Add(oeQ);
                    }
                }
            }
            var filterExPaper = exercisePaperList.Where(o => o.ExercisePaperId == id).ToList();
            ViewData["exercisePaperId"] = id;
            ViewData["ExercisePaperName"] = filterExPaper[0].Name;
            ViewData["TopicsList"] = filterExPaper[0].Topics;
            ViewData["Timer"] = filterExPaper[0].Time;

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult GenerateExercisePaper(IFormCollection form)
        {
            List<string> answerList = new List<string>();

            //Retrieve all answers
            for (int i = 0; i < form.Count(); i++)
            {
                answerList.Add(form["answer[" + i + "]"]);
            }

            List<string> exercisePaperItem = new List<string>();
            for (int i = 0; i < 1; i++)
            {
                exercisePaperItem.Add(form["exercisePaperId"]);
            }

            int exercisePaperId = 0;
            foreach (var item in exercisePaperItem)
            {
                if (item != null)
                {
                    exercisePaperId = Int32.Parse(item);
                }
            }

            int counter = 1;
            string answerString = "";
            foreach (var item in answerList)
            {
                if (item != null)
                {
                    if (answerList.Count() == counter)
                    {
                        answerString += item.Replace(Environment.NewLine, "[nline]");
                    }
                    else if (answerList.Count() > counter)
                    {
                        answerString += item.Replace(Environment.NewLine, "[nline]") + "[/]";
                    }
                    counter++;
                }
            }

            DbSet<ExercisePaper> dbExercisePaper = _dbContext.ExercisePaper;
            List<ExercisePaper> exercisePapers = dbExercisePaper.ToList();
            DbSet<SubmitPaper> dbSubmitPapers = _dbContext.SubmitPaper;
            List<SubmitPaper> submitPapers = dbSubmitPapers.ToList();

            var submittedBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int attemptCount = 0;
            var filteredList = submitPapers.Where(c => c.ExercisePaperId == exercisePaperId && c.SubmittedBy.Contains(submittedBy)).ToList();
            foreach (var count in filteredList)
            {
                attemptCount++;
            }
            attemptCount++;
            //By this logic, all boxes combined can only store 8000 characters total,
            //There is a better way - can use foreach to loop the below to be able to store n number of data
            //rows according to the number of questions there are in the exercise paper
            //So the database would look like
            //[1][1][answer1][...]
            //[2][1][answer2][...]    - the second column represents the exercisePaperId.
            //[3][1][answer3][...]
            //But this is also a little inefficient, so anOTHER way, is to create a different table,
            //this table can be called "OEAnswers", that has Id, StudentId/AppUser's Id [need to create a primary key Id], ExercisePaperId, AttemptCount, Answers
            //This will store many records per question paper attempt for 1 person, for a particular attempt of a question
            SubmitPaper sP = new SubmitPaper();
            sP.ExercisePaperId = exercisePaperId;
            sP.Answers = answerString;
            sP.Attempts = attemptCount;
            sP.SubmittedBy = submittedBy;
            _dbContext.SubmitPaper.Add(sP);
            _dbContext.SaveChanges();

            return RedirectToAction("ViewOEPapers");
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult PaperList()
        {
            DbSet<ExercisePaper> dbs = _dbContext.ExercisePaper;
            List<ExercisePaper> model = dbs.ToList();

            return View(model);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult StudentList(int id)
        {
            DbSet<SubmitPaper> dbs = _dbContext.SubmitPaper;
            List<SubmitPaper> model = dbs.Where(c => c.ExercisePaperId == id).OrderBy(c => c.SubmittedBy).ToList();

            //MAYBE CAN ADD DATETIME STAMP FOR WHEN STUDENT SUBMITS PAPER, SO THAT LECTURER CAN SEE
            //WHICH PAPER IS OVERDUE ATTEMPT, THEN WILL IGNORE


            DbSet<AppUser> dbUsers = _dbContext.AppUser;
            List<AppUser> users = dbUsers.ToList();
            ViewData["Students"] = users;

            return View(model);
        }

        [Authorize(Roles = "Lecturer,Admin")]
        public IActionResult PrintPaper(int id)
        {
            DbSet<OEQuestionsPaper> dbsOEQuestions = _dbContext.OEQuestionsPaper;
            List<OEQuestionsPaper> questionList = dbsOEQuestions.ToList();
            DbSet<OExPaperList> dbsOExPaperList = _dbContext.OExPaperList;
            List<OExPaperList> paperList = dbsOExPaperList.ToList();
            DbSet<ExercisePaper> dbsExercisePaper = _dbContext.ExercisePaper;
            List<ExercisePaper> exercisePaperList = dbsExercisePaper.ToList();

            List<OEQuestionsPaper> model = new List<OEQuestionsPaper>();
            List<int> questionNums = new List<int>();
            var filteredList = paperList.Where(c => c.ExercisePaperId == id).ToList();

            foreach (var item in filteredList)
            {
                questionNums.Add(item.OEQuestionId);
            }

            foreach (var item in questionNums)
            {
                foreach (var x in questionList)
                {
                    if (x.Id == item)
                    {
                        OEQuestionsPaper oeQ = new OEQuestionsPaper();
                        oeQ.Id = x.Id;
                        oeQ.Question = x.Question;
                        oeQ.Figure = x.Figure;
                        oeQ.Answer = x.Answer;
                        oeQ.Topic = x.Topic;
                        model.Add(oeQ);
                    }
                }
            }
            var filterExPaper = exercisePaperList.Where(o => o.ExercisePaperId == id).ToList();



            if (model != null)
                return new ViewAsPdf(model)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
                };
            else
            {
                TempData["Msg"] = "Paper not found!";
                return RedirectToAction("Index");
            }

        }
    }



}

