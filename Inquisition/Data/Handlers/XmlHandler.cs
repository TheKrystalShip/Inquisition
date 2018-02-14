using Inquisition.Data.Models;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Inquisition.Data.Handlers
{
	public static class XmlHandler
	{
		public static void Serialize(Report report)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Report));
			using (TextWriter writer = new StreamWriter(report.Path))
			{
				serializer.Serialize(writer, report);
			}
		}

		public static string Transform(string xmlFileUri, string xsltFileUri)
		{
			XslCompiledTransform xsl = new XslCompiledTransform();
			xsl.Load(xsltFileUri);

			using (StringWriter results = new StringWriter())
			{
				xsl.Transform(xmlFileUri, null, results);
				return results.ToString();
			}
		}
	}
}
