using Microsoft.AspNetCore.Mvc;
using ObjectModel;
using System;

namespace DataAccessLayer
{
    public interface IAdminRepository
    {
        ActionResult<bool> UpdateUsers(String userName, Details User);
    }
}
