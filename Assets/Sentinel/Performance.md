*Dev notes; pending review*

# Performance and optimization

## Current work

This section details current planning overheads, and where optimization work is headed (updated whenever substantial optimizations are performed).
(Overheads don't add up to 100% because player loop is slacking)

```
- 19% Planning actions [FOCUS]
- 14% Insert node (inc. hashing 7%)
-  9% Clone state
```

Stats from running headless (no rendering) on a low voltage mobile processor (fka 'Core M'), solving for the blue sentinel.

Plans per second: 30
Plan length: 33 steps
Iterations: 790

## Past optimizations (most recent first)

If you are looking for inspiration on how to optimize your solver, the recommendation is to start at the *bottom* of this list. Later optimizations tend to be costlier, and more specific.

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
