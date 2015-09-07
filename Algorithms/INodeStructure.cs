using System.Collections.Generic;

namespace Algorithm
{
	public interface INodeStructure
	{
		List<INodeStructure> Children { get; }

		INodeStructure Parent { get; }

		int Order { get; }

		bool IsLeaf { get; }

		Position2D Point { get; set; }
	}
}
