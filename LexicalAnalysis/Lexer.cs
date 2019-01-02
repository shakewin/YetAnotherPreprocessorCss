using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherPreprocessorCss.LexicalAnalysis
{
	class Lexer
	{
		private char peek = ' ';
		private Dictionary<string, string> vars;
		private Stack<string> selectors;
		private int line = 1;
		private StringBuilder strBuffer;

		public Lexer()
		{
			vars = new Dictionary<string, string>();
			selectors = new Stack<string>();
			strBuffer = new StringBuilder();
		}

		void ReadChr()
		{
			peek = Convert.ToChar(Console.Read());
			if (peek == ' ' || peek == '\t' || peek == '\r') ReadChr();
		}
		void AddVar(string nameVar, string valueVar) => vars.Add(nameVar, valueVar);

		public IToken Scan()
		{
			ReadChr();
			for (; ; ReadChr())
			{
				if (peek == '\n') line++;
				else break;
			}

			if (peek == '$')
			{
				ReadChr();

				strBuffer.Clear();
				do
				{
					strBuffer.Append(peek);
					ReadChr();
				} while (Char.IsLetterOrDigit(peek));

				string nameVar = strBuffer.ToString();
				
				if (peek == ':')
				{
					ReadChr();

					strBuffer.Clear();
					do
					{
						strBuffer.Append(peek);
						ReadChr();
					} while (peek != ';');
					
					string valueVar = strBuffer.ToString();

					AddVar(nameVar, valueVar);
					ReadChr();
				}
				else throw new Exception($"Syntax error on {line} line: the variable {nameVar} does not have a value");
			}

			if (peek == '}')
			{
				if (selectors.Count != 0)
					selectors.Pop();
				return new EndSelector();
			}
			else if (peek == '.' || peek == '#')
			{
				strBuffer.Clear();
				do
				{
					strBuffer.Append(peek);
					ReadChr();
				} while (peek != '{');
				
				if (selectors.Count != 0)
				{
					string prevSelector = selectors.Peek();
					selectors.Push($"{prevSelector} {strBuffer.ToString()}");
				}
				else selectors.Push($"{strBuffer.ToString()}");
				
				return new Selector(selectors.Peek());
			}
			else
			{
				strBuffer.Clear();
				do
				{
					strBuffer.Append(peek);
					ReadChr();
				} while (peek != ':');

				string nameProp = strBuffer.ToString();

				ReadChr();

				string valueProp;

				if (peek == '$')
				{
					ReadChr();

					strBuffer.Clear();
					do
					{
						strBuffer.Append(peek);
						ReadChr();
					} while (peek != ';');

					string key = strBuffer.ToString();

					if (vars.ContainsKey(key))
						valueProp = vars[key];
					else throw new Exception($"Syntax error on {line} line: the variable {key} does not exist");

				}
				else
				{
					strBuffer.Clear();
					do
					{
						strBuffer.Append(peek);
						ReadChr();
					} while (peek != ';');

					valueProp = strBuffer.ToString();
				}
				
				return new Property(nameProp, valueProp);
			}
		}
	}
}
