using System.IO;

namespace AnkiVocabBuilder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader vstup = new StreamReader("vstupDeutsch.txt"))
            {
                using (StreamWriter vystup = new StreamWriter("vystup.txt"))
                {
                    var nounApp = new AnkiVocabBuilder();
                    var lineCount = File.ReadLines(@"C:\Users\matej\source\repos\AnkiVocabBuilder\AnkiVocabBuilder\bin\Debug\net7.0\vstupDeutsch.txt").Count();


                    for (int i = 0; i < lineCount; i++)
                    {

                        string input = vstup.ReadLine();

                        if (input != input.ToLower())
                        {
                            string[] output = nounApp.Noun(input, vystup);

                            for (int j = 0; j < 4; j++)
                            {
                                vystup.WriteLine(output[j]);
                            }

                        }
                    }
                }
            }
        }
    }
}