namespace Algorithm
{
	public class TreeProducer
	{
		public INodeStructure CreateFromPlain ()
		{
			var root = new NodeStructure<string> (null, "O");
			var child11 = root.AddChild ("E");
			var child12 = root.AddChild ("F");
			var child13 = root.AddChild ("N");

			var child21 = child11.AddChild ("A");
			var child22 = child11.AddChild ("D");


			child22.AddChild ("G");
			child22.AddChild ("K");
			child22.AddChild ("L");
			child22.AddChild ("M");
			child22.AddChild ("N");

			var child23 = child13.AddChild ("G");
			var child24 = child13.AddChild ("M");

			var child25 = child12.AddChild ("B");
			var child26 = child12.AddChild ("C");



			child26.AddChild ("H1");
			child26.AddChild ("I1");
			child26.AddChild ("J1");
			child26.AddChild ("K1");
			child26.AddChild ("L1");

			child26.AddChild ("H1");
			child26.AddChild ("I1");
			child26.AddChild ("J1");
			child26.AddChild ("K1");
			child26.AddChild ("L1");

			var child31 = child24.AddChild ("H");
			var child32 = child24.AddChild ("I");
			var child33 = child24.AddChild ("J");
			var child34 = child24.AddChild ("K");
			var child35 = child24.AddChild ("L");

			return root;
		}
	}
}