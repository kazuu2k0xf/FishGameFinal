using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FishGame;
/**
 * Classe qui permet la sérialisation d'un highscore
 */
[XmlRoot("highscore", Namespace = "http://www.l3miage.fr/HighScores")]
[Serializable]
public class Highscore
{
    [XmlElement("listespseudos")] public List<Pseudo> ListPseudos { get; set; }

    public Highscore()
    {
        ListPseudos = new List<Pseudo>();
    }
    
    /**
    * Methode qui permet d'ajouter un Pseudo dans la liste
    * Prend en parametre un Pseudo
    */
    public void ajouterPseudo(Pseudo p)
    {
        ListPseudos.Add(p);
    }
}