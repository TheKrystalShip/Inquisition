using System.IO;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Inquisition.Reporting
{
	internal static class XmlHandler
	{
		public static void Serialize<T>(T report) where T: IReport
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			using (TextWriter writer = new StreamWriter(report.Path))
			{
				serializer.Serialize(writer, report);
			}
		}

		public static T Deserialize<T>(string path) where T: class
		{
			object toReturn;
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			using (StringReader reader = new StringReader(path))
			{
				toReturn = serializer.Deserialize(reader);
			}
			return (T) toReturn;
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
