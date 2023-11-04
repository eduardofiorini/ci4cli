using Colorify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Ci4.Process
{
    class Make
    {
        public static void Page(string name, bool crud = false, IEnumerable<string> columns = null)
        {
            #region Variables
            DotNetEnv.Env.Load();

            var pathDir = Environment.CurrentDirectory + @"\app\";
            var controllersDir = pathDir + @"Controllers\";
            var modelsDir = pathDir + @"Models\";
            var viewsDir = pathDir + @"Views\" + name.ToLower() + @"\";
            var templateDir = pathDir + @"Views\templates\";
            var isTemplate = Directory.Exists(templateDir) ? true : false;
            var nameItem = Function.Util.FormatUnderline(name);
            //var route = path + @"Config\route.php";
            //var url = "http://" + (DotNetEnv.Env.GetString("APP_URL_BASE").Replace("http://", "").Replace("https://", "") + DotNetEnv.Env.GetString("APP_URL_PATH")).Replace("//", "/");

            if (Regex.IsMatch(name.Replace("_", ""), @"[^a-zA-Z0-9]"))
            {
                Program._colorify.WriteLine("Only letters, numbers and underline are allowed in the name!", Colors.bgDanger);
                return;
            }
            #endregion

            #region Create Controller
            if (!Directory.Exists(controllersDir))
                Directory.CreateDirectory(controllersDir);

            if (!File.Exists(controllersDir + Function.Util.FormatName(name) + ".php"))
            {
                var controllerFile = File.Create(controllersDir + Function.Util.FormatName(name) + ".php");
                var controllerWriter = new StreamWriter(controllerFile);
                controllerWriter.WriteLine(@"<?php");
                controllerWriter.WriteLine(@"namespace App\Controllers;");
                if (crud)
                {
                    controllerWriter.WriteLine(@"use App\Models\" + Function.Util.FormatName(name) + "Model;");
                }

                var header = isTemplate ? String.Format("echo view('templates/{0}');", "header") : "//" + String.Format("echo view('templates/{0}');", "header");
                var footer = isTemplate ? String.Format("echo view('templates/{0}');", "footer") : "//" + String.Format("echo view('templates/{0}');", "footer");

                controllerWriter.WriteLine(@"class " + Function.Util.FormatName(name) + " extends BaseController");
                controllerWriter.WriteLine(@"{");
                controllerWriter.WriteLine(@"    private $links;");
                if (crud)
                {
                    controllerWriter.WriteLine(@"    private $" + nameItem + "_modal;");
                }
                controllerWriter.WriteLine(@"");
                controllerWriter.WriteLine(@"    function __construct()");
                controllerWriter.WriteLine(@"    {");
                controllerWriter.WriteLine(@"        $this->links = [ 'menu' => '3.m', 'item' => '3.0', 'subItem' => '3.3' ];");
                if (crud)
                {
                    controllerWriter.WriteLine(@"        $this->" + nameItem + "_model = new " + Function.Util.FormatName(name) + "Model();");
                }
                controllerWriter.WriteLine(@"    }");
                controllerWriter.WriteLine(@"");
                controllerWriter.WriteLine(@"    public function index()");
                controllerWriter.WriteLine(@"    {");
                controllerWriter.WriteLine(@"        $data['links'] = $this->links;");
                controllerWriter.WriteLine(@"        $data['title'] = ['modulo' => '" + name.ToUpper() + "', 'icone'  => 'fa fa-list'];");
                controllerWriter.WriteLine(@"        $data['path'] = [['titulo' => 'Início', 'rota' => '/inicio', 'active' => false], ['titulo' => '" + name.ToUpper() + "', 'rota'   => '', 'active' => true]];");
                if (crud)
                {
                    controllerWriter.WriteLine(@"        $data['" + nameItem + "'] = $this->" + nameItem + "_model->findAll();");
                }
                controllerWriter.WriteLine(@"        " + header);
                controllerWriter.WriteLine(@"        echo view('" + name.ToLower() + "/index', $data);");
                controllerWriter.WriteLine(@"        " + footer);
                controllerWriter.WriteLine(@"    }");
                if (crud)
                {
                    controllerWriter.WriteLine(@"");
                    controllerWriter.WriteLine(@"    public function create()");
                    controllerWriter.WriteLine(@"    {");
                    controllerWriter.WriteLine(@"        " + header);
                    controllerWriter.WriteLine(@"        echo view('" + name.ToLower() + "/create', $data);");
                    controllerWriter.WriteLine(@"        " + footer);
                    controllerWriter.WriteLine(@"    }");
                    controllerWriter.WriteLine(@"");
                    controllerWriter.WriteLine(@"    public function edit()");
                    controllerWriter.WriteLine(@"    {");
                    controllerWriter.WriteLine(@"        " + header);
                    controllerWriter.WriteLine(@"        echo view('" + name.ToLower() + "/edit', $data);");
                    controllerWriter.WriteLine(@"        " + footer);
                    controllerWriter.WriteLine(@"    }");
                    controllerWriter.WriteLine(@"");
                    controllerWriter.WriteLine(@"    public function show()");
                    controllerWriter.WriteLine(@"    {");
                    controllerWriter.WriteLine(@"        " + header);
                    controllerWriter.WriteLine(@"        echo view('" + name.ToLower() + "/show', $data);");
                    controllerWriter.WriteLine(@"        " + footer);
                    controllerWriter.WriteLine(@"    }");
                    controllerWriter.WriteLine(@"");
                    controllerWriter.WriteLine(@"    public function delete()");
                    controllerWriter.WriteLine(@"    {");
                    controllerWriter.WriteLine(@"        " + header);
                    controllerWriter.WriteLine(@"        echo view('" + name.ToLower() + "/delete', $data);");
                    controllerWriter.WriteLine(@"        " + footer);
                    controllerWriter.WriteLine(@"    }");
                    controllerWriter.WriteLine(@"");
                    controllerWriter.WriteLine(@"    public function store()");
                    controllerWriter.WriteLine(@"    {");
                    controllerWriter.WriteLine(@"        $data = $this->request->getvar();");
                    controllerWriter.WriteLine(@"        $this->cliente_model->save($data);");
                    controllerWriter.WriteLine(@"        if(isset($dados['id_cliente']))");
                    controllerWriter.WriteLine(@"        {");
                    controllerWriter.WriteLine(@"           $session = session();");
                    controllerWriter.WriteLine(@"           $session->setFlashdata('alert', 'success_edit');");
                    controllerWriter.WriteLine(@"           return redirect()->to('/clientes');");
                    controllerWriter.WriteLine(@"        }");
                    controllerWriter.WriteLine(@"        $session = session();");
                    controllerWriter.WriteLine(@"        $session->setFlashdata('alert', 'success_create');");
                    controllerWriter.WriteLine(@"        return redirect()->to('/clientes');");
                    controllerWriter.WriteLine(@"    }");
                }

                controllerWriter.WriteLine(@"}");
                controllerWriter.Dispose();
                Program._colorify.WriteLine("Controller successfully created!", Colors.bgSuccess);
                Program._colorify.WriteLine(controllersDir + Function.Util.FormatName(name) + ".php", Colors.bgMuted);
            }
            else
            {
                Program._colorify.WriteLine(Function.Util.FormatName(name) + ".php already exists and so was not generated.", Colors.bgDanger);
                Program._colorify.WriteLine(controllersDir + Function.Util.FormatName(name) + ".php", Colors.bgMuted);
            }
            #endregion

            #region Create Model
            if (!Directory.Exists(modelsDir))
                Directory.CreateDirectory(modelsDir);
            if (crud)
            {
                if (!File.Exists(modelsDir + Function.Util.FormatName(name) + "Model.php"))
                {
                    Model(name);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + "Model.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(modelsDir + Function.Util.FormatName(name) + "Model.php", Colors.bgMuted);
                }
            }
            #endregion

            #region Create View
            if (!Directory.Exists(viewsDir))
                Directory.CreateDirectory(viewsDir);

            if (crud)
            {
                if (!File.Exists(viewsDir + "index.php"))
                {
                    var viewFile = File.Create(viewsDir + "index.php");
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
                    Program._colorify.WriteLine(viewsDir + "index.php", Colors.bgMuted);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + @"/index.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(viewsDir + "index.php", Colors.bgMuted);
                }

                if (!File.Exists(viewsDir + "add.php"))
                {
                    var viewFile = File.Create(viewsDir + "add.php");
                    var viewWriter = new StreamWriter(viewFile);
                    viewWriter.WriteLine("{% extends \"templates/default/base.php\" %}");
                    viewWriter.WriteLine("{% block title %}Add - " + Function.Util.FormatName(name.Replace("_", "#")) + "{% endblock %}");
                    viewWriter.WriteLine("{% block body %}");
                    viewWriter.WriteLine("<h1><b>" + Function.Util.FormatName(name.Replace("_", "#")) + " - Add</b></h1>");
                    viewWriter.WriteLine("{% endblock %}");
                    viewWriter.Dispose();
                    Program._colorify.WriteLine("View successfully created!", Colors.bgSuccess);
                    Program._colorify.WriteLine(viewsDir + "add.php", Colors.bgMuted);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + @"/add.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(viewsDir + "add.php", Colors.bgMuted);
                }

                if (!File.Exists(viewsDir + "edit.php"))
                {
                    var viewFile = File.Create(viewsDir + "edit.php");
                    var viewWriter = new StreamWriter(viewFile);
                    viewWriter.WriteLine("{% extends \"templates/default/base.php\" %}");
                    viewWriter.WriteLine("{% block title %}Edit - " + Function.Util.FormatName(name.Replace("_", "#")) + "{% endblock %}");
                    viewWriter.WriteLine("{% block body %}");
                    viewWriter.WriteLine("<h1><b>" + Function.Util.FormatName(name.Replace("_", "#")) + " - Edit</b></h1>");
                    viewWriter.WriteLine("{% endblock %}");
                    viewWriter.Dispose();
                    Program._colorify.WriteLine("View successfully created!", Colors.bgSuccess);
                    Program._colorify.WriteLine(viewsDir + "edit.php", Colors.bgMuted);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + @"/edit.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(viewsDir + "edit.php", Colors.bgMuted);
                }

                if (!File.Exists(viewsDir + "delete.php"))
                {
                    var viewFile = File.Create(viewsDir + "delete.php");
                    var viewWriter = new StreamWriter(viewFile);
                    viewWriter.WriteLine("{% extends \"templates/default/base.php\" %}");
                    viewWriter.WriteLine("{% block title %}Delete - " + Function.Util.FormatName(name.Replace("_", "#")) + "{% endblock %}");
                    viewWriter.WriteLine("{% block body %}");
                    viewWriter.WriteLine("<h1><b>" + Function.Util.FormatName(name.Replace("_", "#")) + " - Delete</b></h1>");
                    viewWriter.WriteLine("{% endblock %}");
                    viewWriter.Dispose();
                    Program._colorify.WriteLine("View successfully created!", Colors.bgSuccess);
                    Program._colorify.WriteLine(viewsDir + "delete.php", Colors.bgMuted);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + @"/delete.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(viewsDir + "delete.php", Colors.bgMuted);
                }
            }
            else
            {
                if (!File.Exists(viewsDir + "index.php"))
                {
                    var viewFile = File.Create(viewsDir + "index.php");
                    var viewWriter = new StreamWriter(viewFile);
                    viewWriter.WriteLine("{% extends \"templates/default/base.php\" %}");
                    viewWriter.WriteLine("{% block title %}" + Function.Util.FormatName(name.Replace("_", "#")) + "{% endblock %}");
                    viewWriter.WriteLine("{% block body %}");
                    viewWriter.WriteLine("<h1>Welcome Page <b>" + Function.Util.FormatName(name.Replace("_", "#")) + "</b></h1>");
                    viewWriter.WriteLine("{% endblock %}");
                    viewWriter.Dispose();
                    Program._colorify.WriteLine("View successfully created!", Colors.bgSuccess);
                    Program._colorify.WriteLine(viewsDir + "index.php", Colors.bgMuted);
                }
                else
                {
                    Program._colorify.WriteLine(Function.Util.FormatName(name) + @"/index.php already exists and so was not generated.", Colors.bgDanger);
                    Program._colorify.WriteLine(viewsDir + "index.php", Colors.bgMuted);
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
                    if (string.IsNullOrEmpty(driver))
                    {
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
