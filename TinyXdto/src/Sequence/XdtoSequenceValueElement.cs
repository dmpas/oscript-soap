/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
namespace TinyXdto
{
	sealed class XdtoSequenceValueElement : IXdtoSequenceElement
	{
		public XdtoSequenceValueElement (XdtoProperty property, IXdtoValue value)
		{
			Value = value;
		}

		public IXdtoValue Value { get; }
		public XdtoProperty Property { get; }

	}
}
