/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Web.Services.Description;
using System.Xml.Schema;
using TinyXdto;

namespace OneScript.Soap
{
	[ContextClass("WSПараметр", "WSParameter")]
	public class Parameter : AutoContext<Parameter>, IWithName
	{
		
		internal Parameter (XmlSchemaElement element, ParameterDirectionEnum direction, XdtoFactory factory)
		{
			Name = element.Name;
			Nillable = element.IsNillable;
			ParameterDirection = direction;
			Documentation = "";
			if (element.SchemaType is XmlSchemaSimpleType) {

				Type = new XdtoValueType (element.SchemaType as XmlSchemaSimpleType, factory);

			} else if (element.SchemaType is XmlSchemaComplexType) {

				Type = new XdtoObjectType (element.SchemaType as XmlSchemaComplexType, factory);

			} else {

				if (element.SchemaTypeName != null) {
					Type = factory.Type (element.SchemaTypeName.Namespace, element.SchemaTypeName.Name);
				}

			}
		}

		internal Parameter (string name,
		                        ParameterDirectionEnum direction = ParameterDirectionEnum.InOut,
								bool nillable = true,
								string documentation = "")
		{
			Name = name;
			Nillable = nillable;
			ParameterDirection = direction;
			Documentation = documentation;
		}

		[ContextProperty("ВозможноПустой", "Nillable")]
		public bool Nillable { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("НаправлениеПараметра", "ParameterDirection")]
		public ParameterDirectionEnum ParameterDirection { get; }

		[ContextProperty("Тип", "Type")]
		public IXdtoType Type { get; }
	}
}

