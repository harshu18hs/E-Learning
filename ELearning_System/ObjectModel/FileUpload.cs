using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ObjectModel
{
    public class FileUpload
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

