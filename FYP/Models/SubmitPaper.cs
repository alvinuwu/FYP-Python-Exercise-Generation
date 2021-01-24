using System;
using System.Collections.Generic;

namespace FYP.Models
{
    public partial class SubmitPaper
    {
        public int Id { get; set; }

        public int ExercisePaperId { get; set; }

        public string Answers { get; set; }

        public int Attempts { get; set; }

        public string SubmittedBy { get; set; }
    }
}
