using System;
using Microsoft.Xna.Framework;
using static FishGame.JeuXML;

namespace FishGame
{
    public class Ennemi
    {
        public string Nom { get; set; }
        public uneTuile Position { get; set; }
        

        public Ennemi(string nom, uneTuile positionDepart)
        {
            Nom = nom;
            Position = positionDepart;
        }


        public void DeplacerVersJoueur(JoueurPecheur joueur, unNiveau niveau)
        {
            int cibleX = joueur.PositionActuelle.Coordonnes._PosX;
            int cibleY = joueur.PositionActuelle.Coordonnes._PosY;

            int monX = Position.Coordonnes._PosX;
            int monY = Position.Coordonnes._PosY;


            int dx = 0;
            int dy = 0;


            if (monX < cibleX) dx = 1;
            else if (monX > cibleX) dx = -1;
            

            else if (monY < cibleY) dy = 1;
            else if (monY > cibleY) dy = -1;


            if (dx != 0 || dy != 0)
            {
                TenterDeplacement(dx, dy, niveau);
            }
        }


        private void TenterDeplacement(int dx, int dy, unNiveau niveau)
        {
            int nouveauX = Position.Coordonnes._PosX + dx;
            int nouveauY = Position.Coordonnes._PosY + dy;


            if (nouveauX >= 0 && nouveauX < niveau.Collones && 
                nouveauY >= 0 && nouveauY < niveau.Lignes)
            {

                Tuile tuileCible = niveau.grille[nouveauX, nouveauY];

                if (!tuileCible.estBloquant)
                {

                    Position.Coordonnes._PosX = nouveauX;
                    Position.Coordonnes._PosY = nouveauY;
                }
            }
        }


        public bool AAttrapeJoueur(JoueurPecheur joueur)
        {
            return Position.Coordonnes._PosX == joueur.PositionActuelle.Coordonnes._PosX &&
                   Position.Coordonnes._PosY == joueur.PositionActuelle.Coordonnes._PosY;
        }
    }
}