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
                throw new Exception("Generator needs at least one argument - path to source file");

            var reader = new StreamReader(args[0] + ".scs");

            var outputFile = args[0];
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

            // properties definitions
            foreach(var property in properties)
            {
                writer.WriteLine(property.DefinitionLine());
            }

            // empty constructor - default values
            writer.WriteLine();
            writer.WriteLine(String.Format("\tpublic {0}() {{", className));
            foreach (var property in properties)
            {
                if(property.HasDefaultValue)
                    writer.WriteLine(property.DefaultValueLine());
            }
            writer.WriteLine("\t}");

            // copy constructor
            writer.WriteLine();
            string templateArgName = "template";
            writer.WriteLine(String.Format("\tprivate {0}({0} {1}) {{", className, templateArgName));
            foreach (var property in properties)
            {
                writer.WriteLine(property.CopyLine(templateArgName));
            }
            writer.WriteLine("\t}");

            // set methods 
            writer.WriteLine();
            foreach (var property in properties)
            {
                writer.WriteLine(property.SetMethodLine(className));
                writer.WriteLine();
            }

            // compare method 
            writer.WriteLine();
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
