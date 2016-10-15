using UnityEngine;
using System.Collections;

namespace Examples.BT
{
	public class Example1 : CoroutineTree
	{
        // Override public property, update frequency of the behavioral tree
        // Updates every second
		public override float Frequency
		{
			get
			{
				return 1.0f;
			}
		}


		void Start ()
		{
			StartTree (ExecutionMode.SingleRun); // We start the tree, running it only once and then stopping
		}

        // We override this function to specify the whole tree coroutine
		protected override IEnumerator Root ()
		{
			return Sequence (       // Sequence: if all elements succeed, the sequence succeed
				LogAction ("A"),    // Log actions always succeed
				Sequence (
					LogAction ("AA"),
					LogAction ("AB"),
					LogAction ("AC")
				),
				CountAction (3),
				Selector ( // Selector: each child is executed in a sequence until one succeeds
					PassAction (Failure),
					PassAction (Success), // This selector should stop here: first child to return a successful status
					PassAction (Failure)
				)
			);
		}

        // This action prints on the console
		IEnumerator LogAction (string message)
		{
			yield return null; // No initialization of NodeData, this action does not need to run for more frames.

			Debug.Log (message);

			yield return Success;
		}


        // This action just returns the result directly, hence "pass"
		IEnumerator PassAction (ResultType result)
		{
            yield return null; // No initialization of NodeData, this action does not need to run for more frames.

			Debug.Log (result);

			yield return result;
		}


        // Custom action: will count for count iterations of the tree
		IEnumerator CountAction (int count)
		{
			// Init

			Debug.Log ("Init counter");

			int current = 0;

            // This is an object we give back to our behavior tree so it can interrupt us if needed
			NodeData data = new NodeData ();
			yield return data;

			// Run
			
			while (!data.ShouldReset && ++current <= count)
			{
				Debug.Log (current);

				yield return Running; // Return running while we are counting
			}

			yield return Success; // This state is done!
		}
	}
}
