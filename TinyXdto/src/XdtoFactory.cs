/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;
using System.Xml.Schema;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using ScriptEngine.HostedScript.Library;

namespace TinyXdto
{
	[ContextClass("TinyФабрикаXDTO", "TinyXDTOFactory")]
	public class XdtoFactory : AutoContext<XdtoFactory>
	{
		private new List<XdtoPackage> _packages = new List<XdtoPackage>();
			
		public XdtoFactory ()
			: this (new XdtoPackage [] { } )
		{
		}

		public XdtoFactory (IEnumerable<XdtoPackage> packages)
		{
			_packages.Add (W3Org.XmlSchema.W3OrgXmlSchemaPackage.Create ());
			foreach (var package in packages) {
				if (!_packages.Contains (package)) {
					_packages.Add (package);
				}
			}

			Packages = new XdtoPackageCollection (_packages);

			ResoveTypes();
		}

		private void ResoveTypes()
		{
			// TODO: убрать это недоразумение
			foreach (var package in Packages)
			{
				foreach (var type in package)
				{
					if (!(type is XdtoObjectType))
					{
						return;
					}
					var objectType = type as XdtoObjectType;
					foreach (var prop in objectType.Properties)
					{
						if (prop.Type is TypeResolver)
						{
							throw new RuntimeException("unresolved types!");
						}
						if (prop.OwnerType is TypeResolver)
						{
							throw new RuntimeException("unresolved types!");
						}
					}
				}
			}
		}

		public XdtoFactory (IEnumerable<XmlSchema> schemas, IEnumerable<XdtoPackage> resolvePackages = null)
		{
			if (resolvePackages != null)
			{
				_packages.AddRange(resolvePackages);
			}
			var w3org = W3Org.XmlSchema.W3OrgXmlSchemaPackage.Create();
			if (!_packages.Contains(w3org))
			{
				_packages.Add(w3org);
			}
			foreach (var schema in schemas) {
				var package = new XdtoPackage (schema, this); // TODO: фабрика ещё не сформирована!
				if (!_packages.Contains (package)) {
					_packages.Add (package);
				}
				package.BuildPackage();
			}
			Packages = new XdtoPackageCollection (_packages);
		}

		[ContextProperty("Пакеты", "Packages")]
		public XdtoPackageCollection Packages { get; }

		private void WriteTypeAttribute (XmlWriterImpl xmlWriter,
		                                 IValue value)
		{
			string typeName;
			string typeUri;

			if (value is XdtoDataObject) {
				var obj = value as XdtoDataObject;
				typeUri = obj.Type ().NamespaceUri;
				typeName = obj.Type ().Name;
			} else if (value is XdtoDataValue) {
				var obj = value as XdtoDataValue;
				typeUri = obj.Type ().NamespaceUri;
				typeName = obj.Type ().Name;
			} else {
				typeName = "string";
				typeUri = XmlNs.xs;
			}

			var ns = xmlWriter.LookupPrefix (typeUri)?.AsString();
			if (string.IsNullOrEmpty (ns)) {

				// WriteAttribute(name, ns, value) при создании нового префикса
				//  не опознаёт префиксы, записанные через WriteNamespaceMapping
				var prefixIndex = xmlWriter.NamespaceContext.NamespaceMappings ().Count () + 2; // TODO: Костыль с +2
				var prefixDepth = xmlWriter.NamespaceContext.Depth;
				ns = string.Format ("d{0}p{1}", prefixDepth, prefixIndex);
				xmlWriter.WriteNamespaceMapping (ns, typeUri);
			}
			xmlWriter.WriteAttribute ("type", XmlNs.xsi, string.Format ("{0}:{1}", ns, typeName));
		}

		private void WriteXdtoSequence (XmlWriterImpl xmlWriter,
		                                XdtoSequence sequence)
		{
			foreach (var element in sequence) {

				if (element == null) {

					// TODO: надо ли что-нибудь делать???

				} else if (element is XdtoSequenceStringElement) {

					xmlWriter.WriteText ((element as XdtoSequenceStringElement).Text);

				} else if (element is XdtoSequenceValueElement) {

					var obj = element as XdtoSequenceValueElement;
					WriteXml (xmlWriter, obj.Value,
					          obj.Property.LocalName, obj.Property.NamespaceURI,
					          XmlTypeAssignmentEnum.Explicit,
					          obj.Property.Form);

				}
			}
		}

		private void WriteXdtoObject (XmlWriterImpl xmlWriter,
		                              XdtoDataObject obj)
		{
			obj.Validate ();

			if (obj.Type ().Sequenced) {
				WriteXdtoSequence (xmlWriter, obj.Sequence ());
				return;
			}

			foreach (var property in obj.Properties()) {

				var typeAssignment = XmlTypeAssignmentEnum.Explicit;

				var value = obj.Get (property) as IXdtoValue;
				if (value != null || property.Nillable)
				{
					WriteXml(xmlWriter, value,
						property.LocalName,
						property.NamespaceURI,
						typeAssignment,
						property.Form);
				}
			}
		}

		[ContextMethod ("ЗаписатьXML", "WriteXML")]
		public void WriteXml (XmlWriterImpl xmlWriter,
		                      IXdtoValue value,
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
					WriteTypeAttribute (xmlWriter, ValueFactory.Create(value));
				}

