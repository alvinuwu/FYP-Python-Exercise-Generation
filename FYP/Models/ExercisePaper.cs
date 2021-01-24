using System;
using System.Collections.Generic;

namespace FYP.Models
{
    public partial class ExercisePaper
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Topics { get; set; }

        public int TotalQns { get; set; }
        public int ExercisePaperId { get; set; }
    }
}
