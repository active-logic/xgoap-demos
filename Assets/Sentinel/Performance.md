*Dev notes; pending review*

# Performance and optimization

Optimization on the demo is an ongoing process; roughly, planning overheads are divided between:

- Effecting planning actions
- Hashing (insert nodes in 'visited' set)
- Cloning (duplicate model state)

The following is an approximation (reflects a Geekbench 4 single score of ~4000)

- Plans per second: 50 (20ms)
- Plan length: 33 steps
- Iterations: 733

## Past optimizations (most recent first)

If you are looking for inspiration on how to optimize your solver, the recommendation is to start at the *bottom* of this list. Later optimizations tend to be costlier, and more specific.

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
