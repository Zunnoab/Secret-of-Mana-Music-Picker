using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAppPathTool
{
    public static class SteamAppPath
    {       
        //An attempt to make this not break if "\" is not the directory separator.
        static string DirectorySeparator;
        static string DoubleDirectorySeparator;

        static SteamAppPath()
        {
            DirectorySeparator = Path.DirectorySeparatorChar.ToString();
            DoubleDirectorySeparator = DirectorySeparator + DirectorySeparator;
        }

        //Get the location of the installation
        public static string GetSteamAppPath(string SteamGameID)
        {
            foreach (string LibraryLocationPath in GetSteamLibraryLocations())
            {
                string CurrentLibrarySearchResult = SearchSteamLibraryForAppPath(SteamGameID, LibraryLocationPath);
                if (!(CurrentLibrarySearchResult == "Not Found"))
                {
                    return CurrentLibrarySearchResult;
                }
            }
            return "Not Found";
        }

        //Gets the file path to a game's installation if found
        public static string SearchSteamLibraryForAppPath(string SteamGameID, string LibraryPath)
        {
            string ManifestFilePath = LibraryPath + DirectorySeparator + "SteamApps" + DirectorySeparator + "appmanifest_" + SteamGameID + ".acf";
            if (File.Exists(ManifestFilePath))
            {
                //This double "using" situation is to prevent locking write access from Steam.
                using (FileStream fs = new FileStream(ManifestFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string CurrentLine = sr.ReadLine();
                        if (CurrentLine.Length > 17 && (CurrentLine.Substring(0, 16) == "\t\"installdir\"\t\t\""))
                        {
                            string GameDirectoryName = CurrentLine.Substring(16, CurrentLine.Length - 17);
                            return LibraryPath + DirectorySeparator + "common" + DirectorySeparator + GameDirectoryName;
                        }
                    }
                }
            }
            return "Not Found";
        }

        //Attempt to read a list of all known Steam Library paths from the "libraryfolders.vdf" file.
        public static List<string> GetSteamLibraryLocations()
        {
            List<string> FoundLibraryLocations = new List<string>();

            //Getting the path to Steam from the registry
            string SteamLibraryListPath = GetSteamPath();

            //Aborting if it's not found
            if (SteamLibraryListPath == "Not Found")
            {
                return FoundLibraryLocations;
            }

            //Finishing library list path
            SteamLibraryListPath = SteamLibraryListPath + DirectorySeparator +"SteamApps";

            //Listing the default Steam library path
            FoundLibraryLocations.Add(SteamLibraryListPath);

            //Listing additional library locations read from the libraryfolders.vdf file.
            if (File.Exists(SteamLibraryListPath + DirectorySeparator + "libraryfolders.vdf"))
            {
                //This double "using" situation is to prevent locking write access from Steam.
                using (FileStream fs = new FileStream(SteamLibraryListPath + DirectorySeparator + "libraryfolders.vdf", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fs))
                {
                    //Examining each line
                    while (!sr.EndOfStream)
                    {
                        //Tab"#"TabTab"Path" <- the format Steam uses for the lines containing paths.
                        //As such a line must be at least 12 long to contain anything parseable.
                        string CurrentLine;
                        CurrentLine = sr.ReadLine();
                        if ((CurrentLine.Length > 12 && CurrentLine.Substring(0, 2) == "\t\"") && CurrentLine.Substring(3, 4) == "\"\t\t\"")
                        {
                            //If the pattern matches, extracting the path and changing double backslashes to single.
                            CurrentLine = CurrentLine.Substring(7, CurrentLine.Length - 8).Replace(DoubleDirectorySeparator, DirectorySeparator);
                            FoundLibraryLocations.Add(CurrentLine);
                        }
                    }
                }
            }
            return FoundLibraryLocations;
        }

        //Attempt to retrieve the Steam installation path
        //Steam should automatically generate this if it's not there when it's run.
        public static string GetSteamPath()
        {
            return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Valve\Steam", "SteamPath", "Not Found").ToString().Replace(@"/", DirectorySeparator);
        }

    }
}
