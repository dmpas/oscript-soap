using System;
using ScriptEngine.Machine;
namespace TinyXdto
{
	public interface IXdtoDeserializer
	{
		/// <summary>
		/// Преобразует значение XDTO в объект системы.
		/// </summary>
		/// <returns>Значение.</returns>
		/// <param name="xdtoValue">Значение XDTO.</param>
		IValue DeserializeXdto (IXdtoValue xdtoValue);
	}
}
