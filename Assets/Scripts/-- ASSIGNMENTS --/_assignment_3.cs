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

    public static PseudoTransform GetGridCellPosition(Input input)
    {
        PseudoTransform retVal;

				// I know it said fixed grid, but I tried solving it so the fixed grid can be scaled
				// or get updated to another size. Don't know if that was intended or not.
				// Hopefully the code is readable, and hopefully it does add margin and cenering
				// dynamically as I set out to achive. And yes, I have a passion for ternary operators

				// gets max amount of cards per row and column
				var cardsPerRow = Mathf.Floor(input.playAreaDimensions.x / input.cardDimensions.x);
				var cardsPerCol = Mathf.Floor(input.playAreaDimensions.y / input.cardDimensions.y);

				// calculates margin left to work with, the remainder of the play area
				var marginSurplus = Vector2.zero;
				marginSurplus.x = (input.playAreaDimensions.x % input.cardDimensions.x);
				marginSurplus.y = (input.playAreaDimensions.y % input.cardDimensions.y);

				// how thick the horizontal and vertical margin can be
				var marginAround = Vector2.zero;
				marginAround.x = marginSurplus.x / (cardsPerRow + 1); 
				marginAround.y = marginSurplus.y / (cardsPerCol + 1); 

				// gets row and column from cell index
				var rowIdx = Mathf.Floor(input.gridCellIdx / cardsPerRow);
				var colIdx = input.gridCellIdx - (cardsPerRow * rowIdx);

				// local grid position
				var grid = Vector2.zero;
				grid.x = input.gridCellIdx < cardsPerRow ? input.gridCellIdx * input.cardDimensions.x : (input.gridCellIdx - cardsPerRow) * input.cardDimensions.x;
				grid.y = input.gridCellIdx < cardsPerRow ? input.cardDimensions.y : 0;

				// offset to center grid
				var offset = Vector2.zero;
				offset.x = cardsPerRow % 2 == 0 ? ((cardsPerRow - 2) / 2) * input.cardDimensions.x - input.cardDimensions.x / 2 : ((cardsPerRow - 1) / 2) * input.cardDimensions.x;
				offset.y = cardsPerCol % 2 == 0 ? ((cardsPerCol - 2) / 2) * input.cardDimensions.y - input.cardDimensions.y / 2 : ((cardsPerCol - 1) / 2) * input.cardDimensions.y;

				// margin around the cards to fill the play area completely
				var margin = Vector2.zero;
				margin.x = colIdx * marginAround.x - marginAround.x; 
				margin.y = rowIdx * marginAround.y + marginAround.y;

				// adds the grid, the offset, and the margin together to represent the final position of the card
				var position = new Vector3(grid.x - offset.x + margin.x, 0, grid.y - offset.y - margin.y); 

        retVal.pos = position;
				retVal.rot = Quaternion.identity;
        retVal.scale = Vector3.one;

        return retVal;
    }
}
}
