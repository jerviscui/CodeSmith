namespace SchemaExplorer
{
    using CodeSmith.Core.Collections;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;
    using System.Text.RegularExpressions;

    public class MySQLSchemaProvider : IDbSchemaProvider, INamedObject
    {
        private DataSet ConvertDataReaderToDataSet(IDataReader reader)
        {
            DataSet set = new DataSet();
            set.Tables.Add(this.ConvertDataReaderToDataTable(reader));
            return set;
        }

        private DataTable ConvertDataReaderToDataTable(IDataReader reader)
        {
            DataTable schemaTable = reader.GetSchemaTable();
            DataTable table2 = new DataTable();
            foreach (DataRow row in schemaTable.Rows)
            {
                string columnName = (string) row["ColumnName"];
                DataColumn column = new DataColumn(columnName, (Type) row["DataType"]);
                table2.Columns.Add(column);
            }
            while (reader.Read())
            {
                DataRow row2 = table2.NewRow();
                for (int i = 0; i <= (reader.FieldCount - 1); i++)
                {
                    row2[i] = reader.GetValue(i);
                }
                table2.Rows.Add(row2);
            }
            return table2;
        }

        private static DbConnection CreateConnection(string connectionString)
        {
            DbConnection connection = CreateDbProviderFactory().CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        private static DbProviderFactory CreateDbProviderFactory()
        {
            return DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
        }

        public ParameterSchema[] GetCommandParameters(string connectionString, CommandSchema command)
        {
            throw new NotSupportedException("GetCommandParameters() is not supported in this release.");
        }

        public CommandResultSchema[] GetCommandResultSchemas(string connectionString, CommandSchema command)
        {
            throw new NotSupportedException("GetCommandResultSchemas() is not supported in this release.");
        }

        public CommandSchema[] GetCommands(string connectionString, DatabaseSchema database)
        {
            string str = string.Format("SELECT ROUTINE_NAME, '' OWNER, CREATED FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = '{0}' AND ROUTINE_TYPE = 'PROCEDURE' ORDER BY 1", database.Name);
            List<CommandSchema> list = new List<CommandSchema>();
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = str;
                command.Connection = connection;
                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        list.Add(new CommandSchema(database, reader.GetString(0), reader.GetString(1), reader.GetDateTime(2)));
                    }
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return list.ToArray();
        }

        public string GetCommandText(string connectionString, CommandSchema commandSchema)
        {
            StringBuilder builder = new StringBuilder();
            string str = string.Format("SELECT ROUTINE_DEFINITION FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = '{0}' AND ROUTINE_NAME = '{1}'", commandSchema.Database.Name, commandSchema.Name);
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = str;
                command.Connection = connection;
                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        builder.Append(reader.GetString(0));
                    }
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return builder.ToString();
        }

        public string GetDatabaseName(string connectionString)
        {
            Match match = new Regex(@"Database\W*=\W*(?<database>[^;]*)", RegexOptions.IgnoreCase).Match(connectionString);
            if (match.Success)
            {
                return match.Groups["database"].ToString();
            }
            return connectionString;
        }

        private static DbType GetDbType(string type, bool isUnsigned)
        {
            switch (type.ToLower())
            {
                case "bit":
                    return DbType.UInt16;

                case "tinyint":
                    if (isUnsigned)
                    {
                        return DbType.Boolean;
                    }
                    return DbType.Boolean;

                case "smallint":
                    if (isUnsigned)
                    {
                        return DbType.UInt16;
                    }
                    return DbType.Int16;

                case "year":
                case "mediumint":
                case "int":
                    if (isUnsigned)
                    {
                        return DbType.UInt32;
                    }
                    return DbType.Int32;

                case "bigint":
                    if (isUnsigned)
                    {
                        return DbType.UInt64;
                    }
                    return DbType.Int64;

                case "float":
                    return DbType.Single;

                case "double":
                    return DbType.Double;

                case "decimal":
                    return DbType.Decimal;

                case "date":
                    return DbType.Date;

                case "datetime":
                    return DbType.DateTime;

                case "timestamp":
                    return DbType.DateTime;

                case "time":
                    return DbType.Time;

                case "enum":
                case "set":
                case "tinytext":
                case "mediumtext":
                case "longtext":
                case "text":
                case "char":
                case "varchar":
                    return DbType.String;

                case "binary":
                case "varbinary":
                case "tinyblob":
                case "blob":
                case "longblob":
                    return DbType.Binary;
            }
            return DbType.Object;
        }

        public ExtendedProperty[] GetExtendedProperties(string connectionString, SchemaObjectBase schemaObject)
        {
            List<ExtendedProperty> extendedProperties = new List<ExtendedProperty>();

            if (schemaObject is ColumnSchema)
            {
                ColumnSchema columnSchema = schemaObject as ColumnSchema;

                string commandText = string.Format(@"SELECT EXTRA, COLUMN_DEFAULT, COLUMN_TYPE, COLUMN_COMMENT
                                                      FROM INFORMATION_SCHEMA.COLUMNS
                                                      WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}' AND COLUMN_NAME = '{2}'",
                                                      columnSchema.Table.Database.Name, columnSchema.Table.Name, columnSchema.Name);

                using (DbConnection connection = CreateConnection(connectionString))
                {
                    connection.Open();

                    DbCommand command = connection.CreateCommand();
                    command.CommandText = commandText;
                    command.Connection = connection;

                    using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            string extra = reader.GetString(0).ToLower();
                            bool columndefaultisnull = reader.IsDBNull(1);
                            string columndefault = "";
                            if (!columndefaultisnull)
                            {
                                columndefault = reader.GetString(1).ToUpper();
                            }
                            string columntype = reader.GetString(2).ToUpper();
                            string columncomment = reader.GetString(3);

                            bool isIdentity = (extra.IndexOf("auto_increment") > -1);
                            extendedProperties.Add(new ExtendedProperty(ExtendedPropertyNames.IsIdentity, isIdentity, columnSchema.DataType));

                            if (isIdentity)
                            {
                                /*
                                MySQL auto_increment doesn't work exactly like SQL Server's IDENTITY
                                I believe that auto_increment is equivalent to IDENTITY(1, 1)
                                However, auto_increment behaves differently from IDENTITY when used
                                with multi-column primary keys.  See the MySQL Reference Manual for details.
                                */
                                extendedProperties.Add(new ExtendedProperty(ExtendedPropertyNames.IdentitySeed, 1, columnSchema.DataType));
                                extendedProperties.Add(new ExtendedProperty(ExtendedPropertyNames.IdentityIncrement, 1, columnSchema.DataType));
                            }

                            extendedProperties.Add(new ExtendedProperty("CS_ColumnDefaultIsNull", columndefaultisnull, DbType.Boolean)); // Added for Backwards Compatibility.
                            extendedProperties.Add(new ExtendedProperty(ExtendedPropertyNames.DefaultValue, columndefault, DbType.String));
                            extendedProperties.Add(new ExtendedProperty("CS_ColumnDefault", columndefault, DbType.String)); // Added for Backwards Compatibility.
                            extendedProperties.Add(new ExtendedProperty(ExtendedPropertyNames.SystemType, columntype, DbType.String));
                            extendedProperties.Add(new ExtendedProperty("CS_ColumnType", columntype, DbType.String)); // Added for Backwards Compatibility.
                            extendedProperties.Add(new ExtendedProperty("CS_ColumnExtra", extra.ToUpper(), DbType.String));
                            extendedProperties.Add(new ExtendedProperty("CS_Description", columncomment, DbType.String));
                        }

                        if (!reader.IsClosed)
                            reader.Close();
                    }

                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }
            if (schemaObject is TableSchema)
            {
                TableSchema tableSchema = schemaObject as TableSchema;
                string commandText = string.Format(@"SHOW CREATE TABLE `{0}`.`{1}`", tableSchema.Database.Name, tableSchema.Name);

                using (DbConnection connection = CreateConnection(connectionString))
                {
                    connection.Open();

                    DbCommand command = connection.CreateCommand();
                    command.CommandText = commandText;
                    command.Connection = connection;

                    using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            string createtable = reader.GetString(1);
                            extendedProperties.Add(new ExtendedProperty("TS_Description", createtable, DbType.String));
                            int engineIndex = createtable.LastIndexOf("ENGINE");
                            int commentIndex = createtable.LastIndexOf("COMMENT=");
                            string tableDescription = reader.GetString(0);
                            if (commentIndex > engineIndex)
                            {
                                tableDescription = createtable.Substring(commentIndex + 9).Replace("'", "");
                            }
                            extendedProperties.Add(new ExtendedProperty("CS_Description", tableDescription, DbType.String));

                        }

                        if (!reader.IsClosed)
                            reader.Close();
                    }

                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }

            return extendedProperties.ToArray();
        }



        private IEnumerable<TableKeySchema> GetMyTableKeys(string connectionString, SchemaObjectBase table)
        {
            DataSet set;
            string str = string.Format("SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS t1 WHERE t1.TABLE_SCHEMA = '{0}' AND t1.TABLE_NAME = '{1}'  AND CONSTRAINT_TYPE = 'FOREIGN KEY'", table.Database.Name, table.Name);
            string str2 = string.Format("SELECT t1.CONSTRAINT_NAME, t1.COLUMN_NAME, t1.POSITION_IN_UNIQUE_CONSTRAINT,  t1.REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE t1  INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS t2  ON t2.TABLE_SCHEMA = t1.TABLE_SCHEMA  AND t2.TABLE_NAME = t1.TABLE_NAME  AND t2.CONSTRAINT_NAME = t1.CONSTRAINT_NAME WHERE t1.TABLE_SCHEMA = '{0}' AND t1.TABLE_NAME = '{1}'  AND t2.CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY t1.CONSTRAINT_NAME, t1.POSITION_IN_UNIQUE_CONSTRAINT", table.Database.Name, table.Name);
            DbProviderFactory factory = CreateDbProviderFactory();
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = str;
                    command.Connection = connection;
                    set = this.ConvertDataReaderToDataSet(command.ExecuteReader());
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            using (DbConnection connection2 = factory.CreateConnection())
            {
                connection2.ConnectionString = connectionString;
                connection2.Open();
                using (DbCommand command2 = connection2.CreateCommand())
                {
                    command2.CommandText = str2;
                    command2.Connection = connection2;
                    set.Tables.Add(this.ConvertDataReaderToDataTable(command2.ExecuteReader()));
                }
                if (connection2.State != ConnectionState.Closed)
                {
                    connection2.Close();
                }
            }
            List<TableKeySchema> list = new List<TableKeySchema>();
            if ((set.Tables.Count > 0) && (set.Tables[0].Rows.Count > 0))
            {
                set.Relations.Add("Contraint_to_Keys", set.Tables[0].Columns["CONSTRAINT_NAME"], set.Tables[1].Columns["CONSTRAINT_NAME"]);
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    string name = row["CONSTRAINT_NAME"].ToString();
                    List<DataRow> list2 = new List<DataRow>(row.GetChildRows("Contraint_to_Keys"));
                    List<string> list3 = new List<string>(list2.Count);
                    List<string> list4 = new List<string>(list2.Count);
                    string foreignKeyTable = table.Name;
                    string primaryKeyTable = list2[0]["REFERENCED_TABLE_NAME"].ToString();
                    foreach (DataRow row2 in list2)
                    {
                        list4.Add(row2["COLUMN_NAME"].ToString());
                        list3.Add(row2["REFERENCED_COLUMN_NAME"].ToString());
                    }
                    list.Add(new TableKeySchema(table.Database, name, list4.ToArray(), foreignKeyTable, list3.ToArray(), primaryKeyTable));
                }
            }
            if (list.Count > 0)
            {
                return list;
            }
            return new List<TableKeySchema>();
        }

        private IEnumerable<TableKeySchema> GetOthersTableKeys(string connectionString, SchemaObjectBase table)
        {
            DataSet set;
            string str = string.Format("SELECT DISTINCT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE t1 WHERE t1.TABLE_SCHEMA = '{0}' AND t1.REFERENCED_TABLE_NAME = '{1}'", table.Database.Name, table.Name);
            string str2 = string.Format("SELECT t1.CONSTRAINT_NAME, t1.TABLE_NAME, t1.COLUMN_NAME, t1.POSITION_IN_UNIQUE_CONSTRAINT,  t1.REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE t1  INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS t2  ON t2.TABLE_SCHEMA = t1.TABLE_SCHEMA  AND t2.TABLE_NAME = t1.TABLE_NAME  AND t2.CONSTRAINT_NAME = t1.CONSTRAINT_NAME WHERE t1.TABLE_SCHEMA = '{0}' AND t1.REFERENCED_TABLE_NAME = '{1}'  AND t2.CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY t1.CONSTRAINT_NAME, t1.POSITION_IN_UNIQUE_CONSTRAINT", table.Database.Name, table.Name);
            DbProviderFactory factory = CreateDbProviderFactory();
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = str;
                    command.Connection = connection;
                    set = this.ConvertDataReaderToDataSet(command.ExecuteReader());
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            using (DbConnection connection2 = factory.CreateConnection())
            {
                connection2.ConnectionString = connectionString;
                connection2.Open();
                using (DbCommand command2 = connection2.CreateCommand())
                {
                    command2.CommandText = str2;
                    command2.Connection = connection2;
                    set.Tables.Add(this.ConvertDataReaderToDataTable(command2.ExecuteReader()));
                }
                if (connection2.State != ConnectionState.Closed)
                {
                    connection2.Close();
                }
            }
            List<TableKeySchema> list = new List<TableKeySchema>();
            if ((set.Tables.Count > 0) && (set.Tables[0].Rows.Count > 0))
            {
                set.Relations.Add("Contraint_to_Keys", set.Tables[0].Columns["CONSTRAINT_NAME"], set.Tables[1].Columns["CONSTRAINT_NAME"]);
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    string name = row["CONSTRAINT_NAME"].ToString();
                    List<DataRow> list2 = new List<DataRow>(row.GetChildRows("Contraint_to_Keys"));
                    List<string> list3 = new List<string>(list2.Count);
                    List<string> list4 = new List<string>(list2.Count);
                    string foreignKeyTable = list2[0]["TABLE_NAME"].ToString();
                    string primaryKeyTable = list2[0]["REFERENCED_TABLE_NAME"].ToString();
                    foreach (DataRow row2 in list2)
                    {
                        list4.Add(row2["COLUMN_NAME"].ToString());
                        list3.Add(row2["REFERENCED_COLUMN_NAME"].ToString());
                    }
                    list.Add(new TableKeySchema(table.Database, name, list4.ToArray(), foreignKeyTable, list3.ToArray(), primaryKeyTable));
                }
            }
            if (list.Count > 0)
            {
                return list;
            }
            return new List<TableKeySchema>();
        }

        public ColumnSchema[] GetTableColumns(string connectionString, TableSchema table)
        {
            string str = string.Format("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_OCTET_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE, CASE IS_NULLABLE WHEN 'NO' THEN 0 ELSE 1 END IS_NULLABLE, COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}' ORDER BY ORDINAL_POSITION", table.Database.Name, table.Name);
            List<ColumnSchema> list = new List<ColumnSchema>();
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = str;
                command.Connection = connection;
                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string str3 = reader.GetString(1);
                        long num = !reader.IsDBNull(2) ? reader.GetInt64(2) : 0L;
                        byte precision = !reader.IsDBNull(3) ? ((byte) reader.GetInt32(3)) : ((byte) 0);
                        int scale = !reader.IsDBNull(4) ? reader.GetInt32(4) : 0;
                        bool allowDBNull = !reader.IsDBNull(5) && reader.GetBoolean(5);
                        string str4 = reader.GetString(6);
                        int size = (num < 0x7fffffffL) ? ((int) num) : 0x7fffffff;
                        bool isUnsigned = str4.IndexOf("unsigned") > -1;
                        DbType dbType = GetDbType(str3, isUnsigned);
                        list.Add(new ColumnSchema(table, name, dbType, str3, size, precision, scale, allowDBNull));
                    }
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return list.ToArray();
        }

        public DataTable GetTableData(string connectionString, TableSchema table)
        {
            DataSet set;
            string str = string.Format("SELECT * FROM {0}", table.Name);
            using (DbConnection connection = CreateDbProviderFactory().CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = str;
                command.Connection = connection;
                set = this.ConvertDataReaderToDataSet(command.ExecuteReader());
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            if (set.Tables.Count > 0)
            {
                return set.Tables[0];
            }
            return new DataTable(table.Name);
        }

        public IndexSchema[] GetTableIndexes(string connectionString, TableSchema table)
        {
            DataSet set;
            string str = string.Format("SELECT INDEX_NAME, COUNT(*) AS COLUMN_COUNT, MAX(NON_UNIQUE) NON_UNIQUE, CASE INDEX_NAME WHEN 'PRIMARY' THEN 1 ELSE 0 END IS_PRIMARY FROM INFORMATION_SCHEMA.STATISTICS WHERE  TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}' GROUP BY INDEX_NAME ORDER BY INDEX_NAME;", table.Database.Name, table.Name);
            string str2 = string.Format("SELECT INDEX_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.STATISTICS WHERE  TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}' ORDER BY INDEX_NAME, SEQ_IN_INDEX;", table.Database.Name, table.Name);
            DbProviderFactory factory = CreateDbProviderFactory();
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = str;
                    command.Connection = connection;
                    set = this.ConvertDataReaderToDataSet(command.ExecuteReader());
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            using (DbConnection connection2 = factory.CreateConnection())
            {
                connection2.ConnectionString = connectionString;
                connection2.Open();
                using (DbCommand command2 = connection2.CreateCommand())
                {
                    command2.CommandText = str2;
                    command2.Connection = connection2;
                    set.Tables.Add(this.ConvertDataReaderToDataTable(command2.ExecuteReader()));
                }
                if (connection2.State != ConnectionState.Closed)
                {
                    connection2.Close();
                }
            }
            List<IndexSchema> list = new List<IndexSchema>();
            if ((set.Tables.Count > 0) && (set.Tables[0].Rows.Count > 0))
            {
                set.Relations.Add("INDEX_to_COLUMNS", set.Tables[0].Columns["INDEX_NAME"], set.Tables[1].Columns["INDEX_NAME"]);
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    string name = row["INDEX_NAME"].ToString();
                    bool isPrimaryKey = ((int) row["IS_PRIMARY"]) == 1;
                    bool isUnique = ((long) row["NON_UNIQUE"]) != 1L;
                    bool isClustered = isPrimaryKey;
                    List<DataRow> list2 = new List<DataRow>(row.GetChildRows("INDEX_to_COLUMNS"));
                    List<string> list3 = new List<string>(list2.Count);
                    foreach (DataRow row2 in list2)
                    {
                        list3.Add(row2["COLUMN_NAME"].ToString());
                    }
                    list.Add(new IndexSchema(table, name, isPrimaryKey, isUnique, isClustered, list3.ToArray()));
                }
            }
            if (list.Count > 0)
            {
                return list.ToArray();
            }
            return new List<IndexSchema>().ToArray();
        }

        public TableKeySchema[] GetTableKeys(string connectionString, TableSchema table)
        {
            List<TableKeySchema> list = new List<TableKeySchema>();
            list.AddRange(this.GetMyTableKeys(connectionString, table));
            list.AddRange(this.GetOthersTableKeys(connectionString, table));
            return list.ToArray();
        }

        public PrimaryKeySchema GetTablePrimaryKey(string connectionString, TableSchema table)
        {
            DataSet set;
            string str = string.Format("SELECT t1.CONSTRAINT_NAME, t1.COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE t1  INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS t2  ON t2.TABLE_SCHEMA = t1.TABLE_SCHEMA  AND t2.TABLE_NAME = t1.TABLE_NAME  AND t2.CONSTRAINT_NAME = t1.CONSTRAINT_NAME WHERE t1.TABLE_SCHEMA = '{0}' AND t1.TABLE_NAME = '{1}' AND t2.CONSTRAINT_TYPE = 'PRIMARY KEY' ORDER BY t1.ORDINAL_POSITION", table.Database.Name, table.Name);
            using (DbConnection connection = CreateDbProviderFactory().CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = str;
                command.Connection = connection;
                set = this.ConvertDataReaderToDataSet(command.ExecuteReader());
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            if ((set.Tables.Count <= 0) || (set.Tables[0].Rows.Count <= 0))
            {
                return null;
            }
            string name = set.Tables[0].Rows[0]["CONSTRAINT_NAME"].ToString();
            string[] memberColumns = new string[set.Tables[0].Rows.Count];
            for (int i = 0; i < set.Tables[0].Rows.Count; i++)
            {
                memberColumns[i] = set.Tables[0].Rows[i]["COLUMN_NAME"].ToString();
            }
            return new PrimaryKeySchema(table, name, memberColumns);
        }

        public TableSchema[] GetTables(string connectionString, DatabaseSchema database)
        {
            string str = string.Format("SELECT TABLE_NAME, '' OWNER, CREATE_TIME ,TABLE_COMMENT FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{0}' AND TABLE_TYPE = 'BASE TABLE' ORDER BY 1", database.Name);
            List<TableSchema> list = new List<TableSchema>();
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = str;
                command.Connection = connection;
                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        DateTime dateCreated = !reader.IsDBNull(2) ? reader.GetDateTime(2) : DateTime.MinValue;
                        string comment = reader.GetString(3);
                        list.Add(new TableSchema(database, reader.GetString(0), reader.GetString(1), dateCreated,new[]
                        {
                            new ExtendedProperty("CS_Description",comment,DbType.String), 
                        }));
                    }
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return list.ToArray();
        }

        public ViewColumnSchema[] GetViewColumns(string connectionString, ViewSchema view)
        {
            string str = string.Format("SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_OCTET_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE, CASE IS_NULLABLE WHEN 'NO' THEN 0 ELSE 1 END IS_NULLABLE, COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}'ORDER BY ORDINAL_POSITION", view.Database.Name, view.Name);
            List<ViewColumnSchema> list = new List<ViewColumnSchema>();
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = str;
                command.Connection = connection;
                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string str3 = reader.GetString(1);
                        long num = !reader.IsDBNull(2) ? reader.GetInt64(2) : 0L;
                        byte precision = !reader.IsDBNull(3) ? ((byte) reader.GetInt32(3)) : ((byte) 0);
                        int scale = !reader.IsDBNull(4) ? reader.GetInt32(4) : 0;
                        bool allowDBNull = !reader.IsDBNull(5) && reader.GetBoolean(5);
                        string str4 = reader.GetString(6);
                        int size = (num < 0x7fffffffL) ? ((int) num) : 0x7fffffff;
                        bool isUnsigned = str4.IndexOf("unsigned") > -1;
                        DbType dbType = GetDbType(str3, isUnsigned);
                        list.Add(new ViewColumnSchema(view, name, dbType, str3, size, precision, scale, allowDBNull));
                    }
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return list.ToArray();
        }

        public DataTable GetViewData(string connectionString, ViewSchema view)
        {
            DataSet set;
            string str = string.Format("SELECT * FROM {0}", view.Name);
            using (DbConnection connection = CreateDbProviderFactory().CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = str;
                command.Connection = connection;
                set = this.ConvertDataReaderToDataSet(command.ExecuteReader());
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            if (set.Tables.Count > 0)
            {
                return set.Tables[0];
            }
            return new DataTable(view.Name);
        }

        public ViewSchema[] GetViews(string connectionString, DatabaseSchema database)
        {
            string str = string.Format("SELECT TABLE_NAME, '' OWNER, CREATE_TIME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{0}' AND TABLE_TYPE = 'VIEW' ORDER BY 1", database.Name);
            List<ViewSchema> list = new List<ViewSchema>();
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = str;
                command.Connection = connection;
                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        DateTime dateCreated = !reader.IsDBNull(2) ? reader.GetDateTime(2) : DateTime.MinValue;
                        list.Add(new ViewSchema(database, reader.GetString(0), reader.GetString(1), dateCreated));
                    }
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return list.ToArray();
        }

        public string GetViewText(string connectionString, ViewSchema view)
        {
            StringBuilder builder = new StringBuilder();
            string str = string.Format("SELECT VIEW_DEFINITION FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}'", view.Database.Name, view.Name);
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                DbCommand command = connection.CreateCommand();
                command.CommandText = str;
                command.Connection = connection;
                using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        builder.Append(reader.GetString(0));
                    }
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return builder.ToString();
        }

        public void SetExtendedProperties(string connectionString, SchemaObjectBase schemaObject)
        {
            throw new NotImplementedException("This method has not been implemented");
        }

        public string Description
        {
            get
            {
                return "MySQL Schema Provider";
            }
        }

        public string Name
        {
            get
            {
                return "MySQLSchemaProvider";
            }
        }
    }
}

