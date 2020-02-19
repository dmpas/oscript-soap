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
		private readonly PrimitiveValuesSerializer _pv = new PrimitiveValuesSerializer();
		
		internal XdtoDataObject (XdtoObjectType type, XdtoDataObject owner, XdtoProperty property)
		{
			_type = type;
			_owner = owner;
			_owningProperty = property;
			_sequence = new XdtoSequence (this, true);
			FillPropertiesFromType();
		}

		private void FillPropertiesFromType()
		{
			if (_type == null)
			{
				return;
			}
			
			var baseType = _type.BaseType;
			while (baseType != null)
			{
				foreach (var typeProperty in baseType.Properties)
				{
					Set(typeProperty, typeProperty.UpperBound == -1 ? new XdtoList(this, typeProperty) : null);
				}
				baseType = baseType.BaseType;
			}

			foreach (var typeProperty in _type.Properties)
			{
				Set(typeProperty, typeProperty.UpperBound == -1 ? new XdtoList(this, typeProperty) : null);
			}
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
				var customProperty = new XdtoProperty (null, this, form, namespaceUri, localName);
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

			list.Add (ValueFactory.Create(dataElement));
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

		public IValue Get (XdtoProperty property)
		{
			if (property != null && _data.ContainsKey (property))
			{
				return _data [property];
			}
			return ValueFactory.Create ();
		}

		public IValue Get (string xpath)
		{
			// TODO: xpath не только name
			var innerProperty = Properties().Get (xpath);
			return Get (innerProperty);
		}
		
		[ContextMethod("Получить", "Get")]
		public IValue Get_External(IValue xpath)
		{
			var rawPath = xpath?.GetRawValue();
			if (rawPath is XdtoProperty)
			{
				return Get(rawPath as XdtoProperty);
			}

			return Get(rawPath.AsString());
		}

		[ContextMethod("ПолучитьXDTO", "GetXDTO")]
		public IXdtoValue GetXdto(XdtoProperty property)
		{
			var value = Get(property);
			if (value is IXdtoValue)
			{
				return value as IXdtoValue;
			}

			if (property.Type is XdtoValueType)
			{
				return new XdtoDataValue(property.Type as XdtoValueType, value?.AsString() ?? "", value);
			}

			return _pv.SerializeXdto(value, null);
		}

		[ContextMethod("ПолучитьXDTO", "GetXDTO")]
		public IXdtoValue GetXdto (string xpath)
		{
			return Get (xpath) as IXdtoValue;
		}

		[ContextMethod ("ПолучитьСписок", "GetList")]
		public IValue GetList (IValue property)
		{
			if (property is XdtoProperty)
			{
				return GetList(property as XdtoProperty);
			}

			return GetList(property.AsString());
		}
		
		public IValue GetList (XdtoProperty property)
		{
			var v = Get(property) as XdtoList;
			if (v == null)
			{
				v = new XdtoList(this, property);
				_data[property] = v;
			}
			return v;
		}

		public IValue GetList (string xpath)
		{
			var property = Properties().Get(xpath);
			return GetList (property);
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
			RemoveProperty(property.LocalName);
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
			// return new XdtoPropertyCollection(_typeProperties);
		}

		[ContextMethod ("Тип", "Type")]
		public XdtoObjectType Type ()
		{
			return _type;
		}

		public void Set (string xpath, IValue value)
		{
			var customProperty = Properties().Get(xpath);
			Set(customProperty, value);
		}

		public void Set (XdtoProperty property, IValue value)
		{
			RegisterProperty(property.LocalName);
			_data [property] = value;
		}

		[ContextMethod ("Установить", "Set")]
		public void Set (IValue property, IValue value)
		{
			var rawProperty = property?.GetRawValue();
			if (rawProperty is XdtoProperty)
			{
				Set(rawProperty as XdtoProperty, value);
			}
			else
			{
				Set(rawProperty.AsString(), value);
			}
		}

		[ContextMethod ("Установлено", "IsSet")]
		public bool IsSet (string xpath)
		{
			// TODO: xpath vs name
			var customProperty = Properties().Get (xpath);
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
			var propertyName = GetPropertyName(propNum);
			return Get(propertyName);
		}

		public override void SetPropValue (int propNum, IValue newVal)
		{
			var propertyName = GetPropertyName(propNum);
			Set(propertyName, newVal);
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

		public XmlDataType XmlType ()
		{
			return new XmlDataType (_type.Name, _type.NamespaceUri);
		}
	}
}

