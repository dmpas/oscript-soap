using System;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;

namespace TinyXdto
{
	public interface IXdtoType : IRuntimeContextInstance
	{
		string Name { get; }
		string NamespaceUri { get; }

		IXdtoReader Reader { get; }
	}
}
