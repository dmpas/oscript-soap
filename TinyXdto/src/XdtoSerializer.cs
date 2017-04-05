using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;
using System.Collections.Generic;
using System.Xml;

namespace TinyXdto
{
	[ContextClass ("СериализаторXDTO", "XDTOSerializer")]
	public class XdtoSerializerImpl : AutoContext<XdtoSerializerImpl>
	{

		private readonly Dictionary<TypeDescriptor, IXdtoSerializer>   serializers = new Dictionary<TypeDescriptor, IXdtoSerializer> ();
		private readonly Dictionary<XmlDataType, IXdtoDeserializer> deserializers = new Dictionary<XmlDataType, IXdtoDeserializer> ();

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
		}
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

		public void RegisterXdtoSerializer (TypeDescriptor type, IXdtoSerializer processor)
		{
			if (processor is IXdtoSerializer) {
				serializers [type] = processor as IXdtoSerializer;
			}
		}

		public void RegisterXdtoDeserializer (XmlDataType type, IXdtoDeserializer processor)
		{
			if (processor is IXdtoDeserializer) {
				deserializers [type] = processor as IXdtoDeserializer;
			}
		}

		[ContextMethod("XMLЗначение", "XMLValue")]
		public IValue XmlValue (IValue type, IValue xmlString)
		{
			throw new NotImplementedException ("XdtoSerializer.XmlValue");
		}


		[ContextMethod("ПрочитатьXDTO")]
		public IValue ReadXdto (IXdtoValue xdtoObject)
		{
			var t = xdtoObject.XmlType ();
			if (!deserializers.ContainsKey (t)) {
				throw new XdtoException (String.Format ("Не поддерживается сериализация для {0}", t));
			}
			var d = deserializers [t];
			return d.DeserializeXdto (xdtoObject);
		}

		[ContextMethod("ПрочитатьXML")]
		public IValue ReadXml (XmlReaderImpl reader, TypeTypeValue requestedType = null)
		{
			var xdtoValue = XdtoFactory.ReadXml (reader);
			return ReadXdto (xdtoValue);
		}

		[ScriptConstructor]
		public static IRuntimeContextInstance Constructor (IValue factory)
		{
			XdtoFactoryImpl rawFactory = factory.GetRawValue () as XdtoFactoryImpl;
			if (rawFactory == null)
				throw RuntimeException.InvalidArgumentType ("factory");

			var primitiveSerializer = new PrimitiveValuesSerializer ();
			var serializer = new XdtoSerializerImpl (rawFactory);
			serializer.RegisterXdtoSerializer (TypeManager.GetTypeByName ("Число"), primitiveSerializer);
			serializer.RegisterXdtoSerializer (TypeManager.GetTypeByName ("Булево"), primitiveSerializer);
			serializer.RegisterXdtoSerializer (TypeManager.GetTypeByName ("Строка"), primitiveSerializer);
			serializer.RegisterXdtoSerializer (TypeManager.GetTypeByName ("Дата"), primitiveSerializer);

			serializer.RegisterXdtoDeserializer (new XmlDataType ("int"), primitiveSerializer);
			serializer.RegisterXdtoDeserializer (new XmlDataType ("decimal"), primitiveSerializer);
			serializer.RegisterXdtoDeserializer (new XmlDataType ("float"), primitiveSerializer);

			return serializer;
		}
	}
}
