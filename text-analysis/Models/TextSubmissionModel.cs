using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace text_analysis.Models
{
    public class TextSubmissionModel
    {
        [Required]
        [Display(Name = "Content")]
        [DataType(DataType.Text)]
        public string Content
        {
            get;
            set;
        }
    }
}
