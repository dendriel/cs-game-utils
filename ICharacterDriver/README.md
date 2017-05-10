Character Driver

Files:
+ ICharacterDriver.cs
+ SDCharacterDriver.cs

SDCharacterDriver is an implementation of a "driver" for a Side Scrolling character.
ICharacterDriver is an interface that must be implemented in order to be able to use some actions in Wasp Behavior.

To setup it,
- Create a new GameObject and drag the SDCharacterDriver into it. It will create a RigidyBody2D, BoxCollider2D and a CircleCollider2D.
- You must also add an animator (even if without animations) into the Game Object;
- Create a Physics Material 2D and set it into the CircleCollider2D (needed for friction feature);
- Configure the walking speed; set to true the options "Axis Movement" and "Always Grounded".

To test it
- You may grab the PlayerController from IPlayerController feature and attach to the Game Object. Call the WalkRight, WalkLeft, WalkDown and WalkUp driver methods from the controller to see the char moving.


The suitable development partner (IMHO) using these resources are the following:

- Create "default" characters. The main character, enemies and npcs. Don't mix controllers with the driver;
- Create controllers and attach to the character as needed. You may use a "PlayerController", for controlling the main character, and Behaviors for controlling the enemies and NPCs.
- Attach the needed controllers to the characters when spawming then.