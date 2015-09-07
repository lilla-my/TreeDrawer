# TreeDrawer
Library implements the algorithm described in http://dirk.jivas.de/papers/buchheim02improving.pdf with some adjustments (BuchheimAlgorithm.cs):
 - Shift of `<minusI.Mod>` and `<minus0.Mod>` after MoveSubtree (in `<Apportion>` method) was not needed.
 - in FirstWalk `<Preliminary>` should be also adjusted by `<midpoint>` and not `<distance>`
 
 Otherway collision were not completely avoided.

There is also varient of Walker's algorithm (WalkerAlgorithm.cs) that is easier to understand but executes not in linear time
