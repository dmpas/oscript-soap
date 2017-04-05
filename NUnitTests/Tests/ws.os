﻿Перем юТест;
Перем Прокси;

////////////////////////////////////////////////////////////////////
// Программный интерфейс

Функция ПолучитьСписокТестов(ЮнитТестирование) Экспорт
	
	юТест = ЮнитТестирование;
	
	ВсеТесты = Новый Массив;

	ВсеТесты.Добавить("ТестДолжен_ПроверитьЧтоСоздается");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьПередачуЧисел");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьПередачуСтрок");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьПередачуДат");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьПередачуБулева");

	Возврат ВсеТесты;
	
КонецФункции

Процедура ТестДолжен_ПроверитьЧтоСоздается() Экспорт

	Определения = Новый WSОпределения("http://vm21297.hv8.ru:10080/httpservice/ws/echo.1cws?wsdl");
	Прокси = Новый WSПрокси(Определения, "http://dmpas/echo", "EchoService", "EchoServiceSoap");

КонецПроцедуры

Процедура ТестДолжен_ПроверитьПередачуЧисел() Экспорт

	Определения = Новый WSОпределения("http://vm21297.hv8.ru:10080/httpservice/ws/echo.1cws?wsdl");
	Прокси = Новый WSПрокси(Определения, "http://dmpas/echo", "EchoService", "EchoServiceSoap");

	юТест.ПроверитьРавенство(Прокси.EchoNumber(1), 1, "Прокси.EchoNumber(1)");
	юТест.ПроверитьРавенство(Прокси.EchoNumber(1.2), 1.2, "Прокси.EchoNumber(1.2)");

	юТест.ПроверитьРавенство(Прокси.EchoNumber(-1), -1, "Прокси.EchoNumber(-1)");
	юТест.ПроверитьРавенство(Прокси.EchoNumber(-1.2), -1.2, "Прокси.EchoNumber(-1.2)");

	юТест.ПроверитьРавенство(Прокси.EchoFloat(1), 1, "Прокси.EchoFloat(1)");
	юТест.ПроверитьРавенство(Прокси.EchoFloat(1.2), 1.2, "Прокси.EchoFloat(1.2)");

	юТест.ПроверитьРавенство(Прокси.EchoFloat(-1), -1, "Прокси.EchoFloat(-1)");
	юТест.ПроверитьРавенство(Прокси.EchoFloat(-1.2), -1.2, "Прокси.EchoFloat(-1.2)");

КонецПроцедуры

Процедура ТестДолжен_ПроверитьПередачуСтрок() Экспорт

	юТест.ПроверитьРавенство(Прокси.EchoString("123"), "123", "Прокси.EchoString(""123"")");
	юТест.ПроверитьРавенство(Прокси.EchoString("123
	|456"), "123
	|456", "Прокси.EchoString(""123--456"")");
	юТест.ПроверитьРавенство(Прокси.EchoString("<&>"), "<&>", "Прокси.EchoString(""<&>"")");

КонецПроцедуры

Процедура ТестДолжен_ПроверитьПередачуДат() Экспорт
	
	юТест.ПроверитьРавенство(Прокси.EchoDateTime('20170405125958'), '20170405125958', "Прокси.EchoDateTime('20170405125958')");

КонецПроцедуры

Процедура ТестДолжен_ПроверитьПередачуБулева() Экспорт

	юТест.ПроверитьРавенство(Прокси.EchoBool(Истина), Истина, "Прокси.EchoBool(Истина)");
	юТест.ПроверитьРавенство(Прокси.EchoBool(Ложь), Ложь, "Прокси.EchoBool(Ложь)");

КонецПроцедуры
