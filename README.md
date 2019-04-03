# YourKingdomFalls
TBS game focused on challenge and providing the player a feeling of disadvantage and unfairness.
Two main goals are:
	1. Provide the player a challenge. Make it more like a puzzle than a strategy game.
	2. Make the player question if the sacrifice is worth it?

Explaination:
	The design is currently based on destroying the enemy unit. 
	This rationale is because we are currently testing it.
	We added a blue knight and brown enemy knight to show the theme of two kingdoms at war.
	We decided it was going to be a medival themed game as well.
	The Physics we included were the flag and the ball which represent control points.
	The turn counter at the bottom comes together to bring about the turn based strategy game.

Justifications:
	* Anas - Player Movement, BFS algorithm. 
		 The player is able to select a tile and then there is a BFS algorithm that guides them to the right location.
		 Character animations FSM.
	* Eric - NPC Movement, A*. 
		 The NPC moves based off of the closest unit and hueristic is Euclidian distance.
	* Alex - Jumping, FSM. 
		 The NPC and player can jump to another level up when higher terrain is selected.
		 Character animations FSM.
	
	     