using System;
namespace TinyXdto
{
	sealed class XdtoSequenceValueElement : IXdtoSequenceElement
	{
		public XdtoSequenceValueElement (XdtoPropertyImpl property, IXdtoValue value)
		{
			Value = value;
		}

		public IXdtoValue Value { get; }
		public XdtoPropertyImpl Property { get; }

	}
}
