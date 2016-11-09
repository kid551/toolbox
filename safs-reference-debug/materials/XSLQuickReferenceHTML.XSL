<xsl:stylesheet version='1.0'
		xmlns:xsl='http://www.w3.org/1999/XSL/Transform'
		xmlns='http://www.w3.org/TR/REC-html40'>
    
    <xsl:output encoding="UTF-8" method="html" version="3.2" omit-xml-declaration="yes"/>
    <xsl:strip-space elements="*" />
    <xsl:param name="engine"/>
    <xsl:include href="XSLCommonDDE.XSL"/>

    <xsl:template match="/">
    <HTML>
      <HEAD>
        <TITLE>
        SAFS <xsl:value-of select="$engine"/> Quick Reference
        </TITLE>
        <LINK rel="stylesheet" href="rrafs.css" type="text/css"/>
        <LINK rel="stylesheet" href="bootstrap.css" type="text/css"/>
      </HEAD>
      <BODY id="bg_color">
        <DIV class="container content" NAME="top" ID="top">
          <CENTER>
          <H1>
	  SAFS <xsl:value-of select="$engine"/> Quick Reference
	  </H1>
          <SMALL>
          Last Updated:<script language="JavaScript">document.write(document.lastModified)</script>
          </SMALL>
          </CENTER>
          <P/>
          <xsl:call-template name="links"/>

          <xsl:apply-templates />

        </DIV>
        <HR/>
        <xsl:call-template name="links"/>
      </BODY>
    </HTML>
  </xsl:template>

    <xsl:template match="drivercommands" name="drivercommands">    
        <HR/>
        <A name="drivercommands"/>
        <B><BIG>DRIVER COMMANDS</BIG></B>
        <P/>
        <xsl:call-template name="link_reference"/>
    </xsl:template>

    <xsl:template match="componentfunctions" name="componentfunctions">    
        <HR/>
        <A name="componentfunctions"/>
        <B><BIG>COMPONENT FUNCTIONS</BIG></B><BR/>
        <SMALL><A href="#top">(back to top)</A></SMALL>
        <P/>
        <xsl:call-template name="link_reference"/>
    </xsl:template>

    <xsl:template match="enginecommands" name="enginecommands">    
        <HR/>
        <A name="enginecommands"/>
        <B><BIG>ENGINE COMMANDS</BIG></B>
        <P/>
        <xsl:call-template name="link_reference"/>
    </xsl:template>

    <xsl:template match="keywords" >
	    <xsl:choose>
	        <xsl:when test="string-length($engine) > 0">
	            <xsl:for-each select="//keyword[engines/engine/@tool=$engine or engines/engine/@vendor='SAFS']">
             	    <xsl:sort select="@displayText"/>
                   	<xsl:apply-templates select="." mode="keyword_out"/>
	            </xsl:for-each>
	        </xsl:when>
	        <xsl:otherwise>	        
	            <xsl:for-each select="//keyword">
             	    <xsl:sort select="@displayText"/>
                   	<xsl:apply-templates select="." mode="keyword_out"/>
	            </xsl:for-each>
	        </xsl:otherwise>
	    </xsl:choose>
    </xsl:template>

    <xsl:template match="keyword" mode="keyword_out">
	    <TR>
	    <TD width="30%" nowrap="true"><xsl:value-of select="@displayText" /></TD>
	    <TD width="12%" nowrap="true" align="CENTER"><xsl:call-template name="keywordEngineLinks" /></TD>
	    <TD><xsl:value-of select="normalize-space(description/short_desc)"/></TD>
	    </TR>
    </xsl:template>

    <xsl:template name="link_reference">
        <xsl:for-each select="file" >
	    <xsl:variable name="library" select="document(@href)" />	    
	    <xsl:choose>
	        <xsl:when test="string-length($engine) > 0">
	            <xsl:variable name="count1" select="count($library/keyword_library/keywords[keyword/engines/engine/@tool=$engine])"/>
	            <xsl:variable name="count2" select="count($library/keyword_library/keywords[keyword/engines/engine/@vendor='SAFS'])"/>
	            <xsl:if test="$count1 + $count2 > 0">
	                <xsl:apply-templates select="." mode="links_out"/>
                </xsl:if>
	        </xsl:when>
	        <xsl:otherwise>	        	        
	            <xsl:apply-templates select="." mode="links_out"/>
	        </xsl:otherwise>
	    </xsl:choose>
        </xsl:for-each>    
    </xsl:template>


    <xsl:template match="*" mode="links_out">
	    <xsl:variable name="library" select="document(@href)" />	    
	    <P/>
            <xsl:text disable-output-escaping="yes">&lt;A HREF='</xsl:text>
            <xsl:value-of select="$library/keyword_library/libraryInfo/@name"/>
            <xsl:text disable-output-escaping="yes">Reference.htm'></xsl:text>
	    <xsl:value-of select="$library/keyword_library/libraryInfo/@name"/>
            <xsl:text disable-output-escaping="yes">&lt;/A></xsl:text>	    
	    <xsl:call-template name="iconLegendLine"/>
	    <TABLE col="3" width="100%" cellpadding="2" border="1" >
	    <xsl:apply-templates select="$library/keyword_library/keywords" />	    
	    </TABLE>
    </xsl:template>        

    <xsl:template name="links">
        Goto: 
        <A href="#drivercommands">Driver Commands</A>, 
        <A href="#componentfunctions">Component Functions</A>, 
        <A href="#enginecommands">Engine Commands</A>, 
        <A href="RRAFSReference.htm">Detailed Reference Doc</A>
    </xsl:template>
    
</xsl:stylesheet>
