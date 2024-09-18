using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_librairies
{
    internal class Media
    {
        public string titre;
        public int reference;
        public int nombreCopies;

        // Afficher les informations sur les objets Media
        public virtual void AfficherInfos()
        {
            Console.WriteLine($"Titre: {titre}, Référence: {reference}, Nombre d'exemplaires: {nombreCopies}");
        }

    }
}
