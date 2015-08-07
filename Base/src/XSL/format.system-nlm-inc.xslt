﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:xs="http://www.w3.org/2001/XMLSchema"
  xmlns:mml="http://www.w3.org/1998/Math/MathML"
  xmlns:xlink="http://www.w3.org/1999/xlink"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  xmlns:tp="http://www.plazi.org/taxpub"
  exclude-result-prefixes="xs">

  <xsl:template match="surname|given-names|prefix|suffix|anonymous|etal">
    <xsl:element name="{name()}">
      <xsl:value-of select="normalize-space()"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="tn//tn | tp:taxon-name//tn | a//tn | ext-link//tn | *[@object_id='82']//tn | *[@id='41']//tn | article/front/notes/sec//tn | *[@id='236' or @id='436' or @id='435' or @id='418' or @id='49' or @id='417' or @id='48' or @id='434' or @id='433' or @id='432' or @id='431' or @id='430' or @id='429' or @id='428' or @id='427' or @id='426' or @id='425' or @id='424' or @id='423' or @id='422' or @id='421' or @id='420' or @id='419' or @id='475' or @id='414']/value//tn | xref//tn | tn//xref">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="tn//tp:taxon-name | tp:taxon-name//tp:taxon-name | a//tp:taxon-name | ext-link//tp:taxon-name | *[@object_id='82']//tp:taxon-name | *[@id='41']//tp:taxon-name | article/front/notes/sec//tp:taxon-name | *[@id='236' or @id='436' or @id='435' or @id='418' or @id='49' or @id='417' or @id='48' or @id='434' or @id='433' or @id='432' or @id='431' or @id='430' or @id='429' or @id='428' or @id='427' or @id='426' or @id='425' or @id='424' or @id='423' or @id='422' or @id='421' or @id='420' or @id='419' or @id='475' or @id='414']/value//tp:taxon-name | xref//tp:taxon-name | tp:taxon-name//xref">
    <xsl:apply-templates/>
  </xsl:template>

    <xsl:template match="tp:treatment-meta/kwd-group/kwd/named-content//tn">
        <xsl:value-of select="string(.)"/>
    </xsl:template>

    <xsl:template match="notes/sec//p//tn | notes/sec//p//tn-part | notes/sec//p//tp:taxon-name | notes/sec//p//tp:taxon-name-part">
        <xsl:apply-templates/>
    </xsl:template>

  <xsl:template match="tn-part//tn-part|tn-part//tp:taxon-name-part|tp:taxon-name-part//tp:taxon-name-part">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="a//a|a//ext-link|ext-link//a|ext-link//ext-link|a//xref|ext-link//xref|xref//xref|xref//a|xref//ext-link">
    <xsl:apply-templates/>
  </xsl:template>

  <!-- Remove xref/bibr from ref. In some cases this is not needed. -->
  <xsl:template match="ref//xref[@ref-type='bibr']">
    <xsl:apply-templates/>
  </xsl:template>

</xsl:stylesheet>
