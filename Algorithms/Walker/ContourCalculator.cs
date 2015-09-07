using System;
using System.Collections.Generic;

namespace Algorithm.Walker
{
    /// <summary>
    /// Straightforward way to calculate contours, 
    /// just collecting positions on each level to left or to right from the current node.
    /// </summary>
	class ContourCalculator
	{
        /// <summary>
        /// Collects left contour of node. 
        /// </summary>
        /// <param name="node">Current node.</param>
        /// <param name="modSum">Keep track of Mod along the way.</param>
        /// <param name="values">Here contour coordinates to be stored.</param>
        public void GetLeft (IWalkerNode node, float modSum, Dictionary<int, float> values)
		{
			if (!values.ContainsKey (node.Y))
				values.Add (node.Y, node.X + modSum);
			else
				values [node.Y] = Math.Min (values [node.Y], node.X + modSum);

			modSum += node.Mod;
			foreach (var child in node.Children) {
				GetLeft (child, modSum, values);
			}
		}

        /// <summary>
        /// Collects right contour of node. 
        /// </summary>
        /// <param name="node">Current node.</param>
        /// <param name="modSum">Keep track of Mod along the way.</param>
        /// <param name="values">Here contour coordinates to be stored.</param>
        public void GetRight (IWalkerNode node, float modSum, Dictionary<int, float> values)
		{
			if (!values.ContainsKey (node.Y)) {
				values.Add (node.Y, node.X + modSum);
			} else {
				values [node.Y] = Math.Max (values [node.Y], node.X + modSum);
			}

			modSum += node.Mod;
			foreach (var child in node.Children) {
				GetRight (child, modSum, values);
			}
		}
	}
}