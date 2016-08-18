using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace OneScript.Soap
{
	public interface IWithName : IValue
	{
		string Name { get; }
	}
}

