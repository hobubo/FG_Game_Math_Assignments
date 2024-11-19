using UnityEngine;
namespace FGMath
{

    static class Assignment2
    {

    /*
                                ----------------------
                                    ASSIGNMENT TWO
                                ----------------------

    When the turn ends, we want to smoothly move all the cards in play over to the discard
    pile, and have them disappear "into" it.

            CONSTRAINTS:

        1. The cards should move smoothly towards the discard position based on t.
        2. The cards should rotate towards the target rotation based on t.
        3. The cards should shrink down from their starting scale to zero based on t.

    Easy Mode = The cards should move in a linear fashion, at constant speed.
    Hard Mode = The cards should follow a non-linear fashion, either in speed, path, or both.
    */


    //
    //  Feel free to add your own members to the class if you think they might be helpful, but
    //  please try to keep everything inside the assignment_1.cs file so it's easier for me to
    //  read! (That's a request by the way, not a requirement. ;) )
    //


    // This is the input struct that will be handed to you, and it contains all the data
    // you need to solve the problem.
    public struct Input
    {

        // Where the cards started from.
        public PseudoTransform startingPosition;

        // Where the cards will end their journey and disappear.
        public PseudoTransform discardPosition;

        // A value [0:1] representing what ratio of the move should be completed.
        public float t;
    }

		private static float easeInCubic(float t)
		{
			return t * t * t;
		}

		private static float easeOutQuint(float t) 
		{
			return 1 - Mathf.Pow(1 - t, 5);
		}

		private static float easeInOutBack(float t) 
		{
			float c1 = 1.70158f;
			float c2 = c1 * 1.525f;

			return t < 0.5
				? ((2 * t) * (2 * t) * ((c2 + 1) * 2 * t - c2)) / 2
				: ((2 * t - 2) * (2 * t - 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
		}

    public static PseudoTransform MoveToDiscard(Input input)
    {
        PseudoTransform retVal;

				var flippedRotation = Quaternion.Euler(new Vector3(0,0,180));

        retVal.pos = Vector3.Lerp(input.startingPosition.pos, input.discardPosition.pos, easeInOutBack(input.t));
        retVal.rot = Quaternion.Lerp(input.startingPosition.rot, flippedRotation, easeOutQuint(input.t));
        retVal.scale = Vector3.Lerp(Vector3.one, Vector3.zero, easeInCubic(input.t));

        return retVal;
    }
}
}
