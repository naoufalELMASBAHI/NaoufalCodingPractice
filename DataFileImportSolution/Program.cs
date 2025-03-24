using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;


/*
 Importation de fichiers de données

Plusieurs fichiers de données provenant de sources externes sont nécessaires pour constituer un ensemble de données complet pour certaines analyses commerciales. Le contenu de chaque fichier est considéré comme l'entrée du programme, sous la forme d'un tableau de chaînes, chaque ligne étant un élément du tableau. Le contenu peut être au format CSV (séparé par des points-virgules) ou à largeur fixe. Le format de sortie cible est un JSON sérialisé, sans aucun espace blanc.

Les schémas de données source et cible sont mappés comme suit :


Champ de sortie cible              | Type de données | Champ d'entrée CSV | Champ d'entrée à largeur fixe
-----------------------------------|-----------------|--------------------|--------------------------------
Identifiant de la source originale | Chaîne          | Réf                | Caractères 1-4
Date de transaction                | DateTime        | Date               | Caractères 5-14
Valeur                             | Décimal         | Montant            | Caractères 15-22
Taux                               | Décimal         | Taux               | Caractères 23-27


Exporter vers Sheets
Écrivez un code capable d'importer le contenu des fichiers texte d'entrée dans le format de données commun utilisé par l'analyse. La solution doit être extensible pour gérer d'autres formats et respecter les principes de la programmation orientée objet.

Tout contenu qui ne correspond pas strictement à l'un des deux formats doit être rejeté avec le message : « L'entrée n'est pas dans un format valide ».
 */


namespace DataFileImportSolution
{



    public enum FileFormat
    {
        CSV,
        FixedWidth
    }

    public interface IFileParser
    {
        List<Dictionary<string, object>> Parse(string[] lines);
        bool CanParse(FileFormat format);
    }

    public class CsvParser : IFileParser
    {
        public List<Dictionary<string, object>> Parse(string[] lines)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            if (lines == null || lines.Length == 0)
            {
                return result;
            }

            string[] headers = lines[0].Split(';');

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(';');
                if (values.Length != headers.Length)
                {
                    continue;
                }

                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int j = 0; j < headers.Length; j++)
                {
                    row[headers[j].Trim()] = values[j].Trim();
                }
                result.Add(row);
            }

            return result;
        }

        public bool CanParse(FileFormat format)
        {
            return format == FileFormat.CSV;
        }
    }

    public class FixedWidthParser : IFileParser
    {
        public List<Dictionary<string, object>> Parse(string[] lines)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            if (lines == null || lines.Length == 0)
            {
                return result;
            }

            foreach (string line in lines)
            {
                if (line.Length < 27)
                {
                    continue;
                }

                Dictionary<string, object> row = new Dictionary<string, object>();
                row["Réf."] = line.Substring(0, 4).Trim();
                row["Date"] = line.Substring(4, 14).Trim();
                row["Montant"] = line.Substring(14, 22).Trim();
                row["Taux"] = line.Substring(22, 26).Trim();

                result.Add(row);
            }

            return result;
        }

        public bool CanParse(FileFormat format)
        {
            return format == FileFormat.FixedWidth;
        }
    }

    public class FileImporter
    {
        private readonly IEnumerable<IFileParser> _parsers;

        public FileImporter(IEnumerable<IFileParser> parsers)
        {
            _parsers = parsers;
        }

        public string Import(string[] lines, FileFormat format)
        {
            try
            {
                IFileParser parser = _parsers.FirstOrDefault(p => p.CanParse(format));

                if (parser == null)
                {
                    throw new ArgumentException($"Format de fichier non pris en charge : {format}");
                }

                List<Dictionary<string, object>> data = parser.Parse(lines);
                return JsonConvert.SerializeObject(data, Formatting.None);
            }
            catch (Exception ex)
            {
                return $"Erreur : {ex.Message}";
            }
        }
    }
    public class DependencyContainer
    {
        private readonly Dictionary<Type, List<object>> _registrations = new Dictionary<Type, List<object>>();

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface, new()
        {
            if (!_registrations.ContainsKey(typeof(TInterface)))
            {
                _registrations[typeof(TInterface)] = new List<object>();
            }
            _registrations[typeof(TInterface)].Add(new TImplementation());
        }

        public IEnumerable<TInterface> ResolveAll<TInterface>()
        {
            if (_registrations.TryGetValue(typeof(TInterface), out List<object> implementations))
            {
                return implementations.Cast<TInterface>();
            }
            return Enumerable.Empty<TInterface>();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new DependencyContainer();
            container.Register<IFileParser, CsvParser>();
            container.Register<IFileParser, FixedWidthParser>();

            var parsers = container.ResolveAll<IFileParser>();
            var importer = new FileImporter(parsers);

            string[] csvData = {
            "Réf.;Date;Montant;Taux",
            "1234;2023-10-26;100.50;1.25",
            "5678;2023-10-27;200.75;1.50"
        };

            string[] fixedWidthData = {
            "12342023-10-26100.50 1.25",
            "56782023-10-27200.75 1.50"
        };

            string csvJson = importer.Import(csvData, FileFormat.CSV);
            Console.WriteLine($"CSV JSON: {csvJson}");

            string fixedWidthJson = importer.Import(fixedWidthData, FileFormat.FixedWidth);
            Console.WriteLine($"FixedWidth JSON: {fixedWidthJson}");

            try
            {
                string invalidDataJson = importer.Import(csvData, (FileFormat)100);
                Console.WriteLine($"Invalid format: {invalidDataJson}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erreur format invalide : {ex.Message}");
            }
        }
    }

}
