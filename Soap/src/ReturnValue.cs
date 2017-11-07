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
using TinyXdto;
using System.Collections.Generic;

namespace OneScript.Soap
{
	[ContextClass("WSВозвращаемоеЗначение", "WSReturnValue")]
	public class ReturnValue : AutoContext<ReturnValue>
	{
		internal ReturnValue (OperationOutput returnValue, TinyXdto.XdtoFactory factory)
		{
			Documentation = returnValue.Documentation;
			MessagePartName = "";

			OutputParamNames = new List<string> ();

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

				var ResponseType = type as XdtoObjectType;

				if (ResponseType == null && Type == null)
				{
					Type = type;
					continue;
				}
				else
				{
					foreach (var property in ResponseType.Properties)
					{

						if (property.LocalName == "return")
						{
							Type = property.Type;
						}
						else
						{
							OutputParamNames.Add(property.LocalName);
						}

					}
				}

				// Поддерживаем сообщения только из одной части
				break;
			}

		}

		internal ReturnValue (IXdtoType type = null,
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
		public IXdtoType Type { get; }

		public string MessagePartName { get; }

		public IList<string> OutputParamNames { get; }
	}
}

