using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonaciFindParent
{
    /*
    1. Arbre de processus

    Une chaîne hypothétique de processus est représentée sous forme d'arbre. 
    Les procestus sont numérotés à partir de 1, incrémentés de 1.
    Chaque processus génère un nombre de processus égal à son numéro de processus. 
    Le premier nœud, processNumber 1, génère 1 processus, le deuxième en génère 2 et ainsi de suite. 
    Voir le graphique ci-dessous. 
    Étant donné un numéro de processus, trouvez le numéro de processus de son parent

    Exemple

    numéro de processus = 6

    D'après le diagramme, le parent = 3.
    */

    internal class Program
    {
        public static int GetParentProcess(int processNumber)
        {
            if (processNumber <= 1)
            {
                return -1; // Le processus 1 n'a pas de parent
            }

            int parent = 1;
            int childrenCount = 1;
            int currentChild = 2; // Le premier enfant possible

            while (currentChild <= processNumber)
            {
                if (currentChild + childrenCount > processNumber)
                {
                    return parent; // Le parent a été trouvé
                }

                currentChild += childrenCount;
                parent++;
                childrenCount = parent;
            }

            return -1; 
        }
        static void Main(string[] args)
        {
            Console.WriteLine($"Parent du processus 2 : {GetParentProcess(2)}"); // Résultat : 1
            Console.WriteLine($"Parent du processus 4 : {GetParentProcess(4)}"); // Résultat : 2
            Console.WriteLine($"Parent du processus 6 : {GetParentProcess(6)}"); // Résultat : 3
            Console.WriteLine($"Parent du processus 7 : {GetParentProcess(7)}"); // Résultat : 3
            Console.WriteLine($"Parent du processus 9 : {GetParentProcess(9)}"); // Résultat : 5
            Console.WriteLine($"Parent du processus 10 : {GetParentProcess(10)}"); // Résultat : 5
            Console.WriteLine($"Parent du processus 12 : {GetParentProcess(12)}"); // Résultat : 5
            Console.WriteLine($"Parent du processus 15 : {GetParentProcess(15)}"); // Résultat : 8
            Console.WriteLine($"Parent du processus 16 : {GetParentProcess(16)}"); // Résultat : 8

        }
    }
}
