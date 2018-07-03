using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using SharpSentinel.UI.Extensions;
using Xceed.Wpf.Toolkit;

namespace SharpSentinel.UI.Controls
{
    public class XmlFormatter : ITextFormatter
    {
        public bool TruncateText { get; set; }

        // TODO: Refactor this to make it more readable and make colorization more robust

        // See:
        // https://github.com/xceedsoftware/wpftoolkit/wiki/RichTextBox
        // https://github.com/xceedsoftware/wpftoolkit/blob/71a12b489e4ae669777f039038766cd764f3bce5/ExtendedWPFToolkitSolution/Src/Xceed.Wpf.Toolkit.LiveExplorer/Core/XamlFormatter.cs

        public string GetText(FlowDocument document)
        {
            return new TextRange(document.ContentStart, document.ContentEnd).Text;
        }

        public void SetText(FlowDocument document, string text)
        {
            document.Blocks.Clear();
            document.PageWidth = 2500;

            this.ColorizeXml(text, document);
        }

        /// <summary>
        /// Color xml-formatted text 
        /// </summary>
        private void ColorizeXml(string xmlText, FlowDocument document)
        {
            var textParts = xmlText.Split('<', '>').ToList();
            var paragraph = new Paragraph();

            for (var i = 0; i < textParts.Count; i++)
            {
                var text = textParts[i];

                var isXmlValue = false;

                if (i > 0 && i < textParts.Count - 1)
                    isXmlValue = textParts[i - 1].ContainsOnlyWhitespaceAndNewLines() == false &&
                                 textParts[i + 1].ContainsOnlyWhitespaceAndNewLines() == false;

                if (text.ContainsOnlyWhitespaceAndNewLines() || isXmlValue)
                {
                    paragraph.Inlines.Add(new Run(text));
                }
                else
                {
                    var colorizedXmlTag = this.ColorizeXmlTag(text);

                    foreach (var currentPart in colorizedXmlTag)
                    {
                        paragraph.Inlines.Add(new Run(currentPart.text)
                        {
                            Foreground = currentPart.color.HasValue ? new SolidColorBrush(currentPart.color.GetValueOrDefault()) : null
                        });
                    }
                }
            }

            document.Blocks.Add(paragraph);
        }

        /// <summary>
        /// Colorize only xmltag and attribute
        /// </summary>
        private List<(string text, Color? color)> ColorizeXmlTag(string xml)
        {
            var list = new List<(string text, Color? color)>();

            // readd "<" as it gets deleted during string.split
            list.Add(("<", this.XmlTagColor));

            // Is a closing tag
            if (xml.StartsWith("/"))
            {
                list.Add((xml, this.XmlTagColor));
            }
            // is an opening tag
            else
            {
                var indexFirstWhiteSpace = xml.IndexOf(" ", StringComparison.Ordinal);
                var tag = xml.Substring(0, indexFirstWhiteSpace == -1 ? xml.Length : indexFirstWhiteSpace);
                list.Add((tag, this.XmlTagColor));

                if (indexFirstWhiteSpace != -1)
                {
                    var attributes = xml.Substring(indexFirstWhiteSpace);

                    while (true)
                    {
                        var first = attributes.IndexOf('"');

                        if (first == 0 || first == -1)
                            break;

                        var attribute = attributes.Substring(0, first);
                        var valueTemp = attributes.Substring(first + 1);
                        var second = valueTemp.IndexOf('"');

                        list.Add((attribute, this.XmlAttributeColor));

                        // this only happens if the last xmltag is not closed properly (due to truncating)
                        if (second == -1)
                        {
                            var value = attributes;
                            list.Add(('"' + value + "...", this.XmlAttributeValueColor));

                            return list;
                        }
                        else
                        {
                            var value = attributes.Substring(first + 1, second);

                            list.Add(('"' + value + '"', this.XmlAttributeValueColor));

                            attributes = attributes.Substring(first + 1 + second + 1);
                        }
                    }
                }
            }

            // readd ">" as it gets deleted during string.split
            list.Add((">", this.XmlTagColor));

            return list;
        }

        // Color scheme as used by Visual Studio Code Light Theme

        private Color XmlTagColor => Colors.Brown;

        private Color XmlAttributeColor => Colors.Red;

        private Color XmlAttributeValueColor => Colors.Blue;
    }
}