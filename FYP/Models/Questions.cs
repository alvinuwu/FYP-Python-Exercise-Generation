using System;
using System.Collections.Generic;

namespace FYP.Models
{
    public partial class Questions
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string Question { get; set; }
        public string Figure { get; set; }
        public string Answer { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
    }
}
