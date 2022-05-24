using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectModel;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IFileRepository
    {
        public ActionResult<bool> DeleteFile(string fileName);
        public ActionResult<bool> FileUpload(List<IFormFile> fileName);
        public ActionResult<string> DownloadFile(string fileName);

    }
}
