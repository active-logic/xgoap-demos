# Type safe action mappings

I want action mappings to be type safe, without planning models incurring dependencies on client APIs. Right now, the action mapping associates:

- a `Func<bool>` which represents the model action.
- an `object` which represents the real action. This is not type safe as the object can be a string, or another data structure representing a message that the client can process.

The proposed method to fix this is to access the client through an interface. This way we can tie an actual invocation as effect.
