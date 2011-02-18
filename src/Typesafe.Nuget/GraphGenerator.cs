using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using NuGet;
using Typesafe.Nuget.Dgml;

namespace Typesafe.Nuget
{
	public class GraphGenerator
	{
		private readonly IPackageRepository repository;
		private readonly IDictionary<string, Assembly> referencedAssemblies = new Dictionary<string, Assembly>();

		public GraphGenerator(PackageSource packageSource)
		{
			if (packageSource == null) throw new ArgumentNullException("packageSource");

			repository = PackageRepositoryFactory.Default.CreateRepository(packageSource);
		}

		public GraphGenerator(IPackageRepository repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			this.repository = repository;
		}

		public bool IncludeAssemblyReferences { get; set; }

		public void WriteGraph(Stream stream)
		{
			var serializer = new XmlSerializer(typeof(DirectedGraph));
			serializer.Serialize(stream, GenerateGraph());
		}

		public DirectedGraph GenerateGraph()
		{
			var nodes = new List<DirectedGraphNode>();
			var links = new List<DirectedGraphLink>();

			GenerateGraph(nodes, links);

			return new DirectedGraph { Nodes = nodes.ToArray(), Links = links.ToArray() };
		}

		private void GenerateGraph(ICollection<DirectedGraphNode> nodes, ICollection<DirectedGraphLink> links)
		{
			foreach (var package in repository.GetPackages())
			{
				var node = package.ToGraphNode(IncludeAssemblyReferences);
				nodes.Add(node);

				if (IncludeAssemblyReferences) WriteAssemblyReferences(package, links);

				foreach (var dependency in package.Dependencies)
				{
					nodes.Add(dependency.ToGraphNode(IncludeAssemblyReferences));
					links.Add(package.GetLinkTo(dependency));
				}
			}
		}

		private static void WriteAssemblyReferences(IPackage package, ICollection<DirectedGraphLink> links)
		{
			foreach (var assemblyReference in package.AssemblyReferences.Where(IsNonBclAssembly))
			{
				links.Add(package.GetLinkTo(assemblyReference));
			}
		}
		private static bool IsNonBclAssembly(IPackageAssemblyReference assemblyReference)
		{
			return !assemblyReference.Name.StartsWith("System") && !assemblyReference.Name.StartsWith("Microsoft");
		}
	}
}
