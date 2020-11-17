using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.BulkDataSave
{
    public interface IUploadToSql<T>
    {
        IList<T> InternalStore { get; set; }
        string TableName { get; set; }
        int CommitBatchSize { get; set; }
        string ConnectionString { get; set; }
        long BulkInsert(DataTable dt);
        long Commit();
        string GetConnectionString(string modelName);
    }
}
