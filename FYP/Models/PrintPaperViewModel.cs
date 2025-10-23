using System.Collections.Generic;

namespace FYP.Models
{
    public class PrintPaperViewModel
    {
        public string PaperName { get; set; }
        public List<OEQuestionsPaper> Questions { get; set; }
    }
}
