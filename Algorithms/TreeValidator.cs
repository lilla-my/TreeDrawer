using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm
{
	public class TreeValidator
	{
		private INodeStructure treeRoot;

		public bool IsValid { get; private set; }

		public TreeValidator (INodeStructure root)
		{
			treeRoot = root;
			IsValid = false;
		}

		public void Validate ()
		{
			var coordinatesByLevels = new Dictionary<int, List<float>> ();

			FulfillRecursively (treeRoot, coordinatesByLevels);
			IsValid = true;
			foreach (var coordinates in coordinatesByLevels.Values) {

				// all coordinates were collected from left to right in FulfillRecursively
				// if <coordinates> is not sorted then we have collision in tree on this level
				if (!IsSorted (coordinates)) {
					IsValid = false;
					return;
				}
			}
		}

		private void FulfillRecursively (INodeStructure node, Dictionary<int, List<float>> dictionary)
		{
			var level = (int)node.Point.Y;
			if (dictionary.ContainsKey (level)) {
				dictionary [level].Add (node.Point.X);
			} else {
				dictionary.Add (level, new List<float>{ node.Point.X });
			}

			node.Children.ForEach (child => FulfillRecursively (child, dictionary));
		}

		private bool IsSorted<T> (IEnumerable<T> list) where T:IComparable<T>
		{
			var y = list.First ();
			return list.Skip (1).All (x => {
				bool b = y.CompareTo (x) < 0;
				y = x;
				return b;
			});
		}
	}
}

