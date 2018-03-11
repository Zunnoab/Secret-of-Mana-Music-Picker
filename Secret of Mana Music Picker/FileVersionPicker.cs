using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD5Gen;

namespace Secret_of_Mana_Music_Picker
{
    //The primary tool.  It will contain very sparse error checking so it must be set up properly to use.
    class FileVersionPicker
    {
        //Where to store copies of the files and the settings
        public string StoragePath { get; set; }

        //Where the root dictory of the choice files are
        public string ParentRoot { get; set; }

        //Where to write the output from the choices made
        //******** THIS IS USED TO OVERWRITE FILES ********
        string OutputPath { get; set; }

        bool FilesCopied = false;
        public List<ChoiceData> Choices;

        public List<string> PullInSources()
        {
            List<string> OutputAnomalies = new List<string>();

            foreach (ChoiceData CurrentChoice in Choices)
            {
                foreach (CopyAndNameInfo ChoiceFile in CurrentChoice.OptionsList)
                {
                    ChoiceFile.CopyFileFromSource(ParentRoot, StoragePath);
                }
            }

            FilesCopied = true;
            return OutputAnomalies;
        }

        //Quick access to the copied MD5 tool.
        string GetMD5(string strInputPath)
        {
            return MD5Generator.GetMD5(strInputPath);
        }

        //Object type for holding the data required to present choices and allow the movement of the proper files.
        public class ChoiceData
        {
            string CurrentChoice;
            string OutputFileName;
            public List<CopyAndNameInfo> OptionsList { get; set; }
            public List<string> ColumnData { get; set; }
        }
        public class CopyAndNameInfo
        {
            public string CopyName { get; set; }
            public string CopyFileName { get; set; }
            public string RelativeSourcePath { get; set; }
            public string MD5 { get; set; }

            public bool CopyFileFromSource(string InputRootPath, string OutputPath)
            {
                try
                {
                    System.IO.File.Copy(Path.Combine(InputRootPath, RelativeSourcePath), Path.Combine(OutputPath, CopyFileName));
                    return true;
                }
                catch
                {
                    //I know this is sloppy, but I'm just trying to get this to work.
                    return false;
                }
            }
        }

    }
}
