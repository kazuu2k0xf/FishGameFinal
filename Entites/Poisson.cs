using Microsoft.Xna.Framework.Graphics;

namespace FishGame
{
    public class Poisson
    {
        public string nom;
        public Tuile position;
        public bool estVisible = true;
        public Texture2D _texture;

        public Poisson(string nom, Tuile pos, Texture2D tex)
        {
            this.nom = nom;
            this.position = pos;
            this._texture = tex;
        }
    }
}