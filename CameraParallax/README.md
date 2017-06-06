Camera Parallax

Files:
+ CameraParallax.cs

This feature provides the parallax or "moving backgrounds" effect for a 2D game.

To use it you may add the CameraParallax script in the main camera (or anywere else) and setup the parallax elements in a list that will be visible on inspector.

This list is of the type "ParallaxElement", so you need to setup the list elements number by hand and then set the elements.

The parallax effect is applied over a Transform, so you can create a holder GameObject to contain many background elements.

For each parallax element it's possible to configure:
- Horizontal and Vertical speed;
- Reversed horizontal speed;
- Reversed vertical speed.

The Camera Parallax feature also align the parallax element according to the player starting position.