# Sentinel tutorial

Let's create a bot which can navigate a terrain and shoot down other bots.

## Implementation overview

In general, a typical GOAP implementation will look like this:

- A controller implements game actions; such as walking, shooting, collecting items and the like. In simple cases this can be just one class.
- Derived from `Activ.GOAP.Agent`, the planning model is implemented as a separate class. You can think of this as a simplified version of the game agent, its actions, and the environment.
- Derived from `Activ.GOAP.GameAI`, the AI itself is responsible for managing planning, and effecting moves.

In Unity3D, both the controller and the AI derive from `MonoBehaviour` - so you can add them to a game object.

## Basic Navigation

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

## Target practice

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
override public Goal<SentinelModel> Goal()
=> new Goal<SentinelModel>(
    m => m.target == null,
    m => m.target?.Dist(m.x, m.y) ?? 0
);
```

Without a heuristic the path-finding 'phase' would be very expensive.

We also need to update the model's `Equals()` and, ideally also `GetHashCode()`; adding model state without updating these is a source of errors.

## A note about methodology

With the above, errors started appearing when introducing the latest action.
A little interactive feedback does not hurt; with this in mind I added an editor to help visualize planning behavior.

Having said that, how the model was fixed is by writing and running tests. The advantage of this method is that we can make sure fixes stay 'in' as development progresses.

## Pathfinding II

Now that the shooting action is done, let's get back to path finding.
