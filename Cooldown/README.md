Cooldown

Files:
+ Cooldown.cs

This is an independent timer that uses "WaitForSeconds" for creating a cooldown.

Cooldown is useful to avoid spaming routines around the code just to create a timer.

The easiest way to use this is to instantiate a new Cooldown setting the time to wait in seconds "foo = new Cooldown(1)" and triggering the cooldown passing a reference to a monobehavior "foo.Start(this)". Then wait it to end by checking "foo.IsWaiting".

- Its possible to request a random cooldown by passing a second value in the class constructor.
- Its possible to stop the cooldown.
- Its possible to start the cooldown with a time different than the used when creating the instance.