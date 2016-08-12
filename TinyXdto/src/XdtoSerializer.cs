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

		[ContextMethod ("ЗаписатьXML", "WriteXML")]
		public void WriteXml (XmlWriterImpl xmlWriter,
							  IValue value,
							  string localName,
							  string namespaceUri,
							  XmlTypeAssignmentEnum typeAssignment = null,
							  XmlFormEnum xmlForm = null)
		{
			XdtoFactory.WriteXml (xmlWriter, value, localName, namespaceUri, typeAssignment, xmlForm);
		}

		private static UndefinedOr<XmlDataType> Define (string xmlType)
		{
			return new UndefinedOr<XmlDataType> (new XmlDataType (xmlType, "http://www.w3.org/2001/XMLSchema"));
		}

		private static UndefinedOr<XmlDataType> Undefined ()
		{
			return new UndefinedOr<XmlDataType> (null);
		}

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

		public UndefinedOr<XmlDataType> XmlType (IValue type)
		{
			return Undefined ();
		}


		// TODO: Убрать static
		public static string XmlString (IValue inValue)
		{
			IValue value = inValue.GetRawValue ();

			if (value.DataType == DataType.Number)
				return string.Format("{0}", value.AsNumber ());

			if (value.DataType == DataType.String)
				return value.ToString ();

			if (value.DataType == DataType.Boolean)
				return value.AsBoolean () ? "true" : "false";

			if (value.DataType == DataType.Date)
				return value.AsDate ().ToString ();

			return "";
		}

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
