using System;
using ScriptEngine.Machine;
using System.Xml;

namespace TinyXdto
{
	public sealed class PrimitiveValuesSerializer : IXdtoSerializer, IXdtoDeserializer
	{

		public IValue DeserializeXdto (IXdtoValue xdtoValue)
		{
			if (xdtoValue is XdtoDataValueImpl) {
				return (xdtoValue as XdtoDataValueImpl).Value;
			}
			throw new NotImplementedException ();
		}

		public IXdtoValue SerializeXdto (IValue value, IXdtoType requestedType)
		{
			var rawValue = value.GetRawValue ();

			if (rawValue.DataType == DataType.Undefined) {
				return null;
			}

			if (rawValue.DataType == DataType.Boolean) {
				return new XdtoDataValueImpl (requestedType as XdtoValueTypeImpl, XmlConvert.ToString(rawValue.AsBoolean()), value);
			}

			if (rawValue.DataType == DataType.Date) {
				return new XdtoDataValueImpl (requestedType as XdtoValueTypeImpl, XmlConvert.ToString (rawValue.AsDate (), "yyyy-MM-ddTHH:mm:ss"), value);
			}

			if (rawValue.DataType == DataType.Number) {
				return new XdtoDataValueImpl (requestedType as XdtoValueTypeImpl, XmlConvert.ToString (rawValue.AsNumber ()), value);
			}

			if (rawValue.DataType == DataType.String) {
				return new XdtoDataValueImpl (requestedType as XdtoValueTypeImpl, rawValue.AsString (), value);
			}
			// Non-primitive value
			return null;
		}

		public XmlDataType GetPossibleType (IValue value)
		{
			var rawValue = value.GetRawValue ();
			if (rawValue.DataType == DataType.Boolean) {
				return new XmlDataType ("boolean");
			}

			if (rawValue.DataType == DataType.Date) {
				return new XmlDataType ("dateTime");
			}

			if (rawValue.DataType == DataType.Number) {
				return new XmlDataType ("decimal");
			}

			if (rawValue.DataType == DataType.String) {
				return new XmlDataType ("string");
			}
			return null;
		}
	}
}
