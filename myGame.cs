using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace FishGame
{
    public class myGame : Game
    {
        private XMLManager<JeuXML>  deserializer_JEU; 
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private JeuXML JeuDeserializer;
        //private Jeu _jeu;
        private KeyboardState _previousKeyboardState;

        private Texture2D _texPecheur, _texCarte, _texPoisson;

        public string pseudo;

        public myGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            
            
            deserializer_JEU = new XMLManager<JeuXML>();
            JeuDeserializer = deserializer_JEU.Load("./xml/UMLFin.xml");
            //JeuDeserializer.MonNiveau.InitialiserNiveau();
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
            //_jeu = new Jeu();
            _previousKeyboardState = Keyboard.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _texPecheur = Content.Load<Texture2D>("pecheur");
            _texCarte = Content.Load<Texture2D>("carte");
            _texPoisson = Content.Load<Texture2D>("poisson");
            
            //JeuDeserializer.MonNiveau._texturePecheur = Content.Load<Texture2D>("pecheur");
            //JeuDeserializer.MonNiveau._textureCarte = Content.Load<Texture2D>("carte");
            //JeuDeserializer.MonNiveau._texturePoisson = Content.Load<Texture2D>("poisson");

            JeuDeserializer.Démarrage(_texCarte, _texPecheur, _texPoisson);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKb = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                currentKb.IsKeyDown(Keys.Escape))
                Exit();

            if (JeuDeserializer.Etat == JeuXML.EtatJeu.JOUE)   //if (_jeu.Etat == EtatDuJeux.Playing)
            {
                JeuDeserializer.MonNiveau._Pecheur.GererMonEntree(currentKb, _previousKeyboardState, JeuDeserializer.MonNiveau);    //_jeu.niveauCourant.joueur.GererEntree(currentKb, _previousKeyboardState, _jeu.niveauCourant);
            }

            if ((JeuDeserializer.Etat== JeuXML.EtatJeu.PERDU || JeuDeserializer.Etat == JeuXML.EtatJeu.GAGNER ) &&       
                (currentKb.IsKeyDown(Keys.Enter) && (_previousKeyboardState.IsKeyUp(Keys.Enter))))     // if ((_jeu.Etat == EtatDuJeux.GameOver || _jeu.Etat == EtatDuJeux.GameWon) && currentKb.IsKeyDown(Keys.Enter) && _previousKeyboardState.IsKeyUp(Keys.Enter))
            {
                JeuDeserializer.Démarrage(_texCarte, _texPecheur, _texPoisson);
            }

            if (JeuDeserializer.Etat== JeuXML.EtatJeu.PERDU || JeuDeserializer.Etat == JeuXML.EtatJeu.GAGNER)
            {

                Highscore score  = new Highscore();
                Pseudos pseudo = new Pseudos(JeuDeserializer.MonNiveau._Pecheur.Nom, 30 -JeuDeserializer.MonNiveau._Pecheur.MouvementRestant);
                score.ajouterPseudo(pseudo);
                   
                XMLManager<Highscore> highscoreManager = new XMLManager<Highscore>();
                highscoreManager.Save("../../../xml/sauvegardePartie.xml", score);
            }

            JeuDeserializer.Update();
            _previousKeyboardState = currentKb;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            _spriteBatch.Begin();
            if (JeuDeserializer.MonNiveau != null)
            {
                //JeuXML.unNiveau niv = JeuDeserializer.MonNiveau;  //Remettre Niveau

                if (JeuDeserializer.MonNiveau.grille != null)
                {
                    foreach (Tuile t in JeuDeserializer.MonNiveau.grille) //Tuile t in niv.grillle
                    {
                        Vector2 pos = new Vector2(t.coordonnees.X * Tuile.Largeur, t.coordonnees.Y * Tuile.Hauteur);  //Vector2 pos = new Vector2(t.coordonnees.X * Tuile.Largeur, t.coordonnees.Y * Tuile.Hauteur);

                        _spriteBatch.Draw(t._texture, pos, t._sourceRect, Color.White); //pareillle
                    }
                }

                if (JeuDeserializer.MonNiveau._Poisson != null && JeuDeserializer.MonNiveau._Poisson.EstVisible) //(niv.poisson != null && niv.poisson.estVisible)
                {
                    int x = JeuDeserializer.MonNiveau._Poisson.Position.Coordonnes._PosX * Tuile.Largeur;   //int x = niv.poisson.position.coordonnees.X * Tuile.Largeur;
                    int y = JeuDeserializer.MonNiveau._Poisson.Position.Coordonnes._PosY * Tuile.Hauteur;  //int y = niv.poisson.position.coordonnees.Y * Tuile.Hauteur;

                    Rectangle destination = new Rectangle(x, y, 100, 60);

                    _spriteBatch.Draw(JeuDeserializer.MonNiveau._texturePoisson, destination, Color.White);  //(niv.poisson._texture, destination, Color.White);
                }

                if (JeuDeserializer.MonNiveau._Pecheur != null)   //(niv.joueur != null)
                { 
                    Vector2 pos = new Vector2(
                        JeuDeserializer.MonNiveau._Pecheur.PositionActuelle.Coordonnes._PosX * Tuile.Largeur,  //niv.joueur.positionActuelle.coordonnees.X * Tuile.Largeur,
                        JeuDeserializer.MonNiveau._Pecheur.PositionActuelle.Coordonnes._PosY * Tuile.Hauteur  //niv.joueur.positionActuelle.coordonnees.Y * Tuile.Hauteur
                    );
                    Rectangle desti = new Rectangle(0, 0, JeuDeserializer.MonNiveau._texturePecheur.Width/7,JeuDeserializer.MonNiveau._texturePecheur.Height/4);
                    _spriteBatch.Draw(JeuDeserializer.MonNiveau._texturePecheur, pos, desti, Color.White);  //(niv.joueur.texture, pos, niv.joueur.sourceRect, Color.White);
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}