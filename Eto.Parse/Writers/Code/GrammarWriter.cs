using System;
using Eto.Parse.Parsers;
using System.Collections.Generic;
using System.IO;

namespace Eto.Parse.Writers.Code
{

	public class GrammarWriter : UnaryWriter<Grammar>
	{
		bool writeNewObject;
		protected override bool WriteNewObject { get { return writeNewObject; } }

		public override string GetName(TextParserWriterArgs args, Grammar parser)
		{
			var writer = args.Writer as CodeParserWriter;
			if (!string.IsNullOrEmpty(writer.GrammarName))
				return "this";
			else
				return NamedWriter.GetIdentifier(parser.Name);
		}

		public override void WriteObject(TextParserWriterArgs args, Grammar parser, string name)
		{
			var writer = args.Writer as CodeParserWriter;
			if (!string.IsNullOrEmpty(writer.GrammarName))
			{
				var iw = args.Output;
				iw.WriteLine("public class {0} : Eto.Parse.Grammar", writer.GrammarName);
				iw.WriteLine("{");
				iw.Indent ++;

				iw.WriteLine("public {0}()", writer.GrammarName);
				iw.Indent ++;
				iw.WriteLine(": base(\"{0}\")", parser.Name.Replace("\"", "\\\""));
				iw.Indent --;
				iw.WriteLine("{");
				iw.Indent ++;

				base.WriteObject(args, parser, name);
			}
			else
			{
				base.WriteObject(args, parser, name);
				args.Output.WriteLine("{0}.Name = \"{1}\";", name, parser.Name);
			}
		}

		public override void WriteContents(TextParserWriterArgs args, Grammar parser, string name)
		{
			base.WriteContents(args, parser, name);
			var writer = args.Writer as CodeParserWriter;
			if (!string.IsNullOrEmpty(writer.GrammarName))
			{
				var iw = args.Output;
				iw.Indent --;
				iw.WriteLine("}");
				iw.Indent --;
				iw.WriteLine("}");
			}
		}
	}
	
}