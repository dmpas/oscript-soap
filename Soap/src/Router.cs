/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using ScriptEngine.HostedScript.Library.Xml;
using TinyXdto;

namespace OneScript.Soap
{
	[ContextClass("WSСтрелочник", "WSRouter")]
	public class Router : AutoContext<Router>, ISoapTransport
	{

		const string xmlns_soap = @"http://schemas.xmlsoap.org/soap/envelope/";

		private readonly List<IRuntimeContextInstance> handlers = new List<IRuntimeContextInstance>();
		private readonly Dictionary<string, Operation> operations = new Dictionary<string, Operation> ();
		private readonly Dictionary<Operation, IRuntimeContextInstance> operationsMapper = new Dictionary<Operation, IRuntimeContextInstance> ();

		public Router (string name, string targetNamespace)
		{
			Name = name;
			NamespaceUri = targetNamespace;
		}

		[ContextProperty("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty ("Имя", "Name")]
		public string Name { get; }

		[ContextMethod("ДобавитьОбработчик", "AddHandler")]
		public void AddHandler (IValue ihandler)
		{
			var handler = ihandler as IRuntimeContextInstance;
			foreach (var methodInfo in handler.GetMethods ()) {

				var operation = new Operation (methodInfo, NamespaceUri);
				operations.Add (operation.Name, operation);
				operationsMapper.Add (operation, handler);

			}

			handlers.Add (handler);
		}

		[ContextMethod ("СформироватьWSDL", "GenerateWSDL")]
		public string GenerateWsdl ()
		{

			var writer = new XmlWriterImpl ();
			writer.SetString ();
			writer.WriteXMLDeclaration ();

			writer.WriteStartElement ("definitions");

			writer.WriteNamespaceMapping ("", "http://schemas.xmlsoap.org/wsdl/");
			writer.WriteNamespaceMapping ("soap12bind", "http://schemas.xmlsoap.org/wsdl/soap12/");
			writer.WriteNamespaceMapping ("soapbind", "http://schemas.xmlsoap.org/wsdl/soap/");
			writer.WriteNamespaceMapping ("tns", NamespaceUri);
			writer.WriteAttribute ("name", Name);
			writer.WriteAttribute ("targetNamespace", NamespaceUri);

			#region definitions/types

			writer.WriteStartElement ("types");

			writer.WriteStartElement ("xs:schema");
			writer.WriteNamespaceMapping ("xs", "http://www.w3.org/2001/XMLSchema");
			writer.WriteNamespaceMapping ("tns", NamespaceUri);

			writer.WriteAttribute ("targetNamespace", NamespaceUri);
			writer.WriteAttribute ("attributeFormDefault", "unqualified");
			writer.WriteAttribute ("elementFormDefault", "qualified");

			foreach (var operation in operations.Values) {

				foreach (var part in operation.Parameters.Parts) {

					writer.WriteStartElement ("xs:element");
					writer.WriteAttribute ("name", part.Name);

					writer.WriteStartElement ("xs:complexType");
					writer.WriteStartElement ("xs:sequence");
					foreach (var param in part.Parameters) {

						if (param.ParameterDirection == ParameterDirectionEnum.Out)
							continue;

						writer.WriteStartElement ("xs:element");
						writer.WriteAttribute ("name", param.Name);
						writer.WriteAttribute ("type", "xs:anyType");
						writer.WriteEndElement (); // xs:element

					}

					writer.WriteEndElement (); // xs:sequence
					writer.WriteEndElement (); // xs:complexType
					writer.WriteEndElement (); // xs:element
				}

				writer.WriteStartElement ("xs:element");
				writer.WriteAttribute ("name", operation.ReturnValue.MessagePartName);

				writer.WriteStartElement ("xs:complexType");
				writer.WriteStartElement ("xs:sequence");

				writer.WriteStartElement ("xs:element");
				writer.WriteAttribute ("name", "return");
				writer.WriteAttribute ("type", "xs:anyType");
				writer.WriteEndElement (); // xs:element

				foreach (var param in operation.Parameters) {

					if (param.ParameterDirection == ParameterDirectionEnum.In)
						continue;

					writer.WriteStartElement ("xs:element");
					writer.WriteAttribute ("name", param.Name);
					writer.WriteAttribute ("type", "xs:anyType");
					writer.WriteEndElement (); // xs:element

				}

				writer.WriteEndElement (); // xs:sequence
				writer.WriteEndElement (); // xs:complexType
				writer.WriteEndElement (); // xs:element
			}


			writer.WriteEndElement (); // xs:schema
			writer.WriteEndElement (); // types
			#endregion

			#region definitions/messages
			foreach (var operation in operations.Values) {

				writer.WriteStartElement ("message");
				writer.WriteAttribute ("name", string.Format ("{0}RequestMessage", operation.Name));

				foreach (var part in operation.Parameters.Parts) {
					writer.WriteStartElement ("part");
					writer.WriteAttribute ("name", part.Name);
					writer.WriteAttribute ("element", part.ElementName);
					writer.WriteEndElement (); // part
				}
				writer.WriteEndElement (); // message

				writer.WriteStartElement ("message");
				writer.WriteAttribute ("name", string.Format ("{0}ResponseMessage", operation.Name));

				writer.WriteStartElement ("part");
				writer.WriteAttribute ("name", "parameters");
				writer.WriteAttribute ("element", operation.ReturnValue.MessagePartName);
				writer.WriteEndElement (); // part

				writer.WriteEndElement (); // message

			}
			#endregion

			#region definitions/portType
			var portTypeName = string.Format ("{0}PortType", Name);

			writer.WriteStartElement ("portType");
			writer.WriteAttribute ("name", portTypeName);

			foreach (var operation in operations.Values) {

				writer.WriteStartElement ("operation");
				writer.WriteAttribute ("name", operation.Name);

				writer.WriteStartElement ("input");
				writer.WriteAttribute ("message", string.Format ("{0}RequestMessage", operation.Name));
				writer.WriteEndElement (); // input

				writer.WriteStartElement ("output");
				writer.WriteAttribute ("message", string.Format ("{0}ResponseMessage", operation.Name));
				writer.WriteEndElement (); // output

				writer.WriteEndElement (); // operation

			}

			writer.WriteEndElement (); // portType
			#endregion

			#region definitions/binding
			var mainBindingName = string.Format ("{0}Binding", Name);

			writer.WriteStartElement ("binding");
			writer.WriteAttribute ("name", mainBindingName);
			writer.WriteAttribute ("type", portTypeName);

			writer.WriteStartElement ("soapbind:binding");
			writer.WriteAttribute ("style", "document");
			writer.WriteAttribute ("transport", "http://schemas.xmlsoap.org/soap/http");
			writer.WriteEndElement (); // soapbind:binding

			foreach (var operation in operations.Values) {

				var soapActionString = string.Format ("{0}#{1}:{2}", NamespaceUri, Name, operation.Name);

				writer.WriteStartElement ("operation");
				writer.WriteAttribute ("name", operation.Name);

				writer.WriteStartElement ("soapbind:operation");
				writer.WriteAttribute ("style", "document");
				writer.WriteAttribute ("soapAction", soapActionString);

				writer.WriteStartElement ("input");
				writer.WriteStartElement ("soapbind:body");
				writer.WriteAttribute ("use", "literal");
				writer.WriteEndElement (); // soapbind:body
				writer.WriteEndElement (); // input

				writer.WriteStartElement ("output");
				writer.WriteStartElement ("soapbind:body");
				writer.WriteAttribute ("use", "literal");
				writer.WriteEndElement (); // soapbind:body
				writer.WriteEndElement (); // output

				writer.WriteEndElement (); // soapbind:operation
				writer.WriteEndElement (); // operation
			}

			writer.WriteEndElement (); // binding

			#endregion

			#region definitions/service
			var serviceUrl = "http://localhost/service";

			writer.WriteStartElement ("service");
			writer.WriteStartElement ("port");

			var mainPortName = string.Format ("{0}Soap", Name);
			writer.WriteAttribute ("name", mainPortName);
			writer.WriteAttribute ("binding", string.Format("tns:{0}", mainBindingName));

			writer.WriteStartElement ("documentation");
			writer.WriteEndElement (); // documentation

			writer.WriteStartElement ("soapbind:address");
			writer.WriteAttribute ("location", serviceUrl);
			writer.WriteEndElement (); // soapbind:address


			writer.WriteEndElement (); // port
			writer.WriteEndElement (); // service

			#endregion

			writer.WriteEndElement (); // definitions

			return writer.Close ().AsString();
		}

		private void WriteFault (XmlWriterImpl writer, string faultString)
		{
			writer.WriteStartElement ("Fault", xmlns_soap);
			writer.WriteStartElement ("faultString", xmlns_soap);

			writer.WriteText (faultString);

			writer.WriteEndElement (); // faultString
			writer.WriteEndElement (); // Fault
		}

		public void Handle (XmlReaderImpl requestReader,
		                    XmlWriterImpl responseWriter)
		{

			var F = new XdtoFactory();
			var S = XdtoSerializerImpl.Constructor(ValueFactory.Create(F)) as XdtoSerializerImpl;
			var inputMessage = F.ReadXml(requestReader) as XdtoDataObject;
			var body = inputMessage.Get("Body") as XdtoDataObject;

			var requestProperty = body.Properties().Get(0);
			var methodName = requestProperty.LocalName;
			if (!operations.ContainsKey(methodName))
			{
				// TODO: SoapException
				throw new RuntimeException($"method not found {methodName}");
			}

			var op = operations[methodName];
			
			// TODO: отдать разбор в операцию
			var args = new List<IValue>();
			var callParams = body.Get(body.Properties().Get(0)) as XdtoDataObject;
			foreach (var pv in callParams.Properties())
			{
				var paramName = pv.LocalName;
				var p = op.Parameters.Get(paramName);
				var xdtoValue = callParams.GetXdto(pv);
				var ivalue = S.ReadXdto(xdtoValue);

				if (p.ParameterDirection != ParameterDirectionEnum.In)
				{
					ivalue = Variable.Create(ivalue, paramName);
				}
				args.Add(ivalue);
			}
			
			var handler = operationsMapper[op];
			var methodIdx = handler.FindMethod(methodName);
			var mi = handler.GetMethodInfo(methodIdx);
			IValue result = null;
			if (mi.IsFunction)
			{
				handler.CallAsFunction(methodIdx, args.ToArray(), out result);
			}
			else
			{
				handler.CallAsProcedure(methodIdx, args.ToArray());
			}
			
			op.WriteResponseBody(responseWriter, S, result, args.ToArray());
		}

		public string Handle (string requestBody)
		{
			var reader = new XmlReaderImpl ();
			reader.SetString (requestBody);

			var writer = new XmlWriterImpl ();
			writer.SetString ();

			writer.WriteStartElement ("Envelope", xmlns_soap);
			writer.WriteNamespaceMapping ("soap", xmlns_soap);
			writer.WriteStartElement ("Body", xmlns_soap);
			
			try
			{
				Handle(reader, writer);
			}
			catch (Exception exc)
			{
				WriteFault (writer, exc.Message);
			}

			writer.WriteEndElement (); // Body
			writer.WriteEndElement (); // Envelope
			
			var responseText = writer.Close ().AsString ();
			return responseText;
		}

		public Interface CreateInterface ()
		{
			return new Interface (NamespaceUri, "", Name,
									 new OperationCollection (operations.Values));
		}

		public Endpoint CreateEndPoint ()
		{
			return new Endpoint (Name, "", CreateInterface (), this);
		}

		[ContextMethod("СоздатьПрокси", "CreateProxy")]
		public Proxy CreateProxy ()
		{
			return new Proxy (null, CreateEndPoint ());
		}

		[ScriptConstructor]
		public static IRuntimeContextInstance Constructor (IValue name, IValue namespaceUri)
		{
			return new Router (name.AsString(), namespaceUri.AsString ());
		}
	}
}
