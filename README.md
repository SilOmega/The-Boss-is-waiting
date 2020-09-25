# The-Boss-is-waiting
A demo of 2D action platform made with Unity

This repository holds part of the C# side of a 2D action platform demo I made with Unity. The game is set in a dark dungeon and the goal is to run to the boss as fast as you can, dealing with foes and traps across the path. The whole project has been refactored for the sake of testability, shifting from Monobehaviours to a combination of Monobehaviours and ScriptableObjects.

The GhostKing's scripts describe the behaviour of the demo's boss. It has a patrolling routine until player is spotted then the first phase starts and melee attacks are performed. When the remaining health falls under a certain level the next phase is triggered. In this final part the boss teleport itself from one side of the screen to the other and shoots projectiles cyclically.

The Player's logic is divided into three scripts with the possibility to jump and attack, while the health's UI is managed through some shared variables.

The other two scripts describe the remaining enemies of the demo, a walking ghoul and a wizard which shoots fireballs when player is spotted.
