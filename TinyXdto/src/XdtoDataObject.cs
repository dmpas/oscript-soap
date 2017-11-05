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

namespace TinyXdto
{
	[ContextClass("ОбъектXDTO", "XDTODataObject")]
	public class XdtoDataObject : DynamicPropertiesAccessor, IXdtoValue
	{
		private readonly XdtoDataObject _owner;
		private readonly XdtoProperty _owningProperty;
		private readonly Dictionary<XdtoProperty, IValue> _data = new Dictionary<XdtoProperty, IValue> ();
		private readonly XdtoObjectType _type;
		private readonly XdtoSequence _sequence;

		internal XdtoDataObject (XdtoObjectType type, XdtoDataObject owner, XdtoProperty property)
		{
			_type = type;
			_owner = owner;
			_owningProperty = property;
			_sequence = new XdtoSequence (this, true);
		}

		[ContextMethod ("Владелец", "Owner")]
		public XdtoDataObject Owner ()
		{
			return _owner;
		}

		[ContextMethod ("ВладеющееСвойство", "OwningProperty")]
		public XdtoProperty OwningProperty ()
		{
			return _owningProperty;
		}


		[ContextMethod("Добавить", "Add")]
		public void Add (XmlFormEnum form, string namespaceUri, string localName, IXdtoValue dataElement)
		{
			if (_type?.Open ?? true) {
				var customProperty = new XdtoProperty (this, form, namespaceUri, localName);
				Add (customProperty, dataElement);
			}

			throw new RuntimeException ("Добавлять можно только в объекты открытого типа!");
		}

		public void Add (XdtoProperty property, IXdtoValue dataElement)
		{
			XdtoList list;
			if (_data.ContainsKey (property)) {

				list = _data [property] as XdtoList;
				if (list == null) {
					list = new XdtoList (this, property);
					_data [property] = list;
				}

			} else {

				list = new XdtoList (this, property);
				_data [property] = list;

			}

			list.Add (dataElement);
		}

		[ContextMethod ("Добавить", "Add")]
		public void Add (string name, IXdtoValue dataElement)
		{
			var property = Properties ().Get (name);
			if (property == null) {
				throw new RuntimeException (String.Format ("Свойство не найдено: {0}", name));
			}

			Add (property, dataElement);
		}

		[ContextMethod("Получить", "Get")]
		public IValue Get (XdtoProperty property)
		{
			if (property != null && _data.ContainsKey (property))
			{
				return _data [property];
			}
			return ValueFactory.Create ();
		}

		[ContextMethod("Получить", "Get")]
		public IValue Get (string xpath)
		{
			// TODO: xpath не только name
			var innerProperty = Properties().Get (xpath);
			return Get (innerProperty);
		}

		[ContextMethod("ПолучитьXDTO", "GetXDTO")]
		public IXdtoValue GetXdto (XdtoProperty property)
		{
			return Get (property) as IXdtoValue;
		}

		[ContextMethod("ПолучитьXDTO", "GetXDTO")]
		public IXdtoValue GetXdto (string xpath)
		{
			return Get (xpath) as IXdtoValue;
		}

		[ContextMethod ("ПолучитьСписок", "GetList")]
		public IValue GetList (XdtoProperty property)
		{
			return Get (property) as XdtoList;
		}

		[ContextMethod ("ПолучитьСписок", "GetList")]
		public XdtoList GetList (string xpath)
		{
			return Get (xpath) as XdtoList;
		}

		[ContextMethod ("Последовательность", "Sequence")]
		public XdtoSequence Sequence ()
		{
			return _sequence;
		}

		[ContextMethod ("Проверить", "Validate")]
		public void Validate ()
		{
			_type?.Validate (this);
		}

		[ContextMethod ("Сбросить", "Unset")]
		public void Unset (XdtoProperty property)
		{
			_data.Remove (property);
		}

		[ContextMethod ("Сбросить", "Unset")]
		public void Unset (string xpath)
		{
			// TODO: xpath не только name
			var innerProperty = Properties().Get (xpath);
			Unset (innerProperty);
		}

		[ContextMethod ("Свойства", "Properties")]
		public XdtoPropertyCollection Properties ()
		{
			return new XdtoPropertyCollection (_data.Keys);
		}

		[ContextMethod ("Тип", "Type")]
		public XdtoObjectType Type ()
		{
			return _type;
		}

		[ContextMethod ("Установить", "Set")]
		public void Set (string xpath, IValue value)
		{
			var customProperty = _type.Properties.Get (xpath);
			_data [customProperty] = value;
		}

		[ContextMethod ("Установить", "Set")]
		public void Set (XdtoProperty property, IValue value)
		{
			_data [property] = value;
		}

		[ContextMethod ("Установлено", "IsSet")]
		public bool IsSet (string xpath)
		{
			// TODO: xpath vs name
			var customProperty = _type.Properties.Get (xpath);
			return _data.ContainsKey (customProperty);
		}

		[ContextMethod ("Установлено", "IsSet")]
		public bool IsSet (XdtoProperty property)
		{
			return _data.ContainsKey (property);
		}

		private static readonly ContextMethodsMapper<XdtoDataObject> _methods = new ContextMethodsMapper<XdtoDataObject> ();

		public override IValue GetPropValue (int propNum)
		{
			// return _values [propNum];
			throw new NotImplementedException ("ОбъектXDTO.GetPropValue");
		}

		public override void SetPropValue (int propNum, IValue newVal)
		{
			// _values [propNum] = newVal;
			throw new NotImplementedException ("ОбъектXDTO.SetPropValue");
		}

		public override MethodInfo GetMethodInfo (int methodNumber)
		{
			return _methods.GetMethodInfo (methodNumber);
		}

		public override void CallAsProcedure (int methodNumber, IValue [] arguments)
		{
			var binding = _methods.GetMethod (methodNumber);
			try {
				binding (this, arguments);
			} catch (System.Reflection.TargetInvocationException e) {
				throw e.InnerException;
			}
		}

		public override void CallAsFunction (int methodNumber, IValue [] arguments, out IValue retValue)
		{
			var binding = _methods.GetMethod (methodNumber);
			try {
				retValue = binding (this, arguments);
			} catch (System.Reflection.TargetInvocationException e) {
				throw e.InnerException;
			}
		}

		public override int FindMethod (string name)
		{
			return _methods.FindMethod (name);
		}

		public override IEnumerable<MethodInfo> GetMethods ()
		{
			for (int i = 0; i < _methods.Count; i++) {
				yield return _methods.GetMethodInfo (i);
			}
		}

		public XmlDataType XmlType ()
		{
			return new XmlDataType (_type.Name, _type.NamespaceUri);
		}
	}
}

