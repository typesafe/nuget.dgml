using NuGet;
using Typesafe.Nuget.Dgml;

namespace Typesafe.Nuget
{
	internal static class PackageAssemblyReferenceExtensions
	{
		public static string GetNodeName(this IPackageAssemblyReference reference)
		{
			return string.Format("{0}", reference.Name);
		}
	}

	internal static class PackageExtensions
	{
		public static string GetNodeName(this IPackage package)
		{
			return string.Format("{0} ({1})", package.Id, package.Version);
		}

		public static string GetNodeName(this PackageDependency dependency)
		{
			return string.Format("{0} ({1})", dependency.Id, dependency.VersionSpec.MinVersion);
		}

		public static DirectedGraphNode ToGraphNode(this IPackage package, bool includeAssemblyReferences = false)
		{
			var node = CreateDirectedGraphNode(package.GetNodeName(), "Package");

			if (includeAssemblyReferences)
			{
				node.Group = GroupEnum.Expanded;
				node.GroupSpecified = true;
			}

			return node;
		}

		public static DirectedGraphNode ToGraphNode(this PackageDependency dependency, bool includeAssemblyReferences = false)
		{
			var node = CreateDirectedGraphNode(dependency.GetNodeName(), "Package");
			
			if (includeAssemblyReferences)
			{
				node.Group = GroupEnum.Expanded;
				node.GroupSpecified = true;
			}

			return node;
		}

		public static DirectedGraphLink GetLinkTo(this IPackage package, PackageDependency dependency)
		{
			return new DirectedGraphLink { Source = package.GetNodeName(), Target = dependency.GetNodeName(), Category1 ="Package" };
		}

		public static DirectedGraphLink GetLinkTo(this IPackage package, IPackageAssemblyReference assemblyReference)
		{
			return new DirectedGraphLink { Source = package.GetNodeName(), Target = assemblyReference.GetNodeName(), Category1 = "Contains" };
		}

		private static DirectedGraphNode CreateDirectedGraphNode(string label, string category)
		{
			return new DirectedGraphNode
			{
				Id = label,
				Label = label,
				Category1 = category
			};
		}
	}
}