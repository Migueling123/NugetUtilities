using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Utilities.Files
{
    public class ManageFileExcel:IManageFileExcel
    {
        private bool _disposed=false;
        public bool SaveFile(HttpPostedFileBase file, string pathLocalSave, string fileName)
        {
            try
            {
                var fullpath = Path.Combine(pathLocalSave, fileName);
                var data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                using (var sw = new FileStream(fullpath, FileMode.Create))
                {
                    sw.Write(data, 0, data.Length);
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public bool DeleteFile(string path)
        {
            try
            {                
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Delete();
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public IList<T> GetList<T>(string path, string fileName,string sheet="Hoja1")where T:new()
        {
            var records = new List<T>();
            using (var stream = File.Open(Path.Combine(path, fileName), FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                {
                    var result=reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true                            
                        }
                    });

                    foreach(DataRow row in result.Tables[sheet].Rows)
                    {
                        var registro = new T();
                        var properties = TypeDescriptor.GetProperties(registro);
                        foreach (var prop in properties.Cast<PropertyDescriptor>().Where(prop => prop.Name != "ExtensionData"))
                        {
                            if (row[prop.Name].ToString() != "")
                                prop.SetValue(registro, Convert.ChangeType(row[prop.Name], prop.PropertyType));                            
                        }
                        records.Add(registro);                        
                    }
                }
            }
            return records;
        }

        #region Liberar recursos no administrados
        public void Dispose() => Dispose(true);
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                
                // Dispose managed state (managed objects).
                //upload.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}