using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;

namespace TinyXdto
{
	[ContextClass ("СериализаторXDTO", "XDTOSerializer")]
	public class XdtoSerializerImpl : AutoContext<XdtoSerializerImpl>
	{

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
						var dataType = XmlTypeOf (rawValue).Value;
						var nsPrefix = xmlWriter.LookupPrefix (dataType.NamespaceUri);
						if (ValueFactory.Create().Equals(nsPrefix)) {
							// TODO: Присвоить новый префикс с хитрым порядком d1p1
							xmlWriter.WriteAttribute ("d1p1", "xmlns", dataType.NamespaceUri);
							nsPrefix = ValueFactory.Create ("d1p1");
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
		}

		private static UndefinedOr<XmlDataType> Define (string xmlType)
		{
			return new UndefinedOr<XmlDataType> (new XmlDataType (xmlType, XmlNs.xs));
		}

		private static UndefinedOr<XmlDataType> Undefined ()
		{
			return new UndefinedOr<XmlDataType> (null);
		}

		[ContextMethod("XMLТипЗнч", "XMLTypeOf")]
		public UndefinedOr<XmlDataType> XmlTypeOf (IValue inValue)
		{
			IValue value = inValue.GetRawValue ();

			if (value.DataType == DataType.Number)
				return Define ("decimal");
			if (value.DataType == DataType.String)
				return Define ("string");
			if (value.DataType == DataType.Boolean)
				return Define ("boolean");
			if (value.DataType == DataType.Date)
				return Define ("dateTime");

			// TODO: Из фабрики

			return Undefined ();
		}

		[ContextMethod("XMLТип", "XMLType")]
		public UndefinedOr<XmlDataType> XmlType (IValue type)
		{
			throw new NotImplementedException ("XdtoSerializer.XmlType");
		}

		[ContextMethod("XMLСтрока", "XMLString")]
		public string XmlString (IValue inValue)
		{
			IValue value = inValue.GetRawValue ();

			if (value.DataType == DataType.Number)
				return string.Format ("{0}", value.AsNumber ());

			if (value.DataType == DataType.String)
				return value.ToString ();

			if (value.DataType == DataType.Boolean)
				return value.AsBoolean () ? "true" : "false";

			if (value.DataType == DataType.Date)
				return value.AsDate ().ToString ();

			// TODO: Из фабрики

			return "";
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

			return new XdtoSerializerImpl (rawFactory);
		}
	}
}
