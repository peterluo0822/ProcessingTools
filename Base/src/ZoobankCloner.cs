﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ProcessingTools.Base.ZooBank
{
    public class ZoobankCloner : Base
    {
        public const string ZooBankPrefix = "http://zoobank.org/";
        private string nlmXml;
        private XmlDocument nlmDocument;

        public ZoobankCloner(string xmlContent)
            : base(xmlContent)
        {
            this.xml = xmlContent;
            this.xmlDocument = new XmlDocument();
            this.xmlDocument.PreserveWhitespace = true;

            this.nlmXml = string.Empty;
            this.nlmDocument = new XmlDocument();
            this.nlmDocument.PreserveWhitespace = true;
            try
            {
                this.xmlDocument.LoadXml(this.xml);
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForType(e, this.GetType().Name, 51);
            }
        }

        public ZoobankCloner(string nlmXmlContent, string xmlContent)
            : base(xmlContent)
        {
            this.xml = xmlContent;
            this.xmlDocument = new XmlDocument();
            this.xmlDocument.PreserveWhitespace = true;

            this.nlmXml = nlmXmlContent;
            this.nlmDocument = new XmlDocument();
            this.nlmDocument.PreserveWhitespace = true;
            try
            {
                this.xmlDocument.LoadXml(this.xml);
                this.nlmDocument.LoadXml(this.nlmXml);
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForType(e, this.GetType().Name, 51);
            }
        }

        public void Clone()
        {
            XmlNamespaceManager xmlNamespaceManager = ProcessingTools.Config.TaxPubNamespceManager(xmlDocument);
            this.nlmXml = Regex.Replace(this.nlmXml, @"\s*<!DOCTYPE [^>]*>", string.Empty);

            try
            {
                this.xmlDocument.LoadXml(this.xml);
                this.nlmDocument.LoadXml(this.nlmXml);
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, this.GetType().Name, 51);
            }

            XmlNodeList nlmNodeList, nodeList;

            Alert.Log("Taxonomic acts:");
            try
            {
                nlmNodeList = this.nlmDocument.SelectNodes("//tp:taxon-treatment/tp:nomenclature", xmlNamespaceManager);
                nodeList = this.xmlDocument.SelectNodes("//tp:taxon-treatment/tp:nomenclature", xmlNamespaceManager);

                if (nlmNodeList.Count == nodeList.Count)
                {
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        XmlNodeList objecIdList = nodeList[i].SelectNodes(".//object-id[@content-type='zoobank']", xmlNamespaceManager);
                        XmlNodeList nlmObjecIdList = nlmNodeList[i].SelectNodes(".//object-id[@content-type='zoobank']", xmlNamespaceManager);
                        if (objecIdList.Count > 0)
                        {
                            if (objecIdList.Count == nlmObjecIdList.Count)
                            {
                                for (int j = 0; j < objecIdList.Count; j++)
                                {
                                    objecIdList[j].InnerXml = nlmObjecIdList[j].InnerXml;
                                    Alert.Log(objecIdList[j].InnerXml);
                                }
                            }
                            else
                            {
                                Alert.Log("Number of ZooBank objec-id tags does not match.");
                            }
                        }
                    }

                    Alert.Log();
                }
                else
                {
                    Alert.Log("Number of nomenclatures tags in these files does not match.");
                }
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, this.GetType().Name, 0);
            }

            Alert.Log("Reference:");
            try
            {
                nlmNodeList = this.nlmDocument.SelectNodes("//self-uri", xmlNamespaceManager);
                nodeList = this.xmlDocument.SelectNodes("//self-uri", xmlNamespaceManager);
                if (nlmNodeList.Count == nodeList.Count)
                {
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        nodeList[i].InnerXml = nlmNodeList[i].InnerXml;
                        Alert.Log(nodeList[i].InnerXml);
                    }

                    Alert.Log();
                }
                else
                {
                    Alert.Log("Number of ZooBank self-uri tags in these files does not match.");
                }
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, this.GetType().Name, 0);
            }

            Alert.Log("Author(s):");
            try
            {
                nodeList = this.xmlDocument.SelectNodes("/article/front/article-meta/contrib-group/contrib/uri[@content-type='zoobank']", xmlNamespaceManager);
                nlmNodeList = this.nlmDocument.SelectNodes("/article/front/article-meta/contrib-group/contrib/uri[@content-type='zoobank']", xmlNamespaceManager);
                if (nlmNodeList.Count == nodeList.Count)
                {
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        nodeList[i].InnerXml = nlmNodeList[i].InnerXml;
                        Alert.Log(nodeList[i].InnerXml);
                    }

                    Alert.Log();
                }
                else
                {
                    Alert.Log("Number of ZooBank uri tags in these files does not match.");
                }
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, this.GetType().Name, 0);
            }

            this.xml = this.xmlDocument.OuterXml;
        }

        public void CloneJsonToXml(string jsonString)
        {
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Json.ZooBank.ZooBankRegistration>));
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                List<Json.ZooBank.ZooBankRegistration> zbr = (List<Json.ZooBank.ZooBankRegistration>)ser.ReadObject(stream);
                Json.ZooBank.ZooBankRegistration z = null;

                if (zbr.Count < 1)
                {
                    Alert.Log("ERROR: No valid ZooBank registation records in JSON File");
                    Alert.Exit(1);
                }
                else if (zbr.Count > 1)
                {
                    Alert.Log("WARNING: More than one ZooBank registration records in JSON File");
                    Alert.Log("         It will be used only the first one");
                    z = zbr[0];
                }
                else
                {
                    z = zbr[0];
                }

                this.ParseXmlStringToXmlDocument();

                // Article lsid
                {
                    string articleLsid = ZooBankPrefix + z.referenceuuid;
                    XmlNode selfUri = xmlDocument.SelectSingleNode("/article/front/article-meta/self-uri[@content-type='zoobank']", NamespaceManager);
                    if (selfUri == null)
                    {
                        Alert.Log("ERROR: article-meta/self-uri/@content-type='zoobank' is missing.\n\n");
                        Alert.Exit(1);
                    }

                    selfUri.InnerText = articleLsid;
                }

                // Taxonomic acts’ lsid
                {
                    int numberOfNomenclaturalActs = z.NomenclaturalActs.Count;
                    int numberOfNewNomenclaturalActs = 0;
                    foreach (Json.ZooBank.NomenclaturalAct na in z.NomenclaturalActs)
                    {
                        // First try to resolve empty parent names
                        if (na.parentname == string.Empty && na.parentusageuuid != string.Empty)
                        {
                            foreach (Json.ZooBank.NomenclaturalAct n in z.NomenclaturalActs)
                            {
                                if (string.Compare(na.parentusageuuid, n.tnuuuid) == 0)
                                {
                                    na.parentname = n.namestring;
                                    break;
                                }
                            }
                        }

                        Alert.Log("\n\n");
                        Alert.Log(na.parentname + (na.parentname == string.Empty ? string.Empty : " ") + na.namestring + " " + na.tnuuuid);

                        string xpath = "//tp:taxon-treatment/tp:nomenclature/tp:taxon-name";
                        switch (na.rankgroup)
                        {
                            case "Genus":
                                xpath += "[tp:taxon-name-part[@taxon-name-part-type='genus']='" + na.namestring + "'][string(../tp:taxon-status)='gen. n.']/object-id[@content-type='zoobank']";
                                break;
                            case "Species":
                                xpath += "[tp:taxon-name-part[@taxon-name-part-type='genus']='" + na.parentname + "'][tp:taxon-name-part[@taxon-name-part-type='species']='" + na.namestring + "'][string(../tp:taxon-status)='sp. n.']/object-id[@content-type='zoobank']";
                                break;
                        }

                        XmlNode objectId = this.xmlDocument.SelectSingleNode(xpath, NamespaceManager);
                        if (objectId != null)
                        {
                            objectId.InnerText = ZooBankPrefix + na.tnuuuid;
                            numberOfNewNomenclaturalActs++;

                            Alert.Log(na.parentname + (na.parentname == string.Empty ? string.Empty : " ") + na.namestring + " " + na.tnuuuid);
                        }
                    }

                    Alert.Log("\n\n\nNumber of nomenclatural acts = " + numberOfNomenclaturalActs + ".\nNumber of new nomenclatural acts = " + numberOfNewNomenclaturalActs + ".\n\n\n");
                }

                this.xml = this.xmlDocument.OuterXml;
            }
            catch (Exception e)
            {
                Alert.RaiseExceptionForMethod(e, this.GetType().Name, 1);
            }
        }
    }
}
