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
		private readonly Dictionary<string, int> _indexes;

		internal OperationImpl(Operation operation)
		{
			Name = operation.Name;
			Documentation = operation.Documentation;
			ReturnValue = new ReturnValueImpl (operation.Messages.Output);

			Parameters = ParameterCollectionImpl.Create (operation.Messages.Input);

			_indexes = new Dictionary<string, int> ();

			int argumentIndex = 0;
			foreach (var messagePart in Parameters.Parts) {
				foreach (var param in messagePart.Parameters) {
					_indexes.Add (param.Name, argumentIndex);
					++argumentIndex;
				}
			}
		}

		[ContextProperty("ВозвращаемоеЗначение", "ReturnValue")]
		public ReturnValueImpl ReturnValue { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Параметры", "Parameters")]
		public ParameterCollectionImpl Parameters { get; }

		public void WriteRequestBody(XmlWriterImpl writer,
			XdtoSerializerImpl serializer,
			IValue [] arguments)
		{

			foreach (var messagePart in Parameters.Parts) {

				writer.WriteStartElement (messagePart.ElementName, messagePart.NamespaceUri);

				foreach (var param in messagePart.Parameters) {

					if (param.ParameterDirection == ParameterDirectionEnum.Out)
						continue;

					var argumentIndex = _indexes [param.Name];					serializer.WriteXml (writer, arguments [argumentIndex], param.Name, messagePart.NamespaceUri);

				}

				writer.WriteEndElement (); // messagePart.ElementName

			}

		}

		public IParsedResponse ParseResponse(XmlReaderImpl reader, XdtoSerializerImpl serializer)
		{

			var retValue = ValueFactory.Create ();
			var outputParams = new Dictionary<int, IValue> ();

			// TODO: Разбирать весь ответ через XDTO
			while (reader.Read ()) {

				if (reader.LocalName.Equals ("Fault")) {
					var faultString = "Soap Exception!";
					while (reader.Read ()) {
						if (reader.LocalName.Equals ("faultString")) {
							reader.Read ();
							reader.MoveToContent ();
							faultString = reader.Value;
							break;
						}
					}

					return new SoapExceptionResponse (faultString);
				}

				if (reader.LocalName.Equals ("return")) {
					reader.Read ();
					reader.MoveToContent ();

					// TODO: отдать фабрике
					retValue = ValueFactory.Create (reader.Value);

					reader.Read ();
					continue;
				}

				// TODO: выходные параметры, отдать фабрике

				var paramName = reader.LocalName;
				reader.Read ();
				reader.MoveToContent ();

				// TODO: отдать фабрике
				var paramValue = ValueFactory.Create (reader.Value);
				reader.Read ();

				if (_indexes.ContainsKey (paramName)) {
					var paramIndex = _indexes [paramName];
					outputParams.Add (paramIndex, paramValue);
				}
			}

			return new SuccessfulSoapResponse(retValue, outputParams);
		}

	}
}
