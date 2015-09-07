using System;
using NUnit.Framework;
using Algorithm.Buchheim;
using Algorithm;

namespace Algorithms.Tests.Buchheim
{

	[TestFixture]
	public class TreeModifierTests
	{
		private readonly float tolerance = 0.01f;
		private int randomLimit = 1000;

		[Test]
		public void Should_center_node_between_first_and_last_child ()
		{
			
			var rand = new Random ();
			var root = new BuchheimNode (null, null, 0);
			root.Preliminary = rand.Next (randomLimit);

			var child1 = root.AddChild (null, 0);
			child1.Preliminary = rand.Next (randomLimit);
			root.AddChild (null, 1);
			root.AddChild (null, 2);
			var child4 = root.AddChild (null, 3);
			child4.Preliminary = rand.Next (randomLimit);

			var treeModifier = new TreeModifier ();
			treeModifier.CenterVertexBasedOnChildren (root);
			var center = .5f * (child1.Preliminary + child4.Preliminary);

			Assert.That (center, Is.EqualTo (root.Preliminary).Within (tolerance), "Should center vertex above its children");
		}

		[Test]
		public void Should_do_nothing_when_centralizing_leaf ()
		{

			var rand = new Random ();
			var leaf = new BuchheimNode (null, null, 0);
			var leafPosition = rand.Next (randomLimit);
			leaf.Preliminary = leafPosition;

			var treeModifier = new TreeModifier ();
			treeModifier.CenterVertexBasedOnChildren (leaf);
			Assert.That (leafPosition, Is.EqualTo (leaf.Preliminary).Within (tolerance));
		}

		[Test]
		public void Should_move_vertex_right_from_left_sibling ()
		{
			var rand = new Random ();
			var root = new BuchheimNode (null, null, 0);
			root.Preliminary = rand.Next (randomLimit);

			root.AddChild (null, 0);
			root.AddChild (null, 1);
			var child3 = root.AddChild (null, 2);
			child3.Preliminary = rand.Next (randomLimit);

			var child4 = root.AddChild (null, 3);
			var child4PreviousPosition = rand.Next ();
			child4.Preliminary = child4PreviousPosition;

			var treeModifier = new TreeModifier ();
			treeModifier.MoveAsideFromLeftSibling (child4);

			Assert.That (child4PreviousPosition + child3.Preliminary, Is.EqualTo (child4.Preliminary).Within (tolerance));
			Assert.That (child3.Preliminary, Is.EqualTo (child4.Mod).Within (tolerance));
		}

		[Test]
		public void Should_move_vertex_right_with_offset_from_left_sibling ()
		{
			var rand = new Random ();
			var root = new BuchheimNode (null, null, 0);
			root.Preliminary = rand.Next (randomLimit);

			root.AddChild (null, 0);
			root.AddChild (null, 1);
			var child3 = root.AddChild (null, 2);
			child3.Preliminary = rand.Next (randomLimit);

			var child4 = root.AddChild (null, 3);
			var child4PreviousPosition = rand.Next (randomLimit);
			child4.Preliminary = child4PreviousPosition;
			var offset = rand.Next (randomLimit);
			var treeModifier = new TreeModifier ();
			treeModifier.MoveAsideFromLeftSibling (child4, offset);

			Assert.That (child4PreviousPosition + child3.Preliminary + offset, Is.EqualTo (child4.Preliminary).Within (tolerance));
			Assert.That (child3.Preliminary, Is.EqualTo (child4.Mod).Within (tolerance));

		}

		[Test]
		public void Should_not_move_aside_left_most_node ()
		{
			var rand = new Random ();
			var root = new BuchheimNode (null, null, 0);
			var rootPreviousPosition = rand.Next (randomLimit);
			var rootPreviousMod = rand.Next (randomLimit);
			root.Preliminary = rootPreviousPosition;
			root.Mod = rootPreviousMod;

			var child = root.AddChild (null, 0);
			var childPreviousPosition = rand.Next (randomLimit);
			var childPreviousMod = rand.Next (randomLimit);
			child.Preliminary = childPreviousPosition;
			child.Mod = childPreviousMod;
			var offset = rand.Next (randomLimit);
			var treeModifier = new TreeModifier ();
			treeModifier.MoveAsideFromLeftSibling (child, offset);

			Assert.That (childPreviousPosition, Is.EqualTo (child.Preliminary).Within (tolerance));
			Assert.That (childPreviousMod, Is.EqualTo (child.Mod).Within (tolerance));

			treeModifier.MoveAsideFromLeftSibling (root, offset);
			Assert.That (rootPreviousPosition, Is.EqualTo (root.Preliminary).Within (tolerance));
			Assert.That (rootPreviousMod, Is.EqualTo (root.Mod).Within (tolerance));

		}

