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

		internal ParameterCollectionImpl (IEnumerable<ParameterImpl> data) : base(data)
		{
		}

		internal static ParameterCollectionImpl Create (OperationInput operation)
		{
			var data = new List<ParameterImpl> ();

			var def = operation.Operation.PortType.ServiceDescription;

			var message = def.Messages [operation.Message.Name];
			var parametersPart = message.FindPartByName ("parameters");
			foreach (var oSchema in def.Types.Schemas) {
				var schema = oSchema as XmlSchema;

				var type = schema.Elements [parametersPart.Element] as XmlSchemaElement;
				if (type == null)
					continue;
				
				var items = ((type.SchemaType as XmlSchemaComplexType).Particle as XmlSchemaSequence).Items;

				foreach (var item in items) {
					data.Add (new ParameterImpl (item as XmlSchemaElement));
				}
			}

			return new ParameterCollectionImpl (data);
		}

	}
}

