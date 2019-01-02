
namespace YetAnotherPreprocessorCss.LexicalAnalysis
{
	class Selector : IToken
	{
		public string Name { get; private set; }

		public Selector(string name)
		{
			Name = name;
		}
	}
}
