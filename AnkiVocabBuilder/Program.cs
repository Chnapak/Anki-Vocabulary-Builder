using System.IO;

namespace AnkiVocabBuilder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader vstup = new StreamReader("vstupDeutsch.txt")) // Opening input of German nouns
            { 
                using (StreamWriter vystup = new StreamWriter("vystup.txt")) // Creating a file for output
                {
                    var nounApp = new AnkiVocabBuilder(); 
                    var lineCount = File.ReadLines("vstupDeutsch.txt").Count(); // i.e. Number of nouns


                    for (int i = 0; i < lineCount; i++) // Iterating through each noun
                    {
                        string lineInput = vstup.ReadLine();

                        // vvvvvvvv   This checks if input is a noun, german noun are always capitalized. 
                        if (lineInput != lineInput.ToLower())
                        {

                            /* Four lines of output per noun:
                               type of word (Noun)
                               singular form with article 
                               plural form with article
                               horizontal break e.g. (----------)
                             */

                            // Note all of these are meant to be guessed, vocabulary still needs to be reviewed.
                            string[] output = nounApp.Noun(lineInput, vystup); 
                            
                            for (int j = 0; j < 4; j++)
                            {
                                vystup.WriteLine(output[j]);    // Writing the noun into the output.
                            }

                        }
                    }
                }
            }
        }
    }
}