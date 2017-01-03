using System;
using ScriptEngine.Machine;
using System.Xml;

namespace TinyXdto
{
	public sealed class SerializedPrimitiveValue
	{
		SerializedPrimitiveValue ()
		{
			Nil = true;
		}

		SerializedPrimitiveValue (XmlDataType dataType, string value)
		{
			Nil = false;
			Type = dataType;
			SerializedValue = value;
		}

		public XmlDataType Type { get; }
		public string SerializedValue { get; }
		public bool Nil { get; }

		public static string Bool (bool value)
		{
			return XmlConvert.ToString (value);
		}

		public static string Date (DateTime value)
		{
			return value.ToString ("o");
		}

		public static string Number (decimal value)
		{
			return XmlConvert.ToString (value);
		}

		public static SerializedPrimitiveValue Create (IValue data)
		{
			IValue rawValue = data.GetRawValue ();

			if (rawValue.DataType == DataType.Undefined) {
				return new SerializedPrimitiveValue ();
			}

			if (rawValue.DataType == DataType.Boolean) {
				return new SerializedPrimitiveValue (new XmlDataType ("boolean"), Bool (data.AsBoolean ()));
			}

			if (rawValue.DataType == DataType.Date) {
				return new SerializedPrimitiveValue (new XmlDataType ("dateTime"), Date (data.AsDate ()));
			}

			if (rawValue.DataType == DataType.Number) {
				return new SerializedPrimitiveValue (new XmlDataType ("decimal"), Number (data.AsNumber ()));
			}

			if (rawValue.DataType == DataType.String) {
				return new SerializedPrimitiveValue (new XmlDataType ("string"), data.AsString ());
			}

			// Non-primitive value
			return null;
		}

		public static IValue Convert (XmlDataType type, string lexicalValue)
		{
			if (!type.NamespaceUri.Equals (XmlNs.xs)) {
				return null;
			}

			if (type.TypeName.Equals ("boolean"))
				return ValueFactory.Create (Boolean.Parse (lexicalValue));

			if (type.TypeName.Equals ("dateTime")) {
				return ValueFactory.Create (DateTime.Parse (lexicalValue));
			}

			if (type.TypeName.Equals ("date")) {
				return ValueFactory.Create (DateTime.Parse (lexicalValue));
			}

			if (type.TypeName.Equals ("time")) {
				return ValueFactory.Create (DateTime.Parse (lexicalValue));
			}

			if (type.TypeName.Equals ("decimal")) {
				return ValueFactory.Create (Decimal.Parse (lexicalValue));
			}

			if (type.TypeName.Equals ("double")
			   || type.TypeName.Equals ("float")) {
				return ValueFactory.Create ((Decimal) Double.Parse (lexicalValue));
			}

			if (type.TypeName.Equals ("anySimpleType")
				|| type.TypeName.Equals ("anyURI")
				|| type.TypeName.Equals ("duration")
				|| type.TypeName.Equals ("gDay")
				|| type.TypeName.Equals ("gMonth")
				|| type.TypeName.Equals ("gYear")
				|| type.TypeName.Equals ("gYearMonth")
				|| type.TypeName.Equals ("NOTATION")
				|| type.TypeName.Equals ("string")
			   ) {
				return ValueFactory.Create (lexicalValue);
			}

			throw new UnserializableDataTypeException (type);
		}
	}
}
