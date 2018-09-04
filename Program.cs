using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConfigReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            // Hang console
        }
    }

    static public class SettingsTool
    {
        static public string GetSetting(string value, string path, int indexBuff = 0)
        /*
         Get settings takes a setting path, the setting to get, and and index buff, the
         indexbuff skips lines when reading from the file
         */
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                // Use a using statement to dispose of streamreader after use (Opens streamreader with path)
                {
                    for (int x = 0; x < indexBuff; x++)
                    // Read the line 'indexBuff' times
                    {
                        sr.ReadLine();
                    }

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    // While sr.ReadLine is not null (iterate through all lines) and set line = sr.ReadLine
                    {
                        if (line.StartsWith($"{value}=", StringComparison.CurrentCultureIgnoreCase))
                        // if the line starts with the parameter plus an equals sign (to find right setting)
                        {
                            return line.Substring(value.Length + 1);
                            // return the last part of the line (substring the chosen setting's length plus one (for equals sign)
                        }
                    }
                    throw new Exception("Setting not found");
                    // Code should be unreachable if the line is found, if setting not found throw an error
                }
            } catch (Exception)
            // Catch statement makes code more manageable
            {
                throw;
            }
        }

        static public void ChangeSetting(string setting, string path, string value, int indexBuff = 0)
        /*
        ChangeSetting functions takes the setting, file path, the new setting value and indexBuff
        This function changes a setting in the file
        */
        {
            try
            {
                string[] lines = File.ReadAllLines(path);
                // Store all lines of the file into an array of strings
                for (int x = indexBuff; x < lines.Length; x++)
                // Iterate through every line of the file excluding 'indexBuff' lines at the start
                {
                    if (lines[x].StartsWith($"{setting}="))
                    // If the line starts with our setting plus an equals sign
                    {
                        lines[x] = $"{setting}={value}";
                        // Set the current iteration index of lines equal to the setting plus the new value
                    }
                }
                File.WriteAllLines(path, lines);
                // Write all items in the array lines to the new file, replacing the old file completely
            } catch (Exception)
            {
                throw;
            }
        }

        static public void AddSetting(string setting, string path, string value, int indexBuff = 0)
        /*
        AddSetting functions takes the setting, file path and the new setting along with the indexBuff again
        This functions adds a new setting to the file
         */
        {
            try
            {
                List<string> lines = File.ReadAllLines(path).ToList();
                // Declare a List of strings equal to an array of lines from file 'path' casted to a list.
                bool settingFound = false;
                /*
                This bool is used so we know if we succeeded in finding an existing setting value
                so we can replace instead of add multiple instances of one setting
                */

                for (int x = indexBuff; x < lines.Count; x++)
                // Iterate through every line exluding the starting 'indexBuff' lines
                {
                    if (lines[x].StartsWith($"{setting}="))
                    // If the current line is equal to the setting
                    {
                        lines[x] = $"{setting}={value}";
                        // Replace the existing setting
                        settingFound = true;
                        // Declare we've succeeded in replacing an existing setting
                    }
                }

                if (settingFound) {
                    File.WriteAllLines(path, lines);
                    // Write the list to the new file with the path 'path'
                } else
                // Setting was not found, append to list instead
                {
                    lines.Add($"{setting}={value}");
                    // Apopends to list with setting and new value
                    File.WriteAllLines(path, lines);
                    // Write the array to the new file with the path 'path'
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        static public List<string> GetAllSettings(string path, int indexBuff = 0)
        /*
        GetAllSettings returns a list of strings, it takes a file path and indexBuff 
        This functions gets all the settings from a selected file 
         */
        {
            try
            {
                List<string> settings = new List<string>();
                // Declare a list of strings called 'settings'

                foreach(string line in File.ReadAllLines(path))
                // Iterate through every line in the selected file
                {
                    if (line.Contains('='))
                    // This checks if the line is valid and has an equals operator
                    {
                        settings.Add(line);
                        // Add the line to the list to be returned later
                    }
                }
                return settings;
                // return our list of settings
            } catch
            {
                throw;
            }
        }

        static public void DeleteAllSettings(string path)
        // This functions deletes all settings from a file, it takes only a file path
        {
            try
            {
                string[] lines = File.ReadAllLines(path);
                // Declare an array of strings equal to every line in the selected file
                for (int x = 0; x < lines.Length; x++)
                // Iterate through every line in the array
                {
                    if (lines[x].Contains('='))
                    // If our line is a valid setting line with the correct equals operator
                    {
                        lines[x] = lines[x].Substring(0, lines[x].IndexOf('='));
                        // Remove the end of the line after the equals operator, leaving the setting null
                    }
                }

                File.WriteAllLines(path, lines);
                // Write array to file
            } catch
            {
                throw;
            }
        }
    }
}
