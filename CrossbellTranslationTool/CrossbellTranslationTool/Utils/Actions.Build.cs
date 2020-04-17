using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CrossbellTranslationTool.Actions
{
	static class Build
	{
		public static void Run(String path, String translationPath)
		{
			var filesystem = new IO.DirectoryFileSystem(path);

			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

			Run(filesystem, Encoding.GetEncoding(936), translationPath);
		}

		static void Run(IO.IFileSystem filesystem, Encoding encoding, String datapath)
		{
			Assert.IsNotNull(filesystem, nameof(filesystem));
			Assert.IsNotNull(encoding, nameof(encoding));
			Assert.IsValidString(datapath, nameof(datapath));

			var data_text = Text.TextFileDescription.GetTextFileData_AoK();

			var stringtableitems = JsonTextItemFileIO.ReadFromFile(Path.Combine(datapath, "stringtable.json"));

			foreach (var item in data_text)
			{
				var textfilepath = Path.Combine(@"data\text", item.FileName);
				var jsonfilepath = Path.ChangeExtension(Path.Combine(datapath, "text", item.FileName), ".json");

				if (File.Exists(jsonfilepath) == true)
				{
					Console.WriteLine(item.FileName);

					using (var reader = filesystem.OpenFile(textfilepath, encoding))
					{
						var buffer = UpdateTextFile(reader, item.FilePointerDelegate, item.RecordCount, jsonfilepath);
						filesystem.SaveFile(textfilepath, buffer);
					}
				}
			}

			foreach (var filepath in filesystem.GetChildren(@"data\battle\dat", "ms*.dat"))
			{
				var filename = Path.GetFileName(filepath);
				var jsonfilepath = Path.ChangeExtension(Path.Combine(datapath, "monster", filename), ".json");

				if (File.Exists(jsonfilepath) == true)
				{
					Console.WriteLine(filename);

					using (var reader = filesystem.OpenFile(filepath, encoding))
					{
						var buffer = UpdateMonsterFile(reader, jsonfilepath);
						filesystem.SaveFile(filepath, buffer);
					}
				}
			}

			UpdateMonsterNote(filesystem, encoding);
		}

		static void UpdateMonsterNote(IO.IFileSystem filesystem, Encoding encoding)
		{
			Assert.IsNotNull(filesystem, nameof(filesystem));
			Assert.IsNotNull(encoding, nameof(encoding));

			Console.WriteLine("monsnote.dt2");

			using (var reader = filesystem.OpenFile(@"data\monsnote\monsnote.dt2", encoding))
			{
				var monsternote = new MonsterNoteFile(reader);

				foreach (var record in monsternote.Records)
				{
					var filename = Path.Combine(@"data\battle\dat", "ms" + record.Id.Substring(3) + ".dat");

					using (var monsterfilereader = filesystem.OpenFile(filename, encoding))
					{
						var monsterfile = OpenMonsterDefinitionFile(monsterfilereader);
						record.MonsterDefinitionFile = monsterfile;
					}
				}

				var filebuffer = monsternote.Write(encoding);
				filesystem.SaveFile(@"data\monsnote\monsnote.dt2", filebuffer);
			}
		}

		static Byte[] UpdateMonsterFile(FileReader reader, String jsonpath)
		{
			Assert.IsNotNull(reader, nameof(reader));
			Assert.IsNotNull(reader, nameof(reader));
			Assert.IsValidString(jsonpath, nameof(jsonpath));

			var monsterfile = OpenMonsterDefinitionFile(reader);

			var json = JsonTextItemFileIO.ReadFromFile(jsonpath);
			var strings = json.Select(x => x.GetBestText()).ToList();

			monsterfile.SetStrings(strings);

			return monsterfile.Write(reader.Encoding);
		}

		static Byte[] UpdateTextFile(FileReader reader, Text.FilePointerDelegate filepointerfunc, Int32 recordcount, String jsonpath)
		{
			Assert.IsNotNull(reader, nameof(reader));
			Assert.IsNotNull(filepointerfunc, nameof(filepointerfunc));
			Assert.IsValidString(jsonpath, nameof(jsonpath));

			var textitems = JsonTextItemFileIO.ReadFromFile(jsonpath);
			var strings = textitems.Select(x => x.Translation).ToList();

			return Text.TextFileIO.Write(reader, filepointerfunc, recordcount, strings);
		}

		static IMonsterDefinitionFile OpenMonsterDefinitionFile(FileReader reader)
		{
			return new MonsterDefinitionFile_Ao(reader);
		}
	}
}