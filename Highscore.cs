using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FishGame;

[XmlRoot("highscore", Namespace = "http://www.l3miage.fr/HighScores")]
[Serializable]
public class Highscore
{
    [XmlElement("listespseudos")] public List<Pseudos> ListPseudos { get; set; }

    public Highscore()
    {
        ListPseudos = new List<Pseudos>();
    }

    public void ajouterPseudo(Pseudos p)
    {
        ListPseudos.Add(p);
    }
}