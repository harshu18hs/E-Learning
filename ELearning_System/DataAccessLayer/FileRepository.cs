using DataAccessLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using ObjectModel;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace DataAccessLayer
{
    public class FileRepository : IFileRepository
    {
        public static IWebHostEnvironment _webHostEnvironment;
        public ApplicationDbContext _databaseContext;
        public FileRepository(ApplicationDbContext databaseContext, IWebHostEnvironment webHostEnvironment)
        {
            _databaseContext = databaseContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public ActionResult<bool> DeleteFile(string fileName)
        {
            string FileToBeDeletedName;
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            else
            {
                string path = _webHostEnvironment.WebRootPath + "C:\\Users\\harshita.sharma\\source\\repos\\ELearning_System\\ELearning\\Files\\";
                var pngfilePath = path + fileName + ".png";
                var jpgFilePath = path + fileName + ".jpg";
                var pdfFilePath = path + fileName + ".pdf";
                var jpegFilePath = path + fileName + ".jpeg";
                var textFilePath = path + fileName + ".txt";
                var docxFilePath = path + fileName + ".docx";
                if (File.Exists(pngfilePath))
                {
                    fileName = fileName + ".png";
                    File.Delete(pngfilePath);
                }
                else if (File.Exists(jpgFilePath))
                {
                    fileName = fileName + ".jpg";
                    File.Delete(jpgFilePath);
                }
                else if (File.Exists(jpegFilePath))
                {
                    fileName = fileName + ".jpg";
                    File.Delete(jpegFilePath);
                }
                else if (File.Exists(pdfFilePath))
                {
                    fileName = fileName + ".pdf";
                    File.Delete(pdfFilePath);
                }
                else if (File.Exists(textFilePath))
                {
                    fileName = fileName + ".txt";
                    File.Delete(textFilePath);
                }
                else if (File.Exists(docxFilePath))
                {
                    fileName = fileName + ".docx";
                    File.Delete(docxFilePath);
                }
                List<FileUpload> UploadedFiles = _databaseContext.Files.ToList();
                foreach (FileUpload file in UploadedFiles)
                {
                    if ((file.fileName + ".docx") == fileName)
                    {
                        _databaseContext.Files.Remove(file);
                        _databaseContext.SaveChanges();
                        return true;
                    }
                    else if ((file.fileName + ".txt") == fileName)
                    {
                        _databaseContext.Files.Remove(file);
                        _databaseContext.SaveChanges();
                        return true;
                    }
                    else if ((file.fileName + ".pdf") == fileName)
                    {
                        _databaseContext.Files.Remove(file);
                        _databaseContext.SaveChanges();
                        return true;
                    }
                    else if ((file.fileName + ".jpg") == fileName)
                    {
                        _databaseContext.Files.Remove(file);
                        _databaseContext.SaveChanges();
                        return true;
                    }
                    else if ((file.fileName + ".png") == fileName)
                    {
                        _databaseContext.Files.Remove(file);
                        _databaseContext.SaveChanges();
                        return true;
                    }
                    else if ((file.fileName + ".jpeg") == fileName)
                    {
                        _databaseContext.Files.Remove(file);
                        _databaseContext.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
        }

        public ActionResult<string> DownloadFile(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            else
            {
                string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                var pngPath = path + fileName + ".png";
                var jpgFilePath = path + fileName + ".jpg";
                var pdfFilePath = path + fileName + ".pdf";
                var jpegFilePath = path + fileName + ".jpeg";
                var textFilePath = path + fileName + ".txt";
                if (File.Exists(pngPath))
                {
                    var stream = new FileStream(pngPath, FileMode.Open);
                    return new FileStreamResult(stream, "image/png");
                }
                else if (File.Exists(jpgFilePath))
                {
                    var stream = new FileStream(jpgFilePath, FileMode.Open);
                    return new FileStreamResult(stream, "image/jpg");
                }
                else if (File.Exists(jpegFilePath))
                {
                    var stream = new FileStream(jpegFilePath, FileMode.Open);
                    return new FileStreamResult(stream, "image/jpeg");
                }
                else if (File.Exists(pdfFilePath))
                {
                    var stream = new FileStream(pdfFilePath, FileMode.Open);
                    return new FileStreamResult(stream, "image/pdf");
                }
                else if (File.Exists(textFilePath))
                {
                    var stream = new FileStream(textFilePath, FileMode.Open);
                    return new FileStreamResult(stream, "image/txt");
                }

                return null;
            }
        }
         public ActionResult<bool> FileUpload(List<IFormFile> fileName)
         {
             foreach (var file in fileName)
             {
                 var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                 bool basePathExists = System.IO.Directory.Exists(basePath);
                 if (!basePathExists) Directory.CreateDirectory(basePath);
                 var Filename = Path.GetFileNameWithoutExtension(file.FileName);
                 var filePath = Path.Combine(basePath, file.FileName);
                 var extension = Path.GetExtension(file.FileName);
                 if (!File.Exists(filePath))
                 {
                     using (var stream = new FileStream(filePath, FileMode.Create))
                     {
                         file.CopyTo(stream);
                     }
                     var FileUpload = new FileUpload
                     {
                         DateOfSubmission = DateTime.UtcNow,
                         FileType = file.ContentType,
                         FileExtension = extension,
                         fileName = Filename, 
                         FilePath = filePath
                     };
                     _databaseContext.Files.Add(FileUpload);
                     _databaseContext.SaveChanges();
                     return true;
                 }
            }
            return false;
        }
    }
}

