Animation Utilities

Files:
+ AnimUtils.cs

Utilities for operations with animations.

- The main feature of this class is "SetupAnimationEvents" that enable us to setup animations from code.

I'd rather setting up animation events directly inside the animation, but my motivation is: I was working in a project in which the art designer send me unity packages with character animations in it. Every time that I imported these packages in my project, all the previously animation events set by me (by hand) were lost. So, instead of bothering the animator (and myself), this feature allows us to set animation events from code.
