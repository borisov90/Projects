namespace IXPConnector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FirebirdSql.Data.FirebirdClient;
    using FirebirdSql.VisualStudio.DataTools;
    using FirebirdSql.Data.EntityFramework6;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity.Core;

    public static class IPXConnector
    {
        public static void Connect()
        {

            #region constant literals
            // Set the ServerType to 1 for connect to the embedded server

            string connectionString =
            "User=sysdba;" +
            "Password=masterkey;" +
            "Database=D:\\Work\\IXP220\\IXPConnector\\IXPConnector\\FirebirdDB\\DB220.FDB;" +
            "DataSource=localhost;" +
            "Port=3050;" +
            "Dialect=3;" +
            "Charset=NONE;" +
            "Role=;" +
            "Connection lifetime=15;" +
            "Pooling=true;" +
            "MinPoolSize=0;" +
            "MaxPoolSize=50;" +
            "Packet Size=8192;" +
            "ServerType=0";

            //D:\Work\IXP220\IXPConnector\IXPConnector\FirebirdDB\DB220.FDB
            #endregion

            try
            {
                FbConnection con = new FbConnection();
                con.ConnectionString = connectionString;
                con.Open();
                Console.WriteLine("You are now connected! \r\n");

                //var commandString = "SELECT * FROM MASTER WHERE MST_SQ like '1'";
                var commandString = "SELECT MASTER.MST_LAST_NAME, TRANSACK.TR_DATE, TRANSACK.TR_TIME, EVENT_TYPE.ET_DESC"
                                    + " FROM TRANSACK"
                                    + " INNER JOIN MASTER"
                                    + " ON TRANSACK.TR_MSTSQ=MASTER.MST_SQ"
                                    + " INNER JOIN EVENT_TYPE"
                                    + " ON TRANSACK.TR_EVENT=EVENT_TYPE.ET_TYPENO"
                                    + " WHERE MASTER.MST_SQ like '1'";


                /* var commandString = "SELECT MASTER.MST_LAST_NAME, TRANSACK.TR_DATE, TRANSACK.TR_TIME, EVENT_TYPE.ET_DESC"
                                    + " FROM TRANSACK"
                                    + " INNER JOIN MASTER"
                                    + " ON TRANSACK.TR_MSTSQ=MASTER.MST_SQ"
                                    + " INNER JOIN EVENT_TYPE"
                                    + " ON TRANSACK.TR_EVENT=EVENT_TYPE.ET_TYPENO"
                                    + " WHERE MASTER.MST_SQ like '21'";*/
                                   

                //gets the names of the users - SELECT MST_FIRST_NAME, MST_LAST_NAME, MST_SQ FROM MASTER

                
                //BIO_TAG
                //REPORT_TRANSACK
                //COLUMN_DETAIL
                //CONFIGURATION
                //DEPARTMENT S_ID, DPT_NO, DPT_NAME, 
                //EVENT_TYPE
                //REASON - RN_NO (1,2,11) / RN_DESC (Работна среща, Лична Работа, Изход)
                //STATUS_TRANSACK
                //TAG
                //TAG_T_A_G
                //TRANSACK
                //MASTER - holds the names of the users, MST_EMAIL, DPT_NO, MST_FIRST_NAME, MST_LAST_NAME, MST_SQ
                //MASTER_SITE - S_ID, MST_SQ_CTRL, MST_SQ

                var transaction = con.BeginTransaction(IsolationLevel.Serializable);
                FbCommand command = new FbCommand(commandString, con, transaction);
                command.ExecuteNonQuery();
                IDataReader reader = command.ExecuteReader();
                Console.WriteLine(commandString + " returns");
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (true)
                        {

                        }
                        Console.Write(reader.GetValue(i) + "\t");
                    }

                    Console.WriteLine();
                }

                transaction.Commit();
                command.Dispose(); // Thus!
                transaction.Dispose();


                // Get the available metadata Collection names
                DataTable metadataCollections = con.GetSchema();

                //foreach (DataColumn row in metadataCollections.Columns)
                //{
                //    foreach (object item in row.ColumnName)
                //    {
                //        if (item is int)
                //        {
                //            Console.Write("Int: {0} |", item);
                //        }
                //        else if (item is string)
                //        {
                //            Console.Write("String: {0} |", item);
                //        }
                //        else if (item is DateTime)
                //        {
                //            Console.Write("DateTime: {0} |", item);
                //        }
                //        Console.WriteLine();
                //    }
                //    Console.WriteLine(row.Table.Columns.Count);
                //}


                // Get datatype information
                DataTable dataTypes = con.GetSchema(DbMetaDataCollectionNames.DataTypes);



                // Get DataSource Information
                DataTable dataSourceInformation = con.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);


                // Get available reserved word
                DataTable reservedWords = con.GetSchema(DbMetaDataCollectionNames.ReservedWords);

                // Get the list of User Tables
                // Restrictions:
                // TABLE_CATALOG
                // TABLE_SCHEMA
                // TABLE_NAME
                // TABLE_TYPE
                DataTable userTables = con.GetSchema("Tables", new string[] { null, null, null, "TABLE" });

               

                
                // Get the list of System Tables
                // Restrictions:
                // TABLE_CATALOG
                // TABLE_SCHEMA
                // TABLE_NAME
                // TABLE_TYPE
                DataTable systemTables = con.GetSchema("Tables", new string[] { null, null, null, "SYSTEM TABLE" });
                

                // Get Table Columns
                // Restrictions:
                // TABLE_CATALOG
                // TABLE_SCHEMA
                // TABLE_NAME
                // COLUMN_NAME
                DataTable tableColumns = con.GetSchema("Columns", new string[] { null, null, "TableName" });

               

                con.Close();
                Console.WriteLine("The connection was closed.");
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't open the connection. Try Again!" + " This is the connection string: " + connectionString);
                return;
            }
        }
        public static object Column(this DataRow source, string columnName)
        {
            var c = source.Table.Columns[columnName];
            if (c != null)
            {
                return source.ItemArray[c.Ordinal];
            }

            throw new ObjectNotFoundException(string.Format("The column '{0}' was not found in this table", columnName));
        }
    }
}
