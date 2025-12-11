using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;




namespace FishGame;



[Serializable]
[XmlRoot("jeu", Namespace ="http://www.l3miage.fr/JeuPoissonCordaUML")] //A modifier avec les vrai URI ET ROOT
public class JeuXML
{
    [XmlElement("niveauCourant")] public unNiveau MonNiveau;   //rajout static pour y avoir acces dans update
    [XmlElement("etatJeu")] public EtatJeu Etat;
    public class JoueurPecheur
    {
        [XmlElement("nom")] public String Nom { get; set; }
        [XmlElement("coordonnees")] public int coordonnees;
        //[XmlElement("texturePecheur")] public String textPecheur;
        [XmlElement("mouvementRestant")] public int MouvementRestant;
        [XmlElement("positionActuelle")] public uneTuile PositionActuelle;  //rajout static  pour deplacer
        [XmlElement("aPoisson")] public bool aPoisson;
        [XmlElement("aOutil")] public bool aOutil;
        
        [XmlIgnore] public Rectangle sourceRect;
        
        
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
        
        
        
        public void seDéplacer(int x, int y, unNiveau niveau)
        {
            if (MouvementRestant <= 0) return;

            int nouveauX = PositionActuelle.Coordonnes._PosX + x;
            int nouveauY = PositionActuelle.Coordonnes._PosY + y;

            //verif si le déplacement es bien dans la map 
            // si déplacement possible, verification si case est pas bloquante
            // si possible on déplace et on décrémente le compteur de mouvement
            if (nouveauX >= 0 && nouveauX < 8 && nouveauY >= 0 && nouveauY < 8)
            {
                Tuile cible =  niveau.grille[nouveauX, nouveauY];  //remettre Tuile si marche pas 
               
                if (!cible.estBloquant)
                {
                    PositionActuelle.Coordonnes._PosX = cible.coordonnees.X;
                    PositionActuelle.Coordonnes._PosY = cible.coordonnees.Y;
                    //PositionActuelle = cible;  //JeuXML.Niveau._Pecheur.PositionActuelle   //positionActuelle = cible;
                    MouvementRestant--;   //JeuXML.Niveau._Pecheur.MouvementRestant--; //mouvementsRestants--;
                }
            }
        }
    }

    public class unPoisson
    {
        [XmlElement("nom")] public String Nom;
        //[XmlElement("texturePoisson")] public String textPoisson;
        [XmlElement("position")] public uneTuile Position;
        [XmlElement("estVisible")] public bool EstVisible; 
        
    }
    public class unNiveau
    {
        
        
        [XmlElement("mouvementMax")] public int mouvementMax;
        [XmlElement("joueur")] public JoueurPecheur _Pecheur;    //rajout static pour y avoir acces dans update
        [XmlElement("poisson")] public unPoisson _Poisson;       //rajout static pour y avoir acces dans update
        [XmlElement("caseSortie")] public uneTuile CaseSortie;
        //[XmlElement("textPecheur")] public String textPecheur;
        //[XmlElement("textPecheur")] public String textPoisson;
        [XmlElement("collones")] public int Collones;
        [XmlElement("lignes")] public int Lignes;
        
        //A ignorer 
        [XmlIgnore] public Texture2D _texturePecheur ;
        [XmlIgnore] public Texture2D _texturePoisson ;
        [XmlIgnore] public Texture2D _textureCarte ;
        [XmlIgnore] public Tuile[,] grille;

        public void Updating()
        {
            if (_Poisson.EstVisible && (_Pecheur.PositionActuelle.Coordonnes._PosX == _Poisson.Position.Coordonnes._PosX) && (_Pecheur.PositionActuelle.Coordonnes._PosY == _Poisson.Position.Coordonnes._PosY))    //if (poisson.estVisible && poisson.position == joueur.positionActuelle)
            {
                _Poisson.EstVisible = false;      //poisson.estVisible = false;
                _Pecheur.aPoisson = true;          //joueur.aPoisson = true;
            }
        }

