using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Essential
{
    public class ConsoleWriter : TextWriter
    {
        private List<string> lines = new List<string>();

        private TextWriter original;
        public ConsoleWriter(TextWriter original)
        {
            this.original = original;
        }
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
        public override void WriteLine(string value)
        {
            lines.Add(value);
            original.WriteLine(value);
        }
        public void ClearIt()
        {
            lines.Clear();
        }
        public override void Write(string value)
        {
            lines.Add(value);
            original.Write(value);
        }
        //You need to override other methods also

        public string[] GetLines()
        {
            return lines.ToArray();
        }
        public List<string> GetList()
        {
            return lines;
        }
        public override string ToString()
        {
            string final = "";
            foreach (string line in lines)
            {
                final = final + NewLine + line;
            }
            return final;
        }
    }
}
