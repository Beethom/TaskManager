using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System;



class App
{
   private readonly string _locationFichier;
    private readonly List<Tache> _taches = new();
    private int _idSuivant = 1;

public App(string? locationFichier = null)
{
    _locationFichier = locationFichier ?? Path.Combine(AppContext.BaseDirectory, "tache.json");
    ChargerTaches();
}

    public void Run()
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

    private void AfficherMenu()
    {
        Console.WriteLine("\nMenu :");
        Console.WriteLine("1. Ajouter une tâche");
        Console.WriteLine("2. Afficher les tâches");
        Console.WriteLine("3. Marquer une tâche comme complète");
        Console.WriteLine("4. Supprimer une tâche");
        Console.WriteLine("5. Quitter");
    }

    private void AjouterTache()
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
            Id = _idSuivant++,
            Description = description,
            EstComplete = false,
            DateCreation = DateTime.Now
        };

        _taches.Add(tache);
        SauvegarderTaches();
        Console.WriteLine($"Tâche ajoutée avec succès #{tache.Id}!");
    }

   

    private void SupprimerTache()
    {
        Console.Write("Entrez l'ID de la tâche à supprimer : ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var tache = _taches.FirstOrDefault(t => t.Id == id);
            if (tache != null)
            {
                _taches.Remove(tache);
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

    private void AfficherTaches()
    {
        if (_taches.Count == 0)
        {
            Console.WriteLine("Aucune tâche à afficher.");
            return;
        }
        Console.WriteLine("\nID | Description | Statut | Date de création");
        Console.WriteLine("---------------------------------------------");
        foreach (var tache in _taches.OrderBy(t => t.EstComplete).ThenBy(t => t.DateCreation))
        {
            string statut = tache.EstComplete ? "Complète" : "A faire";
            Console.WriteLine($"{tache.Id} | {tache.Description} | {(tache.EstComplete ? "Complète" : "En cours")} | {tache.DateCreation}");
        }
    }

    private void MarquerTacheComplete()
    {
        Console.Write("Entrez l'ID de la tâche à marquer comme complète : ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var tache = _taches.FirstOrDefault(t => t.Id == id);
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

    private void SauvegarderTaches()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(_locationFichier, JsonSerializer.Serialize(_taches, options));
       
    }

    private void ChargerTaches()
    {
        if (File.Exists(_locationFichier))
        {
            var json = File.ReadAllText(_locationFichier);
            var tachesChargees = JsonSerializer.Deserialize<List<Tache>>(json) ?? new List<Tache>();
            _taches.Clear();
            _taches.AddRange(tachesChargees);
            _idSuivant = _taches.Any() ? _taches.Max(t => t.Id) + 1 : 1;
        }
        else
        {
            _taches.Clear();
        }
    }
}