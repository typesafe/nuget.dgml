using System;
using System.IO;
using System.Runtime.Versioning;
using NuGet;

namespace Typesafe.Nuget.Tests
{
	internal class TestPackageAssemblyReference : IPackageAssemblyReference
	{
		public Stream GetStream()
		{
			throw new NotImplementedException();
		}

		public string Path { get; set; }
		public FrameworkName TargetFramework { get; set; }
		public string Name { get; set; }
	}
}