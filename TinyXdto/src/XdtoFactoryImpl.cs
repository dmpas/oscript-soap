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

		private void WriteTypeAttribute (XmlWriterImpl xmlWriter,
										 IValue value)
		{
			string typeName;
			string typeUri;

			if (value is XdtoDataObjectImpl) {
				var obj = value as XdtoDataObjectImpl;
				typeUri = obj.Type ().NamespaceUri;
				typeName = obj.Type ().Name;
			} else if (value is XdtoDataValueImpl) {
				var obj = value as XdtoDataValueImpl;
				typeUri = obj.Type ().NamespaceUri;
				typeName = obj.Type ().Name;
			} else {
				typeName = "string";
				typeUri = XmlNs.xs;
			}

			var ns = xmlWriter.LookupPrefix (typeUri);
			xmlWriter.WriteAttribute ("type", XmlNs.xsi, string.Format ("{0}:{1}", ns, typeName));
		}

		private void WriteXdtoObject (XmlWriterImpl xmlWriter,
									  XdtoDataObjectImpl obj)
		{
			obj.Validate ();
			foreach (var property in obj.Properties()) {

				var typeAssignment = XmlTypeAssignmentEnum.Explicit;

				var value = obj.Get (property);
				WriteXml (xmlWriter, value,
						  property.LocalName,
						  property.NamespaceURI,
				          typeAssignment,
						  property.Form);
			}
		}

		[ContextMethod ("ЗаписатьXML", "WriteXML")]
		public void WriteXml (XmlWriterImpl xmlWriter,
		                      IValue value,
		                      string localName,
		                      string namespaceUri = null,
		                      XmlTypeAssignmentEnum? typeAssignment = null,
		                      XmlFormEnum? xmlForm = null)
		{
			xmlForm = xmlForm ?? XmlFormEnum.Element;
			typeAssignment = typeAssignment ?? XmlTypeAssignmentEnum.Implicit;

			if (xmlForm == XmlFormEnum.Element) {

				xmlWriter.WriteStartElement (localName, namespaceUri);

				if (typeAssignment == XmlTypeAssignmentEnum.Implicit) {
					WriteTypeAttribute (xmlWriter, value);
				}

				if (value == null || value.DataType == DataType.Undefined) {
					xmlWriter.WriteAttribute ("nil", XmlNs.xs, "true");
				} else
				if (value is XdtoDataObjectImpl) {
					WriteXdtoObject (xmlWriter, value as XdtoDataObjectImpl);
				} else {
					xmlWriter.WriteText (value.ToString ());
				}

				xmlWriter.WriteEndElement ();

			} else if (xmlForm == XmlFormEnum.Attribute) {

				xmlWriter.WriteAttribute (localName, namespaceUri, value.AsString ());

			} else if (xmlForm == XmlFormEnum.Text) {

				xmlWriter.WriteText (value.AsString ()); // TODO: XmlString ??

			} else
				throw new NotSupportedException ("Какой-то новый тип для XML");
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
		public IXdtoValue Create (IXdtoType type, IValue value = null)
		{
			if (type is XdtoObjectTypeImpl) {
				return new XdtoDataObjectImpl (type as XdtoObjectTypeImpl, null, null);
			}

			var valueType = type as XdtoValueTypeImpl;
			if (valueType == null) {
				throw RuntimeException.InvalidArgumentType (nameof (type));
			}

			if (value == null) {
				throw RuntimeException.InvalidArgumentType (nameof (value));
			}

			if (value.DataType == DataType.String) {
				return new XdtoDataValueImpl (valueType,
											  value.AsString (),
											  value);
			}

			return new XdtoDataValueImpl (valueType,
										  value.AsString (),
										  value);
		}


		[ScriptConstructor]
		static public IRuntimeContextInstance Constructor ()
		{
			return new XdtoFactoryImpl ();
		}
	}
}

