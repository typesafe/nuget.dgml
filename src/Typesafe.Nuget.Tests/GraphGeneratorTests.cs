using System.IO;
using NuGet;
using NUnit.Framework;

namespace Typesafe.Nuget.Tests
{
	[TestFixture]
	public class GraphGeneratorTests
	{
		[Test]
		public void GenerateGraph_should_include_all_packages()
		{
			var repo = new LocalPackageRepository(@"..\..\..\packages");
			var g = new GraphGenerator(repo);
			using (var s = File.OpenWrite("nuget.dgml"))
			{
				g.WriteGraph(s);
				s.Flush();
			}
		}

		[Test]
		public void GenerateGraph_with_assemblies_should_include_all_packages()
		{
			var repo = new LocalPackageRepository(@"..\..\..\packages");
			var g = new GraphGenerator(repo) { IncludeAssemblyReferences = true };
			using (var s = File.OpenWrite("nuget.dgml"))
			{
				g.WriteGraph(s);
				s.Flush();
			}
		}
	}
}