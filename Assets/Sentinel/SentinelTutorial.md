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

A this point path-finding doesn't really shine since obstacles are not accounted for. In practice though, you'd probably delegate path-finding to a subsystem. With this in mind we'll implement shooting a dummy as our next goal.
