<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:hs="http://www.l3miage.fr/HighScores">

    <xsl:output method="html" encoding="UTF-8" indent="yes"/>

    <xsl:template match="/">
        <html>
            <head>
                <link href="../css/highscore.css" rel="stylesheet"/>
                <title>Scores - FishGame</title>
            </head>
            <body>
                <h1>Meilleurs Scores</h1>
                <table>
                    <tr>
                        <th>Pseudo</th>
                        <th>Score</th>
                    </tr>

                    <xsl:apply-templates select="hs:highscore/hs:listespseudos"/>
                </table>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="hs:pseudo">
        <tr>
            <td><xsl:value-of select="hs:nom"/></td>
            <td><xsl:value-of select="hs:nbpas"/></td>
        </tr>
    </xsl:template>

</xsl:stylesheet>