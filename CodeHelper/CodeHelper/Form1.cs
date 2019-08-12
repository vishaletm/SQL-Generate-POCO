using CodeHelperLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void GenerateFromSQL()
        {
            try
            {
                SQLHelper sqlhelper = new SQLHelper();
                var obj = sqlhelper.Generate(txtConnection.Text, txtQuery.Text, txtName.Text);
                var name = txtName.Text;
                var VM = $"{ name}View";
                var IVM = $"I{ name}View";
                var dto = $"{ name}Dto";
                var mapper = $"{VM}Mapper";
                var path = $"D:\\generated\\{name}";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(Path.Combine(path, VM + ".cs")))
                {
                    File.Delete(Path.Combine(path, VM + ".cs"));
                }
                using (StreamWriter sw = File.CreateText(Path.Combine(path, VM + ".cs")))
                {
                    sw.WriteLine(VM);
                    foreach (var ob in obj)
                    {
                        sw.WriteLine($"public {gettype(ob.Type,ob.Nullable)} {ob.Column_name}" + "{get;set;} ");
                    }
                }
                if (File.Exists(Path.Combine(path, IVM + ".cs")))
                {
                    File.Delete(Path.Combine(path, IVM + ".cs"));
                }
                using (StreamWriter sw = File.CreateText(Path.Combine(path, IVM + ".cs")))
                {
                    sw.WriteLine(IVM);
                    foreach (var ob in obj)
                    {
                        sw.WriteLine($" {gettype(ob.Type,ob.Nullable)} {ob.Column_name}" + "{get;set;} ");
                    }
                }


                if (File.Exists(Path.Combine(path, VM + ".sql")))
                {
                    File.Delete(Path.Combine(path, VM + ".sql"));
                }
                using (StreamWriter sw = File.CreateText(Path.Combine(path, VM + ".sql")))
                {
                    sw.WriteLine(IVM);
                    foreach (var ob in obj)
                    {
                        sw.Write(txtQuery.Text);
                    }
                }


                if (File.Exists(Path.Combine(path, dto + ".cs")))
                {
                    File.Delete(Path.Combine(path, dto + ".cs"));
                }
                using (StreamWriter sw = File.CreateText(Path.Combine(path, dto + ".cs")))
                {
                    sw.WriteLine(dto);
                    foreach (var ob in obj)
                    {
                        sw.WriteLine($"public {gettype(ob.Type,ob.Nullable)} {ob.Column_name}" + "{get;set;} ");
                    }
                }

                if (File.Exists(Path.Combine(path, mapper + ".cs")))
                {
                    File.Delete(Path.Combine(path, mapper + ".cs"));
                }
                using (StreamWriter sw = File.CreateText(Path.Combine(path, mapper + ".cs")))
                {
                    sw.WriteLine(mapper);
                    var toObject = new StringBuilder();
                    var toEntity = new StringBuilder();
                    toObject.AppendLine($"var dto=new {dto}()");
                    sw.WriteLine($"ObjectMapper<{IVM}, {dto}>");
                    sw.WriteLine($"");

                    foreach (var ob in obj)
                    {
                        toObject.AppendLine($"dto.{ob.Column_name}=entity.{ob.Column_name};");
                        toEntity.AppendLine($"entity.{ob.Column_name}=dto.{ob.Column_name};");
                    }
                    sw.WriteLine(toObject.ToString());
                    sw.WriteLine($"//************************");
                    sw.WriteLine($"");
                    sw.WriteLine(toEntity.ToString());
                }

           }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public string gettype(string type,string Nullable)
        {
            var nullchar = "";
            if (Nullable.ToLower().Equals("yes"))
                nullchar = "?";
            switch (type)
            {
                case "decimal":
                    return $"decimal{nullchar}";
                case "bigint":
                    return $"long{nullchar}";
                case "int":
                    return $"int{nullchar}";
                case "nvarchar":
                    return "string";
                case "varchar":
                    return "string";
                case "nchar":
                    return "string";
                case "datetime":
                    return $"DateTime{nullchar}";
                case "bit":
                    return $"bool{nullchar}";
                case "char":
                    return "string";
                case "tinyint":
                    return $"byte{nullchar}";                
                case "money":
                    return $"decimal{nullchar}";
                case "date":
                    return $"DateTime{nullchar}";
                case "smallint":
                    return $"int{nullchar}";
                case "uniqueidentifier":
                    return $"Guid{nullchar}";
                case "float":
                    return $"float{nullchar}";
                default:
                    return "unknown";


            }
        }
        public string GetNullable(string Nullable)
        {
            if (Nullable.ToLower().Equals("yes"))
                return "?";
            else
                return "";
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateFromSQL();
        }
    }
}
