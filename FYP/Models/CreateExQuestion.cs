using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public class CreateExQuestion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Topic required")]
        public int Topics { get; set; }

        [Required(ErrorMessage = "Question String required")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Figure String required")]
        public string Figure { get; set; }

        [Required(ErrorMessage = "Answer String required")]
        public string Answer { get; set; }

        public List<string> QuestionVar { get; set; }

        public List<string> FigureVar { get; set; }

        public List<string> AnswerVar { get; set; }

    }
}
