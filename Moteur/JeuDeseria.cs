using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace FishGame;


[XmlRoot("jeu", Namespace ="http://www.l3miage.fr/JeuPoissonEnAccordUML")] 
[Serializable]
public class JeuDeseria
{
    [XmlElement("niveauCourant")] public unNiveau MonNiveau;
    [XmlElement("etatJeu")] public EtatJeu Etat;
    public class JoueurPecheur
    {
        [XmlElement("nom")] public String Nom { get; set; }
        [XmlElement("coordonnees")] public int coordonnees;
   
        [XmlElement("mouvementRestant")] public int MouvementRestant;
        [XmlElement("positionActuelle")] public uneTuile PositionActuelle;
        [XmlElement("aPoisson")] public bool aPoisson;
        [XmlElement("aOutil")] public bool aOutil;
        
        
        /**
        * Sert a la direction du pecheur ( conversion touche vers vecteur direction )
        */
        public void GererMonEntree(KeyboardState currentKb, KeyboardState previousKb, unNiveau niveau)
        {
            int dx = 0;
            int dy = 0;

            if (currentKb.IsKeyDown(Keys.Up) && previousKb.IsKeyUp(Keys.Up)) dy = -1;
            else if (currentKb.IsKeyDown(Keys.Down) && previousKb.IsKeyUp(Keys.Down)) dy = 1;
            else if (currentKb.IsKeyDown(Keys.Left) && previousKb.IsKeyUp(Keys.Left)) dx = -1;
            else if (currentKb.IsKeyDown(Keys.Right) && previousKb.IsKeyUp(Keys.Right)) dx = 1;

            if (dx != 0 || dy != 0)
            {
                this.seDéplacer(dx, dy, niveau);
            }
        }
        
        /**
        * Deplacer le pecheur vers une case avec verification de si le déplacement est valide ou pas
        * decremente le compteur de pas pour chaque déplacement
        */
        public void seDéplacer(int x, int y, unNiveau niveau)
        {
            if (MouvementRestant <= 0) return;

            int nouveauX = PositionActuelle.Coordonnes._PosX + x;
            int nouveauY = PositionActuelle.Coordonnes._PosY + y;

            //Verif si le déplacement es bien dans la map 
            // si déplacement possible, verification si case est pas bloquante
            // si possible on déplace et on décrémente le compteur de mouvement
            if (nouveauX >= 0 && nouveauX < 8 && nouveauY >= 0 && nouveauY < 8)
            {
                Tuile cible =  niveau.grille[nouveauX, nouveauY];  
               
                if (!cible.estBloquant)
                {
                    PositionActuelle.Coordonnes._PosX = cible.coordonnees.X;
                    PositionActuelle.Coordonnes._PosY = cible.coordonnees.Y;
                 
                    MouvementRestant--;   
                }
            }
        }
    }

    public class unPoisson
    {
        [XmlElement("nom")] public String Nom;

        [XmlElement("position")] public uneTuile Position;
        [XmlElement("estVisible")] public bool EstVisible; 
        
    }
    
    public class unNiveau
    {
        
        [XmlElement("mouvementMax")] public int mouvementMax;
        [XmlElement("joueur")] public JoueurPecheur _Pecheur;    
        [XmlElement("poisson")] public unPoisson _Poisson;
        [XmlElement("caseSortie")] public uneTuile CaseSortie;
        [XmlElement("collones")] public int Collones;
        [XmlElement("lignes")] public int Lignes;
        
        [XmlIgnore] public Texture2D _texturePecheur ;
        [XmlIgnore] public Texture2D _texturePoisson ;
        [XmlIgnore] public Texture2D _textureCarte ;
        [XmlIgnore] public Tuile[,] grille;
        
