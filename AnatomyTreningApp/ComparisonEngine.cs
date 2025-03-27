using IIT.Search;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IIT.Compare
{
	public static class ComparisonEngine
	{
		public static bool CompareNames(string mainName, string nameToCompare, bool harderComparison)
		{
			GeneralPhraseTransformer transformer = new();
			string unifiedName = transformer.TransformPhrase(nameToCompare).ToLower();
			string unifiedMainName = transformer.TransformPhrase(mainName).ToLower();

			return CompareWords(unifiedMainName, unifiedName, harderComparison);
		}
		private static bool CompareWords(string mainName, string checkedName, bool harderComparison)
		{
			List<string> subTextList = ConstructSubText(checkedName);
			List<string> subMainTextList = ConstructSubText(mainName);

			bool wordsAreEqual = false;
			bool isEqual;

			int countDifference = subMainTextList.Count - subTextList.Count;
			if (countDifference != 0) return false;

			foreach (string subTestText in subTextList)
			{
				isEqual = false;

				foreach (string subMainTestText in subMainTextList)
				{
					string changedWord = ChangeWord(subTestText, harderComparison);

					if (subMainTestText.Equals(changedWord))
					{
						isEqual = true;
					}
					else if (!harderComparison && Math.Abs(subMainTestText.ToCharArray().Count() - changedWord.ToCharArray().Count()) <= 1)
					{
						isEqual = CheckChars(subMainTestText, changedWord, differenceValue: 2);
					}
					if (isEqual) break;
				}
				wordsAreEqual = isEqual;
				if (!wordsAreEqual) break;
			}
			return wordsAreEqual;
		}
		private static bool CheckChars(string mainWord, string wordsToCheck, int differenceValue)
		{
			int differentCharsCount;
			int differentChars = 0;
			bool differenceIsSmall = true;

			List<char> charsToCheck = wordsToCheck.ToList();
			List<char> mainWordChars = mainWord.ToList();

			differentCharsCount = Math.Abs(mainWordChars.Count - charsToCheck.Count);

			foreach (char charToCheck in charsToCheck)
			{
				if (!mainWordChars.Contains(charToCheck))
				{
					differentCharsCount++;
					differentChars++;
				}
			}
			differentCharsCount += differentChars;
			if (differentCharsCount > differenceValue | differentChars > 1) differenceIsSmall = false;

			return differenceIsSmall;
		}
		private static string CheckWordToChange(List<string> rightExceptions, List<string> leftExceptions, string wordToChange)
		{
			wordToChange = ForeachExceptionsList(rightExceptions, wordToChange);
			wordToChange = ForeachExceptionsList(leftExceptions, wordToChange);
			return wordToChange;
		}

		private static string ForeachExceptionsList(List<string> exceptions, string wordToChange)
		{
			List<char> charWord = wordToChange.ToList();

			foreach (string exception in exceptions)
			{
				List<char> charException = exception.ToList();
				int differentChars = Math.Abs(charException.Count - charWord.Count);

				if (!(differentChars <= 1)) continue;

				if (CheckChars(exception, wordToChange, differenceValue: 1))
				{
					wordToChange = exception;
					break;
				}
			}
			return wordToChange;
		}
		private static string ChangeWord(string wordToChange, bool harderComparison)
		{
			List<string> rightEnglishExceptions = new() { "right" };
			List<string> leftEnglishExceptions = new() { "left" };
			List<string> rightPolishExceptions = new() { "prawy", "prawa", "prawe" };
			List<string> leftPolishExceptions = new() { "lewy", "lewa", "lewe" };
			string changedWord = string.Empty;

			GeneralPhraseTransformer transformer = new();
			string transformedWord = transformer.TransformPhrase(wordToChange.ToLower());

			if (!harderComparison)
			{
				switch (LanguageManager.PrimaryLanguage)
				{
					case Languages.English:
						transformedWord = CheckWordToChange(rightEnglishExceptions, leftEnglishExceptions, transformedWord);
						break;
					case Languages.Polish:
						transformedWord = CheckWordToChange(rightPolishExceptions, leftPolishExceptions, transformedWord);
						break;
					default:
						Debug.LogError("Add here language to this switch with 'left' or 'right' exceptions");
						break;
				}
			}

			if (!IsTransformedWordEqualException(transformedWord, rightEnglishExceptions, leftEnglishExceptions, rightPolishExceptions, leftPolishExceptions)) return wordToChange;

			switch (LanguageManager.PrimaryLanguage)
			{
				case Languages.English:
					if (rightEnglishExceptions.Contains(transformedWord))
						changedWord = "r";
					else if (leftEnglishExceptions.Contains(transformedWord))
						changedWord = "l";
					break;
				case Languages.Polish:
					if (rightPolishExceptions.Contains(transformedWord))
						changedWord = "p";
					else if (leftPolishExceptions.Contains(transformedWord))
						changedWord = "l";
					break;
				default:
					Debug.LogError("Add here language to this switch with 'left' or 'right' exceptions");
					break;
			}
			return changedWord;
		}
		private static bool IsTransformedWordEqualException(string wordToCheck, List<string> rightEnglishExceptions, List<string> leftEnglishExceptions, List<string> rightPolishExceptions, List<string> leftPolishExceptions)
		{
			bool isEqual = false;

			switch (LanguageManager.PrimaryLanguage)
			{
				case Languages.English:
					if (rightEnglishExceptions.Contains(wordToCheck) || leftEnglishExceptions.Contains(wordToCheck)) isEqual = true;
					break;
				case Languages.Polish:
					if (rightPolishExceptions.Contains(wordToCheck) || leftPolishExceptions.Contains(wordToCheck)) isEqual = true;
					break;
				default:
					Debug.LogError("Add here language to this switch with 'left' or 'right' exceptions");
					break;
			}

			return isEqual;
		}
		private static List<string> ConstructSubText(string text)
		{
			string[] subStrings = text.Split();
			List<string> cleanedUpSubStrings = new List<string>();
			for (int i = 0; i < subStrings.Length; i++)
			{
				if (string.IsNullOrEmpty(subStrings[i]) || IsStringWhiteSpace(subStrings[0]))
					continue;

				cleanedUpSubStrings.Add(subStrings[i].ToLower());
			}
			return cleanedUpSubStrings;
		}
		private static bool IsStringWhiteSpace(string str)
		{
			return char.IsWhiteSpace(str, 0);
		}
	}
}

