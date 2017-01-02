using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;
using ScriptEngine.HostedScript.Library.Xml;

namespace OneScript.Soap
{
	[ContextClass("WSСтрелочник", "WSRouter")]
	public class Router : AutoContext<Router>
	{

		private readonly List<IReflectableContext> handlers = new List<IReflectableContext>();
		private readonly Dictionary<string, OperationImpl> operations = new Dictionary<string, OperationImpl> ();
		private readonly Dictionary<OperationImpl, IReflectableContext> operationsMapper = new Dictionary<OperationImpl, IReflectableContext> ();

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
			var handler = ihandler as IReflectableContext;
			foreach (var methodInfo in handler.GetMethods ()) {

				var operation = new OperationImpl (methodInfo, NamespaceUri);
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

		public void Handle (XmlReaderImpl requestReader,
		                    XmlWriterImpl responseWriter)
		{
			// TODO: Отдать фабрике
			responseWriter.WriteStartElement ("Envelope");
			responseWriter.WriteStartElement ("Body");
			responseWriter.WriteStartElement ("Fault");
			responseWriter.WriteStartElement ("FaultString");

			responseWriter.WriteText ("Not implemented!");

			responseWriter.WriteEndElement (); // FaultString
			responseWriter.WriteEndElement (); // Fault
			responseWriter.WriteEndElement (); // Body
			responseWriter.WriteEndElement (); // Envelope
		}

		[ScriptConstructor]
		public static IReflectableContext Constructor (IValue name, IValue namespaceUri)
		{
			return new Router (name.AsString(), namespaceUri.AsString ());
		}
	}
}
