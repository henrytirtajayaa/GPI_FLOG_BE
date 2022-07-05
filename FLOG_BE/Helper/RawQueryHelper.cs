using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using FLOG_BE.Model.Companies.Entities;
using FLOG_BE.Features.Companies.AccountSegment.PostAccountSegment;
using FLOG.Core;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Reflection;

namespace FLOG_BE.Helper
{
    public static class RawQuery
    {
        //DIRECT QUERY ASYNCHRONOUS
        public static async Task<DataTable> SelectRawSqlAsync(DatabaseFacade db, string rawQuery, DbTransaction trans, CommandType commandType = CommandType.Text)
        {
            DataTable dt = new DataTable();

            try
            {
                using (var command = db.GetDbConnection().CreateCommand())
                {
                    command.CommandText = rawQuery;
                    command.CommandType = commandType;

                    if (trans != null)
                    {
                        command.Transaction = trans;
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    }
                }
            }catch(Exception ex)
            {
                throw new System.ArgumentException("[SelectRawSql] " + ex.Message, "Error");
            }

            return dt;
            
        }

        //DIRECT QUERY
        public static DataTable SelectRawSql(DatabaseFacade db, string rawQuery, DbTransaction trans, CommandType commandType = CommandType.Text)
        {
            DataTable dt = new DataTable();

            try
            {
                

                using (var command = db.GetDbConnection().CreateCommand())
                {
                    command.CommandText = rawQuery;
                    command.CommandType = commandType;

                    if(trans != null)
                    {
                        command.Transaction = trans;
                    }

                    db.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("[SelectRawSql] " + ex.Message, "Error");
            }
            finally
            {
                db.CloseConnection();
            }

            return dt;

        }

        public static List<T> Select<T>(DatabaseFacade db, string rawQuery, DbTransaction trans, CommandType commandType = CommandType.Text) where T : new()
        {
            DataTable dt = new DataTable();

            try
            {
                using (var command = db.GetDbConnection().CreateCommand())
                {
                    command.CommandText = rawQuery;
                    command.CommandType = commandType;

                    if (trans != null)
                    {
                        command.Transaction = trans;
                    }

                    db.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[SelectRawSql] ********** ERROR ********** " +ex.Message );
                Console.WriteLine("[SelectRawSql] ********** TRACE ********** " + ex.StackTrace);

                dt.Clear();
            }
            finally
            {
                db.CloseConnection();
            }

            return dt.ConvertToList<T>();

        }

        public static T ConvertToEntity<T>(this DataRow tableRow) where T : new()
        {
            // Create a new type of the entity I want
            Type t = typeof(T);
            T returnObject = new T();

            foreach (DataColumn col in tableRow.Table.Columns)
            {
                string colName = col.ColumnName;

                // Look for the object's property with the columns name, ignore case
                PropertyInfo pInfo = t.GetProperty(colName.ToLower(),
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // did we find the property ?
                if (pInfo != null)
                {
                    object val = tableRow[colName];

                    // is this a Nullable<> type
                    bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                    if (IsNullable)
                    {
                        if (val is System.DBNull)
                        {
                            val = null;
                        }
                        else
                        {
                            // Convert the db type into the T we have in our Nullable<T> type
                            val = Convert.ChangeType
                    (val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                        }
                    }
                    else
                    {
                        // Convert the db type into the type of the property in our entity
                        val = Convert.ChangeType(val, pInfo.PropertyType);
                    }
                    // Set the value of the property with the value from the db
                    pInfo.SetValue(returnObject, val, null);
                }
            }

            // return the entity object with values
            return returnObject;
        }

        public static List<T> ConvertToList<T>(this DataTable table) where T : new()
        {
            Type t = typeof(T);

            // Create a list of the entities we want to return
            List<T> returnObject = new List<T>();

            // Iterate through the DataTable's rows
            foreach (DataRow dr in table.Rows)
            {
                // Convert each row into an entity object and add to the list
                T newRow = dr.ConvertToEntity<T>();
                returnObject.Add(newRow);
            }

            // Return the finished list
            return returnObject;
        }

    }

}
