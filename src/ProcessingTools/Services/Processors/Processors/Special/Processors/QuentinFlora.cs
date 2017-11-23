﻿namespace ProcessingTools.Special.Processors.Processors
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using ProcessingTools.Common.Extensions;
    using ProcessingTools.Contracts;
    using ProcessingTools.Processors.Contracts;
    using ProcessingTools.Processors.Contracts.Special;

    public class QuentinFlora : IQuentinFlora
    {
        private Action<XmlNode> ClearWhitespacingAction => n =>
        {
            n.InnerXml = n.InnerXml
                .RegexReplace(@"\s+", " ")
                .Trim();
        };

        public async Task<object> FormatAsync(IDocument context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return await Task.Run(() =>
            {
                this.InitialFormat(context);
                this.Split1(context);
                this.Split2(context);
                this.FinalFormat(context);

                return true;
            })
            .ConfigureAwait(false);
        }

        private void FinalFormat(IDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            document.SelectNodes("//tp:taxon-name").AsParallel().ForAll(this.ClearWhitespacingAction);
            document.SelectNodes("//p").AsParallel().ForAll(this.ClearWhitespacingAction);
            document.SelectNodes("//title").AsParallel().ForAll(this.ClearWhitespacingAction);

            string xml = document.Xml
                .RegexReplace("\\s*(</?content>|</?ballon_content>)\\s*", "$1")
                .RegexReplace("(</div>)\\s+(<div)", "$1$2")
                .RegexReplace("\\s+(<br/>)\\s+", "$1")
                .RegexReplace("(</span>)\\s*<br/>\\s*(</ballon_content>)", "$1$2");

            ////xml = Regex.Replace(xml, "(</ballon_wrapper>)\\s*(<ballon_wrapper)", "$1<span class=\"space\"/> $2");

            document.Xml = xml;
        }

        private void InitialFormat(IDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            document.SelectNodes("//latinname").AsParallel().ForAll(this.ClearWhitespacingAction);
            document.SelectNodes("//notes").AsParallel().ForAll(this.ClearWhitespacingAction);
            document.SelectNodes("//notes").AsParallel().ForAll(n =>
            {
                n.InnerXml = $"<p>{n.InnerXml}</p>";
            });
            document.SelectNodes("//p").AsParallel().ForAll(this.ClearWhitespacingAction);
        }

        private void Split1(IDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            document.SelectNodes("//sec[@type='family']")
                .AsParallel()
                .ForAll(n =>
                {
                    n.InnerXml = n.InnerXml
                        .RegexReplace("(<genus_header>[\\s\\S]*?)(?=<genus_header>)", "<sec type=\"genus\">\n$1\n</sec>\n")
                        .RegexReplace("(</sec>\\s*|</family_header>\\s*)(<genus_header>[\\s\\S]*?)(?=</sec>)", "$1<sec type=\"genus\">\n$2\n</sec>\n");
                });
        }

        private void Split2(IDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            document.Xml = document.Xml
                .RegexReplace(@"(<family_header>[\s\S]*?)(?=<family_header>)", "<sec type=\"family\">\n$1\n</sec>\n")
                .RegexReplace(@"(</sec>\s*)(<family_header>[\s\S]*?)(?=</sec>)", "$1<sec type=\"family\">\n$2\n</sec>\n");
        }
    }
}
