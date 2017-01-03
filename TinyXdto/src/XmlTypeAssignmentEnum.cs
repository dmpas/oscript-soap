using ScriptEngine;

namespace TinyXdto
{
	[EnumerationType("НазначениеТипаXML", "XMLTypeAssignment")]
	public enum XmlTypeAssignmentEnum
	{

		[EnumItem ("Неявное", "Implicit")]
		Implicit,

		[EnumItem ("Явное", "Explicit")]
		Explicit

	}
}
