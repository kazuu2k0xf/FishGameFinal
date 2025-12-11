using System;
using System.Collections.Generic;
using System.Xml;

namespace FishGame;

/**
 * Classe qui permet de parser en XML reader un fichier xml
 */
public class ParserJeux
{
    /**
     * Méthode qui permet de parser la position de la case de départ d'un joueur
     * Prend en parametre le chemin du fichier xml
     * Retourne une list avec les coordonnées x et y
     */
    static public List<int> ParserPositionJoueur(String filename)
    {
        XmlReader reader = XmlReader.Create(filename);
        List<int> positionJoueur = new List<int>();

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == "joueur")
                    {
                        reader.ReadToDescendant("coordonnees");
                        reader.MoveToAttribute("posX");
                        positionJoueur.Add(int.Parse(reader.Value));
                        reader.MoveToNextAttribute();
                        positionJoueur.Add(int.Parse(reader.Value));
                    }
                    break;
            }
        }
        return positionJoueur;
    }
        
    /**
    * Méthode qui permet de parser la position de la case de départ d'un poisson
    * Prend en parametre le chemin du fichier xml
    * Retourne une list avec les coordonnées x et y
    */
    static public List<int> ParserPositionPoisson(String filename)
    {
        XmlReader reader = XmlReader.Create(filename);
        List<int> positionPoisson = new List<int>();

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == "positionActuelle")
                    {
                        reader.ReadToDescendant("coordonnees");
                        reader.MoveToAttribute("posX");
                        positionPoisson.Add(int.Parse(reader.Value));
                        reader.MoveToNextAttribute();
                        positionPoisson.Add(int.Parse(reader.Value));;
                    }
                    break;
            }
        }

        return positionPoisson;
    }
    
    /**
    * Méthode qui permet de parser la position de la case de fin
    * Prend en parametre le chemin du fichier xml
    * Retourne une list avec les coordonnées x et y
    */
    static public List<int> ParserPositionFin(String filename)
    {
        XmlReader reader = XmlReader.Create(filename);
        List<int> positionFin = new List<int>();
        
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == "caseSortie")
                    {
                        reader.ReadToDescendant("coordonnees");
                        reader.MoveToFirstAttribute();
                        positionFin.Add(int.Parse(reader.Value));
                        reader.MoveToNextAttribute();
                        positionFin.Add(int.Parse(reader.Value));
                    }
                    break;
            }
        }
        return positionFin;
    }
    
    /**
    * Méthode qui permet de parser le nombre de pas du joueur
    * Prend en parametre le chemin du fichier xml
    * Retourne un entier
     * dans ce cas on lutilise pas car on désérialise deja tous le document xml
    */
    static public int ParserNbPas(String filename)
    {
        XmlReader reader = XmlReader.Create(filename);
        int nbPas = 0;

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == "joueur")
                    {
                        nbPas = reader.ReadElementContentAsInt();
                      
                    }
                    break;
            }
        }
        return nbPas;
    }
}