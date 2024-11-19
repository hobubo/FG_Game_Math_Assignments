using UnityEngine;
namespace FGMath
{

    static class Assignment3
    {

    /*
                               --------------------------
                                    ASSIGNMENT THREE
                               --------------------------

    When a card is played, it needs to be moved into the play area, where it will be placed
    onto a fixed grid. The grid should be filled up from left to right, top to bottom.
    Given the width and height of the cards, and the width and height of the play area,
    and given the index of a specific grid slot, return the position that a card should
    move to in order to occupy that grid slot.

        1. Cards can't overlap
        2. Cards can't go off-screen to the right.
        3. Use a scale of 1 and a rotation of Quaternion.Identity.


    EASY MODE: Place the cards starting from the top left of the play area.
    HARD MODE: Add margins to the play area such that the grid will be centered when full.

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
        // The position in the grid, with 0 being top-left.
        public int gridCellIdx;

        // The width of the play area.
        public Vector2 playAreaDimensions;

        // The middle of the play area in world space.
        public Vector3 playAreaCenterPosition;

        // The width and height of the cards, for calculating how far to offset their positions by.
        public Vector2 cardDimensions;

    }

		private static Vector3 getGridPosition(int idx, Vector2 card, Vector2 area)
		{
				var cardsPerRow = Mathf.Floor(area.x / card.x);
				var cardsPerCol = Mathf.Floor(area.y / card.y);

				var rowIdx = Mathf.Floor(idx / cardsPerRow);
				var colIdx = idx - (cardsPerRow * rowIdx);

				var gridUnit = new Vector2(colIdx - (cardsPerRow - 1) / 2, - rowIdx - (cardsPerCol - 1) / 2);
				var gridPosition = new Vector3(gridUnit.x * card.x, 0, gridUnit.y * card.y);

				return gridPosition;
		}


    public static PseudoTransform GetGridCellPosition(Input input)
    {
        PseudoTransform retVal;
				
				var origin = input.playAreaCenterPosition;

				var gridPosition = getGridPosition(input.gridCellIdx, input.cardDimensions, input.playAreaDimensions); 

				var worldPosition = origin + (gridPosition.x * Vector3.right + gridPosition.z * Vector3.forward);

        retVal.pos = worldPosition;
				retVal.rot = Quaternion.identity;
        retVal.scale = Vector3.one;

        return retVal;
    }
}
}
