using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace FishGame
{
    public class myGame : Game
    {
        private XMLManager<JeuDeseria>  deserializer_JEU; 
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private JeuDeseria JeuDeserializer;
        private KeyboardState _previousKeyboardState;

        private Texture2D _texPecheur, _texCarte, _texPoisson;

        private bool _scoreSauvegarde = false;

        public myGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            _graphics.PreferredBackBufferWidth = 1800;
            _graphics.PreferredBackBufferHeight = 1042;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            
            
            deserializer_JEU = new XMLManager<JeuDeseria>();
            JeuDeserializer = deserializer_JEU.Load("./xml/Fishgame.xml");
            if (JeuDeserializer == null)
            {
                throw new Exception("JeuDeserializer est null");
            }
            
            if (JeuDeserializer.MonNiveau == null)
            {
                throw new Exception("MonNiveau est null");
            }
            
        }

        protected override void Initialize()
        {

            _previousKeyboardState = Keyboard.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _texPecheur = Content.Load<Texture2D>("pecheur");
            _texCarte = Content.Load<Texture2D>("carte");
            _texPoisson = Content.Load<Texture2D>("poisson");


            JeuDeserializer.Démarrage(_texCarte, _texPecheur, _texPoisson);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKb = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                currentKb.IsKeyDown(Keys.Escape))
                Exit();

            if (JeuDeserializer.Etat == JeuDeseria.EtatJeu.JOUE) 
            {
                JeuDeserializer.MonNiveau._Pecheur.GererMonEntree(currentKb, _previousKeyboardState, JeuDeserializer.MonNiveau); 
            }

            if ((JeuDeserializer.Etat== JeuDeseria.EtatJeu.PERDU || JeuDeserializer.Etat == JeuDeseria.EtatJeu.GAGNER ) &&       
                (currentKb.IsKeyDown(Keys.Enter) && (_previousKeyboardState.IsKeyUp(Keys.Enter))))
            {
                _scoreSauvegarde = false;
                if (JeuDeserializer.Etat == JeuDeseria.EtatJeu.GAGNER && !_scoreSauvegarde)
                {
                    
                    HighscoreDom highscoreDom = new HighscoreDom("../../../xml/SauvegardePartie.xml");
                
                    highscoreDom.AjouterScore( 30 - JeuDeserializer.MonNiveau._Pecheur.MouvementRestant);
                    _scoreSauvegarde = true;
                }
                JeuDeserializer.Démarrage(_texCarte, _texPecheur, _texPoisson);
            }
            

            JeuDeserializer.MAJEtatJeu();
            _previousKeyboardState = currentKb;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            if (JeuDeserializer.MonNiveau != null)
            {
                if (JeuDeserializer.MonNiveau.grille != null)
                {
                    foreach (Tuile t in JeuDeserializer.MonNiveau.grille)
                    {
                        Vector2 pos = new Vector2(t.coordonnees.X * Tuile.Largeur, t.coordonnees.Y * Tuile.Hauteur);

                        _spriteBatch.Draw(t._texture, pos, t._sourceRect, Color.White); 
                    }
                }

                if (JeuDeserializer.MonNiveau._Poisson != null && JeuDeserializer.MonNiveau._Poisson.EstVisible)
                {
                    int x = JeuDeserializer.MonNiveau._Poisson.Position.Coordonnes._PosX * Tuile.Largeur;
                    int y = JeuDeserializer.MonNiveau._Poisson.Position.Coordonnes._PosY * Tuile.Hauteur;

                    Rectangle destination = new Rectangle(x, y, 100, 60);

                    _spriteBatch.Draw(JeuDeserializer.MonNiveau._texturePoisson, destination, Color.White);  
                }

                if (JeuDeserializer.MonNiveau._Pecheur != null)  
                { 
                    Vector2 pos = new Vector2(
                        JeuDeserializer.MonNiveau._Pecheur.PositionActuelle.Coordonnes._PosX * Tuile.Largeur,
                        JeuDeserializer.MonNiveau._Pecheur.PositionActuelle.Coordonnes._PosY * Tuile.Hauteur
                    );
                    Rectangle desti = new Rectangle(0, 0, JeuDeserializer.MonNiveau._texturePecheur.Width/7,JeuDeserializer.MonNiveau._texturePecheur.Height/4);
                    _spriteBatch.Draw(JeuDeserializer.MonNiveau._texturePecheur, pos, desti, Color.White);
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}