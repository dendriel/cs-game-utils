Generic Manager

Files:
+ GenericManager.cs


Generic Manager provides default features for managing elements data and instances.
This is a "two-in-one" that loads data elements from file, create instances to parse/store this data (elemsList) and spawn game objects in the scene from a given prefab (elemPrefab). *I know that isn't the best idea to mix things up, but this may be used as a template for most managers.

You will find more information in the GenericManager.cs clas documentation.

WARNING: this class is fine for single player games, but won't work for networked games (NetworkBehavoiur won't get along with generic types).