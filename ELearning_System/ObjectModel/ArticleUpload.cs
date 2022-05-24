using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjectModel
{
    public class ArticleUpload
    {
            [Key]
            public int uploadID { get; set; }
            [Required(ErrorMessage = "This field is required")]
            public string fileName { get; set; }
            [Required(ErrorMessage = "This field is required")]
            public DateTime DateOfSubmission { get; set; }
            public string FileExtension { get; set; }
            public string FileType { get; set; }
            public string FilePath { get; set; }
        }
    }
