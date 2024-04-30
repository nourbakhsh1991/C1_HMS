using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Services.Files.Interfaces
{
    public interface IFileService
    {
        string GetPath(string fileName, string root);
        string SaveBackup(string content, string root);
        string SaveFile(IFormFile file, string root);
    }
}
