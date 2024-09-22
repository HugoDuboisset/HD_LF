using TD_librairies;

public class Program
{
    public static void Main(string[] args)
    {

        Library bibliotheque = new Library();
        Library bibliotheque2 = new Library();

        //ajout de plusieurs médias pour faire des tests
        bibliotheque.AjouterMedia(new Livre { titre = "Le seigneur des anneaux", reference = 12345, nombreCopies = 10, auteur = "Tolkien", nombreDePages = 400 });
        bibliotheque.AjouterMedia(new DVD { titre = "Inception", reference = 67890, nombreCopies = 5, duree = 148, realisateur ="Nolan" });
        bibliotheque.AjouterMedia(new Livre { titre = "Inception", reference = 67890, nombreCopies = 5, auteur = "Xavier", nombreDePages = 125 });
        bibliotheque.AjouterMedia(new CD { titre = "Thriller", reference = 11223, nombreCopies = 7, groupe = "Michael Jackson", nombreDePistes = 15 });

        bibliotheque2.AjouterMedia(new Livre { titre = "Le seigneur des anneaux", reference = 12345, nombreCopies = 10, auteur = "Tolkien", nombreDePages = 400 });

        bibliotheque.RechercherMedia("oui-oui");

        // Media media = bibliotheque[12345];
        /*
        bibliotheque.SupprimerMedia(12345);

        Media media = bibliotheque2[12345];
        if (media != null)
        {
            media.AfficherInfos();
        }
        else
        {
            Console.WriteLine("Média non trouvé.");
        }
        */
    }
}