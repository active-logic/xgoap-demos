*Dev notes; pending review*

# Sentinel demo

Let's create a bot which can navigate a terrain and shoot down other bots.

## Implementation overview

In general, a typical GOAP implementation will look like this:

- A controller implements game actions; such as walking, shooting, collecting items and the like. In simple cases this can be just one class.
- Derived from `Activ.GOAP.Agent`, the planning model is implemented as a separate class. You can think of this as a simplified version of the game agent, its actions, and the environment.
- Derived from `Activ.GOAP.GameAI`, the AI itself is responsible for managing planning, and effecting moves.

In Unity3D, both the controller and the AI derive from `MonoBehaviour` - so you can add them to a game object.

## 1. Basic Navigation

### The controller

Initially, the controller, `Sentinel`, implements simple NSWE, kinematic navigation; in other words, move in 4 directions and ignore obstacles.

### The AI

Following a top down approach, starting from the AI is helpful - model requirements are derived from the goals laid out by the game AI.

For basic navigation:
- the *goal* is the target location we're sending the agent to.
- the *heuristic* is the distance from current location to the target location.
- the *actions* are just move left, right, forward, back. As defined by the AI, actions are not implemented here - instead we delegate execution to the controller

### The model

Once we established the AI, the model is straightforward:

- We need to store the current position. This is just to ints (`x`, `y`), exposed as a `Vector2` for calculations.
- We want planning actions matching the client actions defined by the AI.

Additionally, there's a little boiler plate to ensure this model works as intended:

- The model must be *serializable*.
- We implement `Equals` and `GetHashCode`. This is very important otherwise the search underlying the planning process is going to revisit previous states over and over again.
- For convenience we provide a constructor, used to initialize model state from client state.

### Play-test

We add the `Sentinel` and `SentinelAI` components to a placeholder (a cube primitive is enough!) and set the target position to 3, 5; upon switching to play mode, planning starts and the agent promptly starts towards the designated coordinates.

I added a trail renderer; cheap trick but lets us visualize the resulting path.

## 2. Target practice

A this point path-finding doesn't really shine since obstacles are not accounted for. We'll implement shooting a dummy as our next goal, and get back to path-finding later.

We will:
- Add an action to the system
- Modify the goal

First let's add a 'shoot' action to the controller:

```cs
public void Shoot(GameObject target) => Destroy(target);
```

Next, add the shoot action to the AI:

```cs
public void Shoot() => actor.Shoot(nearestTarget);
```

This just retrieves the closest sentinel and shoots at it.

Let's add the model action; in the model we assume that the shoot action is range-limited.

```cs
public Cost Shoot(){
    if(target.Dist(x, y) > range) return false;
    return (target = null, 1);
}
```

This is assuming a `Target` class which holds the target coordinates.

Now we update the goal - we're no longer trying to reach any specific position; instead we would like the agent to shoot the nearest target.

```cs
override public Goal<SentinelModel> Goal() => (
    m => m.target == null,
    m => m.target?.Dist(m.x, m.y) ?? 0
);
```

Without a heuristic the path-finding 'phase' would be very expensive.

We also need to update the model's `Equals()` and, ideally also `GetHashCode()`; adding model state without updating these is a source of errors.

### Methodology note

Adding the *shoot* action didn't go as smoothly as I expected. To figure things out, I wrote a few tests.

More interactive feedback is on the plan for XGOAP. With that in mind, tests are better than interactive feedback (such as viewing generated plans in realtime, or the status of the solver):

- Interactive feedback can help you locate issues quickly.
- Tests do not just prove that an agent can satisfy a given goal; running tests regularly ensures that adding new features does not break old stuff.

## 3. Pathfinding II

Now that shooting's working, let's go back to path-finding.
For this, the following changes were made:

- `Ground` generates a terrain using `GroundModel`.
- `SentinelModel` now includes the ground model to enable detecting obstructions.

## 4. Moving blocks

In this part we're going to add movable blocks, along with an action allowing a bot to pull a block.

### Game action

```cs
public void Pull(GameObject prop){
    target = transform.position - transform.forward;
    (hold = prop.transform).SetParent(transform);
}
```

Here, a prop is parented to the bot, then the bot moves one unit backward. We assume that the agent are facing the prop (the planning model is going to enforce this assumption).

Once the target is reached, the hold is released (see `UpdatePosition`):

```cs
hold?.SetParent(null);
```

### Add props to the map

This is a minor update to the `Ground` and `GroundModel`. Once we have added props at key locations on the map, the bot can no longer get in range; the solver stalls after exploring all possibilities.

### Modeling the 'pull' action.

Since pulling works 'backwards' relative to the agent's facing direction, we need to keep their orientation up to date; we won't add a 'turn' action yet - instead, we assume rotation and translation are linked.

There are two conditions to move a prop:
- The agent must be facing the prop
- The tile behind an agent should not be obstructed

`SentinelModel.Pull` implements the planning action; the ground model is updated accordingly:
- At the prop location, set map ID to 'ground'
- At the sentinel location, set map ID to 'prop'
- Move the sentinal one unit back

The ground model needs a similar update when the game action is applied.

## 5. Warmongering!

Now that interesting behaviors are in place, it would be neat to convert the practice dummy into another sentinel, so we can pit sentinels against one another. Fortunately this is done in one click. Enabling `SentinelAI` on the practice dummy is all it takes.

# Going further

See [performance](Performance.md) for related info.
