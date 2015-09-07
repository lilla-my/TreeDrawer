using System;
using NUnit.Framework;
using Algorithm;
using Algorithm.Buchheim;

namespace Algorithms.Tests
{
	[TestFixture]
	public class BuchheimTests
	{
		[Test]
		public void Should_calculate_nodes_positions_avoiding_collisions ()
		{
			var node = new TreeProducer ().CreateFromPlain ();
			var algorithm = new BuchheimAlgorithm ();
			algorithm.Initialize (node);

			algorithm.Execute ();
		
			algorithm.ApplyCalculatedCoordinates ();

			var validator = new TreeValidator (node);
			validator.Validate ();
			Assert.True (validator.IsValid, "After execution tree should have valid coordinates applied to nodes");
		}

		[Test]
		public void Should_be_correctly_constructed ()
		{
			var algorithm = new BuchheimAlgorithm ();
			Assert.False (algorithm.IsInitialized, "Algorithm should not be yet Initialized");
			Assert.False (algorithm.IsExecuted, "Algorithm should not be yet Executed");
			Assert.False (algorithm.IsResultApplied, "Algorithm should not have yet ResultApplied");
		}

		[Test]
		public void Should_be_correctly_initialized ()
		{
			var node = new TreeProducer ().CreateFromPlain ();
			var algorithm = new BuchheimAlgorithm ();
			algorithm.Initialize (node);
			Assert.True (algorithm.IsInitialized, "Algorithm should be Initialized");
			Assert.False (algorithm.IsExecuted, "Algorithm should not be yet Executed");
			Assert.False (algorithm.IsResultApplied, "Algorithm should not have yet ResultApplied");
		}


		[Test]
		public void Should_be_correctly_executed ()
		{
			var node = new TreeProducer ().CreateFromPlain ();
			var algorithm = new BuchheimAlgorithm ();
			algorithm.Initialize (node);
			algorithm.Execute ();
			Assert.True (algorithm.IsInitialized, "Algorithm should be Initialized");
			Assert.True (algorithm.IsExecuted, "Algorithm should be Executed");
			Assert.False (algorithm.IsResultApplied, "Algorithm should not have yet ResultApplied");
		}

		[Test]
		public void Should_have_result_correctly_applied ()
		{
			var node = new TreeProducer ().CreateFromPlain ();
			var algorithm = new BuchheimAlgorithm ();
			algorithm.Initialize (node);
			algorithm.Execute ();
			algorithm.ApplyCalculatedCoordinates ();
			Assert.True (algorithm.IsInitialized, "Algorithm should be Initialized");
			Assert.True (algorithm.IsExecuted, "Algorithm should be Executed");
			Assert.True (algorithm.IsResultApplied, "Algorithm should have now ResultApplied");
		}

		[Test]
		public void Should_be_correctly_reinitialized ()
		{
			var node = new TreeProducer ().CreateFromPlain ();
			var algorithm = new BuchheimAlgorithm ();
			algorithm.Initialize (node);
			algorithm.Execute ();
			algorithm.ApplyCalculatedCoordinates ();
			algorithm.Initialize (node);

			Assert.True (algorithm.IsInitialized, "Algorithm should be Initialized");
			Assert.False (algorithm.IsExecuted, "Algorithm should not be yet Executed");
			Assert.False (algorithm.IsResultApplied, "Algorithm should not have yet ResultApplied");
		}

		[Test]
		public void Should_throw_on_false_workflow_attempt ()
		{
			var node = new TreeProducer ().CreateFromPlain ();
			var algorithm = new BuchheimAlgorithm ();

			Assert.Throws (typeof(InvalidOperationException), () => algorithm.Execute ());
			Assert.Throws (typeof(InvalidOperationException), () => algorithm.ApplyCalculatedCoordinates ());

			algorithm.Initialize (node);
			Assert.Throws (typeof(InvalidOperationException), () => algorithm.ApplyCalculatedCoordinates ());

			algorithm.Execute ();
			algorithm.ApplyCalculatedCoordinates ();
		}
	}
}

