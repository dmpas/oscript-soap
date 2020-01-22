
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
	return param;
КонецФункции
