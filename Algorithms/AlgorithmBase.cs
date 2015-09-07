using System;

namespace Algorithm
{
	public abstract class AlgorithmBase : IAlgorithm
	{
        /// <summary>
        /// Property to show whether algorithm's been already initialized.
        /// </summary>
		public bool IsInitialized { get; private set; }

        /// <summary>
        /// Property to show whether algorithm's been already executed.
        /// </summary>
		public bool IsExecuted { get ; private set; }

        /// <summary>
        /// Property to show whether algorithm's results were already applied to inner structure.
        /// </summary>
		public bool IsResultApplied { get; private set; }

	    protected AlgorithmBase ()
		{
			IsInitialized = false;
			IsExecuted = false;
			IsResultApplied = false;
		}

		public void Initialize (INodeStructure root)
		{
			InitializeImpl (root);
			IsInitialized = true;
			IsExecuted = false;
			IsResultApplied = false;
		}

		public void Execute ()
		{
			if (!IsInitialized) {
				throw new InvalidOperationException ("Algorithm should be first initialized");
			}
			ExecuteImpl ();
			IsExecuted = true;
			IsResultApplied = false;
		}

		public void ApplyCalculatedCoordinates ()
		{
			if (!IsExecuted) {
				throw new InvalidOperationException ("Algorithm should be first executed");
			}

			ApplyCalculatedCoordinatesImpl ();
			IsResultApplied = true;
		}

        /// <summary>
        /// Actual implementation of preparation work for algorithm.
        /// </summary>
        /// <param name="root">INodeStructure root.</param>
		protected abstract void InitializeImpl (INodeStructure root);

        /// <summary>
        /// Implementation of algorithm.
        /// </summary>
		protected abstract void ExecuteImpl ();

        /// <summary>
        /// Implementation of applying results to INodeStructure.
        /// </summary>
		protected abstract void ApplyCalculatedCoordinatesImpl ();
	}
}
