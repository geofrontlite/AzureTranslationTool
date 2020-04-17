using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CrossbellTranslationTool.Actions
{
	static class Extract
	{
		public static void Run(String path, String translationPath)
		{
			var filesystem = new IO.DirectoryFileSystem(path);
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
			Run(filesystem, Encodings.Chinese, translationPath);
		}

		static void Run(IO.IFileSystem filesystem, Encoding encoding, String datapath)
		{
			Assert.IsNotNull(filesystem, nameof(filesystem));
			Assert.IsNotNull(encoding, nameof(encoding));
			Assert.IsValidString(datapath, nameof(datapath));

			var data_text = Text.TextFileDescription.GetTextFileData_AoK();

			var totalscenastringtable = new SortedSet<String>();

			Directory.CreateDirectory(datapath);
			Directory.CreateDirectory(Path.Combine(datapath, "text"));
			Directory.CreateDirectory(Path.Combine(datapath, "monster"));

			foreach (var item in data_text)
			{
				Console.WriteLine(item.FileName);

				var textfilepath = Path.Combine(@"data\text", item.FileName);
				var jsonfilepath = Path.ChangeExtension(Path.Combine(datapath, "text", item.FileName), ".json");

				using (var reader = filesystem.OpenFile(textfilepath, encoding))
				{
					var strings = Text.TextFileIO.Read(reader, item.FilePointerDelegate, item.RecordCount);
					JsonTextItemFileIO.WriteToFile(strings.Select(x => new TextItem(x)).ToList(), jsonfilepath);
				}
			}

			JsonTextItemFileIO.WriteToFile(totalscenastringtable.Select(x => new TextItem(x)).ToList(), Path.Combine(datapath, "stringtable.json"));

			foreach (var filepath in filesystem.GetChildren(@"data\battle\dat", "ms*.dat"))
			{
				var filename = Path.GetFileName(filepath);
				var jsonfilepath = Path.ChangeExtension(Path.Combine(datapath, "monster", filename), ".json");

				Console.WriteLine(filename);

				using (var reader = filesystem.OpenFile(filepath, encoding))
				{
					var monsterfile = OpenMonsterDefinitionFile(reader);
					var strings = monsterfile.GetStrings();

					JsonTextItemFileIO.WriteToFile(strings.Select(x => new TextItem(x)).ToList(), jsonfilepath);
				}
			}

			Console.WriteLine();
			Console.WriteLine("Done.");
		}

		static IMonsterDefinitionFile OpenMonsterDefinitionFile(FileReader reader)
		{
			return new MonsterDefinitionFile_Ao(reader);
		}
	}
}