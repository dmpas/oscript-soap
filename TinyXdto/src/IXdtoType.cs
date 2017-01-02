using System;
using ScriptEngine.Machine;

namespace TinyXdto
{
	public interface IXdtoType : IValue
	{
		string Name { get; }
		string NamespaceUri { get; }
	}
}

