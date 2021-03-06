﻿namespace ProcessingTools.Bio.Taxonomy.ServiceClient.ZooBank.Models.Json
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Author
    {
        [DataMember(Name = "familyname")]
        public string Familyname { get; set; }

        [DataMember(Name = "givenname")]
        public string Givenname { get; set; }

        [DataMember(Name = "gnubuuid")]
        public string GnubUuid { get; set; }
    }
}