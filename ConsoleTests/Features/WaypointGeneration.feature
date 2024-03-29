﻿Feature: Waypoint Generator

Given a starting location and a target, build a list of waypoints

Scenario: Starting below and left of target
	Given the following map where every cell is 10 cells
	| * | * | * | * | * | * | * | * | * | * |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   | T |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	| S |   |   |   |   |   |   |   |   |   |
	When I generate the waypoints
	Then the waypoints should be as follows
	| *  | * | * | * | *  | * | * | * | * | * |
	|    |   |   |   |    |   |   |   |   |   |
	|    |   |   |   |    |   |   |   |   |   |
	|    |   |   |   |    |   |   |   |   |   |
	| 12 |   |   |   | 13 |   |   |   |   |   |
	| 11 |   |   |   | 10 |   |   |   |   |   |
	| 8  |   |   |   | 9  |   |   |   |   |   |
	| 7  |   |   |   | 6  |   |   |   |   |   |
	| 4  |   |   |   | 5  |   |   |   |   |   |
	| 3  |   |   |   | 2  |   |   |   |   |   |
	| S  |   |   |   | 1  |   |   |   |   |   |

Scenario: Starting below and right of target
	Given the following map where every cell is 10 cells
	| * | * | * | * | * | * | * | * | * | * |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   | T |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   | S |   |   |
	When I generate the waypoints
	Then the waypoints should be as follows
	| * | * | * | * | * | * | * | * | * | * |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   | 6 |   |   |   |   | 7 |   |   |
	|   |   | 5 |   |   |   |   | 4 |   |   |
	|   |   | 2 |   |   |   |   | 3 |   |   |
	|   |   | 1 |   |   |   |   | S |   |   |

Scenario: Starting above and right of target
	Given the following map where every cell is 10 cells
	| * | * | * | * | * | * | * | * | * | * |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   | S |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   | T |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	When I generate the waypoints
	Then the waypoints should be as follows
	| * | * | * | * | * | * | * | * | * | * |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   | 1 |   |   |   |   | S |   |   |
	|   |   | 2 |   |   |   |   | 3 |   |   |
	|   |   | 5 |   |   |   |   | 4 |   |   |
	|   |   | 6 |   |   |   |   | 7 |   |   |
	|   |   | 9 |   |   |   |   | 8 |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |

Scenario: Starting above and left of target
	Given the following map where every cell is 10 cells
	| * | * | * | * | * | * | * | * | * | * |
	|   |   | S |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   | T |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	|   |   |   |   |   |   |   |   |   |   |
	When I generate the waypoints
	Then the waypoints should be as follows
	| * | * | *  | * | * | * | *  | * | * | * |
	|   |   | S  |   |   |   | 1  |   |   |   |
	|   |   | 3  |   |   |   | 2  |   |   |   |
	|   |   | 4  |   |   |   | 5  |   |   |   |
	|   |   | 7  |   |   |   | 6  |   |   |   |
	|   |   | 8  |   |   |   | 9  |   |   |   |
	|   |   | 11 |   |   |   | 10 |   |   |   |
	|   |   | 12 |   |   |   | 13 |   |   |   |
	|   |   |    |   |   |   |    |   |   |   |
	|   |   |    |   |   |   |    |   |   |   |
	|   |   |    |   |   |   |    |   |   |   |
