using ScriptEngine.Machine.Contexts;

namespace testsoap
{
	public class RouterTest : AutoContext<RouterTest>
	{
		[ContextMethod("Sum")]
		public int Sum(int x, int y)
		{
			return x + y;
		}
	}
}