using System;
using System.Xml.Serialization;

namespace FishGame;
     /**
     * CLass pour la sérialisation de Pseudo a partir du schema de Highscores.xsd
     */
[XmlRoot("listespseudos", Namespace = "http://www.l3miage.fr/HighScores")]
[Serializable]
public class Pseudo
{
    private string nom;
    private int nbPas;

    public Pseudo()
    {
        
    }
/**Constructeur de Pseudo **/
    public Pseudo(string nom, int nbPas)
    {
        Nom = nom;
        NbPas = nbPas;
    }
    
    [XmlElement("nom")] public string Nom { get; set; }
    [XmlElement("nbpas")] public int NbPas { get; set; }
    
}