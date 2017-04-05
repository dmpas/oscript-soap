using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Web.Services.Description;
using System.Xml.Schema;
using TinyXdto;

namespace OneScript.Soap
{
	[ContextClass("WSПараметр", "WSParameter")]
	public class ParameterImpl : AutoContext<ParameterImpl>, IWithName
	{
		
		internal ParameterImpl (XmlSchemaElement element, XdtoFactoryImpl factory)
		{
			Name = element.Name;
			Nillable = element.IsNillable;
			ParameterDirection = ParameterDirectionEnum.In;
			Documentation = "";
			if (element.SchemaType is XmlSchemaSimpleType) {

				Type = new XdtoValueTypeImpl (element.SchemaType as XmlSchemaSimpleType);

			} else if (element.SchemaType is XmlSchemaComplexType) {

				Type = new XdtoObjectTypeImpl (element.SchemaType as XmlSchemaComplexType, factory);

			} else {

				if (element.SchemaTypeName != null) {
					Type = factory.Type (element.SchemaTypeName.Namespace, element.SchemaTypeName.Name);
				}

			}
		}

		internal ParameterImpl (string name,
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

