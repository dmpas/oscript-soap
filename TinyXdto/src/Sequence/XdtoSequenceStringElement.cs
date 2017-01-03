namespace TinyXdto
{
	sealed class XdtoSequenceStringElement : IXdtoSequenceElement
	{
		public XdtoSequenceStringElement (string text)
		{
			Text = text;
		}

		public string Text { get; }
	}
}
