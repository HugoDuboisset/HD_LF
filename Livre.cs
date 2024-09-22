using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_librairies
{
    internal class Livre : Media
    {
        public int nombreDePages;
        public string auteur;

        //Afficher les informations spécifiques aux livres en plus de celles de Media
        public override void AfficherInfos()
        {
            // Appel à la méthode de la classe de base pour afficher les informations générales
            base.AfficherInfos();
            // Ajout des informations spécifiques à la classe Livre
            Console.WriteLine($"Auteur: {auteur}, nombre de pages : {nombreDePages}");
        }
    }
}
