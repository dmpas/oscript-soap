/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Web.Services.Description;

namespace OneScript.Soap
{
	[ContextClass("WSИнтерфейс", "WSInterface")]
	public class Interface : AutoContext<Interface>, IWithName
	{
		internal Interface(PortType portType, TinyXdto.XdtoFactory factory)
		{
			Documentation = portType.Documentation;
			Name = portType.Name;
			NamespaceURI = portType.ServiceDescription.TargetNamespace;
			Operations = OperationCollection.Create (portType.Operations, factory);
		}

		internal Interface (string namespaceUri,
							    string documentation,
							    string name,
							    OperationCollection operations)
		{
			NamespaceURI = namespaceUri;
			Documentation = documentation;
			Name = name;
			Operations = operations;
		}

		[ContextProperty("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceURI { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Операции", "Operations")]
		public OperationCollection Operations { get; }
	}
}
