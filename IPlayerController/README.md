Player Controller Interface

Files:
+ IPlayerController.cs
+ KeyboardController.cs
+ MouseController.cs
+ USBController.cs
+ USBAndKeyboardController.cs
+ PlayerController.cs

Player Controller Interface is a game controller implementation focused in providing an abstraction for controllers.
The main idea is that you can use the IPlayerController to hold any specific implementation of a controller (keyboard, USB, mouse, etc).

- PlayerController provides an example on how to use the Player Controller Interface.
- The keyboard and mouse implementation of IPlayerController are OK to be used as they are.
- Before using the USBController, you must setup Unity input ("Edit->Settings->Input") and find out which buttons of your USB controller corresponds to which Unity input.
- USBController allows multiple players. When running a single player game, instantiate the USBController with ID=0; Whn running a multiplayer game, it's necessary to setup the USBController with IDs>0.
- USBAndKeyboardController is an implementation aimed for allowing the player to choose between keyboard and a USB controller.