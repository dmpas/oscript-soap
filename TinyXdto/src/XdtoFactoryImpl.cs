using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;
using System.Xml.Schema;
using System.Collections.Generic;

namespace TinyXdto
{
	[ContextClass("ФабрикаXDTO", "XDTOFactory")]
	public class XdtoFactoryImpl : AutoContext<XdtoFactoryImpl>
	{
		public XdtoFactoryImpl ()
		{
			
		}

		public XdtoFactoryImpl (IEnumerable<XmlSchema> schemas)
		{
			var packages = new List<XdtoPackageImpl> ();
			foreach (var schema in schemas) {
				packages.Add (new XdtoPackageImpl (schema));
			}
		}

		[ContextProperty("Пакеты", "Packages")]
		public XdtoPackageCollectionImpl Packages { get; }

		[ContextMethod ("ЗаписатьXML", "WriteXML")]
		public void WriteXml (XmlWriterImpl xmlWriter,
		                      IValue value,
		                      string localName,
		                      string namespaceUri,
		                      XmlTypeAssignmentEnum? typeAssignment = null,
		                      XmlFormEnum? xmlForm = null)
		{
			xmlWriter.WriteStartElement (localName, namespaceUri);
			xmlWriter.WriteText (value.ToString ());
			xmlWriter.WriteEndElement ();
		}

		[ContextMethod ("Тип", "Type")]
		public UndefinedOr<IXdtoType> Type (string name, string uri)
		{
			return new UndefinedOr<IXdtoType> (null);
		}

		[ContextMethod ("Создать", "Create")]
		public IXdtoValue Create (IXdtoType type, IValue name = null)
		{
			throw new NotImplementedException ("XDTOFactory.Create()");
		}


		[ScriptConstructor]
		static public IRuntimeContextInstance Constructor ()
		{
			return new XdtoFactoryImpl ();
		}
	}
}

