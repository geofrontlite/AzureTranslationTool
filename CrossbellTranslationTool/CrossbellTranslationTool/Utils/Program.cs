using System;
using System.IO;
using System.Linq;

namespace CrossbellTranslationTool
{
	static class Program
	{
		static void Do()
		{
			var scenapdir = @"D:\ED_AO\data\scena";
			var jsondir = @"D:\Code\Ao no Kiseki Translation\PC\scena";
			var jsondir2 = @"D:\Code\Ao no Kiseki Translation\PC\scena2";

			foreach (var scenafilepath in Directory.EnumerateFiles(scenapdir, "*.bin"))
			{
				var scenafilename = Path.GetFileName(scenafilepath);
				var jsonfilename = Path.ChangeExtension(scenafilename, ".json");
				var jsonfilepath = Path.Combine(jsondir, jsonfilename);
				var jsonfilepath2 = Path.Combine(jsondir2, jsonfilename);

				var scena = new ScenarioFile(new FileReader(scenafilepath, Encodings.Chinese), typeof(Bytecode.InstructionTable_AoKScena));

				var textitems = JsonTextItemFileIO.ReadFromFile(jsonfilepath);
				var scenastrings = scena.GetFunctionStrings();

				var scenatextitems = scenastrings.Select(x => x.Select(y => new TextItem(y)).ToList()).ToList();

				foreach (var item in Enumerable.Zip(textitems, scenatextitems.SelectMany(x => x), (lhs, rhs) => new { lhs, rhs }))
				{
					if (item.lhs.Translation != "") item.rhs.Translation = item.lhs.Translation;
				}

				var obj = new { Functions = scenatextitems.Select(x => new { Name = "", Strings = x }), StringTable = scena.GetStringTable() };

				var jsonsettings = new Newtonsoft.Json.JsonSerializerSettings();
				jsonsettings.Formatting = Newtonsoft.Json.Formatting.Indented;
				jsonsettings.Converters.Add(new EncodedStringJsonConverter());

				var jsontext = Newtonsoft.Json.JsonConvert.SerializeObject(obj, jsonsettings);

				File.Delete(jsonfilepath2);
				File.WriteAllText(jsonfilepath2, jsontext);
			}
		}
	}
}