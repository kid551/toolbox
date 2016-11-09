<xsl:stylesheet version='1.0'
		xmlns:xsl='http://www.w3.org/1999/XSL/Transform'
		xmlns='http://www.w3.org/TR/REC-html40'>
    
    <xsl:output encoding="UTF-8" method="html" version="3.2" omit-xml-declaration="yes"/>
    <xsl:strip-space elements="*" />
    <xsl:param name="engine"/>
    
    <xsl:template match="/" >    
    	<HTML><xsl:text disable-output-escaping="yes">&#xA;</xsl:text>
    	<HEAD><TITLE><xsl:value-of select="$engine"/> Reference Index</TITLE>
        <LINK rel="stylesheet" href="rrafs.css" type="text/css"/>
        <LINK rel="stylesheet" href="bootstrap.css" type="text/css"/>
    	</HEAD>
	<xsl:text disable-output-escaping="yes">&#xA;</xsl:text>
    	<BODY class="container content" id="bg_color">
	<xsl:text disable-output-escaping="yes">&#xA;</xsl:text>

        <xsl:for-each select="//file" >
	    <xsl:variable name="library" select="document(@href)" />
	    <xsl:choose>
	        <xsl:when test="string-length($engine) > 0">
	            <xsl:variable name="count1" select="count($library/keyword_library/keywords[keyword/engines/engine/@tool=$engine])"/>
	            <xsl:variable name="count2" select="count($library/keyword_library/keywords[keyword/engines/engine/@vendor='SAFS'])"/>
	            <xsl:if test="$count1 + $count2 > 0">
	                <xsl:apply-templates select="$library/keyword_library/libraryInfo" />
                </xsl:if>
	        </xsl:when>
	        <xsl:otherwise>	        	        
	            <xsl:apply-templates select="$library/keyword_library/libraryInfo" />
	        </xsl:otherwise>
	    </xsl:choose>
        </xsl:for-each>    

	</BODY></HTML><xsl:text disable-output-escaping="yes">&#xA;</xsl:text>
    </xsl:template>

    <xsl:template match="*">
	    <xsl:text disable-output-escaping="yes">&#60;A HREF='</xsl:text>
        <xsl:if test="string-length($engine) > 0">
            <xsl:value-of select="$engine"/>
        </xsl:if>
	    <xsl:value-of select="//libraryInfo/@name" />
	    <xsl:text disable-output-escaping="yes">Index.htm' TARGET='toc3' &#62;</xsl:text>
	    <B><xsl:value-of select="//libraryInfo/@name" /></B>
	    <xsl:text disable-output-escaping="yes">&#60;/A&#62;</xsl:text><BR /><xsl:text disable-output-escaping="yes">&#xA;</xsl:text>
    </xsl:template>

</xsl:stylesheet>
