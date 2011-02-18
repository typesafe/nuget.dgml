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
		private IList<DirectedGraphNode> nodes;
		private ICollection<DirectedGraphLink> links;

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
			nodes = new List<DirectedGraphNode>();
			links = new List<DirectedGraphLink>();

			GenerateNodes();

			return new DirectedGraph { Nodes = nodes.ToArray(), Links = links.ToArray() };
		}

		private void GenerateNodes()
		{
			foreach (var package in repository.GetPackages())
			{
				var node = package.ToGraphNode(IncludeAssemblyReferences);
				nodes.Add(node);

				if (IncludeAssemblyReferences) WriteAssemblyReferences(package);

				foreach (var dependency in package.Dependencies)
				{
					nodes.Add(dependency.ToGraphNode(IncludeAssemblyReferences));
					links.Add(package.GetLinkTo(dependency));
				}
			}
		}

		private void WriteAssemblyReferences(IPackage package)
		{
			foreach (var assemblyReference in package.AssemblyReferences.Where(IsNonBclAssembly))
			{
				var assembly = LoadAssembly(assemblyReference);
				links.Add(package.GetLinkTo(assembly));
				foreach (var referencedAssembly in assembly.GetReferencedAssemblies().Where(IsNonBclAssembly))
				{
					links.Add(assembly.GetLinkTo(referencedAssembly));
				}
			}
		}

		private static bool IsNonBclAssembly(IPackageAssemblyReference assemblyReference)
		{
			var name = assemblyReference.Name;
			return IsBclClassName(name);
		}

		private static bool IsNonBclAssembly(AssemblyName assemblyReference)
		{
			var name = assemblyReference.Name;
			return IsBclClassName(name);
		}

		private static bool IsBclClassName(string name)
		{
			return !name.StartsWith("System") && !name.StartsWith("Microsoft") && !name.StartsWith("mscorlib");
		}

		private Assembly LoadAssembly(IPackageAssemblyReference reference)
		{
			if(!referencedAssemblies.ContainsKey(reference.Name))
			{
				var assembly = Assembly.ReflectionOnlyLoad(reference.GetStream().ReadAllBytes());
				nodes.Add(assembly.ToGraphNode());
				referencedAssemblies.Add(reference.Name, assembly);
			}

			return referencedAssemblies[reference.Name];
		}
	}
}
