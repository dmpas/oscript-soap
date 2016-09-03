using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Web.Services.Description;
using System.Collections.Generic;
using ScriptEngine.HostedScript.Library.Xml;
using TinyXdto;

namespace OneScript.Soap
{
	[ContextClass("WSОперация", "WSOperation")]
	public class OperationImpl : AutoContext<OperationImpl>, IWithName
	{
		private static ParameterDirectionEnum directionEnum = ParameterDirectionEnum.CreateInstance ();

		internal OperationImpl(Operation operation)
		{
			Name = operation.Name;
			Documentation = operation.Documentation;
			ReturnValue = new ReturnValueImpl (operation.Messages.Output);

			Parameters = ParameterCollectionImpl.Create (operation.Messages.Input);
		}

		[ContextProperty("ВозвращаемоеЗначение", "ReturnValue")]
		public ReturnValueImpl ReturnValue { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Параметры", "Parameters")]
		public ParameterCollectionImpl Parameters { get; }

		public void WriteRequestBody(XmlWriterImpl writer, string namespaceUri,
			XdtoSerializerImpl serializer,
			IValue [] arguments)
		{

			int argumentIndex = 0;
			foreach (var messagePart in Parameters.Parts) {

				// TODO: namespace ???
				writer.WriteStartElement (messagePart.ElementName, namespaceUri);

				foreach (var param in messagePart.Parameters) {

					if (param.ParameterDirection.Equals (directionEnum.Out))
						continue;

					serializer.WriteXml (writer, arguments [argumentIndex], param.Name, namespaceUri);

					++argumentIndex;

				}

				writer.WriteEndElement (); // messagePart.ElementName

			}

		}
	}
}
