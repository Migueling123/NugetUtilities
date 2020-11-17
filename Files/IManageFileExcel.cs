using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utilities.Files
{
    public interface IManageFileExcel:IDisposable
    {
        bool SaveFile(HttpPostedFileBase file, string pathLocalSave, string fileName);
        bool DeleteFile(string path);
        IList<T> GetList<T>(string path, string fileName, string sheet = "Hoja1") where T : new();


    }
}
