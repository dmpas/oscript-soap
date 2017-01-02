Function Fun1(val X, val Y, Ok) Export

	Ok = True;
	Return X + Y;

EndFunction

AttachAddin("TinyXdto/bin/Debug/TinyXdto.dll");
AttachAddin("Soap/bin/Debug/Soap.dll");

Router = new WSRouter("OneService", "http://oscript.io/example/");

VT = new ValueTable;
// Router.AddHandler(VT);
Router.AddHandler(ЭтотОбъект);

Message(Router.GenerateWsdl());

Proxy = Router.CreateProxy();
Ok = False;

Result = Proxy.Fun1(1, 2, Ok);

Message("Result: " + Result);
Message("Ok: " + Ok);

