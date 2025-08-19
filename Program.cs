using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System;

class Tache
{
    public int Id { get; set; }
    public string Description { get; set; } = "";
    public bool EstComplete { get; set; }

    public DateTime DateCreation { get; set; } = DateTime.Now;


}

class App
{
    static string locationFichier = "tache.json";

    static List<Tache> taches = new List<Tache>();
    static int idSuivant = 1;

    static void Main()
    {
        ChargerTaches();
        Console.WriteLine("Gestion des Tâches");

        while (true)
        {

            AfficherMenu();
            Console.Write("Choisissez une option : ");
            string? choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    AjouterTache();
                    break;
                case "2":
                    AfficherTaches();
                    break;
                case "3":
                    MarquerTacheComplete();
                    break;
                case "4":
                    SupprimerTache();
                    break;
                case "5":
                    SauvegarderTaches();
                    Console.WriteLine("Au revoir !");
                    return;
                default:
                    Console.WriteLine("Choix invalide, veuillez réessayer.");
                    break;
            }
        }
    }

    static void AfficherMenu()
    {
        Console.WriteLine("\nMenu :");
        Console.WriteLine("1. Ajouter une tâche");
        Console.WriteLine("2. Afficher les tâches");
        Console.WriteLine("3. Marquer une tâche comme complète");
        Console.WriteLine("4. Supprimer une tâche");
        Console.WriteLine("5. Quitter");
    }

    static void AjouterTache()
    {
        Console.Write("Entrez la description de la tâche : ");
        string? description = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("La description ne peut pas être vide.");
            return;
        }

        var tache = new Tache
        {
            Id = idSuivant++,
            Description = description,
            EstComplete = false,
            DateCreation = DateTime.Now
        };

        taches.Add(tache);
        SauvegarderTaches();
        Console.WriteLine($"Tâche ajoutée avec succès #{tache.Id}!");
    }

   

    static void SupprimerTache()
    {
        Console.Write("Entrez l'ID de la tâche à supprimer : ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var tache = taches.FirstOrDefault(t => t.Id == id);
            if (tache != null)
            {
                taches.Remove(tache);
                SauvegarderTaches();
                Console.WriteLine($"Tâche #{id} supprimée avec succès.");
            }
            else
            {
                Console.WriteLine($"Aucune tâche trouvée avec l'ID #{id}.");
            }
        }
        else
        {
            Console.WriteLine("ID invalide.");
        }
    }

    static void AfficherTaches()
    {
        if (taches.Count == 0)
        {
            Console.WriteLine("Aucune tâche à afficher.");
            return;
        }
        Console.WriteLine("\nID | Description | Statut | Date de création");
        Console.WriteLine("---------------------------------------------");
        foreach (var tache in taches.OrderBy(t => t.EstComplete).ThenBy(t => t.DateCreation))
        {
            string statut = tache.EstComplete ? "Complète" : "A faire";
            Console.WriteLine($"{tache.Id} | {tache.Description} | {(tache.EstComplete ? "Complète" : "En cours")} | {tache.DateCreation}");
        }
    }

    static void MarquerTacheComplete()
    {
        Console.Write("Entrez l'ID de la tâche à marquer comme complète : ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var tache = taches.FirstOrDefault(t => t.Id == id);
            if (tache != null)
            {
                tache.EstComplete = true;
                SauvegarderTaches();
                Console.WriteLine($"Tâche #{id} marquée comme complète.");
            }
            else
            {
                Console.WriteLine($"Aucune tâche trouvée avec l'ID #{id}.");
            }
        }
        else
        {
            Console.WriteLine("ID invalide.");
        }
    }

    static void SauvegarderTaches()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(locationFichier, JsonSerializer.Serialize(taches, options));
        // Console.WriteLine("Tâches sauvegardées avec succès."); // Optionally comment out to avoid too much output
    }

    static void ChargerTaches()
    {
        if (File.Exists(locationFichier))
        {
            var json = File.ReadAllText(locationFichier);
            taches = JsonSerializer.Deserialize<List<Tache>>(json) ?? new List<Tache>();
            idSuivant = taches.Any() ? taches.Max(t => t.Id) + 1 : 1;
        }
        else
        {
            taches = new List<Tache>();
        }
    }
}