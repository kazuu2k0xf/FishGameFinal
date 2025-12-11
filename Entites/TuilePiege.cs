using System;
using Microsoft.Xna.Framework;
using static FishGame.JeuDeseria; 

namespace FishGame
{
    public class TuilePiege
    {
        public string Nom { get; set; }
        public uneTuile Position { get; set; }
        public int DegatsMouvement { get; set; }
        public bool EstVisible { get; set; }
        public bool EstDeclenche { get; set; }
        
        public TuilePiege(string nom, uneTuile position, int degats)
        {
            Nom = nom;
            Position = position;
            DegatsMouvement = degats;
            EstVisible = false;   
            EstDeclenche = false;
        }

        /**
         * Si pecheur sur la tuile piege alors décrmente les degats de la case (decremente mouvement )
         */
        public void VerifierDeclenchement(JoueurPecheur joueur)
        {
            if (EstDeclenche) return; 


            if (joueur.PositionActuelle.Coordonnes._PosX == Position.Coordonnes._PosX &&
                joueur.PositionActuelle.Coordonnes._PosY == Position.Coordonnes._PosY)
            {
                joueur.MouvementRestant -= DegatsMouvement;
                
                EstVisible = true;
                EstDeclenche = true;

                if (joueur.MouvementRestant < 0) 
                    joueur.MouvementRestant = 0;
            }
        }
    }
}