Feature: CellsInView

Make sure you can see what you're supposed to see in the First Person View
Background:
	Given the following map
	| * | *  | *  | *  | *  |
	| 1 | 7  | 13 | 19 | 25 |
	| 2 | 8  | 14 | 20 | 26 |
	| 3 | 9  | 15 | 21 | 27 |
	| 4 | 10 | 16 | 22 | 28 |
	| 5 | 11 | 17 | 23 | 29 |
	| 6 | 12 | 18 | 24 | 30 |

Scenario: Facing North at bottom middle
	When my ship is at (0,2) facing North
	Then the visible cells are
	| * | *  | *  | *  | *  |
	| 4 | 10 | 16 | 22 | 28 |
	| 5 | 11 | 17 | 23 | 29 |
	| 6 | 12 | 18 | 24 | 30 |

Scenario: Facing East in the middle
	When my ship is at (2,2) facing East
	Then the visible cells are
	| *  | *  | *  | *  | *  |
	| 26 | 27 | 28 | 29 | 30 |
	| 20 | 21 | 22 | 23 | 24 |
	| 14 | 15 | 16 | 17 | 18 |

Scenario: Facing South in the middle
	When my ship is at (2,2) facing South
	Then the visible cells are
	| *  | *  | *  | *  | *  |
	| 30 | 24 | 18 | 12 | 6  |
	| 29 | 23 | 17 | 11 | 5  |
	| 28 | 22 | 16 | 10 | 4  |

Scenario: Facing West in the middle
	When my ship is at (2,2) facing West
	Then the visible cells are
	| *  | *  | *  | *  | *  |
	| 6  | 5  | 4  | 3  | 2  |
	| 12 | 11 | 10 | 9  | 8  |
	| 18 | 17 | 16 | 15 | 14 |

Scenario: Facing North at origin
	When my ship is at (0,0) facing North
	Then the visible cells are
	| *    | *    | * | *  | *  |
	| null | null | 4 | 10 | 16 |
	| null | null | 5 | 11 | 17 |
	| null | null | 6 | 12 | 18 |

Scenario: Facing East at origin
	When my ship is at (0,0) facing East
	Then the visible cells are
	| *  | *  | *  | *    | *    |
	| 16 | 17 | 18 | null | null |
	| 10 | 11 | 12 | null | null |
	| 4  | 5  | 6  | null | null |

Scenario: Facing South at origin
	When my ship is at (0,0) facing South
	Then the visible cells are
	| *    | *    | *    | *    | *    |
	| null | null | null | null | null |
	| null | null | null | null | null |
	| 18   | 12   | 6    | null | null |

Scenario: Facing West at origin
	When my ship is at (0,0) facing West
	Then the visible cells are
	| *    | *    | *    | *    | *    |
	| null | null | null | null | null |
	| null | null | null | null | null |
	| null | null | 6    | 5    | 4    |
