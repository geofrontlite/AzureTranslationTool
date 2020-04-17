using System;
using System.IO;

namespace CrossbellTranslationTool.Actions
{
	static class FormatJson
	{
		public static void Run(String translationPath)
		{
			foreach (var filepath in Directory.EnumerateFiles(translationPath, "*.json", SearchOption.AllDirectories))
			{
				var textitems = JsonTextItemFileIO.ReadFromFile(filepath);
				JsonTextItemFileIO.WriteToFile(textitems, filepath);
			}
		}
	}
}