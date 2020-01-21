/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace TinyXdto.W3Org.XmlSchema
{
	public static class W3OrgXmlSchemaPackage
	{
		static readonly IXdtoReader stringReader = new StringReader ();
		static readonly IXdtoReader numericReader = new NumericReader ();
		static readonly IXdtoReader booleanReader = new BooleanReader ();
		static readonly IXdtoReader dateTimeReader = new DateTimeReader ();

		private static XdtoPackage _w3package;

		public static XdtoPackage Create ()
		{
			if (_w3package != null)
			{
				return _w3package;
			}

			var types = new List<IXdtoType> ();

			types.Add (new XdtoValueType (new XmlDataType ("anySimpleType"), stringReader));
			types.Add (new XdtoValueType (new XmlDataType ("anyURI"),        stringReader));
			types.Add (new XdtoValueType (new XmlDataType ("duration"),      stringReader));
			types.Add (new XdtoValueType (new XmlDataType ("gDay"),          stringReader));
			types.Add (new XdtoValueType (new XmlDataType ("gMonth"),        stringReader));
			types.Add (new XdtoValueType (new XmlDataType ("gYear"),         stringReader));
			types.Add (new XdtoValueType (new XmlDataType ("gYearMonth"),    stringReader));
			types.Add (new XdtoValueType (new XmlDataType ("NOTATION"),      stringReader));
			types.Add (new XdtoValueType (new XmlDataType ("string"),        stringReader));

			types.Add (new XdtoValueType (new XmlDataType ("decimal"),       numericReader));
			types.Add (new XdtoValueType (new XmlDataType ("float"),         numericReader));
			types.Add (new XdtoValueType (new XmlDataType ("double"),        numericReader));
			types.Add (new XdtoValueType (new XmlDataType ("integer"),       numericReader));
			types.Add (new XdtoValueType (new XmlDataType ("long"),          numericReader));
			types.Add (new XdtoValueType (new XmlDataType ("int"),           numericReader));
			types.Add (new XdtoValueType (new XmlDataType ("byte"),          numericReader));
			types.Add (new XdtoValueType (new XmlDataType ("short"),         numericReader));

			types.Add (new XdtoValueType (new XmlDataType ("unsignedLong"),  numericReader));
			types.Add (new XdtoValueType (new XmlDataType ("unsignedInt"),   numericReader));
			types.Add (new XdtoValueType (new XmlDataType ("unsignedByte"),  numericReader));
			types.Add (new XdtoValueType (new XmlDataType ("unsignedShort"), numericReader));

			types.Add (new XdtoValueType (new XmlDataType ("boolean"),       booleanReader));

			types.Add (new XdtoValueType (new XmlDataType ("date"),          dateTimeReader));
			types.Add (new XdtoValueType (new XmlDataType ("dateTime"),      dateTimeReader));
			types.Add (new XdtoValueType (new XmlDataType ("time"),          dateTimeReader));

			_w3package = new XdtoPackage (XmlNs.xs, types);
			return _w3package;
		}
	}
}
