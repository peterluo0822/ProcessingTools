﻿<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:xs="http://www.w3.org/2001/XMLSchema"
  xmlns:mml="http://www.w3.org/1998/Math/MathML"
  xmlns:xlink="http://www.w3.org/1999/xlink"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  xmlns:tp="http://www.plazi.org/taxpub"
  exclude-result-prefixes="xs">

  <xsl:output method="text" indent="yes" />

  <xsl:template match="text()">
    <xsl:value-of select="string()" />
  </xsl:template>

  <xsl:template match="*">
    <xsl:apply-templates select="node()" />
  </xsl:template>

  <xsl:template match="@* | comment()" />

  <xsl:template match="value | p | th | td | tr | title | article-title | label | name | ref | li">
    <xsl:text>&#10;</xsl:text>
    <xsl:apply-templates select="node()" />
  </xsl:template>

  <xsl:template match="journal-meta | article-id | article-categories | author-notes | pub-date | history | permissions | tp:treatment-meta | xref-group | graphic | media | ext-link | /document/citations" />
</xsl:stylesheet>