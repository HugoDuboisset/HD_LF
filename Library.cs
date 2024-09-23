using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;


namespace TD_librairies
{
    internal class Library
    {
        // listes des médias possédés dans une collection
        private List<Media> collection = new List<Media>();

        // dictionnaire de données permettant de gérer les emprunts 
        // la clé est le nom de l'utilisateur, et la valeur la liste de ses emprunts
        private Dictionary<string, List<Emprunts>> emprunts = new Dictionary<string, List<Emprunts>>();

        //Indexeur utilisant le numéro de référence passé en paramètre
        public Media this[int numeroReference]
        {
            get
            {
                // Rechercher le média correspondant au numéro de référence
                foreach (var media in collection)
                {
                    if (media.reference == numeroReference)
                    {
                        return media;
                    }
                }

                // Si aucun média n'est trouvé, retourner null
                return null;
            }
        }

        /// <summary>
        /// permet d'afficher les informations avec l'indexeur Media
        /// permet de passer en parametres la référence d'un media pour le chercher
        /// appelle la méthode AfficherInfos() de Media si il existe
        /// </summary>
        /// <param name="reference"></param>
        public void AfficherInfos(int reference)
        {
            // Utilisation de l'indexeur pour trouver le média
            Media media = this[reference];

            if (media != null)
            {
                // Appel de la méthode AfficherInfos spécifique au média
                media.AfficherInfos();
            }
            else
            {
                // Si le média n'est pas trouvé, afficher un message
                Console.WriteLine("Média non trouvé.");
            }
        }

        /// <summary>
        /// Permet de chercher un ou des médias dans la bibliothèque avec un critère passé en paramètre
        /// Affiche les informations sur le ou les médias trouvés, ou un message d'erreur si aucune référence ne correspond
        /// </summary>
        /// <param name="critereRecherche">élément permettant de trouver le media</param>
        public void RechercherMedia(string critereRecherche)
        {
            List<Media> resultats = new List<Media>();

            foreach (var media in collection)
            {
                bool correspond = false;

                if (!string.IsNullOrEmpty(critereRecherche))
                {
                    // comparer le critère de recherche passé en paramètre
                    //ici le titre ou le numéro de référence pour tout media
                    if (media.titre.Contains(critereRecherche, StringComparison.OrdinalIgnoreCase))
                    {
                        correspond = true;
                    }

                    if (media.reference.ToString() == critereRecherche)
                    {
                        correspond = true;
                    }

                    //ici un critère par type de media
                    if (media is Livre livre && livre.auteur.Contains(critereRecherche, StringComparison.OrdinalIgnoreCase))
                    {
                        correspond = true;
                    }

                    if (media is DVD film && film.realisateur.Contains(critereRecherche, StringComparison.OrdinalIgnoreCase))
                    {
                        correspond = true;
                    }

                    if (media is CD album && album.groupe.Contains(critereRecherche, StringComparison.OrdinalIgnoreCase))
                    {
                        correspond = true;
                    }
                }

                // Si un critère correspond, ajouter aux résultats
                if (correspond)
                {
                    resultats.Add(media);
                }
            }

            // Afficher les résultats s'il y au moins une oeuvre ajoutée à la liste
            if (resultats.Count > 0)
            {
                Console.WriteLine("Résultats de la recherche :");
                foreach (var media in resultats)
                {
                    media.AfficherInfos();
                }
            }
            else
            {
                Console.WriteLine("Aucun média ne correspond à votre recherche.");
            }
        }


        // méthode ajoutant un media à la collection choisie
        public virtual void AjouterMedia(Media media)
        {
            collection.Add(media);
        }



        /// <summary>
        /// supprime un media de la collection choisie
        /// prend en parametre la référence d'un média        
        /// </summary>
        /// <param name="reference"></param>
        /// <returns>retourne un booléen en fonction de la réussite ou non de la suppression</returns>
        public bool SupprimerMedia(int reference)
        {
            // utilise l'indexeur pour trouver le media  
            Media media = this[reference]; 
            if (media != null)
            {
                collection.Remove(media);
                Console.WriteLine("Le média a été supprimé :");
                media.AfficherInfos(); 
                return true;
            }
            else
            {
                Console.WriteLine("Média non trouvé.");
                return false;
            }
        }


