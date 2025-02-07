using Colorify;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Ci4.Database
{
    class Mysql
    {
        private static MySqlConnectionStringBuilder connection()
        {
            DotNetEnv.Env.Load();
            MySqlConnectionStringBuilder conn = new MySqlConnectionStringBuilder();
            conn.Server = DotNetEnv.Env.GetString("database.default.hostname");
            conn.Port = Convert.ToUInt32(DotNetEnv.Env.GetInt("database.default.port"));
            conn.UserID = DotNetEnv.Env.GetString("database.default.username");
            conn.Password = DotNetEnv.Env.GetString("database.default.password");
            conn.Database = DotNetEnv.Env.GetString("database.default.database");
            return conn;
        }

        private static void Create(string table, string name)
        {
            #region Variables
            DotNetEnv.Env.Load();
            var path = Environment.CurrentDirectory + @"\app\";
            var model = path + @"Models\";
            #endregion

            #region Create Entity
            if (!Directory.Exists(model))
                Directory.CreateDirectory(model);

            if (!File.Exists(model + name + "Models.php"))
            {
                //Get Column Info
                var column = GetAllColumn(table);

                var modelFile = File.Create(model + name + "Models.php");
                var modelWriter = new StreamWriter(modelFile);
                modelWriter.WriteLine(@"<?php");
                modelWriter.WriteLine(@"namespace App\Models;");
                modelWriter.WriteLine(@"use CodeIgniter\Model;");
                modelWriter.WriteLine(@"class " + name + "Model extends Model");
                modelWriter.WriteLine(@"{");

                var primaryKey = "";
                foreach (var item in column)
                {
                    if (item.Key.ToLower() == "pri") {
                        primaryKey = item.Field;
                    }
                }

                modelWriter.WriteLine(@"    protected $table = '" + table + "';");
                modelWriter.WriteLine(@"    protected $primaryKey = '" + primaryKey + "';");

                var itens = @"    protected $allowedFields = [";
                foreach (var item in column)
                {
                    if(item.Field != "created_at" && item.Field != "updated_at" && item.Field != "deleted_at")
                    {
                        itens += Environment.NewLine + @"        '" + item.Field + "',";
                    }
                }
                itens = itens.Remove(itens.Length - 1) + Environment.NewLine + @"    ];";
                modelWriter.WriteLine(itens);
                modelWriter.WriteLine(@"    protected $useTimestamps = true;");
                //modelWriter.WriteLine(@"    protected $useSoftDeletes = true;");
                modelWriter.WriteLine(@"    protected $createdField  = 'created_at';");
                modelWriter.WriteLine(@"    protected $updatedField  = 'updated_at';");
                //modelWriter.WriteLine(@"    protected $deletedField  = 'deleted_at';");
                modelWriter.WriteLine(@"}");
                modelWriter.Dispose();
                Program._colorify.WriteLine("Model successfully created!", Colors.bgSuccess);
                Program._colorify.WriteLine(model + name + "Models.php", Colors.bgMuted);
            }
            else
            {
                Program._colorify.WriteLine(name + "Models.php already exists and so was not generated.", Colors.bgDanger);
                Program._colorify.WriteLine(model + name + "Models.php", Colors.bgMuted);
            }
            #endregion
        }

        public static void Generator(string name)
        {
            try
            {
                
                if (name != "*")
                {
                    Create(name, Function.Util.FormatName(name));
                }
                else
                {
                    IEnumerable<Table> list = new List<Table>(GetAllTable());
                    foreach (var item in list)
                    {
                        Create(item.Name, Function.Util.FormatName(item.Name));
                    }
                }
            }
            catch (Exception)
            {
                Program._colorify.WriteLine("Table '" + name.ToLower() + "' does not exist.", Colors.bgDanger);
            }
            
        }

        public static bool ExistTable(string name)
        {
            try
            {
                GetAllColumn(name);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static IEnumerable<string> FieldTable(string name)
        {
            try
            {
                var columns = GetAllColumn(name);
                List<string> list = new List<string>();
                foreach (var item in columns)
                {
                    if(item.Key.ToLower() != "pri")
                    {
                        if (item.Type.Contains("date") || item.Type.Contains("datetime"))
                        {
                            list.Add(item.Field.ToLower() + "[" + item.Type.ToUpper() + "]");
                        }
                        else
                        {
                            list.Add(item.Field.ToLower());
                        }
                    }
                    else
                    {
                        list.Add("id");
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string AnnotationType(string type)
        {
            if (type.ToUpper() == "TINYINT(1)")
                type = "BOOLEAN";

            if (type.Contains("(") && type.Contains(")"))
            {
                int position = type.IndexOf("(");
                type = type.Substring(0, position);
            }

            switch (type.ToUpper())
            {
                //Number
                case "TINYINT":
                case "SMALLINT":
                    return "smallint";
                case "INT":
                case "MEDIUMINT":
                    return "integer";
                case "BIGINT":
                    return "bigint";
                case "DECIMAL":
                    return "decimal";
                case "FLOAT":
                case "DOUBLE":
                    return "float";
                //Text
                case "CHAR":
                case "VARCHAR":
                    return "string";
                case "TINYTEXT":
                case "TEXT":
                case "MEDIUMTEXT":
                case "LONGTEXT":
                    return "text";
                //Blob
                case "TINYBLOB":
                case "BLOB":
                case "MEDIUMBLOB":
                case "LONGBLOB":
                    return "blob";
                //Boolean
                case "BOOL":
                case "BOOLEAN":
                    return "boolean";
                //Date Time
                case "DATE":
                    return "date";
                case "TIME":
                    return "time";
                case "DATETIME":
                    return "datetime";
                case "TIMESTAMP":
                    return "datetimetz";
                default:
                    return "string";
            }
        }

        private static void ShowInfo()
        {
            string SqlTable = "SHOW TABLES";

            using (var db = new MySqlConnection(connection().ToString()))
            {
                var table = db.Query<string>(SqlTable);
                foreach (var itemT in table)
                {
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine("Table: " + itemT);
                    Console.WriteLine("--------------------------------");

                    string SqlColumn = String.Format("SHOW COLUMNS FROM {0}", itemT);

                    var column = db.Query<Column>(SqlColumn);
                    foreach (var itemC in column)
                    {
                        Console.WriteLine("Field:" + itemC.Field
                                        + " | Type: " + itemC.Type
                                        + " | Null: " + itemC.Null
                                        + " | Key: " + itemC.Key
                                        + " | Default:" + itemC.Default
                                        + " | Extra:" + itemC.Extra);
                    }

                    string SqlFK = String.Format("SELECT table_name, column_name, constraint_name, referenced_table_name, referenced_column_name" +
                                                " FROM information_schema.key_column_usage" +
                                                " WHERE referenced_table_schema = '{0}' AND referenced_table_name = '{1}'", DotNetEnv.Env.GetString("DB_NAME"), itemT);

                    var foreign_key = db.Query<FkTable>(SqlFK);
                    foreach (var itemF in foreign_key)
                    {
                        Console.WriteLine("TableName:" + itemF.table_name
                                        + " | ColumnName: " + itemF.column_name
                                        + " | ConstraintName: " + itemF.constraint_name
                                        + " | ReferencedTableName: " + itemF.referenced_table_name
                                        + " | ReferencedColumnName:" + itemF.referenced_column_name);
                    }
                }



                db.Dispose();
            }

        }

        private static IEnumerable<Table> GetAllTable()
        {
            string sql = "SHOW TABLES";
            using (var db = new MySqlConnection(connection().ToString()))
            {
                var ret = db.Query<string>(sql);
                List<Table> list = new List<Table>();
                foreach (var item in ret)
                {
                    Table table = new Table();
                    table.Name = item;
                    list.Add(table);
                }
                db.Dispose();
                return list;
            }
        }

        private static IEnumerable<Column> GetAllColumn(string table)
        {
            string sql = String.Format("SHOW COLUMNS FROM {0}", table);
            using (var db = new MySqlConnection(connection().ToString()))
            {
                var list = db.Query<Column>(sql);
                db.Dispose();
                return list;
            }
        }

        private static IEnumerable<FkTable> GetForekeyTable(string table)
        {
            string sql = String.Format("SELECT table_name, column_name, constraint_name, referenced_table_name, referenced_column_name" +
                                        " FROM information_schema.key_column_usage" +
                                        " WHERE referenced_table_schema = '{0}' AND referenced_table_name = '{1}'", DotNetEnv.Env.GetString("DB_NAME"), table);

            using (var db = new MySqlConnection(connection().ToString()))
            {
                var list = db.Query<FkTable>(sql);
                db.Dispose();
                return list;
            }
        }

        private static IEnumerable<FkColumn> GetForekeyColumn(string column)
        {
            string sql = String.Format("SELECT referenced_table_name, referenced_column_name" +
                                        " FROM information_schema.key_column_usage" +
                                        " WHERE referenced_table_schema = '{0}' AND referenced_column_name = '{1}' LIMIT 1", DotNetEnv.Env.GetString("DB_NAME"), column);

            using (var db = new MySqlConnection(connection().ToString()))
            {
                var list = db.Query<FkColumn>(sql);
                db.Dispose();
                return list;
            }
        }

        private class FkTable
        {
            public string table_name { get; set; }
            public string column_name { get; set; }
            public string constraint_name { get; set; }
            public string referenced_table_name { get; set; }
            public string referenced_column_name { get; set; }
        }

        private class FkColumn
        {
            public string referenced_table_name { get; set; }
            public string referenced_column_name { get; set; }
        }
        private class Table
        {
            public string Name { get; set; }
        }

        private class Column
        {
            public string Field { get; set; }
            public string Type { get; set; }
            public string Null { get; set; }
            public string Key { get; set; }
            public string Default { get; set; }
            public string Extra { get; set; }
        }

    }
}
