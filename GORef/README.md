Game Object Reference

Files:
+ GORef.cs

Provides a way to store the reference of game objects that must start disabled.

The object WILL start enabled, store its reference, and auto-disable. The references can be access by static methods from GORef.

- To store a reference, just add the GORef in a GameObject. You may use the object tag from unity, or define a custom tag from it. You may also set it to auto-disable after storing its reference;
- To access a reference use GetRef();
- When reloading a scene, the GORef scripts will re-execute and update the reference in the dictionary;
- You may want to configure this as one of the first scripts to be executed, so the references will be available right in Start() phase (change this configuration under "Edit->Project Settings->Script Execution Order).