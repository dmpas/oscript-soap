using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;

namespace TinyXdto
{
	[ContextClass("ОбъектXDTO", "XDTODataObject")]
	public class XdtoDataObjectImpl : DynamicPropertiesAccessor
	{
		private readonly XdtoDataObjectImpl _owner;
		private readonly XdtoPropertyImpl _owningProperty;
		private readonly Dictionary<XdtoPropertyImpl, IValue> _data = new Dictionary<XdtoPropertyImpl, IValue> ();
		private readonly XdtoObjectTypeImpl _type;

		internal XdtoDataObjectImpl (XdtoObjectTypeImpl type, XdtoDataObjectImpl owner, XdtoPropertyImpl property)
		{
			_type = type;
			_owner = owner;
			_owningProperty = property;
		}

		[ContextMethod ("Владелец", "Owner")]
		public XdtoDataObjectImpl Owner ()
		{
			return _owner;
		}

		[ContextMethod ("ВладеющееСвойство", "OwningProperty")]
		public XdtoPropertyImpl OwningProperty ()
		{
			return _owningProperty;
		}


		[ContextMethod("Добавить", "Add")]
		public void Add (XmlFormEnum form, string namespaceUri, string localName, IXdtoValue dataElement)
		{
			// TODO: Извлекать свойство из Фабрики, потому что оно должно иметь Тип
			var customProperty = new XdtoPropertyImpl (this, form, namespaceUri, localName);

			XdtoListImpl list;
			if (_data.ContainsKey (customProperty)) {
				
				list = _data [customProperty] as XdtoListImpl;
				if (list == null) {
					
					// TODO: Копипастъ
					list = new XdtoListImpl ();
					_data [customProperty] = list;
				}

			} else {
				
				list = new XdtoListImpl ();
				_data [customProperty] = list;

			}

			list.Add (dataElement);
		}

		[ContextMethod ("Добавить", "Add")]
		public void Add (string name, IXdtoValue dataElement)
		{
			var innerProperty = _type.Properties.Get (name);

			XdtoListImpl list;
			if (_data.ContainsKey (innerProperty)) {
				
				list = _data [innerProperty] as XdtoListImpl;
				if (list == null) {
					// TODO: Внятное сообщение об ошибке
					throw new RuntimeException ("Xdto List error");
				}
				
			} else {
				
				list = new XdtoListImpl ();
				_data [innerProperty] = list;

			}
			
			list.Add (dataElement);
		}

		[ContextMethod("Получить", "Get")]
		public IValue Get (XdtoPropertyImpl property)
		{
			if (_data.ContainsKey (property))
			{
				return _data [property];
			}
			return ValueFactory.Create ();
		}

		[ContextMethod("Получить", "Get")]
		public IValue Get (string xpath)
		{
			// TODO: xpath не только name
			var innerProperty = _type.Properties.Get (xpath);
			return Get (innerProperty);
		}

		[ContextMethod("ПолучитьXDTO", "GetXDTO")]
		public IXdtoValue GetXdto (XdtoPropertyImpl property)
		{
			return Get (property) as IXdtoValue;
		}

		[ContextMethod("ПолучитьXDTO", "GetXDTO")]
		public IXdtoValue GetXdto (string xpath)
		{
			return Get (xpath) as IXdtoValue;
		}

		[ContextMethod ("ПолучитьСписок", "GetList")]
		public IValue GetList (XdtoPropertyImpl property)
		{
			return Get (property) as XdtoListImpl;
		}

		[ContextMethod ("ПолучитьСписок", "GetList")]
		public XdtoListImpl GetList (string xpath)
		{
			return Get (xpath) as XdtoListImpl;
		}

		[ContextMethod ("Последовательность", "Sequence")]
		public XdtoSequenceImpl Sequence ()
		{
			throw new NotImplementedException ();
		}

		[ContextMethod ("Проверить", "Validate")]
		public void Validate ()
		{
			throw new NotImplementedException ();
		}

		[ContextMethod ("Сбросить", "Unset")]
		public void Unset (XdtoPropertyImpl property)
		{
			_data.Remove (property);
		}

		[ContextMethod ("Сбросить", "Unset")]
		public void Unset (string xpath)
		{
			// TODO: xpath не только name
			var innerProperty = _type.Properties.Get (xpath);
			Unset (innerProperty);
		}

		[ContextMethod ("Свойства", "Properties")]
		public XdtoPropertyCollectionImpl Properties ()
		{
			return new XdtoPropertyCollectionImpl (_data.Keys);
		}

		[ContextMethod ("Тип", "Type")]
		public XdtoObjectTypeImpl Type ()
		{
			return _type;
		}

		// TODO: уточнить правильное объявление метода
		[ContextMethod ("Установить", "Set")]
		public void Set (string xpath, IValue value)
		{
			var customProperty = _type.Properties.Get (xpath);
			_data [customProperty] = value;
		}

		[ContextMethod ("Установить", "Set")]
		public void Set (XdtoPropertyImpl property, IValue value)
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
		public bool IsSet (XdtoPropertyImpl property)
		{
			throw new NotImplementedException ();
		}

		private static readonly ContextMethodsMapper<XdtoDataObjectImpl> _methods = new ContextMethodsMapper<XdtoDataObjectImpl> ();

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
	}
}

