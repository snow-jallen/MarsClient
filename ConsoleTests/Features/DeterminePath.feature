Feature: Determine Path

Given a starting location and a target with difficulty scores, generate a path

Scenario: Starting below and left of target
	Given the following map
	| * | * | * | * |
	| 2 | 2 | 2 | T |
	| 2 | 2 | 1 | 2 |
	| 2 | 1 | 2 | 2 |
	| P | 2 | 2 | 2 |
	When I determine the path I want to take
	Then my path should be as follows
	| * | * | * | * |
	| 2 | 2 | 2 | T |
	| 2 | 2 | 1 | 2 |
	| 2 | 1 | 2 | 2 |
	| P | 2 | 2 | 2 |

