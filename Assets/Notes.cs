/* NOTES *
 * 
 * need to make Players indistructible Entities that hold there own values the ships are just extentions
 * 
 * *3D UI Velocity Arrow only works when ship is moving (wont work when world moves instead of player) & 1st Person is Implemented and boolion set under 3d ship ui script
 *
 * *(Object Compatability)**IMPORTANT for MULTIPLAYER** 
 *  objects spawned must be inside the game manager tranform as a child or a child of a child
 *  ex bullets are under game manager BUT a turrent is under a ship node (the ship is under game manager transform as a child)
 * 
 * *ship physics Should be rebuilt with a proper thrust system (have to decide between knewton,arcade and mixed) 
 * BUT this should be done after floating point percision is fixed**(moving the world around player)
*/
//--------------------------------------------------------------------------------//
/* BUGS 
 * *(start of, weapon issues)
 *  need to set a max angle for the rotation around x and y(LazerWeapon.cs); look at shipUI for refrence might help!
 * *(end of, weapon isues)
 * * client side issue with host shooting client (damage isnt showing on client side UI)
 * *Until physics are fixed client should disable all rigidbody components on objects managed by host 
 * 
 */
//--------------------------------------------------------------------------------//
/* Current Task:
**ADD a relations system:
*   + ADD TDM(not finnished),FFA(might work),PeaceFull  
*   *********Prob need to send json with the faction configuration to the clients and need to make sure the host is the one arranging the teams.************
*   + ADD enamy icon if relation to object is enemy nutrual or ally
*   
* EndOf Current;
*/
//--------------------------------------------------------------------------------//
/* To-Do:
*    **BIG DANGEROUS CHANGE ONLY DO ON DAY IM 110% FOCUSED**
*  *Change Player from being a destructible game object keep a refrence script on a non destructible gameobjects kinda like a ghost that navigates the hiharchy
* 
*  *ADD Debug And Command Console  >> type /help  
*   + activat console via action menu tab>>debugConsole (done)
*   + make it so you can set GameMode in GameModeController.cs via command *need more detailed commanding system*
*  *ADD Rendezvou Icon For Matching Active Target Velocity (Only When Requested via "Target Action UI") //
* 
*  *TODO : Switch to using TODO's primaryly  they are visible in the TaskList window!                                       
*  
*  *REVAMP ShipUI.cs CreateUIObject(), UpdateObjectOutline(), and UpdateRadialMarker() they share alot of the same functions that can be consolidated
* 
*/
/* Concept Ideas:
 * **MAKE IMAGES AND LEAVE THEM IN ROOT** 
 * (1)3D concept >> JumpDrive << is super cooled magneticly locked gyro with led weights 
 * need concept for what causes the gyro to spin and what happens when it fails to super cool or super cooling is turned off(idal)
 * a idea could be some sort of wave force pointing twrd the gyro from the corner locations of a box each one can look like a Pan
 * (2)3D concept >> ShieldDrive << electrical arch between balls leads in corner locations of box and center Moter Controlled axis 
 * with electromagnetic coils & super cooled superconductors dows
 *
*/
//--------------------------------------------------------------------------------//
/* Completed:
 * *ADD Target Action UI (when right clicked or Active Target Hotkey)  
 *  *ADD Active Target ICON (done)
 *  *Active Fix Lazors spawning in game (check subnodes and node ids and figure out how the network works again)(done)
 *  *FIX keep lazers from fireing if mouse is disengaged (done)
 *  *ADD Velocity UI  (make a icon that uses same system as target icons or avoidence vector)
 *  {{ make it so it can be turned off could be annoying to somone who doesnt want this information }}
*/
//--------------------------------------------------------------------------------//
/* PIPE-LINE MILESTONES:

 
* *ADD matchmakeing lobby
*  >>voice chat

* *ADD game mode >> dogfight 
*  >>each player will be enamys to each other
*  >>leader board
*  >>points system for rounds 
*  >>prematch purchase of ship equpment when waiting to respawn
*  
* *ADD/CHANGE make shield a attachable object like the lazers and radar
* *ADD other guns and shields

* * *ADD station for customizing ship
*  >>docking
*  >>ui
*    >>purchase of ship upgrades
*    >>repair if able(game mode dependent)
*    


BIG Physics Update:
* < r = rotation   m = meters   s = second >
*   >>CHANGE the ship specs to somthing like {0.5r/s acceliration and 10m/s acceliration} --> REFER to NOTES(about 
*       redesigning ships physics)
*   >>ADD a flight computer object and hold flight functions withen it like -
*       the collision avoidence system and is sub functions and any functions depended on the target system 
*   >>ADD thrust UI and relitive target velocity
*   >>MAKE 3d model in ui of the ship what thrusters are active,the velocity and rotation
*   >>ADD go more into detail with ship thrusters giving them there own specs rcs and fwrd and reverse thrusters



* *ADD Combat AI
* >>add capital ship
*   >>add turrents


* *ADD Survival Mode in matchmakeing
* >>spawn waves of combat AI
* >>use station for upgradeing and repairing
* >>maybe add station upgrades for defence 
* >>allow station to take damage and loose upgrades or hardpoints
*/
//--------------------------------------------------------------------------------//
/* CLEAN UP:
 * *FIND things to clean or optimize!
 */