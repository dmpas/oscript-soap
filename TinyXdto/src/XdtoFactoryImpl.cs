using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;
using System.Xml.Schema;
using System.Collections.Generic;
using System.Linq;

namespace TinyXdto
{
	[ContextClass("ФабрикаXDTO", "XDTOFactory")]
	public class XdtoFactoryImpl : AutoContext<XdtoFactoryImpl>
	{

		private readonly Dictionary<string, IList<XdtoPackageImpl>> _packages = new Dictionary<string, IList<XdtoPackageImpl>> ();
		public XdtoFactoryImpl ()
		{
			
		}

		public XdtoFactoryImpl (IEnumerable<XmlSchema> schemas)
		{
			var packages = new List<XdtoPackageImpl> ();
			foreach (var schema in schemas) {
				var package = new XdtoPackageImpl (schema);
				packages.Add (package);
			}
			Packages = new XdtoPackageCollectionImpl (packages);

			var byUri = Packages.GroupBy ((p) => p.NamespaceUri);
			foreach (var kv in byUri) {
				_packages.Add (kv.Key, kv.ToList ());
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
		public IXdtoType Type (string name, string uri)
		{

			if (!_packages.ContainsKey (uri))
				throw new RuntimeException ("Тип не определён!");

			// считаем, что в пакете все типы в том же пространстве имён, что и сам пакет
			foreach (var package in _packages [uri]) {

				var type = package.First((t) => t.Name.Equals(name, StringComparison.Ordinal));
				if (type != null) {
					return type;
				}

			}

			return null;
		}

		[ContextMethod ("Создать", "Create")]
		public IXdtoValue Create (IXdtoType type, IValue name = null)
		{
			if (type is XdtoObjectTypeImpl) {
				return new XdtoDataObjectImpl (type as XdtoObjectTypeImpl, null, null);
			}

			throw new NotImplementedException ("XDTOFactory.Create()");
		}


		[ScriptConstructor]
		static public IRuntimeContextInstance Constructor ()
		{
			return new XdtoFactoryImpl ();
		}
	}
}

