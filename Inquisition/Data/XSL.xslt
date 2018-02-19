<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="Report">
    <html>
      <head>
        <style>
          body {
          font-family: Arial;
          }

          table {
          border: 1px solid #DDDDDD;
          width: 100%;
          border-collapse: collapse;
          }

          th, td {
          min-width: 175px;
          padding: 5px;
          text-align: center;
          border: 1px solid #DDDDDD;
          }

          th {
          background-color: #DDDDDD;
          font-weight: bold;
          }
        </style>
      </head>
      <body>
        <h2>Critical Exception Report</h2>
        <table>
          <tr>
            <th>Guild Name</th>
            <th>Guild ID</th>
          </tr>
          <tr>
            <td>
              <xsl:value-of select="GuildName"/>
            </td>
            <td>
              <xsl:value-of select="GuildID"/>
            </td>
          </tr>
          <tr>
            <th>Username</th>
            <th>User ID</th>
            <th>Channel</th>
            <th>Message</th>
          </tr>
          <tr>
            <td>
              <xsl:value-of select="UserName"/>
            </td>
            <td>
              <xsl:value-of select="UserID"/>
            </td>
            <td>
              <xsl:value-of select="Channel"/>
            </td>
            <td>
              <xsl:value-of select="Message"/>
            </td>
          </tr>
        </table>
        <h2>Exception details</h2>
        <table>
          <tr>
            <th>Error Message</th>
            <td>
              <xsl:value-of select="ErrorMessage"/>
            </td>
          </tr>
          <tr>
            <th>Stack Trace</th>
            <td>
              <xsl:value-of select="StackTrace"/>
            </td>
          </tr>
          <tr>
            <th>File Path</th>
            <td>
              <xsl:value-of select="Path"/>
            </td>
          </tr>
        </table>
        <xsl:if test="InnerExceptions">
          <h2>Inner Exceptions</h2>
          <xsl:for-each select="InnerExceptions/Report">
            <table class="inner">
              <tr>
                <th>Error Message</th>
                <td>
                  <xsl:value-of select="ErrorMessage"/>
                </td>
              </tr>
              <tr>
                <th>Stack Trace</th>
                <td>
                  <xsl:value-of select="StackTrace"/>
                </td>
              </tr>
            </table>
          </xsl:for-each>
        </xsl:if>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>