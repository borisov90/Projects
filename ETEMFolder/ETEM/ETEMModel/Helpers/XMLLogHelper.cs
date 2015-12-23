using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml;
using System.IO;

namespace ETEMModel.Helpers
{
    public class XMLLogHelper
    {
        public const string ITEM = "Item";
        public const string PROPERTY_NAME = "PROPERTY_NAME";
        public const string PROPERTY_VALUE = "PROPERTY_VALUE";


        public static XmlDocument CreateNewXMLDoc(string mainNodeValue)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode tagNode = doc.CreateNode(XmlNodeType.Element, mainNodeValue, null);
            doc.AppendChild(tagNode);

            return doc;
        }

        public static XmlDocument AppendValueToMainNode(XmlDocument doc, string itemKey, string itemValue)
        {

            XmlNode tagNode = doc.CreateNode(XmlNodeType.Element, ITEM, null);
            XmlNode itemKeyNode = doc.CreateElement(PROPERTY_NAME);
            itemKeyNode.AppendChild(doc.CreateTextNode(itemKey));
            XmlNode itemValueNode = doc.CreateElement(PROPERTY_VALUE);
            itemValueNode.AppendChild(doc.CreateTextNode(itemValue));

            tagNode.AppendChild(itemKeyNode);
            tagNode.AppendChild(itemValueNode);

            doc.DocumentElement.AppendChild(tagNode);

            return doc;
        }

        public static string SringValueOf(XmlDocument doc)
        {
            string xmlString = null;
            using (StringWriter wr = new StringWriter())
            {
                doc.Save(wr);
                xmlString = wr.ToString();
            }

            return xmlString;
        }



        //XmlNode subTagNode1 = doc.CreateNode(XmlNodeType.Element, "SUBTAG", null);
        //tagNode.AppendChild(subTagNode1);
        //XmlText subTagNode1Value = doc.CreateTextNode("value");
        //subTagNode1.AppendChild(subTagNode1Value);


        //XmlNode subTagNode2 = doc.CreateNode(XmlNodeType.Element, "SUBTAG", null);
        //tagNode.AppendChild(subTagNode2);
        //XmlAttribute subTagNode2Attribute = doc.CreateAttribute("attr");
        //subTagNode2Attribute.Value = "hello";

        //subTagNode2.Attributes.SetNamedItem(subTagNode2Attribute);
        //XmlText subTagNode2Value = doc.CreateTextNode("world");
        //subTagNode2.AppendChild(subTagNode2Value);

        //string xmlString = null;
        //using(StringWriter wr = new StringWriter())
        //{
        //    doc.Save(wr);
        //    xmlString = wr.ToString();
        //}
    }
}