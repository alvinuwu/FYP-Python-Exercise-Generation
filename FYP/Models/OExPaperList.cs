using System;
using System.Collections.Generic;

namespace FYP.Models
{
    public partial class OExPaperList
    {
        public int Id { get; set; }
        public int ExercisePaperId { get; set; }

        public int OEQuestionId { get; set; }
    }
}
