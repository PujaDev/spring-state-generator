using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringStateGenerator
{
    class Property
    {
        readonly public string Type, Name, DefaultValue, Comment;
        readonly public bool HasDefaultValue;

        public Property(string Type, string Name, string DefaultValue = null, string Comment = null)
        {
            this.Type = Type;
            this.Name = Name;
            this.DefaultValue = DefaultValue;
            this.Comment = Comment;
            HasDefaultValue = DefaultValue != null;
        }

        public string DefinitionLine()
        {
            string comment = "";
            if (Comment != null)
            {
                comment = String.Format("\t/// <summary>\n"+
                                        "\t/// {0}\n" +
                                        "\t/// </summary>\n",
                                        Comment);
            }
            return String.Format("{2}\tpublic {0} {1} {{ get; private set; }}",
                Type, Name,comment);
        }

        public string DefaultValueLine()
        {
            return String.Format("\t\t{0} = {1};", Name, DefaultValue);
        }

        public string CopyLine(string TemplateArgName)
        {
            return String.Format("\t\t{0} = {1}.{0};", Name, TemplateArgName);
        }

        public string SetMethodLine(string ClassName)
        {
            return String.Format(
                "\tpublic {1} Set{0}({2} value)\n" +
                "\t{{\n" +
                "\t\tvar copy = new {1}(this);\n" +
                "\t\tcopy.{0} = value;\n" +
                "\t\treturn copy;\n" +
                "\t}}", Name, ClassName, Type);
        }

        public string CompareLine(string OtherStateName, string ResultListName)
        {
            return String.Format(
                "\t\tif(!{0}.Equals({1}.{0}))\n" +
                "\t\t\t{2}.Add(String.Format(\"{0}:\\t{{0}}\\t>>>\\t{{1}}\",{1}.{0},{0}));",
                Name, OtherStateName, ResultListName);
        }

        static public Property FromLine(string line, int lineNum)
        {
            var args = line.Split(';');
            var type = args[0];
            var name = args[1];
            string defaultValue = null;
            if (args.Length > 2 && args[2].Length > 0)
                defaultValue = args[2];
            string comment = null;
            if (args.Length > 3 && args[3].Length > 0)
                comment = args[3];

            return new Property(type, name, defaultValue, comment);
        }
    }
}
