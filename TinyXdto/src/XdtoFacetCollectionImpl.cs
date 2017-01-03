using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace TinyXdto
{
	[ContextClass("КоллекцияФасетовXDTO", "XDTOFacetCollection")]
	public class XdtoFacetCollectionImpl : FixedCollectionOf<XdtoFacetImpl>
	{

		internal XdtoFacetCollectionImpl (IEnumerable<XdtoFacetImpl> data) : base (data)
		{

		}

		[ContextProperty ("Образцы")]
		public XdtoFacetCollectionImpl Patterns
		{
			get 
			{
				return new XdtoFacetCollectionImpl (this.Where ((f) => f.Type == XdtoFacetTypeEnum.Pattern));
			}
		}

		[ContextProperty ("Перечисления")]
		public XdtoFacetCollectionImpl Enumerations
		{
			get
			{
				return new XdtoFacetCollectionImpl (this.Where ((f) => f.Type == XdtoFacetTypeEnum.Enumeration));
			}
		}
	}
}
