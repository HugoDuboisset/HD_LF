using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD_librairies
{
    internal class Emprunts
    {
        public int reference;
        public string utilisateur;
        public DateTime dateEmprunt;

        public Emprunts(int numeroReference, string nomUtilisateur)
        {
            reference = numeroReference;
            utilisateur = nomUtilisateur;
            dateEmprunt = DateTime.Now;
        }

    }
}
