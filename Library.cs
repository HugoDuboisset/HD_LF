using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_librairies
{
    internal class Library
    {
        // listes des médias possédés dans une collection
        private List<Media> collection = new List<Media>();

        // dictionnaire de données permettant d'avoir la référence des emprunts et le nombre d'exemplaires empruntés
        private Dictionary<int, int> emprunts = new Dictionary<int, int>();


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

        //permet d'emprunter un livre
        //prend en parametre le numéro de référence du media pour effectuer l'emprunt
        // retourne un booléen en fonction de la réussite ou non de la réservation
        public bool EmprunterMedia(int reference)
        {
            //utilisation de l'indexeur pour retrouver le media choisi avec sa référence
            Media media = this[reference]; 
            if (media != null)
            {
                //
                if (media.nombreCopies > 0) 
                {
                    media.nombreCopies--; 

                    
                    if (emprunts.ContainsKey(reference))
                    {
                        emprunts[reference]++; 
                    }
                    else
                    {
                        emprunts[reference] = 1; 
                    }
                    // Retourne 'true' si l'emprunt est réussi
                    Console.WriteLine("emprunt réussi");
                    return true; 
                }
                else
                {
                    // Retourne 'false' si le média n'est pas disponible
                    Console.WriteLine("Aucun exemplaire dispo");
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
        /// permet de retourner un emprunt fait par un utilisateur. 
        /// Vérifie si le media existe et s'il existe dans le dictionnaire de données des emprunts. 
        /// S'il existe, réduire le nombre de copies empruntées
        /// s'il n'y a plus de copies empruntées, les supprimer du dictionnaire de données des emprunts
        /// </summary>
        /// <param name="reference"></param>
        /// <returns>retourne true si le retour de l'emprunt est réussi, retourne false en cas d'erreur</returns>
        public bool RetournerMedia(int reference)
        {
            // Utilisation de l'indexeur pour retrouver le média avec sa référence
            Media media = this[reference];
            if (media != null)
            {
                // Vérifier s'il y a des emprunts pour ce média
                if (emprunts.ContainsKey(reference) && emprunts[reference] > 0)
                {
                    
                    media.nombreCopies++;
                    emprunts[reference]--;

                    // Si aucun exemplaire de ce média n'est plus emprunté, supprimer l'entrée du dictionnaire
                    if (emprunts[reference] == 0)
                    {
                        emprunts.Remove(reference);
                    }
                    Console.WriteLine("Retour réussi");
                    return true;
                }
                else
                {
                    // Si aucun exemplaire n'a été emprunté
                    Console.WriteLine("Aucun exemplaire emprunté.");
                    return false;
                }
            }
            else
            {
                // si le media cherché n'existe pas
                Console.WriteLine("Média non trouvé.");
                return false; 
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
    }
}
