using System;
using System.Collections.Generic;
using System.IO;
using NuGet;

namespace Typesafe.Nuget.Tests
{
	internal class TestPackage : IPackage
	{
		public string Id { get; set; }
		public Version Version { get; set; }
		public string Title { get; set; }
		public IEnumerable<string> Authors { get; set; }
		public IEnumerable<string> Owners { get; set; }
		public Uri IconUrl { get; set; }
		public Uri LicenseUrl { get; set; }
		public Uri ProjectUrl { get; set; }
		public bool RequireLicenseAcceptance { get; set; }
		public string Description { get; set; }
		public string Summary { get; set; }
		public string Language { get; set; }
		public string Tags { get; set; }
		public IEnumerable<PackageDependency> Dependencies { get; set; }
		public Uri ReportAbuseUrl { get; set; }
		public int DownloadCount { get; set; }
		public int RatingsCount { get; set; }
		public double Rating { get; set; }
		public IEnumerable<IPackageFile> GetFiles()
		{
			throw new NotImplementedException();
		}

		public Stream GetStream()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IPackageAssemblyReference> AssemblyReferences { get; set; }
	}
}