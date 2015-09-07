using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Algorithm;
using Algorithm.Walker;

namespace DrawTreeSample
{
	public partial class DrawTreeForm : Form
	{
		private const int NodeHeight = 30;
		private const int NodeWidth = 30;
		private const int NodeMarginX = 50;
		private const int NodeMarginY = 40;
		private readonly INodeStructure m_node;
		private static readonly Pen NodePen = Pens.Gray;

		public DrawTreeForm ()
		{
			InitializeComponent ();

			var generator = new TreeProducer ();
			m_node = generator.CreateFromPlain ();

			IAlgorithm algorithm = new WalkerAlgorithm ();
			algorithm.Initialize (m_node);
			algorithm.Execute ();
			algorithm.ApplyCalculatedCoordinates ();


			SetControlSize ();

			DoubleBuffered = true;
			treePanel.Paint += treePanel_Paint;
		}

		#region Custom Painting

		private void treePanel_Paint (object sender, PaintEventArgs e)
		{
			e.Graphics.Clear (Color.White);
			DrawNode (m_node, e.Graphics);
		}

		private void SetControlSize ()
		{
            var treeWidth = 200;
			var treeHeight = 100;
			treePanel.Size = new Size (
				Convert.ToInt32 ((treeWidth * NodeWidth) + ((treeWidth + 1) * NodeMarginX)),
				(treeHeight * NodeHeight) + ((treeHeight + 1) * NodeMarginY));
		}

		private void DrawNode (INodeStructure node, Graphics g)
		{
			// rectangle where node will be positioned
		    var x = GetXCoordinate (node.Point.X, 0);
			var nodeRect = new Rectangle ( x, NodeMarginY + ((int)node.Point.Y * (NodeHeight + NodeMarginY))
                , NodeWidth, NodeHeight);

			// draw box
			g.DrawRectangle (NodePen, nodeRect);

			// draw content
			g.DrawString (node.ToString (), Font, Brushes.Black, nodeRect.X + 10, nodeRect.Y + 10);

			// draw line to parent
			if (node.Parent != null) {
				var nodeTopMiddle = new Point (nodeRect.X + (nodeRect.Width / 2), nodeRect.Y);
				g.DrawLine (NodePen, nodeTopMiddle, new Point (nodeTopMiddle.X, nodeTopMiddle.Y - (NodeMarginY / 2)));
			}

			// draw line to children
			if (node.Children.Count > 0) {
				var nodeBottomMiddle = new Point (nodeRect.X + (nodeRect.Width / 2), nodeRect.Y + nodeRect.Height);
				var y = nodeBottomMiddle.Y + (NodeMarginY / 2);
                g.DrawLine (NodePen, nodeBottomMiddle, new Point (nodeBottomMiddle.X, y));
                
				// draw line over children
				if (node.Children.Count > 1)
				{
				    var xStart = GetXCoordinate (node.Children.FirstOrDefault()?.Point.X ?? 0, NodeWidth / 2);
				    var xEnd = GetXCoordinate (node.Children.LastOrDefault()?.Point.X ?? 0, NodeWidth / 2);
                   
				    var childrenLineStart = new Point (xStart, y);
					var childrenLineEnd = new Point (xEnd, y );

					g.DrawLine (NodePen, childrenLineStart, childrenLineEnd);
				}
			}

			foreach (var item in node.Children) {
				DrawNode (item, g);
			}
		}

	    private int GetXCoordinate (float x, int offset)
	    {
            return Convert.ToInt32 (NodeMarginX + (x * (NodeWidth + NodeMarginX)) + offset);
        }

		#endregion
	}
}
