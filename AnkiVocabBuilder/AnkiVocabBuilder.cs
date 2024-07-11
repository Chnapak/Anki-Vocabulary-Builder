using System.IO;

namespace AnkiVocabBuilder
{
    internal class AnkiVocabBuilder
    {
        // s is the word tested
        public string[] Noun(string s ,StreamWriter vystup)
        {
            /* M - Masculine 
               F - Feminine 
               N - Neuter */

            s = s.Trim(); // To remove space after word

            // Array of suffixes if a word has belongs to a exact gender.
            string[] suffixesM = { "ant", "ast", "ich", "ig", "ismus","ling", "or", "us"};
            string[] suffixesF = { "a", "enz", "ei", "ie", "heit", "keit", "ik", "sion", "tion", "sis", "tät", "ung", "ur", "schaft" };
            string[] suffixesN = { "chen", "lein", "icht", "il", "it", "ma", "ment", "tum", "um"};

            // Suffixes for which you can "guess" the gender.
            string[] lessProbableSuffixesM = { "el", "er", "en" };
            string[] lessProbableSuffixF = { "e" , "anz", "in"};
            string[] lessProbableSuffixN = { "nis", "sal" };


            // For unknown reason I listed the suffixes for foreign words as well - no difference, no use.
            string[] lessProbableSuffixNForeign = { "al", "an", "ar", "är", "at", "ent", "ett", "ier", "iv", "o", "on" };

            bool isMasculine = CheckSuffix(suffixesM, s);
            bool isFeminine = CheckSuffix(suffixesF, s);
            bool isNeuter = CheckSuffix(suffixesN, s);

            bool isProbablyMasculine = CheckSuffix(lessProbableSuffixesM, s);
            bool isProbablyFeminine = CheckSuffix(lessProbableSuffixF, s);
            bool isProbablyNeuter = CheckSuffix(lessProbableSuffixN, s);
            bool isProbablyNeuterForeign = CheckSuffix(lessProbableSuffixNForeign, s);

            // Words starting on "Ge are likely Neuter.
            bool prefixN = s.StartsWith("Ge");

            // Guesses the gender of the word based on the booleans, "?" show uncertainty or unknowns.
            string gender = DetermineGender(isMasculine, isFeminine, isNeuter, isProbablyMasculine, isProbablyFeminine, isProbablyNeuter, isProbablyNeuterForeign, prefixN);

            // Plural form article and the word can determined based on the singular article and the word.
            string pluralforrm = PluralForm(gender, s);

            string[] output = new string[4];
            output[0] = "Podstatne jmeno";
            output[1] = gender + " " + s;   // e.g. "die Schwester"
            output[2] = pluralforrm;        // e.g. "die Familien"
            output[3] = "-----------";
            
            return output;
        }

        // Checks every if input doesn't end with any of the suffixes.
        public bool CheckSuffix(string[] s, string input)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (input.Trim().EndsWith(s[i]))
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

            // Each word can be plural by adding either -en, -s, -e or just changing the article. Depends on gender.
            string[] en_masculine = { "ent", "and", "ant", "ist", "or" };
            string[] en_feminine = { "ion", "ik", "heit", "keit", "schaft", "tät", "ung" };
            string[] en_foreign = { "ma", "um", "us" };
            string[] e_masculine = { "eur", "ich", "ier", "ig", "ling", "ör" };
            string[] s_suffixes = {"a", "i", "o", "u", "y"};
            string[] no_masculine = {"el", "en", "er"};
            string[] no_neuter = {"chen", "lein"};


            // If gender is unknown user is prompted to search up the plural form in the dictonary.
            if (gender == "?")
            {
                // Translation: To be searched
                return "VYHLEDAT";
            }

            if (gender == "der" || gender == "der?")
            {
                if (input.EndsWith("e"))
                {
                    return mnozneCislo + "n";
                }

                has_suffix = CheckSuffix(en_masculine, input);
                if (has_suffix)
                {
                    return mnozneCislo + "en";
                }

                has_suffix = CheckSuffix(e_masculine, input);
                if (has_suffix)
                {
                    return mnozneCislo + "e";
                }

                has_suffix = CheckSuffix(no_masculine, input);
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

                has_suffix = CheckSuffix(en_feminine, input);

                if (has_suffix)
                {
                    return mnozneCislo + "en";
                }
            }
            else if (gender == "das" || gender == "das?")
            {
                has_suffix = CheckSuffix(no_neuter, input);
                if (has_suffix)
                {
                    return mnozneCislo;
                }
            }

            has_suffix = CheckSuffix(s_suffixes, input);

            if (has_suffix)
            {
                return mnozneCislo + "s";
            }
            
            has_suffix = CheckSuffix(en_foreign, input);

            if (has_suffix)
            {
                return mnozneCislo + "en?";
            }

            // Translation: To be searched
            return mnozneCislo + "-(vyhledat)";
        }
    }
}
