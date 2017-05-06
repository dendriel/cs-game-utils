+---------------------------+
+ Camera2DFollowMany.cs
+ CameraFollowElement.cs
+---------------------------+

Camera 2D Follow Many will follow any CameraFollowElement (component).
If there is multiple GOs to follow, the camera will expand accordingly to the configured parameters.

To use it, attach the Camera2DFollowMany to the game camera and attach the CameraFollowElement to any game object that must be followed.
You can configure the Camera2DFollowMany behavior from inspector.

It's possible to configure the following:
- Follow Speed: speed of the camera while following. You can keep it kind of slow to create a delay effect.
- Vertical Offset: verical offset of the camera position. Its default is centralized. Useful to align the camera in games like side scrolling (in which you want to display what is above, not bellow, that would be just the ground).
- Horizontal Offset: same as Vertical Offset, but aligns the camera horizontally.

- Expand Camera X (Y): if the camera must expand horizontally (vertically) (when there is multiple elements to follow).
- Minimum Distance (X) to Expand: distance that the followed elements must reach so the camera can start expanding.
- Maximum Camera Size: Limits the camera size while expanding. Set it to 0 for no limits.

- Top, Bottom, Left and Right Limits: its possible to define bounds from where the camera won't go further.
