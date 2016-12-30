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
		internal XdtoFacetCollectionImpl (IEnumerable<XdtoFacetImpl> data, bool dontInitProperties = false) : base (data)
		{
			if (!dontInitProperties) {

				Patterns = new XdtoFacetCollectionImpl (data.Where ((f) => f.Type == XdtoFacetTypeEnum.Pattern), true);
				Enumerations = new XdtoFacetCollectionImpl (data.Where ((f) => f.Type == XdtoFacetTypeEnum.Enumeration), true);

			} else {
				
				Patterns = new XdtoFacetCollectionImpl (new XdtoFacetImpl [0]);
				Enumerations = new XdtoFacetCollectionImpl (new XdtoFacetImpl [0]);

			}
		}

		[ContextProperty("Образцы")]
		public XdtoFacetCollectionImpl Patterns { get; }

		[ContextProperty ("Перечисления")]
		public XdtoFacetCollectionImpl Enumerations { get; }
	}
}
