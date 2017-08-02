Platform 2D

Files:
+ AxisPlatform2D.cs
+ CircularPlatform2D.sh

Platform 2D provides features for handling platforms in 2D games.

AxisPlatform2D allows to define axis (horizontal and vertical) movement (between two points) to platforms.
CircularPlatform2D allows to define circular movement (around a point) to platforms.

- For the platforms to work smoothly, its necessary to add a trigger collider in order to "attach" passengers to the platform.
- When a GO is touching/attached to the platform, its position will be updated according to the platform movement. This avoid a bounce effect from the interaction between the object and platform that is handled by Unity physics.