using Colorify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ci4.Process
{
    class Create
    {
        public static void Page(string name, bool crud = false, IEnumerable<string> columns = null)
        {
            #region Variables
            DotNetEnv.Env.Load();

            var path = Environment.CurrentDirectory + @"\app\";
            var controllers = path + @"Controllers\";
            var models = path + @"Models\";
            var views = path + @"Views\" + name.ToLower() + @"\";
            //var route = path + @"Config\route.php";
            //var url = "http://" + (DotNetEnv.Env.GetString("APP_URL_BASE").Replace("http://", "").Replace("https://", "") + DotNetEnv.Env.GetString("APP_URL_PATH")).Replace("//", "/");

            if (Regex.IsMatch(name.Replace("_", ""), @"[^a-zA-Z0-9]"))
            {
                Program._colorify.WriteLine("Only letters, numbers and underline are allowed in the name!", Colors.bgDanger);
                return;
            }
            #endregion

            #region Create Controller
            if (!Directory.Exists(controllers))
                Directory.CreateDirectory(controllers);

            if (!File.Exists(controllers + Function.Util.FormatName(name) + ".php"))
            {
                var controllerFile = File.Create(controllers + Function.Util.FormatName(name) + ".php");
                var controllerWriter = new StreamWriter(controllerFile);
                controllerWriter.WriteLine(@"<?php");
                controllerWriter.WriteLine(@"namespace App\Controllers;");
                if (crud)
                {
                    controllerWriter.WriteLine(@"use App\Models\" + Function.Util.FormatName(name) + "Model;");
                }
                controllerWriter.WriteLine(@"class " + Function.Util.FormatName(name) + " extends BaseController");
                controllerWriter.WriteLine(@"{");
                if (crud)
                {
                    controllerWriter.WriteLine(@"    public function index()");
                    controllerWriter.WriteLine(@"    {");
                    controllerWriter.WriteLine(@"        View::render('" + name.ToLower() + "/index', true, array('" + name.ToLower() + "' => " + Function.Util.FormatName(name) + "Model::list" + Function.Util.FormatName(name) + "()));");
                    controllerWriter.WriteLine(@"    }");
                    controllerWriter.WriteLine(@"");
                    controllerWriter.WriteLine(@"    public function create()");
                    controllerWriter.WriteLine(@"    {");
                    controllerWriter.WriteLine(@"        View::render('" + name.ToLower() + "/add', true, array('" + name.ToLower() + "' => " + Function.Util.FormatName(name) + "Model::insert" + Function.Util.FormatName(name) + "()));");
                    controllerWriter.WriteLine(@"    }");
                    controllerWriter.WriteLine(@"");
                    controllerWriter.WriteLine(@"    public function edit()");
                    controllerWriter.WriteLine(@"    {");
                    controllerWriter.WriteLine(@"        View::render('" + name.ToLower() + "/edit', true, array('" + name.ToLower() + "' => " + Function.Util.FormatName(name) + "Model::update" + Function.Util.FormatName(name) + "()));");
                    controllerWriter.WriteLine(@"    }");
                    controllerWriter.WriteLine(@"");
                    controllerWriter.WriteLine(@"    public function delete()");
                    controllerWriter.WriteLine(@"    {");
                    controllerWriter.WriteLine(@"        View::render('" + name.ToLower() + "/delete', true, array('" + name.ToLower() + "' => " + Function.Util.FormatName(name) + "Model::delete" + Function.Util.FormatName(name) + "()));");
                    controllerWriter.WriteLine(@"    }");
                }
                else
                {
                    controllerWriter.WriteLine(@"    public function index()");
                    controllerWriter.WriteLine(@"    {");
                    controllerWriter.WriteLine(@"        return view('"+ name.ToLower() + "/index');");
                    controllerWriter.WriteLine(@"    }");
                }
                controllerWriter.WriteLine(@"}");
                controllerWriter.Dispose();
                Program._colorify.WriteLine("Controller successfully created!", Colors.bgSuccess);
                Program._colorify.WriteLine(controllers + Function.Util.FormatName(name) + ".php", Colors.bgMuted);
            }
            else
            {
                Program._colorify.WriteLine(Function.Util.FormatName(name) + ".php already exists and so was not generated.", Colors.bgDanger);
                Program._colorify.WriteLine(controllers + Function.Util.FormatName(name) + ".php", Colors.bgMuted);
            }
            #endregion

            #region Create Model
            if (!Directory.Exists(models))
                Directory.CreateDirectory(models);

            if (crud == true && !File.Exists(models + Function.Util.FormatName(name) + "Model.php"))
            {
                var modelFile = File.Create(models + Function.Util.FormatName(name) + "Model.php");
                var modelWriter = new StreamWriter(modelFile);
                modelWriter.WriteLine(@"<?php");
                modelWriter.WriteLine(@"namespace App\Models");
                modelWriter.WriteLine(@"{");
                modelWriter.WriteLine(@"    use System\Core\Model;");
                modelWriter.WriteLine(@"    class " + Function.Util.FormatName(name) + "Model extends Model");
                modelWriter.WriteLine(@"    {");
                modelWriter.WriteLine(@"        public function __construct()");
                modelWriter.WriteLine(@"        {");
                modelWriter.WriteLine(@"            parent::__construct();");
                modelWriter.WriteLine(@"        }");
                if (crud)
                {
                    string varName = name.Replace("_", "").ToLower();
                    modelWriter.WriteLine(@"");
                    modelWriter.WriteLine(@"        public static function list" + Function.Util.FormatName(name) + "()");
                    modelWriter.WriteLine(@"        {");
                    modelWriter.WriteLine(@"            require __DIR__.'/../config/bootstrap.php';");
                    modelWriter.WriteLine(@"            if(isset($entityManager)){");
                    modelWriter.WriteLine(@"                $" + varName + @"Repository = $entityManager->getRepository('\App\Entities\" + Function.Util.FormatName(name) + "');");
                    modelWriter.WriteLine(@"                $" + varName + " = $" + varName + "Repository->findAll();");
                    modelWriter.WriteLine(@"                return $" + varName + ";");
                    modelWriter.WriteLine(@"            }else{");
                    modelWriter.WriteLine(@"                return null;");
                    modelWriter.WriteLine(@"            }");
                    modelWriter.WriteLine(@"        }");
                    modelWriter.WriteLine(@"");
                    modelWriter.WriteLine(@"        public static function insert" + Function.Util.FormatName(name) + "()");
                    modelWriter.WriteLine(@"        {");
                    modelWriter.WriteLine(@"            require __DIR__.'/../config/bootstrap.php';");
                    modelWriter.WriteLine(@"        }");
                    modelWriter.WriteLine(@"");
                    modelWriter.WriteLine(@"        public static function update" + Function.Util.FormatName(name) + "()");
                    modelWriter.WriteLine(@"        {");
                    modelWriter.WriteLine(@"            require __DIR__.'/../config/bootstrap.php';");
                    modelWriter.WriteLine(@"        }");
                    modelWriter.WriteLine(@"");
                    modelWriter.WriteLine(@"        public static function delete" + Function.Util.FormatName(name) + "()");
                    modelWriter.WriteLine(@"        {");
                    modelWriter.WriteLine(@"            require __DIR__.'/../config/bootstrap.php';");
                    modelWriter.WriteLine(@"        }");
                }
                modelWriter.WriteLine(@"    }");
                modelWriter.WriteLine(@"}");
                modelWriter.Dispose();
                Program._colorify.WriteLine("Model successfully created!", Colors.bgSuccess);
                Program._colorify.WriteLine(models + Function.Util.FormatName(name) + "Model.php", Colors.bgMuted);
            }
            else
            {
                Program._colorify.WriteLine(Function.Util.FormatName(name) + "Model.php already exists and so was not generated.", Colors.bgDanger);
                Program._colorify.WriteLine(models + Function.Util.FormatName(name) + "Model.php", Colors.bgMuted);
            }
            #endregion

            #region Create View
            if (!Directory.Exists(views))
                Directory.CreateDirectory(views);

            if (crud)
            {
                if (!File.Exists(views + "index.php"))
                {
                    var viewFile = File.Create(views + "index.php");
                    var viewWriter = new StreamWriter(viewFile);
                    viewWriter.WriteLine("{% extends \"templates/default/base.php\" %}");
                    viewWriter.WriteLine("{% block title %}List - " + Function.Util.FormatName(name.Replace("_", "#")) + "{% endblock %}");
                    viewWriter.WriteLine("{% block body %}");
                    viewWriter.WriteLine("<h1><b>" + Function.Util.FormatName(name.Replace("_", "#")) + " - List</b></h1>");
                    viewWriter.WriteLine("{% for item in " + name.ToLower() + " %}");
                    foreach (var item in columns)
                    {
                        var column = item.Replace("[DATE]", "|date(\"" + DotNetEnv.Env.GetString("APP_DATE") + "\")");
                        column = column.Replace("[DATETIME]", "|date(\"" + DotNetEnv.Env.GetString("APP_DATETIME") + "\")");

                        viewWriter.WriteLine("{{ item." + column + " }}");
                    }
                    viewWriter.WriteLine("<br>");
                    viewWriter.WriteLine("{% else %}");
                    viewWriter.WriteLine("No " + Function.Util.FormatName(name.Replace("_", "#")).ToLower() + " have been found.");
                    viewWriter.WriteLine("{% endfor %}");
                    viewWriter.WriteLine("{% endblock %}");
                    viewWriter.Dispose();
                    Program._colorify.WriteLine("View successfully created!", Colors.bgSuccess);
                    Program._colorify.WriteLine(views + "index.php", Colors.bgMuted);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + @"/index.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(views + "index.php", Colors.bgMuted);
                }

                if (!File.Exists(views + "add.php"))
                {
                    var viewFile = File.Create(views + "add.php");
                    var viewWriter = new StreamWriter(viewFile);
                    viewWriter.WriteLine("{% extends \"templates/default/base.php\" %}");
                    viewWriter.WriteLine("{% block title %}Add - " + Function.Util.FormatName(name.Replace("_", "#")) + "{% endblock %}");
                    viewWriter.WriteLine("{% block body %}");
                    viewWriter.WriteLine("<h1><b>" + Function.Util.FormatName(name.Replace("_", "#")) + " - Add</b></h1>");
                    viewWriter.WriteLine("{% endblock %}");
                    viewWriter.Dispose();
                    Program._colorify.WriteLine("View successfully created!", Colors.bgSuccess);
                    Program._colorify.WriteLine(views + "add.php", Colors.bgMuted);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + @"/add.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(views + "add.php", Colors.bgMuted);
                }

                if (!File.Exists(views + "edit.php"))
                {
                    var viewFile = File.Create(views + "edit.php");
                    var viewWriter = new StreamWriter(viewFile);
                    viewWriter.WriteLine("{% extends \"templates/default/base.php\" %}");
                    viewWriter.WriteLine("{% block title %}Edit - " + Function.Util.FormatName(name.Replace("_", "#")) + "{% endblock %}");
                    viewWriter.WriteLine("{% block body %}");
                    viewWriter.WriteLine("<h1><b>" + Function.Util.FormatName(name.Replace("_", "#")) + " - Edit</b></h1>");
                    viewWriter.WriteLine("{% endblock %}");
                    viewWriter.Dispose();
                    Program._colorify.WriteLine("View successfully created!", Colors.bgSuccess);
                    Program._colorify.WriteLine(views + "edit.php", Colors.bgMuted);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + @"/edit.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(views + "edit.php", Colors.bgMuted);
                }

                if (!File.Exists(views + "delete.php"))
                {
                    var viewFile = File.Create(views + "delete.php");
                    var viewWriter = new StreamWriter(viewFile);
                    viewWriter.WriteLine("{% extends \"templates/default/base.php\" %}");
                    viewWriter.WriteLine("{% block title %}Delete - " + Function.Util.FormatName(name.Replace("_", "#")) + "{% endblock %}");
                    viewWriter.WriteLine("{% block body %}");
                    viewWriter.WriteLine("<h1><b>" + Function.Util.FormatName(name.Replace("_", "#")) + " - Delete</b></h1>");
                    viewWriter.WriteLine("{% endblock %}");
                    viewWriter.Dispose();
                    Program._colorify.WriteLine("View successfully created!", Colors.bgSuccess);
                    Program._colorify.WriteLine(views + "delete.php", Colors.bgMuted);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + @"/delete.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(views + "delete.php", Colors.bgMuted);
                }
            }
            else
            {
                if (!File.Exists(views + "index.php"))
                {
                    var viewFile = File.Create(views + "index.php");
                    var viewWriter = new StreamWriter(viewFile);
                    viewWriter.WriteLine("{% extends \"templates/default/base.php\" %}");
                    viewWriter.WriteLine("{% block title %}" + Function.Util.FormatName(name.Replace("_", "#")) + "{% endblock %}");
                    viewWriter.WriteLine("{% block body %}");
                    viewWriter.WriteLine("<h1>Welcome Page <b>" + Function.Util.FormatName(name.Replace("_", "#")) + "</b></h1>");
                    viewWriter.WriteLine("{% endblock %}");
                    viewWriter.Dispose();
                    Program._colorify.WriteLine("View successfully created!", Colors.bgSuccess);
                    Program._colorify.WriteLine(views + "index.php", Colors.bgMuted);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + @"/index.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(views + "index.php", Colors.bgMuted);
                }
            }
            #endregion
        }

        public static void Model(string name)
        {
            DotNetEnv.Env.Load();
            var driver = !string.IsNullOrEmpty(DotNetEnv.Env.GetString("database.default.DBDriver")) ? DotNetEnv.Env.GetString("database.default.DBDriver") : "";
            switch (driver)
            {
                case null:
                case "":
                    Program._colorify.WriteLine("Please, check the .env file to see if the database variables are commented out with # or are incorrect.", Colors.bgDanger);
                    break;
                case "MySQLi":
                    try
                    {
                        Database.Mysql.Generator(name);
                    }
                    catch (Exception)
                    {
                        Program._colorify.WriteLine("The connection to MySql failed, make sure the MySql service has started.", Colors.bgDanger);
                    }
                    break;
                default:
                    if (string.IsNullOrEmpty(driver)) {
                        Program._colorify.WriteLine("Please define a database in the settings of the .env file.", Colors.bgDanger);
                    }
                    else
                    {
                        Program._colorify.WriteLine("The selected database \"" + DotNetEnv.Env.GetString("database.default.DBDriver") + "\" is not yet supported for automatic generations, only Mysql.", Colors.bgDanger);
                    }
                    break;
            }
        }

        public static void Crud(string name)
        {
            if (Database.Mysql.ExistTable(name))
            {
                var columns = Database.Mysql.FieldTable(name);
                Model(name);
                Page(name, true, columns);
                Program._colorify.WriteLine("Crud " + name.ToLower() + " successfully created!", Colors.bgSuccess);
            }
            else
            {
                Program._colorify.WriteLine("Table '" + name.ToLower() + "' does not exist.", Colors.bgDanger);
            }
        }

    }
}
