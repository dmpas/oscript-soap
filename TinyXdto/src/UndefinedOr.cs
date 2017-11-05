/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine;

namespace TinyXdto
{
	public class UndefinedOr<OneType> : IValue where OneType : class, IValue
	{
		private readonly OneType _value = null;

		public UndefinedOr (OneType value)
		{
			if (value != null)
				_value = value;
		}

		public bool IsUndefined ()
		{
			return _value == null;
		}

		public OneType Value
		{
			get {
				if (IsUndefined ())
					throw new RuntimeException ("Wrong value!");
				return _value;
			}
		}

		public DataType DataType {
			get {
				return GetRawValue ().DataType;
			}
		}

		public TypeDescriptor SystemType {
			get {
				return GetRawValue ().SystemType;
			}
		}

		public bool AsBoolean ()
		{
			return GetRawValue ().AsBoolean ();
		}

		public DateTime AsDate ()
		{
			return GetRawValue ().AsDate ();
		}

		public decimal AsNumber ()
		{
			return GetRawValue ().AsNumber ();
		}

		public IRuntimeContextInstance AsObject ()
		{
			return GetRawValue ().AsObject ();
		}

		public string AsString ()
		{
			return GetRawValue ().AsString ();
		}

		public int CompareTo (IValue other)
		{
			return GetRawValue ().CompareTo (other);
		}

		public bool Equals (IValue other)
		{
			return GetRawValue().Equals (other);
		}

		public IValue GetRawValue ()
		{
			if (IsUndefined())
				return ValueFactory.Create ();
			return _value.GetRawValue ();
		}
	}
}
