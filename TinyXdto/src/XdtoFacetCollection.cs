/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace TinyXdto
{
	[ContextClass("КоллекцияФасетовXDTO", "XDTOFacetCollection")]
	public class XdtoFacetCollection : FixedCollectionOf<XdtoFacet>
	{

		internal XdtoFacetCollection (IEnumerable<XdtoFacet> data) : base (data)
		{

		}

		[ContextProperty ("Образцы")]
		public XdtoFacetCollection Patterns
		{
			get 
			{
				return new XdtoFacetCollection (this.Where ((f) => f.Type == XdtoFacetTypeEnum.Pattern));
			}
		}

		[ContextProperty ("Перечисления")]
		public XdtoFacetCollection Enumerations
		{
			get
			{
				return new XdtoFacetCollection (this.Where ((f) => f.Type == XdtoFacetTypeEnum.Enumeration));
			}
		}
	}
}
