﻿using System;
using System.Collections.Generic;

namespace MyIssues
{
	public static partial class Helpers  
	{
		public static int[] GetLanguageColor(string lang)
		{
			int[] color = null;
			if (!String.IsNullOrEmpty(lang) && LanguageColors.TryGetValue(lang, out color))
			{
				return color;
			}
			return new int[] { 204, 100, 80 };;
		}
		public static Dictionary<string, int[]> LanguageColors = new Dictionary<string, int[]>
		{
			{"1C Enterprise",new int[]{129,76,204}},
			{"ABAP",new int[]{232,39,75}},
			{"ActionScript",new int[]{136,43,15}},
			{"Ada",new int[]{2,248,140}},
			{"Agda",new int[]{49,86,101}},
			{"AGS Script",new int[]{185,217,255}},
			{"Alloy",new int[]{100,200,0}},
			{"AMPL",new int[]{230,239,187}},
			{"ANTLR",new int[]{157,195,255}},
			{"API Blueprint",new int[]{42,204,168}},
			{"APL",new int[]{90,129,100}},
			{"Apollo Guidance Computer",new int[]{11,61,145}},
			{"AppleScript",new int[]{16,31,31}},
			{"Arc",new int[]{170,42,254}},
			{"Arduino",new int[]{189,121,209}},
			{"ASN.1",new int[]{174,234,208}},
			{"ASP",new int[]{106,64,253}},
			{"AspectJ",new int[]{169,87,176}},
			{"Assembly",new int[]{110,76,19}},
			{"ATS",new int[]{26,198,32}},
			{"AutoHotkey",new int[]{101,148,185}},
			{"AutoIt",new int[]{28,53,82}},
			{"Batchfile",new int[]{193,241,46}},
			{"Bison",new int[]{106,70,63}},
			{"BlitzMax",new int[]{205,100,0}},
			{"Boo",new int[]{212,190,193}},
			{"Brainfuck",new int[]{47,37,48}},
			{"C",new int[]{85,85,85}},
			{"C#",new int[]{23,134,0}},
			{"C++",new int[]{243,75,125}},
			{"Chapel",new int[]{141,198,63}},
			{"Cirru",new int[]{204,204,255}},
			{"Clarion",new int[]{219,144,30}},
			{"Clean",new int[]{63,133,175}},
			{"Click",new int[]{228,230,243}},
			{"Clojure",new int[]{219,88,85}},
			{"CoffeeScript",new int[]{36,71,118}},
			{"ColdFusion",new int[]{237,44,214}},
			{"ColdFusion CFC",new int[]{237,44,214}},
			{"Common Lisp",new int[]{63,182,139}},
			{"Component Pascal",new int[]{176,206,78}},
			{"Crystal",new int[]{119,103,145}},
			{"CSS",new int[]{86,61,124}},
			{"Cucumber",new int[]{91,32,99}},
			{"Cuda",new int[]{58,78,58}},
			{"D",new int[]{186,89,94}},
			{"Dart",new int[]{0,180,171}},
			{"DM",new int[]{68,114,101}},
			{"Dogescript",new int[]{204,167,96}},
			{"Dylan",new int[]{108,97,110}},
			{"E",new int[]{204,206,53}},
			{"Eagle",new int[]{129,76,5}},
			{"eC",new int[]{145,57,96}},
			{"ECL",new int[]{138,18,103}},
			{"Eiffel",new int[]{148,109,87}},
			{"EJS",new int[]{169,30,80}},
			{"Elixir",new int[]{110,74,126}},
			{"Elm",new int[]{96,181,204}},
			{"Emacs Lisp",new int[]{192,101,219}},
			{"EmberScript",new int[]{255,244,243}},
			{"EQ",new int[]{167,134,73}},
			{"Erlang",new int[]{184,57,152}},
			{"F#",new int[]{184,69,252}},
			{"Factor",new int[]{99,103,70}},
			{"Fancy",new int[]{123,157,180}},
			{"Fantom",new int[]{219,222,213}},
			{"FLUX",new int[]{136,204,255}},
			{"Forth",new int[]{52,23,8}},
			{"FORTRAN",new int[]{77,65,177}},
			{"FreeMarker",new int[]{0,80,178}},
			{"Frege",new int[]{0,202,254}},
			{"Game Maker Language",new int[]{143,178,0}},
			{"Glyph",new int[]{228,204,152}},
			{"Gnuplot",new int[]{240,169,240}},
			{"Go",new int[]{55,94,171}},
			{"Golo",new int[]{136,86,42}},
			{"Gosu",new int[]{130,147,127}},
			{"Grammatical Framework",new int[]{121,170,122}},
			{"Groff",new int[]{236,222,190}},
			{"Groovy",new int[]{230,159,86}},
			{"Hack",new int[]{135,135,135}},
			{"Haml",new int[]{236,226,169}},
			{"Handlebars",new int[]{1,169,214}},
			{"Harbour",new int[]{14,96,227}},
			{"Haskell",new int[]{41,181,68}},
			{"Haxe",new int[]{223,121,0}},
			{"HTML",new int[]{228,75,35}},
			{"Hy",new int[]{119,144,178}},
			{"IDL",new int[]{163,82,47}},
			{"Io",new int[]{169,24,141}},
			{"Ioke",new int[]{7,129,147}},
			{"Isabelle",new int[]{254,254,0}},
			{"J",new int[]{158,237,255}},
			{"Java",new int[]{176,114,25}},
			{"JavaScript",new int[]{241,224,90}},
			{"JFlex",new int[]{219,202,0}},
			{"JSONiq",new int[]{64,212,126}},
			{"Julia",new int[]{162,112,186}},
			{"Jupyter Notebook",new int[]{218,91,11}},
			{"Kotlin",new int[]{241,142,51}},
			{"KRL",new int[]{40,67,31}},
			{"Lasso",new int[]{153,153,153}},
			{"Latte",new int[]{168,255,151}},
			{"Less",new int[]{161,217,161}},
			{"Lex",new int[]{219,202,0}},
			{"LFE",new int[]{0,66,0}},
			{"LiveScript",new int[]{73,152,134}},
			{"LLVM",new int[]{24,86,25}},
			{"LOLCODE",new int[]{204,153,0}},
			{"LookML",new int[]{101,43,129}},
			{"LSL",new int[]{61,153,112}},
			{"Lua",new int[]{0,0,128}},
			{"Makefile",new int[]{66,120,25}},
			{"Mask",new int[]{249,119,50}},
			{"Matlab",new int[]{187,146,172}},
			{"Max",new int[]{196,167,156}},
			{"MAXScript",new int[]{0,166,166}},
			{"Mercury",new int[]{255,43,43}},
			{"Metal",new int[]{143,20,233}},
			{"Mirah",new int[]{199,169,56}},
			{"MTML",new int[]{183,225,244}},
			{"NCL",new int[]{40,67,31}},
			{"Nemerle",new int[]{61,60,110}},
			{"nesC",new int[]{148,176,199}},
			{"NetLinx",new int[]{10,160,255}},
			{"NetLinx+ERB",new int[]{116,127,170}},
			{"NetLogo",new int[]{255,99,117}},
			{"NewLisp",new int[]{135,174,215}},
			{"Nginx",new int[]{148,105,233}},
			{"Nimrod",new int[]{55,119,91}},
			{"Nit",new int[]{0,153,23}},
			{"Nix",new int[]{126,126,255}},
			{"Nu",new int[]{201,223,64}},
			{"NumPy",new int[]{156,138,249}},
			{"Objective-C",new int[]{67,142,255}},
			{"Objective-C++",new int[]{104,102,251}},
			{"Objective-J",new int[]{255,12,90}},
			{"OCaml",new int[]{59,225,51}},
			{"Omgrofl",new int[]{202,187,255}},
			{"ooc",new int[]{176,183,126}},
			{"Opal",new int[]{247,237,224}},
			{"Oxygene",new int[]{205,208,227}},
			{"Oz",new int[]{250,183,56}},
			{"Pan",new int[]{204,0,0}},
			{"Papyrus",new int[]{102,0,204}},
			{"Parrot",new int[]{243,202,10}},
			{"Pascal",new int[]{227,241,113}},
			{"PAWN",new int[]{219,178,132}},
			{"Perl",new int[]{2,152,195}},
			{"Perl6",new int[]{0,0,251}},
			{"PHP",new int[]{79,93,149}},
			{"PigLatin",new int[]{252,215,222}},
			{"Pike",new int[]{0,83,144}},
			{"PLSQL",new int[]{218,216,216}},
			{"PogoScript",new int[]{216,0,116}},
			{"PostScript",new int[]{218,41,28}},
			{"PowerBuilder",new int[]{143,15,141}},
			{"Processing",new int[]{0,150,216}},
			{"Prolog",new int[]{116,40,60}},
			{"Propeller Spin",new int[]{127,162,167}},
			{"Puppet",new int[]{48,43,109}},
			{"Pure Data",new int[]{145,222,121}},
			{"PureBasic",new int[]{90,105,134}},
			{"PureScript",new int[]{29,34,45}},
			{"Python",new int[]{53,114,165}},
			{"QML",new int[]{68,165,28}},
			{"R",new int[]{25,140,231}},
			{"Racket",new int[]{34,34,143}},
			{"Ragel in Ruby Host",new int[]{157,82,0}},
			{"RAML",new int[]{119,217,251}},
			{"Rebol",new int[]{53,138,91}},
			{"Red",new int[]{245,0,0}},
			{"Ren'Py",new int[]{255,127,127}},
			{"Rouge",new int[]{204,0,136}},
			{"Ruby",new int[]{112,21,22}},
			{"RUNOFF",new int[]{102,90,78}},
			{"Rust",new int[]{222,165,132}},
			{"SaltStack",new int[]{100,100,100}},
			{"SAS",new int[]{179,73,54}},
			{"Sass",new int[]{207,100,154}},
			{"Scala",new int[]{194,45,64}},
			{"Scheme",new int[]{30,74,236}},
			{"SCSS",new int[]{207,100,154}},
			{"Self",new int[]{5,121,170}},
			{"Shell",new int[]{137,224,81}},
			{"Shen",new int[]{18,15,20}},
			{"Slash",new int[]{0,126,255}},
			{"Slim",new int[]{255,143,119}},
			{"Smalltalk",new int[]{89,103,6}},
			{"SourcePawn",new int[]{92,118,17}},
			{"SQF",new int[]{63,63,63}},
			{"Squirrel",new int[]{128,0,0}},
			{"SRecode Template",new int[]{52,138,52}},
			{"Stan",new int[]{178,1,29}},
			{"Standard ML",new int[]{220,86,109}},
			{"SuperCollider",new int[]{70,57,11}},
			{"Swift",new int[]{255,172,69}},
			{"SystemVerilog",new int[]{218,225,194}},
			{"Tcl",new int[]{228,204,152}},
			{"Terra",new int[]{0,0,76}},
			{"TeX",new int[]{61,97,23}},
			{"Turing",new int[]{207,20,43}},
			{"TypeScript",new int[]{43,116,137}},
			{"Unified Parallel C",new int[]{78,54,23}},
			{"UnrealScript",new int[]{165,76,77}},
			{"Vala",new int[]{251,229,205}},
			{"Verilog",new int[]{178,183,248}},
			{"VHDL",new int[]{173,178,203}},
			{"VimL",new int[]{25,159,75}},
			{"Visual Basic",new int[]{148,93,183}},
			{"Volt",new int[]{31,31,31}},
			{"Vue",new int[]{44,62,80}},
			{"Web Ontology Language",new int[]{156,201,221}},
			{"wisp",new int[]{117,130,209}},
			{"X10",new int[]{75,107,239}},
			{"xBase",new int[]{64,58,64}},
			{"XC",new int[]{153,218,7}},
			{"XQuery",new int[]{82,50,231}},
			{"XSLT",new int[]{235,140,235}},
			{"Yacc",new int[]{75,108,75}},
			{"Zephir",new int[]{17,143,158}}
		};
	}
}