/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Collections.Generic;
using System.Web.Services.Description;
using System.Linq;
using System.Xml.Schema;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияПараметров", "WSParameterCollection")]
	public class ParameterCollection : FixedCollectionOf<Parameter>
	{

		private List<MessagePartProxy> _parts = new List<MessagePartProxy> ();

		internal ParameterCollection (IEnumerable<Parameter> data, IEnumerable<MessagePartProxy> parts) : base(data)
		{
			_parts.AddRange (parts);
		}

		public IEnumerable<MessagePartProxy> Parts { get { return _parts; } }

		internal static ParameterCollection Create (OperationInput operation, ReturnValue returnValue, TinyXdto.XdtoFactory factory)
		{
			var data = new List<Parameter> ();
			var parts = new List<MessagePartProxy> ();

			var def = operation.Operation.PortType.ServiceDescription;

			var message = def.Messages [operation.Message.Name];

			foreach (var oPart in message.Parts) {
				var parametersPart = oPart as MessagePart;

				var partParameters = new List<Parameter> ();
				foreach (var oSchema in def.Types.Schemas) {
					var schema = oSchema as XmlSchema;

					var type = schema.Elements [parametersPart.Element] as XmlSchemaElement;
					if (type == null)
						continue;
				
					var items = ((type.SchemaType as XmlSchemaComplexType).Particle as XmlSchemaSequence).Items;

					foreach (var item in items) {
						var element = item as XmlSchemaElement;
						var direction = returnValue.OutputParamNames.Contains (element.Name)
												   ? ParameterDirectionEnum.InOut
												   : ParameterDirectionEnum.In
												   ;

						partParameters.Add (new Parameter (element, direction, factory));
					}
				}

				data.AddRange (partParameters);
				parts.Add (new MessagePartProxy {
					Parameters = partParameters,
					Name = parametersPart.Name,
					ElementName = parametersPart.Element.Name,
					NamespaceUri = parametersPart.Element.Namespace
				});
			}

			// TODO: добавить ТОЛЬКО выходные параметры

			return new ParameterCollection (data, parts);
		}

	}
}

