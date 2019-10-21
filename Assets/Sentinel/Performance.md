*Dev notes; pending review*

# Performance and optimization

## Speeding up ground model comparisons

Initially the ground model encoded both props and tiles. This caused large hashing overheads since the solver needs to check the map for equality whenever hashing.
This was improved by separating the props (what can be moved) from the map itself (static).

## Speeding up the clone method

With comparisons out of the way (for now), cloning represents almost 100% of processing overheads. So we'd like to provide our own. This works like a charm. Cloning via serialization may be slow, but another advantage when cloning manually is that we don't need a deep clone - if we already know that certain data isn't going to change, we just forward references.

Speeding up the clone method relegated cloning to ~ 1% of processing overheads.

## Back to hashing and equality

At this point the ground model comparison is 50% of planning cost, courtesy `SequenceEqual`. Can this be improved?

- Use arrays, not lists.
- If we know the type, we don't need a generic equality comparer
- Avoid pattern matching (may trigger GC)

Tweaked hashcodes too.

## A ghost in the closet

We could continue with petty optimizations and get linear improvements but most planners have a common problem: the more variables in your model, the bigger the state space.

In this case we introduced the agent's orientation as a variable. Though represented as a vector this encodes 4 states. So we're making our search space 4 times bigger. But for what?

We only care about orientation when there's a block to pull. In most cases, then, orientation could be ignored.

This is handled as follows:
- After a move (NSEW), check surrounding tiles.
- If no prop is found, zero the direction vector.

Paradoxically, we've added a state to the 'orientation' variable; but because this is a 'ground state' it can be used to dedup locations in the state space.

NOTE: pending quantitative results on how this impacted the search
