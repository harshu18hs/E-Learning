using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObjectModel;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IArticleRepository
    {
        public ActionResult<bool> DeleteArticle(string fileName);
        public ActionResult<bool> ArticleUpload(List<IFormFile> fileName);
        public ActionResult<string> DownloadArticle(string fileName);


    }
}
