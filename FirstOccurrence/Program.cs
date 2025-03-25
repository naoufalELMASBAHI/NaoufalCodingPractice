using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

/*
Étant donné une chaîne de caractères text et un motif de recherche searchPattern, trouvez l'index basé sur zéro de la première occurrence de searchPattern dans text.

Contraintes :
Le motif searchPattern est composé de lettres minuscules et peut contenir un unique caractère joker "*", qui peut correspondre à n'importe quel caractère unique.
La chaîne text contient uniquement des lettres minuscules.
 */
namespace FirstOccurrence
{
    internal class Program
    {
        public static int firstOccurrence(string texte, string patern)
        { 
            if (String.IsNullOrEmpty(texte.Trim()) || String.IsNullOrEmpty(patern.Trim()))
                return -1;
            string regexPattern = Regex.Escape(patern).Replace("\\*", ".*");
            Match match = Regex.Match(texte, regexPattern, RegexOptions.IgnoreCase);
            return match.Success ? match.Index : -1;
        }

        public static void Main()
        {
            string texte = "Bonjour, comment ça va ?";
            string mot = "com*ent";

            int resultat = firstOccurrence(texte, mot);
            Console.WriteLine(resultat); // true
        }
    }
}

