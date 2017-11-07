/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[ContextClass("СвойствоXDTO", "XDTOProperty")]
	public class XdtoProperty : AutoContext<XdtoProperty>, INamed
	{
		private IXdtoType _type;
		private IXdtoType _ownerType;

		internal XdtoProperty ()
		{
		}

		internal XdtoProperty (XmlFormEnum form,
			string namespaceUri,
			string localName,
			IXdtoType type = null)
		{
			NamespaceURI = namespaceUri;
			LocalName = localName;
			Name = localName;
			Form = form;
			LowerBound = 1;
			UpperBound = 1;
			_type = type;
		}

		internal XdtoProperty (IXdtoType ownerType, XdtoDataObject ownerObject,
			XmlFormEnum form,
			string namespaceUri,
			string localName,
			int lowerBound = 1,
			int upperBound = 1,
			IXdtoType type = null)
		{
			NamespaceURI = namespaceUri;
			LocalName = localName;
			Name = localName;
			Form = form;
			_ownerType = ownerType;
			OwnerObject = ownerObject;
			LowerBound = lowerBound;
			UpperBound = upperBound;
			_type = type;
		}
		
		[ContextProperty ("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceURI { get; }

		[ContextProperty ("ВерхняяГраница", "UpperBound")]
		public int UpperBound { get; }

		[ContextProperty ("ВозможноПустое", "Nillable")]
		public bool Nillable { get; }

		[ContextProperty ("ЗначениеПоУмолчанию", "DefaultValue")]
		public XdtoDataValue DefaultValue { get; }

		[ContextProperty ("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Квалифицированное", "Qualified")]
		public bool Qualified { get; }

		[ContextProperty ("ЛокальноеИмя", "LocalName")]
		public string LocalName { get; }

		[ContextProperty ("НижняяГраница", "LowerBound")]
		public int LowerBound { get; }

		[ContextProperty ("ОбъектВладелец", "OwnerObject")]
		public XdtoDataObject OwnerObject { get; }

		[ContextProperty ("Тип", "Type")]
		public IXdtoType Type {
			get {
				if (_type is TypeResolver) {
					_type = (_type as TypeResolver).Resolve ();
				}
				return _type;
			}
		}

		[ContextProperty ("ТипВладелец", "OwnerType")]
		public IXdtoType OwnerType {
			get {
				if (_ownerType is TypeResolver) {
					_ownerType = (_ownerType as TypeResolver).Resolve ();
				}
				return _ownerType;
			}
		}

		[ContextProperty ("Фиксированное", "Fixed")]
		public bool Fixed { get; }

		[ContextProperty ("Форма", "Form")]
		public XmlFormEnum Form { get; }

		public override bool Equals (object obj)
		{
			var asThis = obj as XdtoProperty;
			if (asThis == null)
				return false;

			// TODO: вменяемое сравнение (СвойствоXDTO.Equals())

			return string.Equals (NamespaceURI, asThis.NamespaceURI, StringComparison.Ordinal)
						 && string.Equals (LocalName, asThis.LocalName, StringComparison.Ordinal);
		}

		public override int GetHashCode ()
		{
			return (NamespaceURI?.GetHashCode () ?? 0) + (LocalName?.GetHashCode () ?? 0);
		}

		public string GetComparableName()
		{
			return Name;
		}
	}
}

