using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AnkiVocabBuilder
{
    internal class AnkiVocabBuilder
    {
        public string[] Noun(string s ,StreamWriter vystup)
        {
            string[] suffixesM = { "ant", "ast", "ich", "ig", "ismus","ling", "or", "us"};
            string[] suffixesF = { "a", "anz", "enz", "ei", "ie", "in", "heit", "keit", "ik", "sion", "tion", "sis", "tät", "ung", "ur", "schaft" };
            string[] suffixesN = { "chen", "lein", "icht", "il", "it", "ma", "ment", "tum", "um"};

            string[] lessProbableSuffixesM = { "el", "er", "en" };
            string[] lessProbableSuffixF = { "e" };
            string[] lessProbableSuffixN = { "nis", "sal" };

            string[] lessProbableSuffixNForeign = { "al", "an", "ar", "är", "at", "ent", "ett", "ier", "iv", "o", "on" };

            bool isMasculine = CheckSuffix(suffixesM, s);
            bool isFeminine = CheckSuffix(suffixesF, s);
            bool isNeuter = CheckSuffix(suffixesN, s);

            bool isProbablyMasculine = CheckSuffix(lessProbableSuffixesM, s);
            bool isProbablyFeminine = CheckSuffix(lessProbableSuffixF, s);
            bool isProbablyNeuter = CheckSuffix(lessProbableSuffixN, s);
            bool isProbablyNeuterForeign = CheckSuffix(lessProbableSuffixNForeign, s);

            bool prefixN = s.StartsWith("Ge");

            string gender = DetermineGender(isMasculine, isFeminine, isNeuter, isProbablyMasculine, isProbablyFeminine, isProbablyNeuter, isProbablyNeuterForeign, prefixN);

            if (gender == string.Empty)
            {
                gender = "?";
            }

            string pluralforrm = PluralForm(gender, s);

            string[] output = new string[4];
            output[0] = "Podstatne jmeno";
            output[1] = gender + " " + s;
            output[2] = pluralforrm;
            output[3] = "#################";
            
            return output;
        }
        public bool CheckSuffix(string[] s, string input)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (input.EndsWith(s[i]))
                {
                    return true;
                }
            }
            return false;
        }


        public string DetermineGender(bool masculine, bool feminine, bool neuter, bool likelyM, bool likelyF, bool likelyN, bool likelyNForeign, bool likelyNPrefix)
        {
            if (masculine)
            {
                return "der";
            }
            else if (feminine)
            {
                return "die";
            }
            else if (neuter) 
            {
                return "das";
            }
            else if (likelyM)
            {
                return "der?";
            }
            else if (likelyF)
            {
                return "die?";
            }
            else if (likelyN || likelyNForeign || likelyNPrefix)
            {
                return "das?";
            }
            return "?"; 
        }
        public string PluralForm(string gender, string input)
        {
            bool has_suffix = false;
            string mnozneCislo = "die " + input;

            string[] en_masculine = { "ent", "and", "ant", "ist", "or" };
            string[] en_feminine = { "ion", "ik", "heit", "keit", "schaft", "tät", "ung" };
            string[] en_foreign = { "ma", "um", "us" };
            string[] e_masculine = { "eur", "ich", "ier", "ig", "ling", "ör" };
            string[] s_suffixes = {"a", "i", "o", "u", "y"};
            string[] no_masculine = {"el", "en", "er"};
            string[] no_neuter = {"chen", "lein"};

            if (gender == "?")
            {
                return "VYHLEDAT";
            }

            if (gender == "der" || gender == "der?")
            {
                if (input.EndsWith("e"))
                {
                    return mnozneCislo + "n";
                }

                has_suffix = ContainsSuffix(en_masculine, input);
                if (has_suffix)
                {
                    return mnozneCislo + "en";
                }

                has_suffix = ContainsSuffix(e_masculine, input);
                if (has_suffix)
                {
                    return mnozneCislo + "e";
                }

                has_suffix = ContainsSuffix(no_masculine, input);
                if (has_suffix)
                {
                    return mnozneCislo;
                }
            }
            else if (gender == "die" || gender == "die?")
            {
                if (input.EndsWith("e"))
                {
                    return mnozneCislo + "n";
                }
                else if (input.EndsWith("in"))
                {
                    return mnozneCislo + "nen";
                }

                has_suffix = ContainsSuffix(en_feminine, input);

                if (has_suffix)
                {
                    return mnozneCislo + "en";
                }
            }
            else if (gender == "das" || gender == "das?")
            {
                has_suffix = ContainsSuffix(no_neuter, input);
                if (has_suffix)
                {
                    return mnozneCislo;
                }
            }

            has_suffix = ContainsSuffix(s_suffixes, input);

            if (has_suffix)
            {
                return mnozneCislo + "s";
            }
            
            has_suffix = ContainsSuffix(en_foreign, input);

            if (has_suffix)
            {
                return mnozneCislo.Remove(input.Length - 2) + "en?";
            }

            return mnozneCislo + "-(vyhledat)";
        }
        public bool ContainsSuffix(string[] array, string input) 
        {
            for (int i = 0; i < array.Length;i++)
            {
                if (input.EndsWith(array[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
