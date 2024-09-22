using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_librairies
{
    internal class CD : Media
    {
        public string groupe;
        public int nombreDePistes;

        //Afficher les informations spécifiques aux CD en plus de celles de Media

        public override void AfficherInfos()
        {
            // Appel à la méthode de la classe de base pour afficher les infos générales
            base.AfficherInfos();
            // Ajout des informations de la classe CD
            Console.WriteLine($"Groupe: {groupe}, nombre de pistes : {nombreDePistes}");
        }

    }
}
