using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame
{

    public class Tuile
    {
        public Point coordonnees; 
        public bool estBloquant;

        public Texture2D _texture;
        public Rectangle _sourceRect;

        public const int Largeur = 225;
        public const int Hauteur = 130;

        public Tuile(Point position, bool bloquant, Texture2D texture, int tileIndex)
        {
            this.coordonnees= new Point((int)position.X, (int)position.Y);
            this.estBloquant = bloquant;
            this._texture = texture;

            int nbColonneImageForet = texture.Width / Largeur;

            int texX = (tileIndex % nbColonneImageForet) * Largeur;

            int texY = (tileIndex / nbColonneImageForet) * Hauteur;

            this._sourceRect = new Rectangle(texX, texY, Largeur, Hauteur);
        }

      
    }
}