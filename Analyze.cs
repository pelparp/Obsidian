using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Obsidian.Program;
using System.Text.RegularExpressions;

namespace Obsidian
{
    class Analyze
    {
        public static void Process(Options opts)
        {
            //Read the JSE into MalText
            string MalText = System.IO.File.ReadAllText(opts.InFile);
            
            //Normalize the malware file by removing spaces, tabs, new lines and tabs. Makes RegEx easy
            MalText = MalText.Replace(" ", "").Replace("\r\n", "").Replace("\n", "").Replace("\t", "");

            //Defining regexes for index0 and index1
            string index0 = @"\[this\[[A-Za-z0-9\']{1,40}\]\]=[0-9]{1,4}";
            string index1 = @"\(this\[[A-Za-z0-9\'\-\/\)\]\[]{1,40}\]=[0-9]{1,4}";
            _log.Info("Using the RegEx patterns {0} {1}", index0, index1);

            //recover elements that match index0 and index1 regexes and store them in two distinct int arrays 
            int[] index0_elems = RecoverIndex(MalText, index0);
            int[] index1_elems = RecoverIndex(MalText, index1);
            _log.Info("Index elements have been recovered");

            //recover text by subtracting corresponding elements in index0 and index1 
            RecoverText(index0_elems, index1_elems,opts);
        }
        private static int[] RecoverIndex(string text, string expr)
        {
            //store all matches in an int array and return the final array
            var arr = Regex.Matches(text, expr)
                .Cast<Match>()
                .Select(m => Int32.Parse(m.ToString().Split('=').Last()))
                .ToArray();
            return arr;
        }
        private static void RecoverText(int[] index0, int[] index1, Options options)
        {
            //check if index0 and index1 lengths are the same. If they don't, we have failed in correctly extracting index elements.  
            if (index0.Length == index1.Length)
            {
                //if lengths of index0 and index1 match up then create a new string array with the same length to store your result
                string[] result = new string[index0.Length];
                for (int i = 0; i < index0.Length; i++)
                {
                    int res = index1[i] - index0[i];
                    result[i] += Convert.ToChar(res).ToString();
                }
                //concat your string array and have fun
                Console.WriteLine(Environment.NewLine +"RECOVERED STRING: " + string.Concat(result).ToString());
                if (options.OutDir != null)
                {
                    File.WriteAllText(options.OutDir.Trim('\\') + "\\Obsidian_recovered_text_" + DateTime.Now.Ticks.ToString() + ".txt", string.Concat(result).ToString());
                }
            }
            else
            {
                _log.Error("Index lengths do not match up. Regexes may need to be modified in the source.");
            }
        }
    }
}
