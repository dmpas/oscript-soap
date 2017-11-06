﻿Перем юТест;

////////////////////////////////////////////////////////////////////
// Программный интерфейс

Функция ПолучитьСписокТестов(ЮнитТестирование) Экспорт
	
	юТест = ЮнитТестирование;
	
	ВсеТесты = Новый Массив;

	ВсеТесты.Добавить("ТестДолжен_ПроверитьСозданиеФабрикиИНаличиеВстроенныхТипов");
	ВсеТесты.Добавить("ТестДолжен_СхемаСБезымяннымТипом");

	Возврат ВсеТесты;
	
КонецФункции

Процедура ТестДолжен_СхемаСБезымяннымТипом() Экспорт

	Ф = Новый ФабрикаXDTO(ОбъединитьПути(КаталогТестовыхДанных, "Tests/Schema01.xsd"));
	ПространствоИмен = "unnamedComplexType";
	Тип = Ф.Тип(ПространствоИмен, "TheComplexType");
	
	Объект = Ф.Создать(Тип);

КонецПроцедуры

Процедура ТестДолжен_ПроверитьСозданиеФабрикиИНаличиеВстроенныхТипов() Экспорт

	ВстроенноеПространствоИмен = "http://www.w3.org/2001/XMLSchema";

	Фабрика = Новый ФабрикаXDTO();

	ПроверяемыеТипы = Новый Массив;
	ПроверяемыеТипы.Добавить("integer");
	ПроверяемыеТипы.Добавить("decimal");
	ПроверяемыеТипы.Добавить("float");
	ПроверяемыеТипы.Добавить("double");
	ПроверяемыеТипы.Добавить("short");
	ПроверяемыеТипы.Добавить("byte");
	ПроверяемыеТипы.Добавить("int");
	ПроверяемыеТипы.Добавить("long");
	ПроверяемыеТипы.Добавить("unsignedShort");
	ПроверяемыеТипы.Добавить("unsignedByte");
	ПроверяемыеТипы.Добавить("unsignedInt");
	ПроверяемыеТипы.Добавить("unsignedLong");
	ПроверяемыеТипы.Добавить("string");
	ПроверяемыеТипы.Добавить("boolean");
	ПроверяемыеТипы.Добавить("date");
	ПроверяемыеТипы.Добавить("dateTime");
	ПроверяемыеТипы.Добавить("time");

	Для Каждого мИмяТипа Из ПроверяемыеТипы Цикл
		юТест.ПроверитьНеРавенство(Фабрика.Тип(ВстроенноеПространствоИмен, мИмяТипа), Неопределено, "Тип " + мИмяТипа);
	КонецЦикла;

	юТест.ПроверитьРавенство(Фабрика.Тип(ВстроенноеПространствоИмен, "неверный тип"), Неопределено);

КонецПроцедуры
