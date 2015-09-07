namespace Algorithm.Buchheim
{
    /// <summary>
    /// Gets contours for current Node.
    /// </summary>
	public class EdgeWalker
	{
		public IBuchheimNode CurrentNode { get; private set; }

		public float ModSum { get; private set; }

        /// <summary>
        /// Helper property used in Apportion.
        /// </summary>
		public float Offset => CurrentNode?.Preliminary + ModSum ?? 0f;

        public bool Finished => CurrentNode == null;

        public EdgeWalker (IBuchheimNode node)
		{
			CurrentNode = node;
			ModSum = node?.Mod ?? 0f;
		}

        /// <summary>
        /// Set new ancestor.
        /// </summary>
        /// <param name="ancestor"></param>
		public void SetAncestor (IBuchheimNode ancestor)
		{
			if (!Finished) {
				CurrentNode.Ancestor = ancestor;
			}
		}

        /// <summary>
        /// Set new Thread for node.
        /// </summary>
        /// <param name="walker"></param>
		public void SetThreadAndUpdateMod (EdgeWalker walker)
		{
			if (!Finished) {
				CurrentNode.Thread = walker.CurrentNode;
				CurrentNode.Mod += walker.ModSum - ModSum;
			}
		}

        /// <summary>
        /// Procedure described in article for Left Subforest (Sect. 3).
        /// </summary>
		public void MoveLeft ()
		{
			if (!Finished) {
				CurrentNode = CurrentNode.GetNextLeft ();
			}
		}

        /// <summary>
        /// Procedure described in article for Right Subforest (Sect.3).
        /// </summary>
		public void MoveRight ()
		{
			if (!Finished) {
				CurrentNode = CurrentNode.GetNextRight ();
			}
		}

		public void UpdateSum ()
		{
			if (!Finished) {
				ModSum += CurrentNode.Mod;
			}
		}

        /// <summary>
        /// Checks whether any moves left are still possible.
        /// </summary>
		public bool IsTheLastOnLeft ()
		{
			return (CurrentNode?.GetNextLeft () == null);
		}

        /// <summary>
        /// Checks whether any moves right are still possible.
        /// </summary>
        public bool IsTheLastOnRight ()
		{
			return (CurrentNode?.GetNextRight () == null);
		}
	}
}