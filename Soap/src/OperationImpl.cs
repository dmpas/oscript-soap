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
		private readonly Dictionary<string, int> _indexes = new Dictionary<string, int> ();

		internal OperationImpl(Operation operation, XdtoFactoryImpl factory)
		{
			Name = operation.Name;
			NamespaceUri = operation.PortType.ServiceDescription.TargetNamespace;
			Documentation = operation.Documentation;
			ReturnValue = new ReturnValueImpl (operation.Messages.Output, factory);

			Parameters = ParameterCollectionImpl.Create (operation.Messages.Input,
			                                             ReturnValue,
			                                             factory);

			int argumentIndex = 0;
			foreach (var messagePart in Parameters.Parts) {
				foreach (var param in messagePart.Parameters) {
					_indexes.Add (param.Name, argumentIndex);
					++argumentIndex;
				}
			}
		}

		internal OperationImpl (MethodInfo methodInfo, string namespaceUri)
		{
			Name = methodInfo.Name;
			Documentation = "";
			NamespaceUri = namespaceUri;
			ReturnValue = new ReturnValueImpl (
				null,
				string.Format("tns:{0}ResponseMessage", Name),
				nillable: true);

			var messagePart = new MessagePartProxy ();
			messagePart.Name = "parameters";
			messagePart.ElementName = String.Format ("tns:{0}", methodInfo.Name);
			messagePart.NamespaceUri = namespaceUri;
			messagePart.Parameters = new List<ParameterImpl> ();

			int argumentIndex = 0;
			foreach (var paramInfo in methodInfo.Params) {
				var paramName = string.Format ("p{0}", argumentIndex);
				var param = new ParameterImpl (paramName,
											   paramInfo.IsByValue
				                               	? ParameterDirectionEnum.In
				                               	: ParameterDirectionEnum.InOut,
				                               true,
				                               Documentation);

				messagePart.Parameters.Add (param);
				_indexes.Add (paramName, argumentIndex);
				++argumentIndex;
			}

			Parameters = new ParameterCollectionImpl (messagePart.Parameters,
													  new MessagePartProxy [] { messagePart });
		}

		[ContextProperty("ВозвращаемоеЗначение", "ReturnValue")]
		public ReturnValueImpl ReturnValue { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Параметры", "Parameters")]
		public ParameterCollectionImpl Parameters { get; }

		public string NamespaceUri { get; }

		public void WriteRequestBody(XmlWriterImpl writer,
			XdtoSerializerImpl serializer,
			IValue [] arguments)
		{

			foreach (var messagePart in Parameters.Parts) {

				writer.WriteStartElement (messagePart.ElementName, messagePart.NamespaceUri);

				foreach (var param in messagePart.Parameters) {

					if (param.ParameterDirection == ParameterDirectionEnum.Out)
						continue;

					var argumentIndex = _indexes [param.Name];
					var typeAssignment = XmlTypeAssignmentEnum.Implicit;

					if (param.Type is XdtoValueTypeImpl)
						typeAssignment = XmlTypeAssignmentEnum.Explicit;
					serializer.WriteXml (writer, arguments [argumentIndex], param.Name, messagePart.NamespaceUri, typeAssignment);

				}

				writer.WriteEndElement (); // messagePart.ElementName

			}

		}

		// Особенности реализации: возвращаемое значение и исходящие параметры
		// передаём ОДНИМ сообщением, хотя протокол разрешает несколько сообщений

		public void WriteResponseBody (XmlWriterImpl writer,
									   XdtoSerializerImpl serializer,
									   IValue returnValue,
									   IValue [] arguments)
		{
			writer.WriteStartElement (ReturnValue.MessagePartName, NamespaceUri);

			serializer.WriteXml (writer, returnValue, "return", NamespaceUri);
			foreach (var param in Parameters) {
				if (param.ParameterDirection == ParameterDirectionEnum.In)
					continue;

				var argumentIndex = _indexes [param.Name];
				serializer.WriteXml (writer, arguments [argumentIndex], param.Name, NamespaceUri);
			}

			writer.WriteEndElement (); // messagePartName
		}

		public IParsedResponse ParseResponse(XmlReaderImpl reader, XdtoSerializerImpl serializer)
		{

			var retValue = ValueFactory.Create ();
			var outputParams = new Dictionary<int, IValue> ();

			var xmlNodeTypeEnum = XmlNodeTypeEnum.CreateInstance ();
			var xmlElementStart = xmlNodeTypeEnum.FromNativeValue (System.Xml.XmlNodeType.Element);

			if (!reader.Read ()
				|| !reader.LocalName.Equals ("Envelope")
				|| !reader.NodeType.Equals (xmlElementStart)
			   // TODO: перевести XML на простые перечисления
			   ) {
				return new SoapExceptionResponse ("Wrong response!");
			}
			
			reader.MoveToContent ();

			if (!reader.Read ()
				|| !reader.LocalName.Equals ("Body")
				|| !reader.NodeType.Equals (xmlElementStart)
			   // TODO: перевести XML на простые перечисления			   ) {
				return new SoapExceptionResponse ("Wrong response!");
			}

			var xdtoResult = serializer.XdtoFactory.ReadXml (reader, ReturnValue.ResponseType) as XdtoDataObjectImpl;
			retValue = xdtoResult.Get ("return");

			if (retValue is IXdtoValue) {
				retValue = serializer.ReadXdto (retValue as IXdtoValue);
			}

			foreach (var param in Parameters) {
				if (param.ParameterDirection == ParameterDirectionEnum.In)
					continue;

				var argumentIndex = _indexes [param.Name];
				IValue paramValue = xdtoResult.Get (param.Name);
				if (paramValue is IXdtoValue) {
					paramValue = serializer.ReadXdto (paramValue as IXdtoValue);
				}
				outputParams.Add (argumentIndex, paramValue);
			}

			return new SuccessfulSoapResponse(retValue, outputParams);
		}

	}
}
