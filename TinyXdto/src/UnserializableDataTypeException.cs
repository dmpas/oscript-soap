/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
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
