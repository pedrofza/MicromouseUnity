# UnimouseSim
A 3D simulator for the micromouse competiton developed in the Unity platform. Unity version 2020.1.0f1

# External Dependencies
This project depends on Extenject 9.1.0 [https://github.com/svermeulen/Extenject/releases/tag/v9.1.0]. Extenject is a dependency injection library for Unity, based on Zenject [https://github.com/modesttree/Zenject].

# Usage
- Download the release binaries and the Arduino library [link](https://github.com/PDR5/MicromouseUnity/releases/tag/0.1).
- Add the library to the Arduino IDE environment
- Upload the code for the microcontroller. Examples available for [Wall distance](https://github.com/PDR5/MicromouseUnity/tree/master/Example-arduino/VeraoParede), [Speed control](https://github.com/PDR5/MicromouseUnity/tree/master/Example-arduino/VeraoVelocidade), [Track](https://github.com/PDR5/MicromouseUnity/tree/master/Example-arduino/VeraoCircuito) and [Micromouose](https://github.com/PDR5/MicromouseUnity/blob/master/Example-arduino/Micromouse.zip). The micromouse example uses the modified flood fill algorithm.
- Run the executable file.
- Press ESC to choose between the different environments.
- Insert port and baud rate (top-right of the screen) and click "Connect".