        public void InitialiserNiveau()
        {
            bool[,] arbreCollision = new bool[Collones, Lignes];

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
            grille = new Tuile[Collones, Lignes]; //remettre new Tuile si ca marche pas 

            for (int x = 0; x < Lignes; x++)
            {
                for (int y = 0; y < Lignes; y++)
                {
                    int tileIndex = (y * Lignes) + x;
                    bool estBloquant = arbreCollision[x, y];

                    grille[x, y] = new Tuile(new Point(x,y), estBloquant, _textureCarte, tileIndex); //remettre Tuile simarche pas
                }
            }
            
            //Parser pour la position du joueur3
            Tuile posJoueur = grille[ParserJeux.ParserPositionJoueur("./xml/niveau1.xml")[0], ParserJeux.ParserPositionJoueur("./xml/niveau1.xml")[1]];
            //joueur = new Pecheur("Joueur", posJoueur, ParserJeux.ParserNbPas("./xml/niveau1.xml"), _texPecheur);

            Tuile posPoisson = grille[ParserJeux.ParserPositionPoisson("./xml/niveau1.xml")[0], ParserJeux.ParserPositionPoisson("./xml/niveau1.xml")[1]];
            //poisson = new Poisson("Saumon", posPoisson, _texPoisson);

            //CaseSortie = grille[ParserJeux.ParserPositionFin("./xml/niveau1.xml")[0], ParserJeux.ParserPositionFin("./xml/niveau1.xml")[1]];
        }
        
    }

    public class uneTuile
    {
        public class unPoint{
            [XmlAttribute("posX")] public int _PosX { set; get; }
            [XmlAttribute("posY")] public int _PosY { set; get; }
            
            /*
            public unPoint(int posX, int posY)
            {
                _PosX = posX;
                _PosY = posY;
            }*/
            
        }
        
        [XmlIgnore] public Rectangle _sourceRect;
        [XmlElement("coordonnees")] public unPoint Coordonnes { set; get; }
        [XmlElement("estBloquant")] public bool _EstBloquant { set; get; }
        [XmlElement("constLargeur")] public int _ConstLargeur { set; get; }
        [XmlElement("constHauteur")] public int _ConstHauteur { set; get; }

        [XmlIgnore] public Texture2D _texture;
        /*
        public uneTuile(unPoint pos,bool estBloquant, int constLargeur, int constHauteur )
        {
            Coordonnes = pos;
            _EstBloquant = estBloquant;
            _ConstLargeur = constLargeur;
            _ConstHauteur = constHauteur;
        
        
        
        public uneTuile(Point pos,bool estBloquant, Texture2D texture, int tileIndex )
        {
            Coordonnes._PosX = pos.X;
            Coordonnes._PosY = pos.X;
            _EstBloquant = estBloquant;
            _texture = texture;

            // découpe l'image c'est un 8x8 
            int nbColonneImageForet = texture.Width / _ConstLargeur;

            // recupération pos colonne par le decoupage
            int texX = (tileIndex % nbColonneImageForet) * _ConstLargeur;

            // recup pos ligne par le découpage
            int texY = (tileIndex / nbColonneImageForet) * _ConstHauteur;

            _sourceRect = new Rectangle(texX, texY, _ConstLargeur, _ConstHauteur);
        }
        
        
        public uneTuile(unPoint pos)
        {
            Coordonnes = pos;
        }}*/
    }

    public enum EtatJeu 
    {
        [XmlEnum("Playing")] JOUE,
        [XmlEnum("GameOver")] PERDU,
        [XmlEnum("GameWon")] GAGNER
    }
    
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
    
    
    public void Démarrage(Texture2D tCarte, Texture2D tPech, Texture2D tPois)
    {
        MonNiveau._texturePecheur = tPech; //il recupère deja grace a la deserialisation
        MonNiveau._texturePoisson = tPois;
        MonNiveau._textureCarte = tCarte;
        
        MonNiveau.InitialiserNiveau();
        
        List<int> posDepart = ParserJeux.ParserPositionJoueur("./xml/niveau1.xml"); 
        MonNiveau._Pecheur.PositionActuelle.Coordonnes._PosX = posDepart[0];
        MonNiveau._Pecheur.PositionActuelle.Coordonnes._PosY = posDepart[1];
        MonNiveau._Pecheur.MouvementRestant = MonNiveau.mouvementMax;   
        MonNiveau._Pecheur.aPoisson = false;
        MonNiveau._Poisson.EstVisible = true;

        Etat = EtatJeu.JOUE;
    }
    


    public void Update()
    {
        if (Etat != EtatJeu.JOUE) return;

        MonNiveau.Updating();

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
        

        
    
    
    
    
    /*
    public override string ToString() {
        String s = String.Empty;
        s += "Je suis un pecheur est mon nom est : " + MonNiveau.Pecheur.Nom;
        s += "\n Il me reste : " + MonNiveau.Pecheur.MouvementRestant + "restant.";
        s += "\n J'ai le poisson : " + MonNiveau.Pecheur.aPoisson;
        s += "\n J'ai mon outil : " + MonNiveau.Pecheur.aOutil;
        s += "\n Ma Position actuelle en X est : " + MonNiveau.Pecheur.PositionActuelle.Coordonnes._PosX;
        s += "\n Ma Position actuelle en Y est : " + MonNiveau.Pecheur.PositionActuelle.Coordonnes._PosY;
        return s;
    }
    */
    
}