				if (value == null) {
					
					xmlWriter.WriteAttribute ("nil", XmlNs.xsi, "true");

				} else if (value is XdtoDataObject) {
					
					WriteXdtoObject (xmlWriter, value as XdtoDataObject);

				} else if (value is XdtoDataValue) {

					var dataValue = value as XdtoDataValue;
					xmlWriter.WriteText (dataValue.LexicalValue);

				} else {
					
					xmlWriter.WriteText (value.ToString ());

				}

				xmlWriter.WriteEndElement ();

			} else if (xmlForm == XmlFormEnum.Attribute) {

				xmlWriter.WriteAttribute (localName, namespaceUri, value.ToString());

			} else if (xmlForm == XmlFormEnum.Text) {

				xmlWriter.WriteText (value.ToString ()); // TODO: XmlString ??

			} else
				throw new NotSupportedException ("Какой-то новый тип для XML");
		}

		[ContextMethod ("Тип", "Type")]
		public IXdtoType Type (string uri, string name)
		{

			var package = _packages.Find((p) => p.NamespaceUri.Equals(uri, StringComparison.Ordinal));
			if (package == null)
				return null;

			var type = package.FirstOrDefault((t) => t.Name.Equals(name, StringComparison.Ordinal));
			return type;
		}

		public IXdtoType Type (XmlQualifiedName xmlType)
		{
			return Type (new XmlDataType (xmlType.Name, xmlType.Namespace));
		}

		[ContextMethod("Тип", "Type")]
		public IXdtoType Type (IValue xmlType)
		{
			if (xmlType is XmlDataType)
				return Type ((xmlType as XmlDataType).NamespaceUri, (xmlType as XmlDataType).TypeName);

			if (xmlType is XmlExpandedName)
				return Type ((xmlType as XmlExpandedName).NamespaceUri, (xmlType as XmlExpandedName).LocalName);

			throw RuntimeException.InvalidArgumentType (nameof (xmlType));
		}

		[ContextMethod ("Создать", "Create")]
		public IXdtoValue Create (IXdtoType type, IValue value = null)
		{
			if (type is XdtoObjectType) {
				return new XdtoDataObject (type as XdtoObjectType, null, null);
			}

			var valueType = type as XdtoValueType;
			if (valueType == null) {
				throw RuntimeException.InvalidArgumentType (nameof (type));
			}

			if (value == null) {
				throw RuntimeException.InvalidArgumentType (nameof (value));
			}

			if (value.DataType == DataType.String) {
				return new XdtoDataValue (valueType,
				                              value.AsString (),
				                              value);
			}

			return new XdtoDataValue (valueType,
			                              value.AsString (),
			                              value);
		}

		public IXdtoValue ReadXml (XmlReaderImpl reader, IXdtoType type = null)
		{
			if (reader.MoveToContent() == null)
			{
				// TODO: бросить исключение??
				return null;
			}
			if (type == null) {
				var explicitType = reader.GetAttribute (ValueFactory.Create ("type"), XmlNs.xsi);
				if (explicitType.DataType == DataType.Undefined) {

					type = new XdtoObjectType ();

				} else {

					var defaultNamespace = reader.NamespaceContext.DefaultNamespace;

					// Задан тип - пытаемся его распознать
					var sType = explicitType.AsString ();
					var nameElements = sType.Split (':');

					var typeUri = nameElements.Count () > 1
					                          ? nameElements [0]
					                          : defaultNamespace;
					var typeName = nameElements.Count () > 1
					                          ? nameElements [1]
					                          : nameElements [0];

					var nsMapping = reader.NamespaceContext.LookupNamespaceUri(typeUri);
					if (nsMapping != null && nsMapping.DataType == DataType.String) {
						typeUri = nsMapping.AsString ();
					}

					type = this.Type (typeUri, typeName);
				}
			}

			if (type is XdtoObjectType) {
				return type.Reader.ReadXml (reader, type, this);
			}

			var xmlNodeTypeEnum = XmlNodeTypeEnum.CreateInstance ();
			var xmlElementStart = xmlNodeTypeEnum.FromNativeValue (XmlNodeType.Element);
			if (type is XdtoValueType) {
				if (reader.NodeType.Equals (xmlElementStart)) {
					reader.Read ();
				}
				var result = type.Reader.ReadXml (reader, type, this);
				reader.Skip ();
				return result;
			}

			throw new NotSupportedException ("Неожиданный тип XDTO!");
		}

		[ScriptConstructor]
		static public IRuntimeContextInstance Constructor ()
		{
			return new XdtoFactory ();
		}

		[ScriptConstructor]
		static public IRuntimeContextInstance Constructor(IValue source, IValue packages = null)
		{
			var rawSource = source.GetRawValue();
			var rawPackages = packages?.GetRawValue();
			var schemas = new List<XmlSchema>();
			var resolvePackages = new List<XdtoPackage>();
			
			if (rawSource.DataType == DataType.String)
			{
				// source - путь к xsd-файлу
				using (var fs = new FileStream(rawSource.AsString(), FileMode.Open))
				using (var r = XmlReader.Create(fs))
				{
					var ss = new XmlSchemaSet();
					ss.Add(null, r);
					foreach (var schema in ss.Schemas())
					{
						schemas.Add(schema as XmlSchema);
					}
				}
			}

			if (rawPackages is IEnumerable<IValue>)
			{
				foreach (var pack in rawPackages as IEnumerable<IValue>)
				{
					resolvePackages.Add(pack as XdtoPackage);
				}
			}
			
			return new XdtoFactory(schemas, resolvePackages);
		}
	}
}

