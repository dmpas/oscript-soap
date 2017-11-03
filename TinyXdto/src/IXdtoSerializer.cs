using System;
using ScriptEngine.Machine;

namespace TinyXdto
{
	public interface IXdtoSerializer
	{
		/// <summary>
		/// Создаёт значение XDTO или объект XDTO по значению или объекту скрипта.
		/// </summary>
		/// <returns>ЗначениеXDTO,ОбъектXDTO</returns>
		/// <param name="value">Значение.</param>
		/// <param name="requestedType">Запрашиваемый тип XDTO. Может не указываться, возвращаемое значение может иметь отличный тип XDTO.</param>
		IXdtoValue SerializeXdto (IValue value, IXdtoType requestedType);

		/// <summary>
		/// Определяет тип, в который будет сконвертировано значение, если требуемый тип не указан явно.
		/// </summary>
		/// <returns>ТипЗначенияXDTO.</returns>
		/// <param name="value">Значение.</param>
		XmlDataType GetPossibleType (IValue value);

	}
}
