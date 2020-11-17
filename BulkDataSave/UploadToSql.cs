using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Utilities.AddPropertiesTo;

namespace Utilities.BulkDataSave
{
    public class UploadToSql<T>: IUploadToSql<T>
    {
        public IList<T> InternalStore { get; set; }
        public string TableName { get; set; }
        public int CommitBatchSize { get; set; } = 1000;
        public string ConnectionString { get; set; }

        /// <summary>
        /// load list to the data base
        /// </summary>
        /// <returns>number of register rows</returns>
        public long Commit()
        {
            long rowAffected=0;
            if (InternalStore.Count > 0)
            {
                DataTable dt;
                int numberOfPages = (InternalStore.Count / CommitBatchSize) + (InternalStore.Count % CommitBatchSize == 0 ? 0 : 1);
                for (int pageIndex = 0; pageIndex < numberOfPages; pageIndex++)
                {
                    dt = InternalStore.Skip(pageIndex * CommitBatchSize).Take(CommitBatchSize).ToDataTable();
                    rowAffected += BulkInsert(dt);
                }
            }
            return rowAffected;
        }

        /// <summary>
        /// Inserting bulk data
        /// </summary>
        /// <param name="dt">type datatable with the information of database</param>
        /// <returns>number of register row </returns>
        public long BulkInsert(DataTable dt)
        {
            long rowAffected=0L;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                // make sure to enable triggers
                // more on triggers in next post
                var bulkCopy =
                new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers |
                                SqlBulkCopyOptions.UseInternalTransaction, null)
                {
                    // set the destination table name
                    DestinationTableName = TableName,
                    NotifyAfter = dt.Rows.Count
                };

                connection.Open();
                // write the data in the "dataTable"
                bulkCopy.SqlRowsCopied += (s, e) => rowAffected = e.RowsCopied;
                bulkCopy.WriteToServer(dt);                
                connection.Close();
            }
            return rowAffected;
        }

        /// <summary>
        /// Get connection string from a Model string connection
        /// </summary>
        /// <param name="modelName"> Model string connection</param>
        /// <returns>strign connection</returns>
        public string GetConnectionString(string modelName)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings[modelName].ConnectionString;
                string[] ArrayConexion = constr.Split(';');
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                foreach (var item in ArrayConexion)
                {
                    if (item.StartsWith("initial catalog"))
                    {
                        builder.InitialCatalog = item.Substring(item.IndexOf("=") + 1);
                        builder.ApplicationName = item.Substring(item.IndexOf("=") + 1);
                    }
                    if (item.Contains("data source"))
                    {
                        builder.DataSource = item.Substring(item.IndexOf("data source=") + 12);
                    }
                    if (item.StartsWith("user id"))
                    {
                        builder.UserID = item.Substring(item.IndexOf("=") + 1);
                    }
                    if (item.StartsWith("password"))
                    {
                        builder.Password = item.Substring(item.IndexOf("=") + 1);
                    }
                }
                return builder.ToString();
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message, ex);
                return ex.Message;
            }
        }
    }
}
