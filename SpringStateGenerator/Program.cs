using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringStateGenerator
{
    class Program
    {
        const string HEADER = "using System;\n" +
                "using System.Collections.Generic;\n" +
                "\n" +
                "[Serializable]\n" +
                "public class {0} : SceneState\n" +
                "{{\n";

        const string FOOTER = "}";

        static List<Property> properties;
        static string className;

        static void Main(string[] args)
        {
            if (args.Length == 0)
                throw new Exception("Generator needs at least one argument - path to the source file");
            var fileName = args[0];
            if (fileName.EndsWith(".scs"))
            {
                fileName = fileName.Substring(0, fileName.Length - 4);
            }
            var reader = new StreamReader(fileName + ".scs");

            var outputFile = fileName;
            if (args.Length >= 2)
                outputFile = args[1];
            // var writer = Console.Out;
            var writer = new StreamWriter(outputFile+".cs");

            className = reader.ReadLine();

            writer.WriteLine(String.Format(HEADER, className));

            properties = new List<Property>();
            int lineNum = 2;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                properties.Add(Property.FromLine(line,lineNum));
                lineNum++;
            }
            properties.Sort(new PropertyComparer());

            // properties definitions
            foreach(var property in properties)
            {
                writer.WriteLine(property.DefinitionLine());
            }

            writer.WriteLine();
            // empty constructor - for deserialization
            writer.WriteLine("\t// empty constructor - for deserialization");
            writer.WriteLine(String.Format("\tpublic {0}() {{}}\n", className));

            // initial constructor - default values
            writer.WriteLine("\t// initial constructor - default values");
            writer.WriteLine(String.Format("\tpublic {0}(bool initial) {{", className));
            foreach (var property in properties)
            {
                if (property.HasDefaultValue)
                    writer.WriteLine(property.DefaultValueLine());
            }
            writer.WriteLine("\t}");

            writer.WriteLine();
            // copy constructor
            writer.WriteLine("\t// copy constructor");
            string templateArgName = "template";
            writer.WriteLine(String.Format("\tprivate {0}({0} {1}) {{", className, templateArgName));
            foreach (var property in properties)
            {
                writer.WriteLine(property.CopyLine(templateArgName));
            }
            writer.WriteLine("\t\tSetCharacterPosition();");
            writer.WriteLine("\t}");

            // set methods 
            writer.WriteLine();
            foreach (var property in properties)
            {
                writer.WriteLine(property.SetMethodLine(className));
                writer.WriteLine();
            }

            writer.WriteLine();
            // compare method 
            writer.WriteLine("\t// compare method");
            string otherStateArgName = "other";
            string resultListName = "result";
            writer.WriteLine(
                String.Format(
                    "\tpublic List<string> CompareChanges({0} {1}) {{\n" +
                    "\t\tvar {2} = new List<string>();\n",
                    className, otherStateArgName, resultListName));
            foreach (var property in properties)
            {
                writer.WriteLine(property.CompareLine(otherStateArgName,resultListName));
                writer.WriteLine();
            }
            writer.WriteLine(String.Format("\t\treturn {0};\n\t}}",resultListName));

            writer.WriteLine("}");


            writer.Flush();
        }
    }
}
