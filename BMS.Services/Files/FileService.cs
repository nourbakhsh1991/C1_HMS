using Microsoft.AspNetCore.Http;
using PersianDate.Standard;
using BMS.Services.Files.Interfaces;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Shared.Extentions;

namespace BMS.Services.Files
{
    public class FileService : IFileService
    {

        public string GetPath(string fileName, string root)
        {
            try
            {
                var str = root + "\\Files";
                if (!System.IO.Directory.Exists(str))
                    System.IO.Directory.CreateDirectory(str);
                str += "\\" + DateTime.UtcNow.ToFa("yyyy_MM_dd");
                if (!System.IO.Directory.Exists(str))
                    System.IO.Directory.CreateDirectory(str);
                return str + "\\" + fileName;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public string SaveFile(IFormFile file, string root)
        {
            try
            {

                var extension = System.IO.Path.GetExtension(file.FileName);

                var time = DateTime.UtcNow.ToFa("yyyy_MM_dd_hh_mm_ss_" + DateTime.Now.Millisecond);
                var path = "";
                var counter = 0;
                while (true)
                {
                    path = GetPath(time + "_" + counter, root);
                    if (System.IO.File.Exists(root + path + extension))
                    {
                        counter++;
                        continue;
                    }
                    break;
                }
                Image thumb = null;
                if (file.IsImage())
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        using (var img = Image.FromStream(memoryStream))
                        {
                            var h = img.Height;
                            var w = img.Width;
                            var max = h >= w ? h : w;
                            var aspectH = (h * 1.0f) / (max);
                            var aspectW = (w * 1.0f) / (max);
                            thumb = img.ResizeImage((int)(aspectW * 300), (int)(aspectH * 300));

                        }
                    }

                using (var stream = new FileStream(path + extension, FileMode.Create))
                {
                    if (thumb != null)
                    { thumb.Save(path + "_thumb" + extension); }
                    else
                    {
                        if (file.IsImage())
                            using (var streamthumb = new FileStream(path + "_thumb" + extension, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                    }
                    file.CopyTo(stream);
                }
                return (path + extension).Substring(root.Length);
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public string SaveBackup(string content, string root)
        {
            try
            {
                var extension = ".json";
                var time = DateTime.UtcNow.ToFa("yyyy_MM_dd_hh_mm_ss_" + DateTime.Now.Millisecond);
                var path = "";
                var counter = 0;
                while (true)
                {
                    path = GetPath(time + "_" + counter, root);
                    if (System.IO.File.Exists(root + path + extension))
                    {
                        counter++;
                        continue;
                    }
                    break;
                }
                File.WriteAllText(path + extension, content);

                return (path + extension).Substring(root.Length);
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
