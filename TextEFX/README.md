Text Effect

Files:
+ TextEFX.cs
+ TextManager.cs

Text Effect provides the in-game text popup feature.

TextManager is kind of a "text factory". Create a GameObject and assign TextManager script to it. Then create child game objects and setup the TextEFX in then (you may configure TextEFX from inspector).

TextEFX allows the following configurations:

- Offset (to align the text);
- Distance to cover (pop up effect);
- Popup (text) speed;
- Decay time (how much time the text must be visible);

Other configurations to set when enabling TextEFX:
- Position to be displayed (the offset will be added to this position);
- Text to be displayed;
- Starting and Ending colors.