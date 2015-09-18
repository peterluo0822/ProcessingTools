﻿namespace ProcessingTools.BaseLibrary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml;

    public class Net
    {
        public static string XmlHttpRequest(string urlString, string xmlContent)
        {
            string response = null;

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(xmlContent);

                httpWebRequest = (HttpWebRequest)WebRequest.Create(urlString);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = bytes.Length;
                httpWebRequest.ContentType = "text/xml; encoding='utf-8'";

                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        response = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                httpWebResponse.Close();
                httpWebResponse = null;
                httpWebRequest = null;
            }

            return response;
        }

        public static XmlDocument XmlHttpRequest(string urlString, XmlDocument xml)
        {
            XmlDocument response = new XmlDocument();

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(xml.OuterXml);

                httpWebRequest = (HttpWebRequest)WebRequest.Create(urlString);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = bytes.Length;
                httpWebRequest.ContentType = "text/xml; encoding='utf-8'";

                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        response.InnerXml = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                httpWebResponse.Close();
                httpWebResponse = null;
                httpWebRequest = null;
            }

            return response;
        }

        public static XmlDocument AphiaSoapXml(string scientificName)
        {
            XmlDocument xml = new XmlDocument();
            xml.InnerXml = @"<?xml version=""1.0""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
    xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
    <soap:Body>
        <getAphiaRecords xmlns=""http://tempuri.org/"">
            <scientificname>" + scientificName + @"</scientificname>
            <marine_only>flase</marine_only>
        </getAphiaRecords>
    </soap:Body>
</soap:Envelope>";
            return xml;
        }

        public static XmlDocument SearchAphia(string scientificName)
        {
            XmlDocument response = Net.XmlHttpRequest("http://www.marinespecies.org/aphia.php?p=soap", Net.AphiaSoapXml(scientificName));
            return response;
        }

        public static Json.Gbif.GbifResult SearchGbif(string scientificName)
        {
            Json.Gbif.GbifResult gbifResult = null;
            try
            {
                using (var client = new HttpClient())
                {
                    string responseString = client.GetStringAsync("http://api.gbif.org/v0.9/species/match?verbose=true&name=" + scientificName).Result;
                    DataContractJsonSerializer data = new DataContractJsonSerializer(typeof(Json.Gbif.GbifResult));
                    MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(responseString));
                    gbifResult = (Json.Gbif.GbifResult)data.ReadObject(stream);
                }
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 0);
            }

            return gbifResult;
        }

        /// <summary>
        /// Search scientific name in Catalogue Of Life (CoL)
        /// </summary>
        /// <param name="scientificName">scientific name of the taxon which rank is searched</param>
        /// <returns>taxonomic rank of the scientific name</returns>
        public static XmlDocument SearchCatalogueOfLife(string scientificName)
        {
            // http://www.catalogueoflife.org/col/webservice?name=Tara+spinosa&response=full
            XmlDocument xml = null;
            try
            {
                using (var client = new HttpClient())
                {
                    string responseString = client.GetStringAsync("http://www.catalogueoflife.org/col/webservice?name=" + scientificName + "&response=full").Result;
                    xml = new XmlDocument();
                    xml.LoadXml(responseString);
                }
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 0);
            }

            return xml;
        }

        /// <summary>
        /// Search scientific name in The Paleobiology Database (PBDB)
        /// </summary>
        /// <param name="scientificName">scientific name of the taxon which rank is searched</param>
        /// <returns>taxonomic rank of the scientific name</returns>
        public static string SearchNameInPaleobiologyDatabase(string scientificName)
        {
            /*
             * https://paleobiodb.org/data1.1/taxa/single.txt?name=Dascillidae
             * "taxon_no","orig_no","record_type","associated_records","rank","taxon_name","common_name","status","parent_no","senior_no","reference_no","is_extant"
             * "69296","69296","taxon","","family","Dascillidae","soft bodied plant beetle","belongs to","69295","69296","5056","1"
             */
            string result = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    string responseString = client.GetStringAsync("https://paleobiodb.org/data1.1/taxa/single.txt?name=" + scientificName).Result;
                    Alert.Log(responseString);

                    string keys = Regex.Match(responseString, "\\A[^\r\n]+").Value;
                    string values = Regex.Match(responseString, "\n[^\r\n]+").Value;
                    Match matchKeys = Regex.Match(keys, "(?<=\")[^,\"]*(?=\")");
                    Match matchValues = Regex.Match(values, "(?<=\")[^,\"]*(?=\")");
                    Hashtable response = new Hashtable();

                    while (matchKeys.Success && matchValues.Success)
                    {
                        response.Add(matchKeys.Value, matchValues.Value);
                        matchKeys = matchKeys.NextMatch();
                        matchValues = matchValues.NextMatch();
                    }

                    ICollection responseKeys = response.Keys;
                    foreach (var str in responseKeys)
                    {
                        Alert.Log(str + " --- " + response[str]);
                    }

                    if (response["taxon_name"].ToString().CompareTo(scientificName) == 0)
                    {
                        result = response["rank"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 0);
            }

            return result;
        }

        public static Json.Pbdb.PbdbAllParents SearchParentsInPaleobiologyDatabase(string scientificName)
        {
            //// https://paleobiodb.org/data1.1/taxa/list.json?name=Dascillidae&rel=all_parents

            Json.Pbdb.PbdbAllParents pbdbAllParents = null;
            try
            {
                using (var client = new HttpClient())
                {
                    string responseString = client.GetStringAsync("https://paleobiodb.org/data1.1/taxa/list.json?name=" + scientificName + "&rel=all_parents").Result;
                    DataContractJsonSerializer data = new DataContractJsonSerializer(typeof(Json.Pbdb.PbdbAllParents));
                    MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(responseString));
                    pbdbAllParents = (Json.Pbdb.PbdbAllParents)data.ReadObject(stream);
                }
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, 0);
            }

            return pbdbAllParents;
        }

        public static XmlDocument SearchWithGlobalNamesResolverGet(string[] scientificNames, int[] sourceId = null)
        {
            XmlDocument xmlResult = null;
            try
            {
                string searchString = BuildGlobalNamesResolverSearchString(scientificNames, sourceId);

                using (HttpClient client = new HttpClient())
                {
                    string responseString = client.GetStringAsync("http://resolver.globalnames.org/name_resolvers.xml?" + searchString).Result;

                    xmlResult = new XmlDocument();
                    xmlResult.PreserveWhitespace = true;
                    xmlResult.LoadXml(responseString);
                }
            }
            catch
            {
                throw;
            }

            return xmlResult;
        }

        public static XmlDocument SearchWithGlobalNamesResolverPost(string[] scientificNames, int[] sourceId = null)
        {
            XmlDocument xmlResult = null;

            try
            {
                string postData = BuildGlobalNamesResolverSearchString(scientificNames, sourceId);
                byte[] data = Encoding.UTF8.GetBytes(postData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://resolver.globalnames.org/name_resolvers.xml");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                Task<WebResponse> response = request.GetResponseAsync();

                xmlResult = new XmlDocument();
                xmlResult.PreserveWhitespace = true;
                xmlResult.Load(response.Result.GetResponseStream());
            }
            catch
            {
                throw;
            }

            return xmlResult;
        }

        public static XmlDocument SearchWithGlobalNamesResolverPostNewerRequestVersion(string[] scientificNames, int[] sourceId = null)
        {
            XmlDocument xmlResult = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    values.Add("data", string.Join("\r\n", scientificNames));

                    if (sourceId != null)
                    {
                        values.Add("data_source_ids", string.Join("|", sourceId));
                    }

                    using (HttpContent content = new FormUrlEncodedContent(values))
                    {
                        Task<HttpResponseMessage> response = client.PostAsync("http://resolver.globalnames.org/name_resolvers.xml", content);
                        Task<string> responseString = response.Result.Content.ReadAsStringAsync();

                        xmlResult = new XmlDocument();
                        xmlResult.PreserveWhitespace = true;
                        xmlResult.LoadXml(responseString.Result);
                    }
                }
            }
            catch
            {
                throw;
            }

            return xmlResult;
        }

        private static string BuildGlobalNamesResolverSearchString(string[] scientificNames, int[] sourceId)
        {
            StringBuilder searchStringBuilder = new StringBuilder();
            searchStringBuilder.Append("names=");

            if (scientificNames != null && scientificNames.Length > 0)
            {
                Regex whiteSpaces = new Regex(@"\s+");
                searchStringBuilder.Append(string.Join("|", scientificNames.Select(s => whiteSpaces.Replace(s.Trim(), "+"))));
            }
            else
            {
                throw new ArgumentNullException("scientificNames should be a non-empty array of strings.");
            }

            if (sourceId != null)
            {
                searchStringBuilder.Append("&data_source_ids=");
                searchStringBuilder.Append(string.Join("|", sourceId));
            }

            string searchString = searchStringBuilder.ToString();

            return searchString;
        }

        /*
         * https://extract.hcmr.gr/
         * http://tagger.jensenlab.org/GetHTML?document=Both+samples+were+dominated+by+Zetaproteobacteria+Fe+oxidizers.+This+group+was+most+abundant+at+Volcano+1,+where+sediments+were+richer+in+Fe+and+contained+more+crystalline+forms+of+Fe+oxides.&entity_types=-2+-25+-26+-27
         */
        public static XmlDocument UseGreekTagger(string xmlContent)
        {
            XmlDocument result = new XmlDocument();
            XmlElement rootElement = result.CreateElement("response");

            if (xmlContent != null && xmlContent.Length > 0)
            {
                StringBuilder responseCollector = new StringBuilder();

                using (HttpClient client = new HttpClient())
                {
                    GreekTaggerSendPartialString(xmlContent, responseCollector, client);
                }

                try
                {
                    rootElement.InnerXml = responseCollector.ToString();
                }
                catch (Exception e)
                {
                    throw new XmlException("UseGreekTagger: Builded root element by string builder.", e);
                }
            }
            else
            {
                throw new ArgumentNullException("Content string to send is empty.");
            }

            result.AppendChild(rootElement);

            return result;
        }

        private static void GreekTaggerSendPartialString(string xmlContent, StringBuilder responseCollector, HttpClient client)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("document", xmlContent);
            values.Add("entity_types", "-25 -26 -27");
            values.Add("format", "xml");

            try
            {
                using (HttpContent content = new FormUrlEncodedContent(values))
                {
                    try
                    {
                        Task<HttpResponseMessage> response = client.PostAsync("http://tagger.jensenlab.org/GetEntities", content);
                        Task<string> responseString = response.Result.Content.ReadAsStringAsync();
                        XmlDocument xmlResponse = new XmlDocument();
                        xmlResponse.LoadXml(Regex.Replace(responseString.Result, @"xmlns=""[^""]*""", string.Empty));
                        responseCollector.Append(xmlResponse.DocumentElement.InnerXml);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch (UriFormatException)
            {
                int xmlContentLength = xmlContent.Length;
                int length = xmlContentLength / 2;

                GreekTaggerSendPartialString(xmlContent.Substring(0, length), responseCollector, client);

                GreekTaggerSendPartialString(xmlContent.Substring(length, length), responseCollector, client);
            }
            catch
            {
                throw;
            }
        }
    }
}
