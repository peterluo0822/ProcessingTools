﻿namespace ProcessingTools.Base
{
    public class GeoNamesTagger : TaggerBase
    {
        private const string TagName = "geoname";

        public GeoNamesTagger(Config config, string xml)
            : base(config, xml)
        {
        }

        public GeoNamesTagger(IBase baseObject)
            : base(baseObject)
        {
        }

        public void TagGeonames(IXPathProvider xpathProvider, IDataProvider dataProvider)
        {
            string query = @"select [Name] from [dbo].[geonames] order by len([Name]) desc;";

            dataProvider.Xml = this.Xml;
            dataProvider.ExecuteSimpleReplaceUsingDatabase(xpathProvider.SelectContentNodesXPath, query, TagName);
            this.Xml = dataProvider.Xml;
        }
    }
}
