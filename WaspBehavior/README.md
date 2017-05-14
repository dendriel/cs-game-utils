Wasp Behavior

Files:
+ WaspBehavior.cs
+ WaspBehaviorEvent.cs

```C
 WARNING: Wasp Behavior uses a Behavior Library implementation by Thomas H. Jonell. I could not find the original git repository for its implementation, but there is a fork in here: https://github.com/listentorick/UnityBehaviorLibrary
 
 The attached version of Behavior Library was modified by me at some points.
 ```
 
Wasp behavior aims to easy the development of game agents (or AIs).

Basically, you must extend this class (WaspBehavior) and implement the "BuildBehavior" method. In there, you will setup you behavior tree and set it inside the "behavior" parameter "behavior = new Behavior(BehaviorComponent).

WaspBehaviorEvent is like an action you setup in you BT but it will trigger an action once, test for some condition in every update and return sucess only when the condition is satisfied.

A behavior event is an action that returns Success only when its effect has been finished. For instance, an image alpha fading finalizes when all the image alpha is zero, so the action will return success. Other example would be displaying a message and waiting until the player hit the action button to dismiss the message.

WaspBehaviorEvent is mostly useful for animations or in-game conditionals.