        /**
        * Verifie interaction entre pecheur et poisson ( ramassage du poisson )
        */
        public void MAJEtatPoissonEtAPoisson()
        {
            if (_Poisson.EstVisible && (_Pecheur.PositionActuelle.Coordonnes._PosX == _Poisson.Position.Coordonnes._PosX) && (_Pecheur.PositionActuelle.Coordonnes._PosY == _Poisson.Position.Coordonnes._PosY))
            {
                _Poisson.EstVisible = false;     
                _Pecheur.aPoisson = true;          
            }
        }
        /**
         * Construction du niveau
         * definition des collisions
         * création grille (8x8 -- 64 tuile)
         * chargement position du pecheur et poisson par le parseur
         */
        public void InitialiserNiveau()
        {
            bool[,] arbreCollision = new bool[Collones, Lignes];

   
            arbreCollision[0, 0] = true;
            arbreCollision[1, 0] = true;
            arbreCollision[2, 0] = true;
            arbreCollision[3, 0] = true;
            arbreCollision[0, 1] = true;
            arbreCollision[0, 7] = true;
            arbreCollision[6, 0] = true;
            arbreCollision[7, 0] = true;
            arbreCollision[1, 1] = true;
            arbreCollision[2, 1] = true;
            arbreCollision[2, 1] = true;
            arbreCollision[3, 1] = true;
            arbreCollision[1, 3] = true;
            arbreCollision[2, 5] = true;
            arbreCollision[3, 5] = true;
            arbreCollision[3, 4] = true;
            arbreCollision[2, 6] = true;
            arbreCollision[3, 6] = true;
            arbreCollision[2, 5] = true;

            // création grille de 0 a 63 car 8X8
            grille = new Tuile[Collones, Lignes];

            for (int x = 0; x < Lignes; x++)
            {
                for (int y = 0; y < Lignes; y++)
                {
                    int tileIndex = (y * Lignes) + x;
                    bool estBloquant = arbreCollision[x, y];

                    grille[x, y] = new Tuile(new Point(x,y), estBloquant, _textureCarte, tileIndex);
                }
            }
        }
    }

    public class uneTuile
    {
        
        public class unPoint{
            [XmlAttribute("posX")] public int _PosX { set; get; }
            [XmlAttribute("posY")] public int _PosY { set; get; }
            
            public unPoint(){}
            
        }
        
        [XmlIgnore] public Rectangle _sourceRect;
        [XmlElement("coordonnees")] public unPoint Coordonnes { set; get; }
        [XmlElement("estBloquant")] public bool _EstBloquant { set; get; }
        [XmlElement("constLargeur")] public int _ConstLargeur { set; get; }
        [XmlElement("constHauteur")] public int _ConstHauteur { set; get; }

        [XmlIgnore] public Texture2D _texture;
        
    }
    // Mon enumération pour l'état du jeu
    public enum EtatJeu 
    {
        [XmlEnum("Playing")] JOUE,
        [XmlEnum("GameOver")] PERDU,
        [XmlEnum("GameWon")] GAGNER
    }
    
    // Getter pour récupérer l'état du jeu 
    public static EtatJeu getEtatJeu(String valeur)
    {
        switch(valeur)
        {
            case "Playing" : return EtatJeu.JOUE;
            case "GameOver" : return EtatJeu.PERDU;
            case "GameWon" : return EtatJeu.GAGNER;
        }
        return EtatJeu.JOUE;
    }
    
    /**
     * Initialise une nouvelle partie ou redemarre une partie existante.
     * tCarte texture de la carte
     * tPech texture pecheur
     * tPech texture poisson
     */
    public void Démarrage(Texture2D tCarte, Texture2D tPech, Texture2D tPois)
    {
        MonNiveau._texturePecheur = tPech;
        MonNiveau._texturePoisson = tPois;
        MonNiveau._textureCarte = tCarte;
        
        MonNiveau.InitialiserNiveau();

        // Non nécéssaire car déjà récupérer avec le deserializer
        ParserJeux parserJeux = new ParserJeux();
        List<int> posDepart = parserJeux.ParserPositionJoueur("./xml/Fishgame.xml"); 
        MonNiveau._Pecheur.PositionActuelle.Coordonnes._PosX = posDepart[0];
        MonNiveau._Pecheur.PositionActuelle.Coordonnes._PosY = posDepart[1];
        //Mais on le fait pour montrer qu'ont sait utiliser un parser


        MonNiveau._Pecheur.MouvementRestant = MonNiveau.mouvementMax;   
        MonNiveau._Pecheur.aPoisson = false;
        MonNiveau._Poisson.EstVisible = true;

        Etat = EtatJeu.JOUE;
    }
    

    /**
     *    Met a jour l'etat du jeu + verifie condition de victoire / defaite
     */
    public void MAJEtatJeu()
    {
        if (Etat != EtatJeu.JOUE) return;

        MonNiveau.MAJEtatPoissonEtAPoisson();

        if (MonNiveau._Pecheur.MouvementRestant <= 0)
        {
            Etat = EtatJeu.PERDU;
        }
        else if (MonNiveau._Pecheur.aPoisson &&
                 (MonNiveau._Pecheur.PositionActuelle.Coordonnes._PosX == MonNiveau.CaseSortie.Coordonnes._PosX) && (MonNiveau._Pecheur.PositionActuelle.Coordonnes._PosY == MonNiveau.CaseSortie.Coordonnes._PosY) )
        {
            Etat = EtatJeu.GAGNER;
        }
    }
}