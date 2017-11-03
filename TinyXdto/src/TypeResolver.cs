using System;
using System.Xml;
using System.Collections.Generic;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

namespace TinyXdto
{
	sealed class TypeResolver : IXdtoType
	{
		private readonly XdtoFactoryImpl _factory;
		private readonly XmlQualifiedName _xmlType;

		public TypeResolver (XdtoFactoryImpl factory, XmlQualifiedName xmlType)
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

		public IEnumerable<MethodInfo> GetMethods ()
		{
			throw new NotSupportedException ();
		}

		public int FindMethod (string name)
		{
			throw new NotSupportedException ();
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

		public bool IsIndexed {
			get {
				throw new NotSupportedException ();
			}
		}

		public bool DynamicMethodSignatures {
			get {
				throw new NotSupportedException ();
			}
		}

		#endregion
	}
}
