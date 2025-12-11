using System.Xml;

namespace FishGame;
/**
 * Cette classe permet de sauvegarder (ecrire) le score du joueur dans le xml sauvegardePartie.xml
 */
public class HighscoreDom
{
    private XmlDocument doc;
    private XmlNode root;
    private XmlNamespaceManager nsmgr;
    
    public HighscoreDom(string filename)
    {
        doc = new XmlDocument();
        doc.Load(filename);
        root = doc.DocumentElement;
        nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("hs","http://www.l3miage.fr/HighScores");
    }

    /**
     * Permet d'ajouter un score au xml
     */
    public void AjouterScore( int nbPasUtiliser)
    {
        string filePath = "../../../xml/sauvegardePartie.xml"; 
        
        XmlElement nvPseudo = doc.CreateElement(root.Prefix, "pseudo", root.NamespaceURI);

        XmlElement nvNom = doc.CreateElement(root.Prefix, "nom", root.NamespaceURI);
        nvNom.InnerText = nbPlayer();
        nvPseudo.AppendChild(nvNom);

        XmlElement nvNbPasUtiliser = doc.CreateElement(root.Prefix, "nbpas", root.NamespaceURI);
        nvNbPasUtiliser.InnerText = nbPasUtiliser.ToString();
        nvPseudo.AppendChild(nvNbPasUtiliser);
        
        XmlNode listepseudosNode = root.SelectSingleNode("hs:listespseudos", nsmgr);

        listepseudosNode.AppendChild(nvPseudo);

        doc.Save(filePath);
    }

    public string nbPlayer()
    {
        XmlNode listepseudos = root.SelectSingleNode("hs:listespseudos", nsmgr);
        int nbplayer = 0;
        foreach (XmlNode pseudos in listepseudos)
        {
            nbplayer++;
        }

        nbplayer += 1;
        return "Player"+ nbplayer;
    }
}