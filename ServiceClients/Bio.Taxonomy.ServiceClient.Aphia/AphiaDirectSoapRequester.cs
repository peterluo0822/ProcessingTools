﻿namespace ProcessingTools.Bio.Taxonomy.ServiceClient.Aphia
{
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    using ProcessingTools.Common;
    using ProcessingTools.Net;
    using ProcessingTools.Net.Constants;
    using ProcessingTools.Xml.Extensions;

    public class AphiaDirectSoapRequester
    {
        private const string BaseAddress = "http://www.marinespecies.org";
        private const string ApiUrl = "aphia.php?p=soap";
        private readonly Encoding encoding = Defaults.DefaultEncoding;

        public XmlDocument AphiaSoapXml(string scientificName)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
    xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
    <soap:Body>
        <getAphiaRecords xmlns=""http://tempuri.org/"">
            <scientificname>" + scientificName + @"</scientificname>
            <marine_only>flase</marine_only>
        </getAphiaRecords>
    </soap:Body>
</soap:Envelope>");
            return xml;
        }

        public async Task<XmlDocument> SearchAphia(string scientificName)
        {
            var connector = new NetConnector(BaseAddress);
            var response = await connector.Post(
                ApiUrl,
                this.AphiaSoapXml(scientificName).OuterXml,
                ContentTypeConstants.XmlContentType,
                this.encoding);

            return response.ToXmlDocument();
        }
    }
}