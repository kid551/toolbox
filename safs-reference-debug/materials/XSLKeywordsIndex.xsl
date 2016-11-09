<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/TR/REC-html40">
  <xsl:output encoding="UTF-8" method="html" version="3.2" omit-xml-declaration="yes"/>
  <xsl:strip-space elements="*"/>
  <xsl:param name="engine"/>
  <xsl:include href="XSLCommonDDE.XSL"/>


  <xsl:template match="/">
    <HTML>
    	<xsl:text disable-output-escaping="yes">
		</xsl:text>
      <HEAD>
        <TITLE>
        	<xsl:if test="string-length($engine) > 0">
            	<xsl:value-of select="$engine"/><xsl:text>&#32;</xsl:text>
          	</xsl:if>
          	<xsl:value-of select="/keyword_library/libraryInfo/@name"/>
        </TITLE>
        
        <LINK rel="stylesheet" href="rrafs.css" type="text/css"/>
        <LINK rel="stylesheet" href="bootstrap.css" type="text/css"/>
      </HEAD>
	<xsl:text disable-output-escaping="yes">
	</xsl:text>
      <BODY id="bg_color">
        <xsl:text disable-output-escaping="yes">
</xsl:text>
        <DIV class="container content" NAME="top" ID="top">
          <xsl:text disable-output-escaping="yes">
</xsl:text>
          <xsl:text disable-output-escaping="yes">&lt;A HREF='</xsl:text>
          
             <xsl:if test="string-length($engine) > 0">
                 <xsl:value-of select="$engine"/>
             </xsl:if>
             <xsl:value-of select="/keyword_library/libraryInfo/@name"/>
          
          <xsl:text disable-output-escaping="yes">Reference.htm' TARGET='content' ></xsl:text>
          <H4>
          <xsl:if test="string-length($engine) > 0">
              <xsl:value-of select="$engine"/><xsl:text>&#32;</xsl:text>
          </xsl:if>
 		  <xsl:value-of select="/keyword_library/libraryInfo/@name" />
		  </H4>
		  <xsl:text disable-output-escaping="yes">&lt;/A></xsl:text>
          <xsl:text disable-output-escaping="yes"/>
          <SMALL>Last Updated:<script language="JavaScript">document.write(document.lastModified)</script><DIV NAME="list" ID="deprecated">(RED = Deprecated)</DIV></SMALL>
          <BR/>
          <TABLE col="2" width="100%">
          <xsl:call-template name="list"/>
          </TABLE>
        </DIV>
        <xsl:text disable-output-escaping="yes"/>
      </BODY>
    </HTML>
    <xsl:text disable-output-escaping="yes"/>
  </xsl:template>
  
  
  <xsl:template name="list">
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

  <xsl:template match="*" mode="keyword_out">
      <TR><TD>
      <xsl:if test="starts-with(@deprecated,'Y')">
        <xsl:text disable-output-escaping="yes">&lt;A ID="linkdeprecated" HREF='</xsl:text>
      </xsl:if>
      <xsl:if test="not(starts-with(@deprecated,'Y'))">
        <xsl:text disable-output-escaping="yes">&lt;A HREF='</xsl:text>
      </xsl:if>
      <xsl:if test="string-length($engine) > 0">
          <xsl:value-of select="$engine"/>
      </xsl:if>
      <xsl:value-of select="//libraryInfo/@name"/>
      <xsl:text disable-output-escaping="yes">Reference.htm#detail_</xsl:text>
      <xsl:value-of select="@displayText"/>
      <xsl:text disable-output-escaping="yes">' TARGET='content' ></xsl:text>
      <xsl:value-of select="@displayText"/>
      <xsl:text disable-output-escaping="yes">&lt;/A>  </xsl:text>
      </TD><TD>
      <xsl:call-template name="keywordEngineLinks" />
      </TD>
      </TR>
  </xsl:template>
  
</xsl:stylesheet>
