﻿namespace ProcessingTools.Services.CatalogueOfLife
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Xml;

    /// <summary>
    /// Implementations of some of the Catalogue Of Life (CoL) API-s.
    /// </summary>
    public class CatalogueOfLifeDataRequester
    {
        /// <summary>
        /// Search scientific name in Catalogue Of Life (CoL)
        /// </summary>
        /// <param name="scientificName">scientific name of the taxon which rank is searched</param>
        /// <returns>taxonomic rank of the scientific name</returns>
        /// <example>http://www.catalogueoflife.org/col/webservice?name=Tara+spinosa&response=full</example>
        public static async Task<XmlDocument> SearchCatalogueOfLife(string scientificName)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync("http://www.catalogueoflife.org/col/webservice?name=" + scientificName + "&response=full");

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(response);

                    return xml;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
