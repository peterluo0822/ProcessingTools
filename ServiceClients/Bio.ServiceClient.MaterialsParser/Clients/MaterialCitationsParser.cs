﻿namespace ProcessingTools.Bio.ServiceClient.MaterialsParser
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Contracts;

    using ProcessingTools.Infrastructure.Net.Contracts;

    public class MaterialCitationsParser : IMaterialCitationsParser
    {
        private const string BaseAddress = "http://plazi2.cs.umb.edu";
        private const string ParserUrl = "/GgWS/wss/test";

        private readonly IConnector connector;
        private readonly Encoding encoding;

        public MaterialCitationsParser(IConnector connector)
            : this(connector, Encoding.UTF8)
        {
        }

        public MaterialCitationsParser(IConnector connector, Encoding encoding)
        {
            if (connector == null)
            {
                throw new ArgumentNullException(nameof(connector));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            this.encoding = encoding;

            this.connector = connector;
            connector.BaseAddress = BaseAddress;
        }

        public async Task<string> Invoke(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            var values = new Dictionary<string, string>
            {
                { "dataField", content },
                { "dataUrl", string.Empty },
                { "functionName", "XmlExample.webService" },
                { "dataFormat", "XML" },
                { "INTERACTIVE", "no" }
            };

            var response = await this.connector.Post(ParserUrl, values, this.encoding);

            return response;
        }
    }
}
