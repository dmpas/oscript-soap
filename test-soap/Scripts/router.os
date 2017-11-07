Function Fun1(val X, val Y, Ok) Export

	Ok = True;
	Return X + Y;

EndFunction

Router = new WSRouter("OneService", "http://oscript.io/example/");

Router.AddHandler(ЭтотОбъект);

Proxy = Router.CreateProxy();
Ok = False;

Result = Proxy.Fun1(1, 2, Ok);

Message("Result: " + Result);
Message("Ok: " + Ok);
