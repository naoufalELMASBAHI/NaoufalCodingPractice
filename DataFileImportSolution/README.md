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