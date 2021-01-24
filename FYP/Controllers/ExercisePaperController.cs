﻿using System;
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

namespace FYP.Controllers
{
    public class ExercisePaperController : Controller
    {
        private AppDbContext _dbContext;
        public ExercisePaperController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
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

        [Authorize]
        public IActionResult AutoGenerateQn()
        {
            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.ToList();
            ViewData["topics"] = new SelectList(model, "Id", "Name");

            return View();
        }

        [Authorize]
        public IActionResult AutoGenerateQnPt2(int id, int topicId)
        {
            DbSet<OEQuestion_Templates> oeQT = _dbContext.OEQuestion_Templates;
            var model = oeQT.Where(c => c.Id == id).FirstOrDefault();
            TempData["Topic"] = topicId;


            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AutoGenerateQnPt2(IFormCollection form)
        {
            var topicId = Int32.Parse(TempData["Topic"].ToString());
            DbSet<Topics> dbTopic = _dbContext.Topics;
            var topic = dbTopic.Where(c => c.Id == topicId).FirstOrDefault().ToString();

            var figure = form["figure"];
            var question = form["question"];
            var answer = form["answer"];
            var checkCount = form["useCount"];
            var useCount = 0;
            if (!checkCount.Equals(""))
            {
                useCount = Int32.Parse(form["useCount"]);
            }

            OEQuestions oeQuestions = new OEQuestions();
            oeQuestions.Figure = figure;
            oeQuestions.Question = question;
            oeQuestions.Answer = answer;
            oeQuestions.Topic = topic;
            if (useCount != 0)
            {
                oeQuestions.UseCount = useCount;
            }
            else
            {
                oeQuestions.UseCount = null;
            }

            _dbContext.OEQuestions.Add(oeQuestions);
            _dbContext.SaveChanges();

            TempData["Msg"] = "New Open-Ended Question added!";
            TempData["MsgType"] = "success";

            return RedirectToAction("ViewOEQuestions");
        }

        public IActionResult RandomiseOEF(int id, int rand)
        {
            DbSet<OEQuestion_Templates> dbs = _dbContext.OEQuestion_Templates;
            var oeList = dbs.Where(l => l.Id == id).FirstOrDefault();

            Random r = new Random();
            var figureSplit = oeList.FigureVar.Split(", ");

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
            var questionSplit = oeList.QuestionVar.Split(", ");

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
            var answerSplit = oeList.AnswerVar.Split(", ");

            var answer = answerSplit[rand];


            if (answer == "")
            {
                answer = oeList.Answer;

            }


            return PartialView("_DisplayA", answer);
        }

        [Authorize]
        public IActionResult ViewOEQuestions()
        {
            DbSet<OEQuestions> oeQ = _dbContext.OEQuestions;
            List<OEQuestions> oeList = oeQ.ToList();

            return View(oeList);
        }

        [Authorize]
        public IActionResult GenerateOE()
        {
            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.ToList();
            ViewData["topics"] = new SelectList(model, "Id", "Name");

            return View();
        }

        [Authorize]
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
                        var item_ = item.Replace(",", "[comma]");
                        if (questionNullRemove.Count() == questionCount)
                        {
                            questionVar += item_.Replace(Environment.NewLine, "[nline]");
                        }
                        else if (questionNullRemove.Count() > questionCount)
                        {
                            questionVar += item_.Replace(Environment.NewLine, "[nline]") + ", ";
                        }
                        questionCount++;
                    }

                }

