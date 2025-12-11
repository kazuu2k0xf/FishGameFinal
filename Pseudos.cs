using System;
using System.Xml.Serialization;

namespace FishGame;

[XmlRoot("listespseudos", Namespace = "http://www.l3miage.fr/HighScores")]
[Serializable]
public class Pseudos
{
    private string nom;
    private int nbPas;

    public Pseudos()
    {
        
    }

    public Pseudos(string nom, int nbPas)
    {
        Nom = nom;
        NbPas = nbPas;
    }
    
    [XmlElement("nom")] public string Nom { get; set; }
    [XmlElement("nbpas")] public int NbPas { get; set; }
    
}