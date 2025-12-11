using System;
using System.Collections.Generic;
using System.Xml;

namespace FishGame;

public class ParserJeux
{
        static public List<int> ParserPositionJoueur(String filename)
        {
            //Parse le fichier
            XmlReader reader = XmlReader.Create(filename);
            List<int> positionJoueur = new List<int>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "Joueur")
                        {
                            reader.MoveToFirstAttribute();
                            positionJoueur.Add(int.Parse(reader.Value));
                            reader.MoveToNextAttribute();
                            positionJoueur.Add(int.Parse(reader.Value));
                        }
                        break;
                }
            }
            return positionJoueur;
        }
        
    static public List<int> ParserPositionPoisson(String filename)
    {
        //Parse le fichier
        XmlReader reader = XmlReader.Create(filename);
        List<int> positionPoisson = new List<int>();
        int x = 0;

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == "Poisson")
                    {
                        reader.MoveToFirstAttribute();
                        positionPoisson.Add(int.Parse(reader.Value));
                        reader.MoveToNextAttribute();
                        positionPoisson.Add(int.Parse(reader.Value));
                    }
                    break;
            }
        }

        return positionPoisson;
    }

    static public List<int> ParserPositionFin(String filename)
    {
        //Parse le fichier
        XmlReader reader = XmlReader.Create(filename);
        List<int> positionFin = new List<int>();

        int x = 0;

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == "Fin")
                    {
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
    
    static public int ParserNbPas(String filename)
    {
        //Parse le fichier
        XmlReader reader = XmlReader.Create(filename);
        int nbPas = 0;

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name == "Pas")
                    {
                        nbPas = reader.ReadElementContentAsInt();
                       
                    }
                    break;
            }
        }
        return nbPas;
    }
}