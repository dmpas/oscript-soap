using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;

namespace TinyXdto
{
	[ContextClass ("СериализаторXDTO", "XDTOSerializer")]
	public class XdtoSerializerImpl : AutoContext<XdtoSerializerImpl>
	{

		static readonly XmlFormEnum XmlForm = XmlFormEnum.CreateInstance ();
		static readonly XmlTypeAssignmentEnum XmlTypeAssignment = XmlTypeAssignmentEnum.CreateInstance ();

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

		private string GetStringForPrimitiveValue (IValue value)
		{
			if (ValueFactory.Create().Equals(value))
				return null;
			return XmlString (value);
		}

		[ContextMethod ("ЗаписатьXML", "WriteXML")]
		public void WriteXml (XmlWriterImpl xmlWriter,
							  IValue value,
							  string localName,
							  string namespaceUri,
							  EnumerationValue typeAssignment = null,
							  EnumerationValue xmlForm = null)
		{

			IValue rawValue = value.GetRawValue ();

			if (!IsPrimitiveValue(rawValue)) {
				XdtoFactory.WriteXml (xmlWriter, value, localName, namespaceUri, typeAssignment, xmlForm);
				return;
			}
			var primitive = GetStringForPrimitiveValue (rawValue);

			if (xmlForm == null || xmlForm.Equals (XmlForm.Element)) {

				xmlWriter.WriteStartElement (localName, namespaceUri);

				if (primitive == null) {
					xmlWriter.WriteAttribute ("nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				} else {
					if (typeAssignment != null && XmlTypeAssignment.Explicit.Equals (typeAssignment)) {
						var dataType = XmlTypeOf (rawValue).Value;
						// TODO: Ждать 15-й версии для НайтиПрефикс
						// var nsPrefix = xmlWriter.LookupPrefix (dataType.NamespaceUri);
						// if (ValueFactory.Create().Equals(nsPrefix)) {
						// 	// TODO: Присвоить новый префикс с хитрым порядком d1p1
						// }
						// var typeValue = String.Format("{0}:{1}", nsPrefix, dataType.TypeName);
						// xmlWriter.WriteAtribute("type", "http://www.w3.org/2001/XMLSchema-instance", typeValue);
					}
				}

				xmlWriter.WriteText (primitive);
				xmlWriter.WriteEndElement ();

			} else
			if (xmlForm.Equals (XmlForm.Attribute)) {

				xmlWriter.WriteAttribute (localName, namespaceUri, primitive);

			} else if (xmlForm.Equals (XmlForm.Text)) {

				xmlWriter.WriteText (primitive);

			} else {
				throw RuntimeException.InvalidArgumentType ("xmlForm");
			}
		}

		private static UndefinedOr<XmlDataType> Define (string xmlType)
		{
			return new UndefinedOr<XmlDataType> (new XmlDataType (xmlType, "http://www.w3.org/2001/XMLSchema"));
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

			return Undefined ();
		}

		[ContextMethod("XMLТип", "XMLType")]
		public UndefinedOr<XmlDataType> XmlType (IValue type)
		{
			return Undefined ();
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
