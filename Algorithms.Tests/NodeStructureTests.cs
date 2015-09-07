using NUnit.Framework;
using System;
using Algorithm;

namespace Algorithms.Tests
{
	[TestFixture ()]
	public class NodeStructureTests
	{
		[Test]
		public void Test_root_to_be_initialized_correctly ()
		{
			var content = Guid.NewGuid ().ToString ();
			var node = new NodeStructure<string> (null, content);

			Assert.AreEqual (null, node.Parent, "Parent of root should be set to null");
			Assert.AreEqual (content, node.Content, "Content should be initialized with value passed to constructor");
			Assert.AreEqual (0, node.Order, "Order is a # of current node in its parent's children starting with 0");
			Assert.AreEqual (0, node.Point.Y, "Root node should be on '0' level (and have Y_coordinate set to 0)");
			Assert.NotNull (node.Children, "Children should be initialized in constructor for farther use");
		}

		[Test]
		public void Test_children_to_be_initialized_within_hierarhy ()
		{
			var content = Guid.NewGuid ().ToString ();
			var node = new NodeStructure<string> (null, content);

			for (var i = 0; i < 5; i++) {
				var child = new NodeStructure<string> (node, content);

				Assert.AreEqual (node, child.Parent);
				Assert.AreEqual (i, child.Order, "Order is a # of current node in its parent's children starting with 0");
				Assert.AreEqual (1f, child.Point.Y, "child should be put underneath parent (and have Y_coordinate set to parent's Y+1)");
				Assert.Contains (child, node.Children, "child should be added to parent's list of children");

				for (var j = 0; j < 5; j++) {
					var grandchild = new NodeStructure<string> (child, "");

					Assert.AreEqual (child, grandchild.Parent);
					Assert.AreEqual (j, grandchild.Order, "Order is a # of current node in its parent's children starting with 0");
					Assert.AreEqual (child.Point.Y + 1, grandchild.Point.Y, "child should be put underneath parent (and have Y_coordinate set to parent's Y+1)");
					Assert.Contains (grandchild, child.Children, "child should be added to parent's list of children");
				}
			}
		}

		[Test]
		public void Test_node_to_override_ToString_method_with_ContentToString ()
		{
			
			var stringNode = new NodeStructure<string> (null, "stringOverriden");
			Assert.AreEqual (stringNode.Content.ToString (), stringNode.ToString (), "ToString should be overriden using Content.ToString() value");

			var intNode = new NodeStructure<int> (null, 12);
			Assert.AreEqual (intNode.Content.ToString (), intNode.ToString (), "ToString should be overriden using Content.ToString() value");
		}
	}
}

