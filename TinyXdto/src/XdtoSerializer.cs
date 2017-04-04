using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;
using System.Collections.Generic;

namespace TinyXdto
{
	[ContextClass ("СериализаторXDTO", "XDTOSerializer")]
	public class XdtoSerializerImpl : AutoContext<XdtoSerializerImpl>
	{

		private readonly Dictionary<TypeDescriptor, IXdtoSerializer>   serializers = new Dictionary<TypeDescriptor, IXdtoSerializer> ();
		private readonly Dictionary<TypeDescriptor, IXdtoDeserializer> deserializers = new Dictionary<TypeDescriptor, IXdtoDeserializer> ();

		public XdtoSerializerImpl (XdtoFactoryImpl factory)
		{
			XdtoFactory = factory;
		}

		[ContextProperty ("ФабрикаXDTO", "XDTOFactory")]
		public XdtoFactoryImpl XdtoFactory { get; }

		private bool IsPrimitiveValue (IValue value)
		{
			return value.DataType != DataType.Object
						&& value.DataType != DataType.NotAValidValue
						&& value.DataType != DataType.Type;
		}

		[ContextMethod ("ЗаписатьXML", "WriteXML")]
		public void WriteXml (XmlWriterImpl xmlWriter,
							  IValue value,
							  string localName,
							  string namespaceUri,
		                      XmlTypeAssignmentEnum? typeAssignment = null,
							  XmlFormEnum? xmlForm = null)
		{
			xmlForm = xmlForm ?? XmlFormEnum.Element;
			typeAssignment = typeAssignment ?? XmlTypeAssignmentEnum.Implicit;

			var xdtoValue = WriteXdto (value);
			XdtoFactory.WriteXml (xmlWriter, xdtoValue, localName, namespaceUri, typeAssignment, xmlForm);

			/*
			IValue rawValue = value.GetRawValue ();
			var primitive = SerializedPrimitiveValue.Create (rawValue);

			if (primitive == null) {
				XdtoFactory.WriteXml (xmlWriter, value, localName, namespaceUri, typeAssignment, xmlForm);
				return;
			}

			if (xmlForm == null || xmlForm == XmlFormEnum.Element) {

				xmlWriter.WriteStartElement (localName, namespaceUri);

				if (primitive.Nil) {
					xmlWriter.WriteAttribute ("nil", XmlNs.xsi, "true");
				} else {
					if (typeAssignment == XmlTypeAssignmentEnum.Explicit) {
						var dataType = XmlTypeOf (rawValue);
						var nsPrefix = xmlWriter.LookupPrefix (dataType.NamespaceUri);
						if (ValueFactory.Create().Equals(nsPrefix)) {
							var prefixIndex = xmlWriter.NamespaceContext.NamespaceMappings ().Count () + 1;
							var prefixDepth = xmlWriter.NamespaceContext.Depth;
							var prefix = string.Format ("d{0}p{1}", prefixDepth, prefixIndex);
							xmlWriter.WriteNamespaceMapping (prefix, dataType.NamespaceUri);
							nsPrefix = ValueFactory.Create (prefix);
						}
						var typeValue = String.Format("{0}:{1}", nsPrefix, dataType.TypeName);
						xmlWriter.WriteAttribute("type", XmlNs.xsi, typeValue);
					}
				}

				xmlWriter.WriteText (primitive.SerializedValue);
				xmlWriter.WriteEndElement ();

			} else
			if (xmlForm == XmlFormEnum.Attribute) {

				xmlWriter.WriteAttribute (localName, namespaceUri, primitive.SerializedValue);
				
			} else if (xmlForm == XmlFormEnum.Text) {

				xmlWriter.WriteText (primitive.SerializedValue);

			}
			*/
		}

		/*
		private static UndefinedOr<XmlDataType> Define (string xmlType)
		{
			return new UndefinedOr<XmlDataType> (new XmlDataType (xmlType, XmlNs.xs));
		}

		private static UndefinedOr<XmlDataType> Undefined ()
		{
			return new UndefinedOr<XmlDataType> (null);
		}
		*/
		[ContextMethod("ЗаписатьXDTO")]
		public IXdtoValue WriteXdto (IValue inValue)
		{
			var value = inValue.GetRawValue ();
			var td = value.SystemType;

			if (serializers.ContainsKey (td)) {
				var s = serializers [td];
				var xdtoType = XdtoFactory.Type (s.GetPossibleType (inValue));
				return s.SerializeXdto (inValue, xdtoType);
			}
			return null;
		}

		[ContextMethod("XMLТипЗнч", "XMLTypeOf")]
		public XmlDataType XmlTypeOf (IValue inValue)
		{
			var xdtoValue = WriteXdto (inValue);
			return xdtoValue?.XmlType ();
		}

		[ContextMethod("XMLТип", "XMLType")]
		public UndefinedOr<XmlDataType> XmlType (IValue type)
		{
			throw new NotImplementedException ("XdtoSerializer.XmlType");
		}

		[ContextMethod("XMLСтрока", "XMLString")]
		public string XmlString (IValue inValue)
		{
			var xdtoValue = WriteXdto (inValue);
			return (xdtoValue as XdtoDataValueImpl).LexicalValue;
		}

		public void RegisterXdtoType (TypeDescriptor type, object processor)
		{
			if (processor is IXdtoSerializer) {
				serializers [type] = processor as IXdtoSerializer;
			}

			if (processor is IXdtoDeserializer) {
				deserializers [type] = processor as IXdtoDeserializer;
			}

		}

		[ContextMethod("XMLЗначение", "XMLValue")]
		public IValue XmlValue (IValue type, IValue xmlString)
		{
			throw new NotImplementedException ("XdtoSerializer.XmlValue");
		}

		[ScriptConstructor]
		public static IRuntimeContextInstance Constructor (IValue factory)
		{
			XdtoFactoryImpl rawFactory = factory.GetRawValue () as XdtoFactoryImpl;
			if (rawFactory == null)
				throw RuntimeException.InvalidArgumentType ("factory");

			var primitiveSerializer = new PrimitiveValuesSerializer ();
			var serializer = new XdtoSerializerImpl (rawFactory);
			serializer.RegisterXdtoType (TypeManager.GetTypeByName ("Число"), primitiveSerializer);
			serializer.RegisterXdtoType (TypeManager.GetTypeByName ("Булево"), primitiveSerializer);
			serializer.RegisterXdtoType (TypeManager.GetTypeByName ("Строка"), primitiveSerializer);
			serializer.RegisterXdtoType (TypeManager.GetTypeByName ("Дата"), primitiveSerializer);

			return serializer;
		}
	}
}
