<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/TR/REC-html40">
  <xsl:output encoding="UTF-8" method="html" version="3.2" omit-xml-declaration="yes"/>
  <xsl:strip-space elements="*"/>
  <xsl:param name="engine"/>
  <xsl:include href="XSLCommonDDE.XSL"/>


  <xsl:template match="/">
    <HTML>
      
      <xsl:text disable-output-escaping="yes"/>
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
      
      
      <xsl:text disable-output-escaping="yes"/>      
      <BODY>
        <xsl:text disable-output-escaping="yes"/>
        <DIV class="container content" NAME="top" ID="top">
          <xsl:text disable-output-escaping="yes">
		  </xsl:text>
		  
          <H1>
            <xsl:if test="string-length($engine) > 0">
              <xsl:value-of select="$engine"/><xsl:text>&#32;</xsl:text>
            </xsl:if>
            <xsl:value-of select="/keyword_library/libraryInfo/@name"/>
          </H1>
          
          <xsl:text disable-output-escaping="yes">
		  </xsl:text>
          Last Updated:<script language="JavaScript">document.write(document.lastModified)</script>
          <BR/>
          <BR/>
          <xsl:apply-templates select="/keyword_library/libraryInfo" mode="full_description"/>
        </DIV>
        
        
        <BR/>
        
        
        <xsl:text disable-output-escaping="yes">
		</xsl:text>		
        <DIV NAME="list" ID="list">
          <xsl:text disable-output-escaping="yes">
		  </xsl:text>
          <xsl:call-template name="list"/>
        </DIV>
        
        
        <xsl:text disable-output-escaping="yes">
		</xsl:text>
        <DIV NAME="detail" ID="detail">
          <xsl:text disable-output-escaping="yes">
		  </xsl:text>
          <xsl:call-template name="detail"/>
        </DIV>
        
        
        <xsl:text disable-output-escaping="yes">
		</xsl:text>
		
      </BODY>
      
    </HTML>
    <xsl:text disable-output-escaping="yes">
	</xsl:text>
  </xsl:template>
  
  
  <xsl:template match="*" mode="full_description">
    <DIV NAME="list" ID="short_desc">
      <xsl:copy-of select="./description/short_desc"/>
    </DIV>
    <BR/>
    <xsl:text disable-output-escaping="yes">
	</xsl:text>
    <DIV NAME="list" ID="detail_desc">
      <xsl:if test="count(./description/detailed_desc) > 0">
        <xsl:copy-of select="./description/detailed_desc"/>
        <BR/>
        <xsl:text disable-output-escaping="yes">
		</xsl:text>
      </xsl:if>
    </DIV>
	<xsl:for-each select="./description/note">
		<xsl:apply-templates select="." mode="note_out"/>
	</xsl:for-each>
  </xsl:template>
  
  <xsl:template match="*" mode="note_out">
    <DIV ID="note_desc">
		<xsl:text disable-output-escaping="yes">&lt;SPAN CLASS='note'>NOTE:&lt;/SPAN>&lt;BR/></xsl:text>

		<xsl:for-each select="./engines/engine">
			<xsl:variable name="hasDomainsInfo" select="domains"/>
			<xsl:apply-templates select="." mode="enginelinks_out"/>
			<xsl:if test="$hasDomainsInfo">
				<xsl:for-each select="domains/domain">
					<xsl:apply-templates select="." mode="domainlinks_out"/>
				</xsl:for-each>						
			</xsl:if>
			<xsl:text disable-output-escaping="yes"> : </xsl:text>
		</xsl:for-each>
		
		<xsl:if test="count(./value) > 0">
			<xsl:copy-of select="./value"/><BR/>
		</xsl:if>
    </DIV>
  </xsl:template>  
  
  <xsl:template name="list">
    <xsl:call-template name="iconLegendLine" />            
    <TABLE class="table">
    	<thead class="thead-inverse">
    		<tr>
		      <th>Keyword Name</th>
		      <th>Supported Engines</th>
		      <th>Description</th>
		    </tr>
		</thead>
		<tbody>
	    <xsl:choose>
	        <xsl:when test="string-length($engine) > 0">
	            <xsl:for-each select="//keyword[engines/engine/@tool=$engine or engines/engine/@vendor='SAFS']">
             	    <xsl:sort select="@displayText"/>
                   	<xsl:apply-templates select="." mode="list_out"/>
	            </xsl:for-each>
	        </xsl:when>
	        
	        <xsl:otherwise>	        
	            <xsl:for-each select="//keyword">
             	    <xsl:sort select="@displayText"/>
                   	<xsl:apply-templates select="." mode="list_out"/>
	            </xsl:for-each>
	        </xsl:otherwise>
	        
	    </xsl:choose>
	    </tbody>
    </TABLE>
    <HR/>
  </xsl:template>
        
        
  <xsl:template match="*" mode="list_out">
        <TR>
          <TD nowrap='true' width="27%">
            <xsl:if test="starts-with(@deprecated,'Y')">
              <xsl:text disable-output-escaping="yes">&lt;A ID="linkdeprecated" NAME='list_</xsl:text>
            </xsl:if>
            <xsl:if test="not(starts-with(@deprecated,'Y'))">
              <xsl:text disable-output-escaping="yes">&lt;A NAME='list_</xsl:text>
            </xsl:if>
            <xsl:value-of select="@displayText"/>
            <xsl:text disable-output-escaping="yes">' HREF='#detail_</xsl:text>
            <xsl:value-of select="@displayText"/>
            <xsl:text disable-output-escaping="yes">' ></xsl:text>
            <xsl:value-of select="@displayText"/>
            <xsl:text disable-output-escaping="yes">&lt;/A></xsl:text>
          </TD>
          
          <TD width="12%" align="CENTER" >
            <xsl:call-template name="keywordEngineLinks" />            
          </TD>
          
          <xsl:text disable-output-escaping="yes">
		  </xsl:text>
          <TD>
            <xsl:if test="starts-with(@deprecated,'Y')">Deprecated For:<xsl:value-of select="@deprecatedFor"/></xsl:if>
            <xsl:if test="not(starts-with(@deprecated,'Y'))">
              <xsl:value-of select="./description/short_desc"/>
            </xsl:if>
          </TD>
          
        </TR>
  </xsl:template>


  <xsl:template name="detail">  
      <xsl:choose>
  	        <xsl:when test="string-length($engine) > 0">
  	            <xsl:for-each select="//keyword[engines/engine/@tool=$engine or engines/engine/@vendor='SAFS']">
               	    <xsl:sort select="@displayText"/>
                     	<xsl:apply-templates select="." mode="detail_out"/>
  	            </xsl:for-each>
  	        </xsl:when>
  	        <xsl:otherwise>	        
  	            <xsl:for-each select="//keyword">
               	    <xsl:sort select="@displayText"/>
                     	<xsl:apply-templates select="." mode="detail_out"/>
  	            </xsl:for-each>
  	        </xsl:otherwise>
  	  </xsl:choose>
   </xsl:template>
   

   <xsl:template match="*" mode="detail_out" >
      <H3>
        <xsl:text disable-output-escaping="yes">&lt;A NAME='detail_</xsl:text>
        <xsl:value-of select="@displayText"/>
        <xsl:text disable-output-escaping="yes">'>&lt;/A></xsl:text>
        <xsl:if test="string-length($engine) > 0">
            <xsl:value-of select="$engine"/><xsl:text>&#32;</xsl:text>
        </xsl:if>
        <xsl:value-of select="/keyword_library/libraryInfo/@name"/>
        <xsl:text>::</xsl:text>
        <xsl:if test="starts-with(@deprecated,'Y')">
          <SPAN ID="deprecated">
            <xsl:value-of select="@displayText"/>
          </SPAN>
          <xsl:text>  (deprecated for: </xsl:text>
          <B>
            <xsl:value-of select="@deprecatedFor"/>
          </B>
          <xsl:text>)</xsl:text>
        </xsl:if>
        <xsl:if test="not(starts-with(@deprecated,'Y'))">
          <BIG>
            <xsl:value-of select="@displayText"/>
          </BIG>
        </xsl:if>
        
        
      </H3> 
	  
	  <xsl:text>   </xsl:text>
      <xsl:call-template name="keywordEngineLinks" />
        
	  <xsl:text disable-output-escaping="yes">
	  </xsl:text>
      <xsl:apply-templates select="." mode="full_description"/>
      <BR/>
      <DIV NAME="list" ID="other">
        <p><B>Fields: </B><SMALL>[ ]=Optional with Default Value</SMALL></p>
      <code class="safs">
      <xsl:text disable-output-escaping="yes">
	  </xsl:text><xsl:text disable-output-escaping="yes">&lt;OL start="</xsl:text>
          <xsl:if test="/keyword_library/libraryType/@type = 'COMPONENT'">
              <xsl:text disable-output-escaping="yes">5" ></xsl:text>
          </xsl:if>
	  <xsl:if test="/keyword_library/libraryType/@type = 'DRIVER'">
              <xsl:text disable-output-escaping="yes">3" ></xsl:text>
          </xsl:if>
	  <xsl:if test="/keyword_library/libraryType/@type = 'ENGINE'">
              <xsl:text disable-output-escaping="yes">3" ></xsl:text>
          </xsl:if>
          <xsl:for-each select="./parameters/parameter">
          <LI>
            <xsl:if test="starts-with(@optional,'Y')">
              <xsl:text>[ </xsl:text>
              <B>
                <xsl:value-of select="@displayText"/>
              </B>
              <xsl:text> = </xsl:text>
              <xsl:value-of select="@default"/>
              <xsl:text> ]</xsl:text>
            </xsl:if>
            <xsl:if test="not(starts-with(@optional,'Y'))">
              <B>
                <xsl:value-of select="@displayText"/>
              </B>
            </xsl:if>
            <BR/>
            <xsl:text disable-output-escaping="yes">
		  </xsl:text>
            <xsl:apply-templates select="." mode="full_description"/>
            <xsl:text disable-output-escaping="yes">
		  </xsl:text>
          </LI>
        </xsl:for-each><xsl:text disable-output-escaping="yes">&lt;/OL ></xsl:text>
        </code>
        
        <br />
        
        <p><B>Examples:</B></p>
        
        <code class="safs">
		<xsl:text disable-output-escaping="yes"/><UL>
          <xsl:for-each select="./examples/example">
            <LI>
              <B>
                <xsl:copy-of select="./usage"/>
              </B>
              <BR/>
              <xsl:text disable-output-escaping="yes"/>
              <xsl:apply-templates select="." mode="full_description"/>
            </LI>
          </xsl:for-each>
        </UL>
        </code>
        
        <br />
        
        <xsl:call-template name="iconLegendLine"/>
        <HR />
      </DIV>
  </xsl:template>

</xsl:stylesheet>
