using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_librairies
{
    internal class DVD : Media
    {
        public string realisateur;
        public float duree;

        //Afficher les informations spécifiques aux DVD en plus de celles de Media
        public override void AfficherInfos()
        {
            // Appel à la méthode de la classe de base pour afficher les infos générales
            base.AfficherInfos();
            // Ajout des informations spécifiques à la classe DVD
            Console.WriteLine($"réalisateur: {realisateur}, durée du film : {duree}");
        }


    }
}
