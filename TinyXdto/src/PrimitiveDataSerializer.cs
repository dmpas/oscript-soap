using System;
using ScriptEngine.Machine;
using System.Xml;

namespace TinyXdto
{

	public class PrimitiveDataSerializer
	{
		PrimitiveDataSerializer ()
		{
			Nil = true;
		}

		PrimitiveDataSerializer (XmlDataType dataType, string value)
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

		public static PrimitiveDataSerializer Create (IValue data)
		{
			IValue rawValue = data.GetRawValue ();

			if (rawValue.DataType == DataType.Undefined) {
				return new PrimitiveDataSerializer ();
			}

			if (rawValue.DataType == DataType.Boolean) {
				return new PrimitiveDataSerializer (new XmlDataType ("boolean"), Bool (data.AsBoolean ()));
			}

			if (rawValue.DataType == DataType.Date) {
				return new PrimitiveDataSerializer (new XmlDataType ("dateTime"), Date (data.AsDate ()));
			}

			if (rawValue.DataType == DataType.Number) {
				return new PrimitiveDataSerializer (new XmlDataType ("decimal"), Number (data.AsNumber ()));
			}

			if (rawValue.DataType == DataType.String) {
				return new PrimitiveDataSerializer (new XmlDataType ("string"), data.AsString ());
			}

			// Non-primitive value
			return null;
		}

	}
}
