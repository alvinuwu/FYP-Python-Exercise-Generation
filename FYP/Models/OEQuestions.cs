using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public partial class OEQuestions
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Question { get; set; }
        public string Figure { get; set; }
        public string Answer { get; set; }
        public int? UseCount { get; set; }
        public int? Difficulty { get; set; }
        public int? Marks { get; set; }
    }
}