                int figureCount = 1;
                string figureVar = "";
                var figureNullRemove = new List<string>();
                foreach (var item in createExQuestion.QuestionVar)
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
                        var item_ = item.Replace(",", "[comma]");
                        if (figureNullRemove.Count() == figureCount)
                        {
                            figureVar += item_.Replace(Environment.NewLine, "[nline]");
                        }
                        else if (figureNullRemove.Count() > figureCount)
                        {
                            figureVar += item_.Replace(Environment.NewLine, "[nline]") + ", ";
                        }
                        figureCount++;
                    }
                }

                int answerCount = 1;
                string answerVar = "";
                var answerNullRemove = new List<string>();
                foreach (var item in createExQuestion.QuestionVar)
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
                        var item_ = item.Replace(",", "[comma]");
                        if (answerNullRemove.Count() == answerCount)
                        {
                            answerVar += item_.Replace(Environment.NewLine, "[nline]");
                        }
                        else if (answerNullRemove.Count() > answerCount)
                        {
                            answerVar += item_.Replace(Environment.NewLine, "[nline]") + ", ";
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



                TempData["Msg"] = "New Question added!";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
                TempData["MsgType"] = "danger";
            }


            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult GenerateOE2()
        {
            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.ToList();
            ViewData["topics"] = new SelectList(model, "Id", "Name");

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult GenerateOE2(CreateExQuestion createExQuestion)
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
                string questionVar = "";
                foreach (var item in createExQuestion.QuestionVar)
                {
                    if (item != null)
                    {
                        var item_ = item.Replace(",", "[comma]");
                        if (createExQuestion.QuestionVar.Count() == questionCount)
                        {
                            questionVar += item_.Replace(Environment.NewLine, "[nline]");
                        }
                        else if (createExQuestion.QuestionVar.Count() > questionCount)
                        {
                            questionVar += item_.Replace(Environment.NewLine, "[nline]") + ", ";
                        }
                        questionCount++;
                    }

                }

                int figureCount = 1;
                string figureVar = "";
                foreach (var item in createExQuestion.FigureVar)
                {
                    if (item != null)
                    {
                        var item_ = item.Replace(",", "[comma]");
                        if (createExQuestion.FigureVar.Count() == figureCount)
                        {
                            figureVar += item_.Replace(Environment.NewLine, "[nline]");
                        }
                        else if (createExQuestion.FigureVar.Count() > figureCount)
                        {
                            figureVar += item_.Replace(Environment.NewLine, "[nline]") + ", ";
                        }
                        figureCount++;
                    }
                }

                int answerCount = 1;
                string answerVar = "";
                foreach (var item in createExQuestion.AnswerVar)
                {
                    if (item != null)
                    {
                        var item_ = item.Replace(",", "[comma]");
                        if (createExQuestion.AnswerVar.Count() == answerCount)
                        {
                            answerVar += item_.Replace(Environment.NewLine, "[nline]");
                        }
                        else if (createExQuestion.AnswerVar.Count() > answerCount)
                        {
                            answerVar += item_.Replace(Environment.NewLine, "[nline]") + ", ";
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



                TempData["Msg"] = "New Question added!";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
                TempData["MsgType"] = "danger";
            }


            return RedirectToAction("Index");
        }

        public IActionResult OE2(string word)
        {
            List<string> sList = new List<string>();
            
            //refer to the notepad reference for the example of how this shud be


            //for (var i = 0; i < maxNum; i++) {
                
            //    var pHolder = "{" + i + "}";
            //    sList.Add(pHolder);
                    
            //    $("#divFigure").html("OE2?word=" + i);
                
            //}

            
            //for (var i = 0; i < maxNum; i++) {
            //    if (words.includes("{" + i + "}")) {
            //        $("#divFigure").load("OE2?word=" + i);
            //    }
            //}

            return PartialView("_OE2", word);
        }

        [Authorize]
        public IActionResult Create()
        {
            DbSet<Topics> topics = _dbContext.Topics;
            List<Topics> model = topics.ToList();
            ViewData["topics"] = new SelectList(model, "Id", "Name");

            return View();
        }

        [Authorize]
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
                int idCounter = papersList.Count() + 1;  //Sets the exercisepaperId later on
                var topicList = "";
                var itemString = "";
                var i = 1;
                List<int> lstTopic = new List<int>();
                foreach (var item in createExPaper.Topics)
                {
                    lstTopic.Add(item);
                }
                
                //Formats string of topics
                foreach(var item in lstTopic)
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
                List<int> selectedTopics = new List<int>();

                //loop through all topic Ids
                foreach (var item in createExPaper.Topics)
                {
                    selectedTopics.Add(item);
                }

                //determine number of questions per topic
                int qnPerTopic = totalQns / createExPaper.Topics.Count();
                int qnLeftOver = totalQns % createExPaper.Topics.Count(); //Takes leftover if division has remainder, can also use totalQns - qnPerTopic to get same result
                
                
                //Extra questions selected by random topic
                List<int> randomTopicSelector = new List<int>();
                for (int qnCount = 0; qnCount < qnLeftOver; qnCount++)
                {
                    randomTopicSelector.Add(r.Next(1, createExPaper.Topics.Count()));
                }
                                
                DbSet<OEQuestion_Templates> oeQT = _dbContext.OEQuestion_Templates;

                //For each topic, add in questions
                foreach (var item in selectedTopics)
                {
                    List<OEQuestion_Templates> oeQTList = oeQT.Where(c => c.TopicId == item).ToList();
                    List<OEQuestion_Templates> chosen = new List<OEQuestion_Templates>();
                    List<int> topicTotalQns = new List<int>();
                    int noOfQns = oeQTList.Count();
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
                            chosen.Add(oeQTList[rand]);
                            oeQTList.Remove(oeQTList[rand]);
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
                                chosen.Add(oeQTList[rand]);
                                topicTotalQns.Remove(topicTotalQns[rand]);
                            }
                        }
                    }

                    foreach (var obj in chosen)
                    {
                        //NEED TO GROUP Figure + FigureVar, Question + QuestionVar, Answer + AnswerVar
                        #region Combine figure, question, and answer
                        var figure_ = obj.Figure;
                        var figureVariants = obj.FigureVar;
                        var combineFigure = figure_ + "|||" + figureVariants;

                        var figureSplit = combineFigure.Split("|||");

                        var question_ = obj.Question;
                        var questionVariants = obj.QuestionVar;
                        var combineQuestion = question_ + "|||" + questionVariants;

                        var questionSplit = combineQuestion.Split("|||");

                        var answer_ = obj.Answer;
                        var answerVariants = obj.AnswerVar;
                        var combineAnswer = answer_ + "|||" + answerVariants;

                        var answerSplit = combineAnswer.Split("|||");
                        #endregion

                        //Choosing of question
                        int variant_num = r.Next(0, figureSplit.Length - 1);               //Random using figure's list
                        var figureFinal = figureSplit[variant_num];                        //Figure based on random
                        var questionFinal = questionSplit[variant_num];                    //Question based on random
                        var answerFinal = answerSplit[variant_num];                        //Answer based on random
                        int topicId = item;                                                //Topic
                        DbSet<Topics> dbTopic = _dbContext.Topics;
                        var topic = dbTopic.Where(c => c.Id == topicId).FirstOrDefault().ToString();

                        //Insert values into type of OEQuestions
                        OEQuestions oeQ = new OEQuestions();                               //Type OEQuestions to store data of the same type
                        oeQ.Figure = figureFinal;
                        oeQ.Question = questionFinal;
                        oeQ.Answer = answerFinal;
                        oeQ.Topic = topic;

                        //Insert into db
                        _dbContext.OEQuestions.Add(oeQ);
                        _dbContext.SaveChanges();

                        //Set questionID inside exercise paper db
                        DbSet<OEQuestions> oeQuestions = _dbContext.OEQuestions;
                        List<OEQuestions> oeQuestionsList = oeQuestions.ToList();
                        var questionId = 0;
                        foreach (var item_ in oeQuestionsList)
                        {
                            questionId = item_.Id; //Cannot use .Count because some questions may be deleted
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


            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult GenerateExercisePaper(int id)
        {
            DbSet<OEQuestions> dbsOEQuestions = _dbContext.OEQuestions;
            List<OEQuestions> questionList = dbsOEQuestions.ToList();
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

            List<OEQuestions> model = new List<OEQuestions>();
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
                        OEQuestions oeQ = new OEQuestions();
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
                        answerString += item;
                    }
                    else if (answerList.Count() > counter)
                    {
                        answerString += item + ", ";
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

            return RedirectToAction("Index");
        }

        public IActionResult PaperList()
        {
            DbSet<ExercisePaper> dbs = _dbContext.ExercisePaper;
            List<ExercisePaper> model = dbs.ToList();

            return View(model);
        }

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



    }
}
