using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Web.Services.Description;

namespace OneScript.Soap
{
	[ContextClass("WSВозвращаемоеЗначение", "WSReturnValue")]
	public class ReturnValueImpl : AutoContext<ReturnValueImpl>
	{
		internal ReturnValueImpl (OperationOutput returnValue, TinyXdto.XdtoFactoryImpl factory)
		{
			Type = ValueFactory.Create ();
			Documentation = returnValue.Documentation;
			MessagePartName = "";

			var message = returnValue.Operation.PortType.ServiceDescription.Messages [returnValue.Message.Name];
			foreach (var oPart in message.Parts) {
				
				var returnPart = oPart as MessagePart;
				var package = factory.Packages.Get (returnPart.Element.Namespace);
				if (package == null) {
					continue;
				}

				var type = package.Get (returnPart.Element.Name);
				if (type == null) {
					continue;
				}
				Type = ValueFactory.Create(type);
				break;
			}

		}

		internal ReturnValueImpl (IValue type = null,
		                          string messagePartName = "",
		                          bool nillable = false,
		                          string documentation = "")
		{
			Type = type;
			Nillable = nillable;
			Documentation = documentation;
			MessagePartName = messagePartName;
		}

		[ContextProperty("ВозможноПустое", "Nillable")]
		public bool Nillable { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Тип", "Type")]
		public IValue Type { get; }

		public string MessagePartName { get; }
	}
}

