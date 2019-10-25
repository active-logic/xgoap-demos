*Dev notes; pending review*

# Performance and optimization

Optimization on the demo is an ongoing process; roughly, planning overheads are divided between:

- Effecting planning actions
- Hashing (track visited nodes and sort open list)
- Cloning (duplicate model state)

The following is an approximation (reflects a Geekbench 4 single score of ~4000)

- Plans per second: 105 (~20ms)
- Plan length: 33 steps
- Iterations: 733

Currently looking at making this 2 to 2.5 times faster:
- Cloning can be made faster by pooling. Already tested, saves ~80%
- Hashing can be improved by using a custom structure (vs a HashSet, for visited nodes) and improving how the open list is sorted; currently uses a heuristic that says new nodes should add towards the top, but still traverses, on average a big 50 nodes per iteration.
- Planning actions are 20% of current overheads, path to further optimizations unclear.

## Past optimizations (most recent first)

If you are looking for inspiration on how to optimize your solver, the recommendation is to start at the *bottom* of this list. Later optimizations tend to be costlier, and more specific.

## 7. Relax cost ordering

Less precise cost ordering makes inserting into the open list faster. Overall, makes headless test twice as fast; benefits less startling in the demo itself.

## 6. Fixed hash implementations

Got a lot more from `HashSet` with a half decent hash function.

## 5. Checking less often for nearby props

As part of effecting moves, we now check for nearby props.
But we don't actually care about nearby props, or the agent's orientation. Very specifically we only need to know whether a prop can be pulled.
Refactoring reflects this. Also cleaned up redundant numerical conversions.

## 4. Trim the state space

In this case the state space was inflated because the agent's NSEW orientation was taken into account (x4).
Only caring about orientation when there's a block to pull. In most cases, then, orientation can be replaced by a ground value of (0, 0)

Handled as follows:
- After a move, check surrounding tiles.
- If no prop is found, zero the orientation vector.

[PENDING quantitative result]

## 3. Faster hashing

- Store data in arrays (vs lists) when possible.
- Avoid generic equality comparers
- Avoid pattern matching (somehow this fairly intensive allocations)
- Tweak hashcodes

## 2. Speed up the clone method

The default is deep clone via serialization. This is helpful when getting started, and to ensure no clone related errors are introduced (beware - 'rogue clones' introduce subtle bugs which may affect your mental health).

At this point cloning accounted for ~100% of planning overheads. Got this down to ~1%.

## 1. Exclude planning-static states from equals comparisons

Comparing state is used in hashing since we don't want to revisit previously entered states.
In the sentinel demo, map state is mostly static so I separated ground state (never changes) from prop states (props can be moved)
