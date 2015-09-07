namespace Algorithm
{
	public interface IAlgorithm
	{
		bool IsInitialized { get; }

		bool IsExecuted { get; }

		bool IsResultApplied { get; }

		
		/// <summary>
		/// Initializes algorithm using specified node as root.
		/// </summary>
		/// <param name="root">Root.</param>
		void Initialize (INodeStructure root);

		/// <summary>
		/// Executes the main calculation for locating nodes in tree.
		/// </summary>
		void Execute ();

		/// <summary>
		/// Applies the calculated coordinates to INodestructure.Point.
		/// </summary>
		void ApplyCalculatedCoordinates ();

	}
}
