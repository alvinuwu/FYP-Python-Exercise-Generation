using System;
using System.Collections.Generic;

namespace FYP.Models
{
    public partial class OEQuestionsPaper
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Question { get; set; }
        public string Figure { get; set; }
        public string Answer { get; set; }
        public int? UseCount { get; set; }
    }
}
