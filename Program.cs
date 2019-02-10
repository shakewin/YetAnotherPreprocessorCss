using System;
using System.IO;
using YetAnotherPreprocessorCss.LexicalAnalysis;

namespace YetAnotherPreprocessorCss
{
	class Program
	{
		static void Main(string[] args)
		{
			string path = args[0];
			path += ".yapc";
			Lexer lex = new Lexer(path);
			FileInfo fi = new FileInfo(path);
			path = $"{fi.DirectoryName}\\{fi.Name.Replace(".yapc", ".css")}";
			StreamWriter sw = File.CreateText(path);
			bool openSelector = false;
			while(true)
			{
				IToken token = lex.Scan();
				if (token is EOF) break;
				if (token is Selector)
				{
					if (openSelector)
					{
						sw.WriteLine("}");
						sw.WriteLine($"{(token as Selector).Name} {{");
						openSelector = true;
					}
					else
					{
						sw.WriteLine($"{(token as Selector).Name} {{");
						openSelector = true;
					}
				}
				if (token is EndSelector) openSelector = false;
				if (token is Property) sw.WriteLine($"\t{(token as Property).Name} : {(token as Property).Value};");
			}
			sw.WriteLine("}");
			sw.Close();
			Console.WriteLine("Completed");
		}
	}
}
