/*Name: Charles E. Phyle III
 * Date: 5/22/2022
 */

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Module2
{
    class Program
    {


        public static void Main(string args)
        {
            

        
                var cwd = System.IO.Directory.GetCurrentDirectory();
                if (Directory.GetFiles(cwd).Contains("Module2.txt"))
                {
                    //skip into the program
                }
                Console.WriteLine("Where is the folder that contains the file you would like to analyize? (EX: C:\\Users\\user\\folder) !NOTE: Currently only works with .txt files!");//Ask the User for where the file is stored
                var newDirectory = Console.ReadLine();
                newDirectory = args;
                bool canNavigate = false;
                while (!canNavigate)
                {
                    try
                    {
                        ChangeCwd(newDirectory);
                        canNavigate = true;

                    }
                    catch (DirectoryNotFoundException)
                    {
                        Console.WriteLine("Please check the address and try again!");
                        newDirectory = Console.ReadLine();
                    }

                }

                //Write a for loop to make sure they are in the directory they wanted to use
                Console.WriteLine($"You are in {newDirectory}");
                int t = 0;
                string[] vs = Directory.GetFiles(newDirectory);  //Grabs all file names from the CWD and stores them

                foreach (string i in vs)    //Cleans up the file names to be just the file names  !!make it a new class
                {
                    string temp = vs[t];
                    if (temp.Contains(newDirectory))
                    {
                        string[] vs1 = temp.Split($"{newDirectory}\\");
                        vs[t] = vs1[1];
                    }
                    t++;
                }
                int fileNumber = 1;
                foreach (var file in vs)
                {
                    Console.WriteLine($"{fileNumber}: {file}");
                    fileNumber++;
                }

                //Ask user what file they would like to see statistics for
                Console.WriteLine("Which file number would you like to see statistics for?");
                int userFileChoice = Convert.ToInt32(Console.ReadLine()) - 1;

                string wholeDocument = File.ReadAllText($"{newDirectory}\\{vs[userFileChoice]}");

                Console.WriteLine($"You chose: {newDirectory}\\{vs[userFileChoice]}");



                string[] vs2 = wholeDocument.Split(" "); //Split the document on all spaces

                vs2 = FileCleaner(",", vs2);
                vs2 = FileCleaner(".", vs2);
                vs2 = FileCleaner("—", vs2);
                vs2 = FileCleaner("-", vs2);
                vs2 = FileCleaner(@"""", vs2);
                vs2 = FileCleaner("\"", vs2);
                vs2 = FileCleaner(";", vs2);
                vs2 = FileCleaner("!", vs2);
                vs2 = FileCleaner("`", vs2);
                vs2 = FileCleaner("?", vs2);
                vs2 = FileCleaner("`", vs2);
                vs2 = FileCleaner("“", vs2);
                vs2 = FileCleaner("”", vs2);
                vs2 = FileCleaner("‘", vs2);
                vs2 = FileCleaner(":", vs2);
                vs2 = FileCleaner(@"\r", vs2);
                vs2 = FileCleaner(@"\n", vs2);
                vs2 = FileCleaner(@"\t", vs2);



                vs2 = vs2.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray(); //Finnaly this fixed the array mismatch
                vs2 = vs2.Select(x => x.ToLowerInvariant()).ToArray();  //drop all the strings to lower case to count all variations of the same word.


                var numberOfWords = vs2.Length;

                var query = vs2.GroupBy(r => r).Where(r => (r != null)).Select(grp => new
                {
                    Value = grp.Key,
                    Count = grp.Count()

                });


                var x = 0;
                string[] word = new string[numberOfWords];
                int[] popularity = new int[numberOfWords];
                foreach (var i in query)                             //used to sort the .txt file into unique words and count the occurances of them
                {
                    word[x] = i.Value;
                    popularity[x] = i.Count;
                    x++;
                }

                word = word.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                popularity = popularity.Where(x => (x != 0)).ToArray();     //no need to waste ram


                Array.Sort(popularity, word);
                Console.WriteLine("Would you like to save a copy of the results? (Y/N)");
                var userChoice = Console.ReadLine();
                if (userChoice == "Y" || userChoice == "Yes")
                {
                    using (FileStream fs = File.Create($"{newDirectory}\\testdoc.txt"))
                    {

                        for (int i = 0; i < word.Length; i++)
                        {
                            AddText(fs, $"{word[i]}\t\t {popularity[i]} \n");
                        }
                        try
                        {
                            string[] vs1 = Directory.GetFiles($"{newDirectory}");
                            if (vs1.Contains($"{newDirectory}\\testdoc.txt"))
                            {
                                Console.WriteLine("The file is now saved!");
                            };
                        }
                        finally
                        {
                            Console.WriteLine("Sorry the application could not save the file please try again");
                        }

                    }
                }
                DisplayArray(word, popularity);
            }

        
        static string[] FileCleaner(string stringToRemove, string[] arrayThatHasIt)
        {
            string[] result;
            int x = 0;
            foreach (var i in arrayThatHasIt)
            {
                string temp = arrayThatHasIt[x];
                if (temp.Contains(stringToRemove))
                {
                    string[] vs3 = temp.Split(stringToRemove);

                    if (vs3[0] == stringToRemove)
                    {
                        arrayThatHasIt[x] = vs3[1];
                    }
                    else
                    {
                        arrayThatHasIt[x] = vs3[0];
                    }
                }
                x++;
            }
            result = arrayThatHasIt;
            return result;
        }
        static void DisplayArray(Object[] arr1, int[] arr2)
        {
            for (int i = 0; i < arr1.Length; i++)
            {
                Console.WriteLine($"{arr1[i]} {arr2[i]}");
            }
        }
        static void ChangeCwd(string newCwd)
        {
            System.IO.Directory.SetCurrentDirectory(newCwd);
            var NewCwd = System.IO.Directory.GetCurrentDirectory();
        }
        private static void AddText(FileStream fs, string value) //used to write results to a new txt file in same directory
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }

}
