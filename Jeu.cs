using Microsoft.Xna.Framework.Graphics;

namespace FishGame
{
    public enum EtatDuJeux
    {
        Playing,
        GameOver,
        GameWon
    }

    public class Jeu
    {
        public Niveau niveauCourant;
        public EtatDuJeux Etat { get; set; } = EtatDuJeux.Playing;
        /** Initialise Etat du jeu et texture envoyer a niveau**/
        public void Démarrer(Texture2D tCarte, Texture2D tPech, Texture2D tPois)
        {
            niveauCourant = new Niveau(tCarte, tPech, tPois);

            niveauCourant.InitialiserNiveau();

            Etat = EtatDuJeux.Playing;
        }
        
        public void Update()
        {
            if (Etat != EtatDuJeux.Playing) return;

            niveauCourant.Update();

            if (niveauCourant.joueur.mouvementsRestants <= 0)
            {
                Etat = EtatDuJeux.GameOver;
            }
            else if (niveauCourant.joueur.aPoisson &&
                     niveauCourant.joueur.positionActuelle == niveauCourant.caseSortie)
            {
                Etat = EtatDuJeux.GameWon;
            }
        }
    }
}