using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishGame
{
    public  class Pecheur
    {
        public string nom;
        public static int nbPecheur;
        public int mouvementsRestants;
        public Tuile positionActuelle;
        public bool aPoisson = false;

        public Texture2D texture;
        public Rectangle sourceRect;

        private int _frameWidth;
        private int _frameHeight;

        public Pecheur(string nom, Tuile posDepart, int mouv, Texture2D tex)
        {
            this.nom = nom + " numero " + nbPecheur;
            this.positionActuelle = posDepart;
            this.mouvementsRestants = mouv;
            this.texture = tex;
            nbPecheur++;

            int colPecheur = 7;
            int lignePecheur = 4;

            _frameWidth = tex.Width / colPecheur;
            _frameHeight = tex.Height / lignePecheur;

            sourceRect = new Rectangle(0, 0, _frameWidth, _frameHeight);
        }
        
        /**
        * Deplacer le pecheur vers une case avec verification de si le déplacement est valide ou pas
        * decremente le compteur de pas pour chaque déplacement
        */
        public void Déplacer(int x, int y, Niveau niveau)
        {
            if (mouvementsRestants <= 0) return;

            int nouveauX = positionActuelle.coordonnees.X + x;
            int nouveauY = positionActuelle.coordonnees.Y+ y;
            
            if (nouveauX >= 0 && nouveauX < 8 && nouveauY >= 0 && nouveauY < 8)
            {
                Tuile cible =  niveau.grille[nouveauX, nouveauY];
               
                if (!cible.estBloquant)
                {
                 
                    positionActuelle = cible;
                    mouvementsRestants--;
                }
            }
        }
        
        /**
        * Sert a la direction du pecheur ( conversion touche vers vecteur direction )
        */
        public void GererEntree(KeyboardState currentKb, KeyboardState previousKb, Niveau niveau)
        {
            int dx = 0;
            int dy = 0;

            if (currentKb.IsKeyDown(Keys.Up) && previousKb.IsKeyUp(Keys.Up)) dy = -1;
            else if (currentKb.IsKeyDown(Keys.Down) && previousKb.IsKeyUp(Keys.Down)) dy = 1;
            else if (currentKb.IsKeyDown(Keys.Left) && previousKb.IsKeyUp(Keys.Left)) dx = -1;
            else if (currentKb.IsKeyDown(Keys.Right) && previousKb.IsKeyUp(Keys.Right)) dx = 1;

            if (dx != 0 || dy != 0)
            {
                this.Déplacer(dx, dy, niveau);
            }
        }
    }
}