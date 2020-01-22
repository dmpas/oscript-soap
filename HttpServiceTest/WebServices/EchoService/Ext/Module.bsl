
Функция EchoNumber(param)
	
	Возврат param;
	
КонецФункции

Функция EchoFloat(param)
	
	Возврат param;
	
КонецФункции

Функция EchoString(param)
	
	Возврат param;
	
КонецФункции

Функция EchoDateTime(param)
	
	Возврат param;
	
КонецФункции

Функция EchoBool(param)
	
	Возврат param;
	
КонецФункции

Функция WhoAmI()
	
	Возврат ИмяПользователя();
	
КонецФункции

Функция EchoObject(param)
	
	param.ПолучитьСписок("ListedProperty").Добавить("Text1");
	param.ПолучитьСписок("ListedProperty").Добавить("Text2");
	
	ListEl = ФабрикаXDTO.Создать(ФабрикаXDTO.Тип("http://example.com/soap-mixed", "List"));
	ListEl.Установить("name", "123");
	param.ПолучитьСписок("ListFromOtherPackage").Добавить(ListEl);
	
	ListEl = ФабрикаXDTO.Создать(ФабрикаXDTO.Тип("http://example.com/soap-mixed", "List"));
	ListEl.Установить("name", "456");
	param.ПолучитьСписок("ListFromOtherPackage").Добавить(ListEl);
	
	return param;
	
КонецФункции
