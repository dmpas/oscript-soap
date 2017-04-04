using System;
using ScriptEngine.Machine;

namespace TinyXdto
{
	public interface IXdtoValue : IValue
	{
		XmlDataType XmlType ();
	}
}

