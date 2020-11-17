using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Utilities.AddPropertiesTo
{
    public static class UploadToSqlHelper
    {
        /// <summary>
        /// Converting any IEnumerable object to DataTable Type
        /// </summary>
        /// <typeparam name="T">IEnumerable object</typeparam>
        /// <param name="data">data with  DataTable format</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// It takes a specific number of registers from a list in aleatory way  
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <param name="tList"></param>
        /// <param name="count">Number of registers that you wish getting</param>
        /// <returns></returns>
        public static TList TakeRandom<TList>( this TList tList, int count)
            where TList: IList, new()
        {
            var ran = new Random();
            var rList = new TList();
            while (count > 0 && tList.Count > 0)
            {
                var n = ran.Next(0, tList.Count);
                var e = tList[n];
                rList.Add(e);
                tList.RemoveAt(n);
                count--;
            }
            return rList;
        }
        /// <summary>
        /// It Converts a POCO Class to dictionary <string,object>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ClassToDictionary<T>(this T objeto )
        {
           return objeto.GetType()
                       .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .ToDictionary(prop => prop.Name, prop => prop.GetValue(objeto, null));
        }
    }
}
