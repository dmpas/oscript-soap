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
		private List<XmlSchema> _schema = new List<XmlSchema> ();

		public XdtoFactoryImpl ()
		{
			
		}

		public XdtoFactoryImpl (IEnumerable<XmlSchema> schema)
		{
			_schema.AddRange (schema);
		}

		[ContextMethod ("ЗаписатьXML", "WriteXML")]
		public void WriteXml (XmlWriterImpl xmlWriter,
		                      IValue value,
		                      string localName,
		                      string namespaceUri,
		                      XmlTypeAssignmentEnum typeAssignment = null,
		                      XmlFormEnum xmlForm = null)
		{
			// TODO: XdtoSerializer.WriteXml
			xmlWriter.WriteStartElement (localName, namespaceUri);
			xmlWriter.WriteText (value.ToString ());
			xmlWriter.WriteEndElement ();
		}

		[ContextMethod ("Тип", "Type")]
		public IValue Type (string name, string uri)
		{
			return ValueFactory.Create ();
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

