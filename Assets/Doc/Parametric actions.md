# Designing and implementing parametric actions

## Non parametric actions

In the current implementation we iterate the range of actions, then for each occurence:
- Clone the state
- Effect the action and test its return value. A small twist here is that we really need to call the action onto the object, since actions are represented as delegates.
- A return value of false indicates the action either is projected to fail, or not available so we skip to the next action.
- Next we validate the action. This is to weed out cases where an action has zero or a negative cost, which is not allowed except with breadth first search.
- Get the name of the action
- Insert a node characterized by:
    - The name of the action, later used as handle to effect the action.
    - The mutated model ('agent')
    - The previous node

# Parametric actions

Parametric actions are implemented in the same way except, instead of the action being described as a string, it is described as an arbitrary object. Although we can generify over the type of this object, this is not convenient so I'll leave this out for now.
