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
	public class ParameterCollectionImpl : FixedCollectionOf<ParameterImpl>
	{

		private List<MessagePartProxy> _parts = new List<MessagePartProxy> ();

		internal ParameterCollectionImpl (IEnumerable<ParameterImpl> data, IEnumerable<MessagePartProxy> parts) : base(data)
		{
			_parts.AddRange (parts);
		}

		public IEnumerable<MessagePartProxy> Parts { get { return _parts; } }

		internal static ParameterCollectionImpl Create (OperationInput operation)
		{
			var data = new List<ParameterImpl> ();
			var parts = new List<MessagePartProxy> ();

			var def = operation.Operation.PortType.ServiceDescription;

			var message = def.Messages [operation.Message.Name];

			foreach (var oPart in message.Parts) {
				var parametersPart = oPart as MessagePart;

				var partParameters = new List<ParameterImpl> ();
				foreach (var oSchema in def.Types.Schemas) {
					var schema = oSchema as XmlSchema;

					var type = schema.Elements [parametersPart.Element] as XmlSchemaElement;
					if (type == null)
						continue;
				
					var items = ((type.SchemaType as XmlSchemaComplexType).Particle as XmlSchemaSequence).Items;

					foreach (var item in items) {
						partParameters.Add (new ParameterImpl (item as XmlSchemaElement));
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

			return new ParameterCollectionImpl (data, parts);
		}

	}
}