        // Gestion des emprunts


        /// <summary>
        /// permet à l'utilisateur d'emprunter un média.
        /// vérifie que le média soit toujours disponible, ou existe.
        /// vérifie ensuite que l'utilisateur existe. S'il n'a jamais emprunté, il est ajouté au dictionnaire de données.
        /// S'il existe, le média est directement ajouté à son dictionnaire de données
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="utilisateur"></param>
        /// <returns>booléen en fonction de si l'emprunt réussi ou non</returns>
        public bool EmprunterMedia(int reference, string utilisateur)
        {
            Media media = this[reference];
            if (media != null)
            {
                //vérifie que des exemplaires du média soient encore disponibles avant pouvoir continuer l'emprunt
                if (media.nombreCopies > 0)
                {
                    media.nombreCopies--;

                    // Ajouter un emprunt pour cet utilisateur
                    // si c'est le premier emprunt de l'utilisateur, l'ajouter au dictionnaire de données
                    if (!emprunts.ContainsKey(utilisateur))
                    {
                        emprunts[utilisateur] = new List<Emprunts>();
                    }

                    //créé un nouvel objet emprunt 
                    emprunts[utilisateur].Add(new Emprunts(reference, utilisateur));
                    Console.WriteLine("Emprunt réussi");
                    return true;
                }
                else
                {
                    Console.WriteLine("Aucun exemplaire disponible");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Média non trouvé.");
                return false;
            }
        }

        /// <summary>
        /// Vérifie que le media existe, et que l'utilisateur l'ai bien emprunté. 
        /// Si c'est le cas, il est supprimé de son dictionnaire de données.
        /// Si l'utilisateur n'a plus d'emprunts à son actif, son entrée dans le dictionnaire est supprimée. 
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="utilisateur"></param>
        /// <returns>boléen en fonction de si le retour est effectué correctement</returns>
        public bool RetournerMedia(int reference, string utilisateur)
        {
            Media media = this[reference];
            if (media != null)
            {
                // Vérifier si l'utilisateur a emprunté ce média
                if (emprunts.ContainsKey(utilisateur))
                {
                    //utilise l'indexeur pour trouver le media emprunté, et recupére le premier trouvé correspondant
                    var empruntsUtilisateur = emprunts[utilisateur];
                    var emprunt = empruntsUtilisateur.FirstOrDefault(e => e.reference == reference);
                    if (emprunt != null)
                    {
                        media.nombreCopies++;
                        empruntsUtilisateur.Remove(emprunt);

                        // Si l'utilisateur n'a plus d'emprunts il est supprimé du dictionnaire
                        if (empruntsUtilisateur.Count == 0)
                        {
                            emprunts.Remove(utilisateur);
                        }

                        Console.WriteLine("Retour réussi");
                        return true;
                    }
                }
                Console.WriteLine("L'utilisateur n'a pas emprunté ce média.");
                return false;
            }
            else
            {
                Console.WriteLine("Média non trouvé.");
                return false;
            }
        }

        public void ListerEmprunts(string utilisateur)
        {
            if (emprunts.ContainsKey(utilisateur))
            {
                var empruntsUtilisateur = emprunts[utilisateur];
                Console.WriteLine($"Médias empruntés par {utilisateur} :");

                foreach (var emprunt in empruntsUtilisateur)
                {
                    var media = this[emprunt.reference];
                    media.AfficherInfos();
                }
            }
            else
            {
                Console.WriteLine($"{utilisateur} n'a emprunté aucun média.");
            }
        }

        /// <summary>
        /// Permet d'afficher les statistiques de la bibliothèque
        /// Le nombre total de media compte le nombre d'entrées dans la collection
        /// pour chaque media compte leur nombre d'exemplaire, et en cas d'emprunts compte le nombre d'exemplaires empruntés
        /// </summary>
        public void AfficherStatistiques()
        {
            int nombreTotalMedias = collection.Count;
            int nombreTotalExemplairesDisponibles = 0;
            int nombreTotalExemplairesEmpruntes = 0;

            // Parcourir tous les médias dans la collection
            foreach (var media in collection)
            {
                nombreTotalExemplairesDisponibles += media.nombreCopies;

                // Compter les exemplaires empruntés pour ce média
                int nombreEmpruntes = 0;

                if (emprunts.Values.Any(e => e.Any(emp => emp.reference == media.reference)))
                {
                    nombreEmpruntes = emprunts.Values.Sum(e => e.Count(emp => emp.reference == media.reference));
                }

                nombreTotalExemplairesEmpruntes += nombreEmpruntes;
            }

            // Affichage des statistiques
            Console.WriteLine("===== Statistiques de la bibliothèque =====");
            Console.WriteLine($"Nombre total de médias : {nombreTotalMedias}");
            Console.WriteLine($"Nombre total d'exemplaires disponibles : {nombreTotalExemplairesDisponibles}");
            Console.WriteLine($"Nombre total d'exemplaires empruntés : {nombreTotalExemplairesEmpruntes}");
        }


        // surchage des opérateurs 
       
        //Surcharge de l'opérateur + pour ajouter un nouveau media
        //vérifie que ce média n'existe pas déjà avant de l'ajouter
        //retourne la collection acutalisée
        public static Library operator +(Library library, Media media)
        {
            // Vérifier si le média existe déjà dans la collection
            Media mediaExiste = library[media.reference];
            if (mediaExiste != null)
            {
                // Si le média existe déjà, afficher un message
                Console.WriteLine("Le média existe déjà dans la bibliothèque.");
            }
            else
            {
                // Sinon, ajouter le nouveau média avec un nombre d'exemplaires par défaut
                library.AjouterMedia(media);
                Console.WriteLine("Média ajouté");
            }

            return library;
        }

        /// <summary>
        /// ajoute des copies supplémentaires à un media
        /// vérifie que le média existe déjà avant d'ajouter des copies
        /// </summary>
        /// <param name="library"></param>
        /// <param name="data"></param>
        /// <returns>la collection acutalisée</returns>
        public static Library operator +(Library library, (Media media, int copies) data)
        {
            Media media = data.media;
            int copies = data.copies;

            // Vérifier si le média existe déjà dans la collection
            Media mediaExiste = library[media.reference];
            if (mediaExiste != null)
            {
                // Ajouter les copies supplémentaires
                mediaExiste.nombreCopies += copies;
                Console.WriteLine($"{copies} copies ajoutées au média");
            }
            else
            {
                // Si le média n'existe pas, l'ajouter avec le nombre de copies spécifié
                Console.WriteLine("Ce Media n'existe pas");

            }

            return library;
        }

        /// <summary>
        /// Surcharge de l'operateur - pour retirer des medias de la bibliotheque
        /// vérifie que le media existe, et si c'est le cas, le supprime de la collection
        /// </summary>
        /// <param name="library"></param>
        /// <param name="media"></param>
        /// <returns>la bibliothèque actualisée</returns>
        public static Library operator -(Library library, Media media)
        {
            Media mediaExiste = library[media.reference];
            if (mediaExiste != null)
            {
                // Retirer complètement le média de la collection
                library.collection.Remove(mediaExiste);
                Console.WriteLine("Le média a été retiré ");
            }
            else
            {
                Console.WriteLine("le média n'existe.");
            }

            return library;
        }

        /// <summary>
        /// surcharge de l'opérateur - pour retirer des exemplaires d'un media
        /// verifie que le media existe, et qu'il y ai assez d'exemplaires à retirer
        /// </summary>
        /// <param name="library"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Library operator -(Library library, (Media media, int nombreExemplaires) data)
        {
            Media mediaExiste = library[data.media.reference];
            if (mediaExiste != null)
            {
                if (mediaExiste.nombreCopies >= data.nombreExemplaires)
                {
                    mediaExiste.nombreCopies -= data.nombreExemplaires;
                    Console.WriteLine($"{data.nombreExemplaires} exemplaires retirés");
                }
                else
                {
                    Console.WriteLine("nombre d'exemplaires insuffisants");
                }
            }
            else
            {
                Console.WriteLine("le média n'existe pas");
            }

            return library;
        }


        

    }
}