		[Test]
		public void Should_recalculate_mod_and_preliminary_for_children_ctarting_from_right_end ()
		{
			var rand = new Random ();
			var root = new BuchheimNode (null, null, 0);
			root.Preliminary = rand.Next (randomLimit);
			var treeModifier = new TreeModifier ();
			Assert.DoesNotThrow (() => treeModifier.ExecuteShifts (root), "ExecuteShifts() shouldn't throw on leaf");

			var child0 = root.AddChild (null, 2);
			child0.Change = rand.Next (randomLimit);
			child0.Shift = rand.Next (randomLimit);


			var child1 = root.AddChild (null, 0);
			child1.Change = rand.Next (randomLimit);
			child1.Shift = rand.Next (randomLimit);

			var child2 = root.AddChild (null, 1);
			child2.Change = rand.Next (randomLimit);
			child2.Shift = rand.Next (randomLimit);

			var child3 = root.AddChild (null, 2);
			child3.Change = rand.Next (randomLimit);
			child3.Shift = rand.Next (randomLimit);

			treeModifier.ExecuteShifts (root);

			var sum = 0f;
			Assert.That (sum, Is.EqualTo (child3.Mod).Within (tolerance));
			sum += child3.Change + child3.Shift;
			Assert.That (sum, Is.EqualTo (child2.Mod).Within (tolerance));
			sum += child2.Change + child2.Shift + child3.Change;
			Assert.That (sum, Is.EqualTo (child1.Mod).Within (tolerance));
			sum += child1.Change + child1.Shift + child2.Change + child3.Change;
			Assert.That (sum, Is.EqualTo (child0.Mod).Within (tolerance));


			sum = 0f;
			Assert.That (sum, Is.EqualTo (child3.Preliminary).Within (tolerance));
			sum += child3.Change + child3.Shift;
			Assert.That (sum, Is.EqualTo (child2.Preliminary).Within (tolerance));
			sum += child2.Change + child2.Shift + child3.Change;
			Assert.That (sum, Is.EqualTo (child1.Preliminary).Within (tolerance));
			sum += child1.Change + child1.Shift + child2.Change + child3.Change;
			Assert.That (sum, Is.EqualTo (child0.Preliminary).Within (tolerance));

		}

		[Test]
		public void Should_shift_right_node_from_left_one ()
		{
			var treeModifier = new TreeModifier ();
			var left = new BuchheimNode (null, null, 0);
			left.Shift = -1;
			left.Change = -2;
			left.Mod = 1;
			left.Preliminary = 2;

			var right = new BuchheimNode (null, null, 10);
			right.Shift = -1;
			right.Change = -2;
			right.Mod = 1;
			right.Preliminary = 2;


			treeModifier.MoveSubtree (left, right, 20);


			Assert.That (-1f, Is.EqualTo (left.Shift).Within (tolerance), "leftVertex.Shift shouldn't change");
			Assert.That (0f, Is.EqualTo (left.Change).Within (tolerance), " <shift / (rightIndex-leftIndex)> should be added to leftVertex.Change");
			Assert.That (2f, Is.EqualTo (left.Preliminary).Within (tolerance), "leftVertex.Preliminary shouldn't change");
			Assert.That (1f, Is.EqualTo (left.Mod).Within (tolerance), "leftVertex.Mod shouldn't change");


			Assert.That (19f, Is.EqualTo (right.Shift).Within (tolerance), "<shift> should be added to rightVertex.Shift");
			Assert.That (-4f, Is.EqualTo (right.Change).Within (tolerance), "<shift / (rightIndex-leftIndex)> should be subtracted from rightVertex.Change");
			Assert.That (22f, Is.EqualTo (right.Preliminary).Within (tolerance), "<shift> should be added to rightVertex.Preliminary");
			Assert.That (21f, Is.EqualTo (right.Mod).Within (tolerance), "<shift> should be added to rightVertex.Mod");
		}
	}
}
