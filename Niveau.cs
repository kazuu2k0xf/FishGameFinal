using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame
{
    public class Niveau
    {
        public Tuile[,] grille;
        public Pecheur joueur;
        public Poisson poisson;
        public  Tuile  caseSortie;

        private Texture2D _texCarte, _texPecheur, _texPoisson;
        private const int Columns = 8;
        private const int Rows = 8;

        public Niveau(Texture2D carte, Texture2D pecheur, Texture2D poissonTexture)
        {
            _texCarte = carte;
            _texPecheur = pecheur;
            _texPoisson = poissonTexture;
        }

        public void InitialiserNiveau()
        {
            bool[,] arbreCollision = new bool[Columns, Rows];

            // definition case collision
            arbreCollision[0, 0] = true;
            arbreCollision[1, 0] = true;
            arbreCollision[2, 0] = true;
            arbreCollision[3, 0] = true;
            arbreCollision[0, 1] = true;
            arbreCollision[0, 2] = true;
            arbreCollision[0, 6] = true;
            arbreCollision[1, 7] = true;
            arbreCollision[0, 7] = true;
            arbreCollision[6, 0] = true;
            arbreCollision[7, 0] = true;
            arbreCollision[1, 1] = true;
            arbreCollision[2, 1] = true;

            // création grille de 0 a 63 car 8X8
            grille = new Tuile[Columns, Rows];

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    int tileIndex = (y * Rows) + x;
                    bool estBloquant = arbreCollision[x, y];

                    grille[x, y] = new Tuile(new Point(x,y), estBloquant, _texCarte, tileIndex);
                }
            }
            
            //Parser pour la position du joueur3
            Tuile posJoueur = grille[ParserJeux.ParserPositionJoueur("./xml/niveau1.xml")[0], ParserJeux.ParserPositionJoueur("./xml/niveau1.xml")[1]];
            joueur = new Pecheur("Joueur", posJoueur, ParserJeux.ParserNbPas("./xml/niveau1.xml"), _texPecheur);

            Tuile posPoisson = grille[ParserJeux.ParserPositionPoisson("./xml/niveau1.xml")[0], ParserJeux.ParserPositionPoisson("./xml/niveau1.xml")[1]];
            poisson = new Poisson("Saumon", posPoisson, _texPoisson);

            caseSortie = grille[ParserJeux.ParserPositionFin("./xml/niveau1.xml")[0], ParserJeux.ParserPositionFin("./xml/niveau1.xml")[1]];
        }

        public void Update()
        {
            if (poisson.estVisible && poisson.position == joueur.positionActuelle)
            {
                poisson.estVisible = false;
                joueur.aPoisson = true;
            }
        }
    }
}