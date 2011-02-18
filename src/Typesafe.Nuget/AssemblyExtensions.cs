using System.Reflection;
using Typesafe.Nuget.Dgml;

namespace Typesafe.Nuget
{
	internal static class AssemblyExtensions
	{
		public static string GetNodeName(this Assembly assembly)
		{
			var assemblyName = assembly.GetName();
			return string.Format("{0}.dll ({1})", assemblyName.Name, assemblyName.Version);
		}

		public static string GetNodeName(this AssemblyName assembly)
		{
			return string.Format("{0}.dll ({1})", assembly.Name, assembly.Version);
		}

		public static DirectedGraphNode ToGraphNode(this Assembly assembly)
		{
			return new DirectedGraphNode
			       	{
			       		Id = assembly.GetNodeName(),
			       		Label = assembly.GetNodeName(),
			       		Category1 = "Assembly"
			       	};
		}
		public static DirectedGraphLink GetLinkTo(this Assembly reference, AssemblyName assemblyReference)
		{
			return new DirectedGraphLink { Source = reference.GetNodeName(), Target = assemblyReference.GetNodeName() };
		}
	}
}