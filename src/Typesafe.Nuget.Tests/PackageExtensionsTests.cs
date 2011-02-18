using System;
using NuGet;
using NUnit.Framework;
using Typesafe.Nuget.Dgml;

namespace Typesafe.Nuget.Tests
{
	[TestFixture]
	public class PackageExtensionsTests
	{
		[Test]
		public void GetNodeName_should_return_name_and_version()
		{
			var p = new TestPackage { Id = "test-package", Version = new Version("1.2.3") };

			Assert.AreEqual("test-package (1.2.3)", p.GetNodeName());
		}

		[Test]
		public void ToGraphNode_should_return_node_with_packge_label()
		{
			var p = new TestPackage { Id = "test-package", Version = new Version("1.2.3") };

			Assert.AreEqual("test-package (1.2.3)", p.ToGraphNode().Label);
		}

		[Test]
		public void ToGraphNode_should_return_node_with_packge_category()
		{
			var p = new TestPackage { Id = "test-package", Version = new Version("1.2.3") };

			Assert.AreEqual("Package", p.ToGraphNode().Category1);
		}

		[Test]
		public void ToGraphNode_with_assembly_inclusion_should_return_node_with_group_expanded()
		{
			var p = new TestPackage { Id = "test-package", Version = new Version("1.2.3") };
			var node = p.ToGraphNode(true);

			Assert.IsTrue(node.GroupSpecified);
			Assert.AreEqual(GroupEnum.Expanded, node.Group);
		}

		[Test]
		public void GetLinkTo_dependency_should_return_link_with_packge_category()
		{
			var p = new TestPackage { Id = "test-package", Version = new Version("1.2.3") };
			var dependency = new PackageDependency("dep", new VersionSpec { MinVersion = new Version("1.2.3") });

			Assert.AreEqual("Package", p.GetLinkTo(dependency).Category1);
		}

		[Test]
		public void GetLinkTo_dependency_should_return_link_with_package_name_as_source()
		{
			var p = new TestPackage { Id = "test-package", Version = new Version("1.2.3") };
			var dependency = new PackageDependency("dep", new VersionSpec { MinVersion = new Version("1.2.3") });

			Assert.AreEqual("test-package (1.2.3)", p.GetLinkTo(dependency).Source);
		}

		[Test]
		public void GetLinkTo_dependency_should_return_link_with_dependency_name_as_target()
		{
			var p = new TestPackage { Id = "test-package", Version = new Version("1.2.3") };
			var dependency = new PackageDependency("dep", new VersionSpec { MinVersion = new Version("1.2.3") });

			Assert.AreEqual("dep (1.2.3)", p.GetLinkTo(dependency).Target);
		}

		[Test]
		public void GetLinkTo_AssemblyReference_should_return_link_with_package_id_as_source()
		{
			var p = new TestPackage { Id = "test-package", Version = new Version("1.2.3") };
			var r = new TestPackageAssemblyReference {Name = "ref"};

			Assert.AreEqual("test-package", p.GetLinkTo(r).Source);
		}
	}
}
