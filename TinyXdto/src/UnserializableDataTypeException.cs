namespace TinyXdto
{
	public class UnserializableDataTypeException : XdtoException
	{
		public UnserializableDataTypeException (string namespaceUri, string typeName)
			: base (string.Format ("Неопознанный тип {{{0}}}{1}!", namespaceUri, typeName))
		{
		}

		public UnserializableDataTypeException (XmlDataType type)
			: this (type.NamespaceUri, type.TypeName)
		{
		}
	}
}
