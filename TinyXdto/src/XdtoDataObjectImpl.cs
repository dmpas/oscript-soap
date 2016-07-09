using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[ContextClass("ОбъектXDTO", "XDTODataObject")]
	public class XdtoDataObjectImpl : AutoContext<XdtoDataObjectImpl>
	{
		internal XdtoDataObjectImpl ()
		{
		}

		[ContextMethod ("Владелец", "Owner")]
		public XdtoDataObjectImpl Owner ()
		{
			throw new NotImplementedException ();
		}

		[ContextMethod ("ВладеющееСвойство", "OwningProperty")]
		public XdtoPropertyImpl OwningProperty ()
		{
			throw new NotImplementedException ();
		}


		[ContextMethod("Добавить", "Add")]
		public void Add (XmlFormEnum form, string namespaceUri, string localName, IXdtoValue dataElement)
		{
			throw new NotImplementedException ("XDTODataObject.Add()");
		}

		[ContextMethod ("Добавить", "Add")]
		public void Add (string name, IXdtoValue dataElement)
		{
			throw new NotImplementedException ("XDTODataObject.Add()");
		}

		[ContextMethod("Получить", "Get")]
		public IValue Get (XdtoPropertyImpl property)
		{
			throw new NotImplementedException ("XDTODataObject.Get()");
		}

		[ContextMethod("Получить", "Get")]
		public IValue Get (string xpath)
		{
			throw new NotImplementedException ("XDTODataObject.Get()");
		}

		[ContextMethod("ПолучитьXDTO", "GetXDTO")]
		public IValue GetXdto (XdtoPropertyImpl property)
		{
			throw new NotImplementedException ("XDTODataObject.Get()");
		}

		[ContextMethod("ПолучитьXDTO", "GetXDTO")]
		public IValue GetXdto (string xpath)
		{
			throw new NotImplementedException ("XDTODataObject.Get()");
		}

		[ContextMethod ("ПолучитьСписок", "GetList")]
		public IValue GetList (XdtoPropertyImpl property)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod ("ПолучитьСписок", "GetList")]
		public IValue GetList (string xpath)
		{
			throw new NotImplementedException ();
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
			throw new NotImplementedException ();
		}

		[ContextMethod ("Сбросить", "Unset")]
		public void Unset (string xpath)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod ("Свойства", "Properties")]
		public XdtoPropertyCollectionImpl Properties ()
		{
			throw new NotImplementedException ();
		}

		[ContextMethod ("Тип", "Type")]
		public XdtoObjectTypeImpl Type ()
		{
			throw new NotImplementedException ();
		}

		// TODO: уточнить правильное объявление метода
		[ContextMethod ("Установить", "Set")]
		public void Set (string xpath, IValue value)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod ("Установить", "Set")]
		public void Set (XdtoPropertyImpl property, IValue value)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod ("Установлено", "IsSet")]
		public bool IsSet (string xpath)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod ("Установлено", "IsSet")]
		public bool IsSet (XdtoPropertyImpl property)
		{
			throw new NotImplementedException ();
		}
	}
}

