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

		public static XdtoPackageImpl Create ()
		{
			var types = new List<IXdtoType> ();

			types.Add (new XdtoValueTypeImpl (new XmlDataType ("anySimpleType"), stringReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("anyURI"),        stringReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("duration"),      stringReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("gDay"),          stringReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("gMonth"),        stringReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("gYear"),         stringReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("gYearMonth"),    stringReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("NOTATION"),      stringReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("string"),        stringReader));

			types.Add (new XdtoValueTypeImpl (new XmlDataType ("decimal"),       numericReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("float"),         numericReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("double"),        numericReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("integer"),       numericReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("long"),          numericReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("int"),           numericReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("byte"),          numericReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("short"),         numericReader));

			types.Add (new XdtoValueTypeImpl (new XmlDataType ("unsignedLong"),  numericReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("unsignedInt"),   numericReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("unsignedByte"),  numericReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("unsignedShort"), numericReader));

			types.Add (new XdtoValueTypeImpl (new XmlDataType ("boolean"),       booleanReader));

			types.Add (new XdtoValueTypeImpl (new XmlDataType ("date"),          dateTimeReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("dateTime"),      dateTimeReader));
			types.Add (new XdtoValueTypeImpl (new XmlDataType ("time"),          dateTimeReader));

			return new XdtoPackageImpl (XmlNs.xs, types);
		}
	}
}
