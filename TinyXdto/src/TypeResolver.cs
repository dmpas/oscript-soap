/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using System.Xml;
using System.Collections.Generic;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

namespace TinyXdto
{
	// TODO: убить это недоразумение
	sealed class TypeResolver : IXdtoType
	{
		private readonly XdtoFactory _factory;
		private readonly XmlQualifiedName _xmlType;

		public TypeResolver (XdtoFactory factory, XmlQualifiedName xmlType)
		{
			_factory = factory;
			_xmlType = xmlType;
		}

		public IXdtoType Resolve ()
		{
			return _factory.Type (_xmlType);
		}

		public string Name {
			get {
				return Resolve ()?.Name;
			}
		}

		public string NamespaceUri {
			get {
				return Resolve ()?.NamespaceUri;
			}
		}

		public IXdtoReader Reader {
			get {
				return Resolve ()?.Reader;
			}
		}


		#region NotSupportedException

		public IValue GetIndexedValue (IValue index)
		{
			throw new NotSupportedException ();
		}

		public void SetIndexedValue (IValue index, IValue val)
		{
			throw new NotSupportedException ();
		}

		public int FindProperty (string name)
		{
			throw new NotSupportedException ();
		}

		public bool IsPropReadable (int propNum)
		{
			throw new NotSupportedException ();
		}

		public bool IsPropWritable (int propNum)
		{
			throw new NotSupportedException ();
		}

		public IValue GetPropValue (int propNum)
		{
			throw new NotSupportedException ();
		}

		public void SetPropValue (int propNum, IValue newVal)
		{
			throw new NotSupportedException ();
		}

		public int GetPropCount()
		{
			return 0;
		}

		public string GetPropName(int propNum)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<MethodInfo> GetMethods ()
		{
			throw new NotSupportedException ();
		}

		public int FindMethod (string name)
		{
			throw new NotSupportedException ();
		}

		public int GetMethodsCount()
		{
			return 0;
		}

		public MethodInfo GetMethodInfo (int methodNumber)
		{
			throw new NotSupportedException ();
		}

		public void CallAsProcedure (int methodNumber, IValue [] arguments)
		{
			throw new NotSupportedException ();
		}

		public void CallAsFunction (int methodNumber, IValue [] arguments, out IValue retValue)
		{
			throw new NotSupportedException ();
		}

		public bool IsIndexed => false;

		public bool DynamicMethodSignatures { get; } = false;

		#endregion
	}
}
