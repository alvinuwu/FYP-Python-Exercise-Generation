using System;
using System.Collections.Generic;

namespace FYP.Models
{
    public partial class OEQuestion_Templates
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string Question { get; set; }
        public string Figure { get; set; }
        public string Answer { get; set; }

        public string QuestionVar { get; set; }
        public string FigureVar { get; set; }
        public string AnswerVar { get; set; }

    }
}
