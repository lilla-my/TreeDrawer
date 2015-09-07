using NUnit.Framework;
using Algorithm.Buchheim;

namespace Algorithms.Tests.Buchheim
{
    [TestFixture]
    public class EdgeWalkerTests
    {
        private readonly float m_tolerance = 0.01f;

        [Test]
        public void Should_initialize_correctly ()
        {
            var initialMod = 5f;
            var leaf = new BuchheimNode (null, null, 0) { Mod = initialMod };
            var edgeWalker = new EdgeWalker (leaf);
            Assert.AreSame (leaf, edgeWalker.CurrentNode);
            Assert.AreEqual (initialMod, edgeWalker.ModSum);
        }

        [Test]
        public void Should_be_finished_when_node_is_null ()
        {
            var edgeWalker = new EdgeWalker (null);
            Assert.True (edgeWalker.Finished);

            var leaf = new BuchheimNode (null, null, 0);

            edgeWalker = new EdgeWalker (leaf);
            Assert.False (edgeWalker.Finished);
        }


        [Test]
        public void Should_be_leftMost_when_null_or_leftMost ()
        {
            var edgeWalker = new EdgeWalker (null);
            Assert.True (edgeWalker.IsTheLastOnLeft ());

            var leaf = new BuchheimNode (null, null, 0);

            edgeWalker = new EdgeWalker (leaf);
            Assert.True (edgeWalker.IsTheLastOnLeft ());
        }


        [Test]
        public void Should_be_rightMost_when_null_or_rightMost ()
        {
            var edgeWalker = new EdgeWalker (null);
            Assert.True (edgeWalker.IsTheLastOnRight ());

            var leaf = new BuchheimNode (null, null, 0);

            edgeWalker = new EdgeWalker (leaf);
            Assert.True (edgeWalker.IsTheLastOnRight ());
        }

        [Test]
        public void Should_be_able_to_move_left_when_has_children ()
        {
            var root = new BuchheimNode (null, null, 0);

            var child0 = root.AddChild (null, 0);
            root.AddChild (null, 1);
            var edgeWalker = new EdgeWalker (root);
            Assert.False (edgeWalker.IsTheLastOnLeft ());

            edgeWalker.MoveLeft ();
            Assert.AreSame (child0, edgeWalker.CurrentNode);
            Assert.True (edgeWalker.IsTheLastOnLeft ());
        }

        [Test]
        public void Should_be_able_to_move_right_when_has_children ()
        {
            var root = new BuchheimNode (null, null, 0);

            root.AddChild (null, 0);
            var child1 = root.AddChild (null, 1);

            var edgeWalker = new EdgeWalker (root);
            Assert.False (edgeWalker.IsTheLastOnRight ());

            edgeWalker.MoveRight ();
            Assert.AreSame (child1, edgeWalker.CurrentNode);
            Assert.True (edgeWalker.IsTheLastOnRight ());
        }

        [Test]
        public void Should_add_mod_on_update ()
        {
            var root = new BuchheimNode (null, null, 0);
            root.Mod = 10;
            var edgeWalker = new EdgeWalker (root);
            edgeWalker.UpdateSum ();
            Assert.That (20f, Is.EqualTo (edgeWalker.ModSum).Within (m_tolerance));
        }

        [Test]
        public void Should_return_offset_as_modSum_added_to_node_preliminary ()
        {
            var root = new BuchheimNode (null, null, 0);
            root.Mod = 10;
            root.Preliminary = 5;
            var edgeWalker = new EdgeWalker (root);
            Assert.That (15f, Is.EqualTo (edgeWalker.Offset).Within (m_tolerance));
            edgeWalker.UpdateSum ();

            Assert.That (25f, Is.EqualTo (edgeWalker.Offset).Within (m_tolerance));
        }

        [Test]
        public void Should_be_able_to_update_ancestor_when_not_finished ()
        {

            var root = new BuchheimNode (null, null, 0);
            var edgeWalker = new EdgeWalker (root);
            var ancestor = new BuchheimNode (null, null, 1);
            edgeWalker.SetAncestor (ancestor);
            Assert.That (ancestor, Is.EqualTo (edgeWalker.CurrentNode.Ancestor));

        }

        [Test]
        public void Should_be_able_to_update_thread_and_recalculate_mod_when_not_finished ()
        {
            var root = new BuchheimNode (null, null, 0) { Mod = 1f };
            var edgeWalker = new EdgeWalker (root);
            var thread = new BuchheimNode (null, null, 1) { Mod = 20f };
            root.Mod = 5f;
            var edgeWalkerWithThread = new EdgeWalker (thread);
            edgeWalker.SetThreadAndUpdateMod (edgeWalkerWithThread);
            Assert.That (thread, Is.EqualTo (edgeWalker.CurrentNode.Thread));
            Assert.That (5f + 20f - 1f, Is.EqualTo (edgeWalker.CurrentNode.Mod));

        }

    }
}

