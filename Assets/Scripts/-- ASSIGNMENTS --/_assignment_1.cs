using UnityEngine;
namespace FGMath
{

static class Assignment1
{
    /*
                                ----------------------
                                    ASSIGNMENT ONE
                                ----------------------

    We want to show the player's hand in the middle-bottom of the screen. Each card needs
    to know where it should move to on the screen. We also need to hilight the cards when
    the player puts the mouse over them by moving the cards slightly.

            CONSTRAINTS:

        1. The cards need to be laid out from left to right.
        2. The root position for the hand must be in the center of the cards.
        3. The cards shouldn't overlap too much, all text should be readable.
        4. The value must be returned as a PseudoTransform.

    Easy Mode = The cards should be arranged in a straight line.
    Hard Mode = The cards should be arranged in a fan, with a slight rotation.
    */


    // This is the input struct that will be handed to you, and it contains all the data
    // you need to solve the problem.
    public struct Input
    {
        // The position of the parent transform, and is in the middle of the screen.
        public Vector3 handPosition;

        // The index of the card in the hand. Cards are numbered left to right, starting at 0.
        public int cardIdx;

        // That total number of cards in the hand.
        public int cardCount;

        // The width and height of the cards, for calculating how far to offset their positions by.
        public Vector2 cardDimensions;

        // How much we should offset the card by when the player's mouse is over it.
        public Vector3 selectedOffset;

        // The rotation of the hand. 
        public Quaternion handRotation;
    }
    
    //
    //  Feel free to add your own members to the class if you think they might be helpful, but
    //  please try to keep everything inside the assignment_1.cs file so it's easier for me to
    //  read! (That's a request by the way, not a requirement. ;) )
    //

		private static Quaternion GetRotFromIdx(int index, int count)
		{
				var centerOfCount = count % 2 == 0 ? count / 2 -.5f : count / 2;
				var centeredIdx = index - centerOfCount;
				var rotScalarInDeg = 12;
				var y = centeredIdx * rotScalarInDeg;
				return Quaternion.Euler(new Vector3( -30, y, 0 ));
		}
		
		private static Vector3 GetPosFromRot(int index, Quaternion rotation)
		{
				var cardForward = rotation * Vector3.forward;
				var distance = 12f;
				var heightOffset = .05f;
				return cardForward * distance - Vector3.forward * distance + Vector3.up * heightOffset * index;
		}

    /// Find the position that a specific card index should move towards, based on the inputs.
    public static PseudoTransform GetCardPosition(Input input, bool isHovered)
    {
        PseudoTransform retVal;

				var rotation = GetRotFromIdx(input.cardIdx, input.cardCount);
				var position = GetPosFromRot(input.cardIdx, rotation);

				retVal.pos = isHovered ? position + input.selectedOffset : position;
				retVal.rot = rotation;
        retVal.scale = Vector3.one;

        return retVal;
    }
}
}
