﻿
namespace YetAnotherPreprocessorCss.LexicalAnalysis
{
	class Property : IToken
	{
		public string Name { get; private set; }
		public string Value { get; private set; }

		public Property(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}
