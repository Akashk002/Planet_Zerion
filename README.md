Game play Video 1 - 
Game Play Video 2 -

Game Story :-                                           

Amid the silence of deep space, our agency discovered a newly charted world—Zerion. A planet rich in rare-earth fossils, exotic minerals, and volatile atmospheric gases. Its environment, surprisingly Earth-like, was perfect for colonization and scientific experimentation.

What began as exploration quickly became expansion.
A permanent settlement—Colony 7—was established on the planet’s surface.
Teams were deployed to mine its resources, study its terrain, and build the foundation of a new human frontier.

For a time, progress was rapid.
We tapped into Zerion’s fossil-rich crust and extracted energy-laden minerals unlike any found on Earth. These materials fueled our operations—power, research, infrastructure.

But the silence didn’t last.

Unbeknownst to us, Zerion was not an unclaimed paradise.
From the orbit of a nearby planet, an ancient alien civilization stirred—observers of the galaxy, now awakened by our presence. To them, humanity’s arrival was not a scientific pursuit… it was an intrusion.

They didn’t send a warning.They sent warships.

Long-range sensors intercepted their approach—massive spacecraft armed for destruction, heading straight for the colony.

As the threat closed in, command initiated emergency protocols:
The extraction of remaining resources was accelerated.
Defense systems, long dormant, were powered up.
Access to advanced strike craft was unlocked—capable of intercepting and repelling airborne threats.
Zerion had gone from a beacon of hope to the front line of an interstellar conflict.

The alien fleet will not stop until the colony is destroyed.
But neither will we.

The fight for Zerion is not just about survival —It is about claiming our place among the stars.
The defense has begun.

Mission 1: First Steps
Your journey begins at your home base—My Home, Your personal command base.

Your first task is to collect resources in the form of rocks, scattered across the terrain. These rare-earth materials are essential for unlocking advanced buildings and spacecraft systems.

You’ve already unlocked the Resource Warehouse Room, where you can access and activate key facilities such as the Spacecraft Terminal.

At the Spacecraft Terminal, you can:
- Unlock available spacecraft
- Select and enter your chosen ship

Start gathering resources and prepare to expand your operations—the defense of the colony begins now.

Spacecraft Unlocked:
- Corvette_1
- Corvette_2

Warning - don,t go too close to enemy they will attack you through missile 

Mission 2: Activate Surveillance Systems
With basic infrastructure in place, it's time to strengthen colony security.

The Drone Controller Room is now unlocked. You now have access to two types of support drones:
- Carrier Drone: Automatically collects resource rocks and delivers them to storage.
- Security Drone: Patrols the skies, detects approaching enemy aircraft, and activates an early-warning siren if a threat is detected.

These systems will help you prepare for incoming alien activity and maintain a steady supply of resources for further upgrades.

New Spacecraft Unlocked:
- Corvette_3
- Corvette_4

Mission 3: Defend the Skies
Enemy scouts have been spotted near orbit. Defense systems must be activated immediately.

Your objective is to:
- Strengthen aerial detection by upgrading the Security Drone system
- Unlock and power up Automated Defense Modules to intercept enemies
- Manually take flight using your spacecraft to intercept alien scouts before they reach the colony

Be prepared for increasing enemy activity. This is the first wave—stay sharp.

New Spacecraft Unlocked:
- Corvette_5
- Frigate_1

Mission 4: Engage and Strike
With colony defenses online, it's time to take the fight to the enemy.

You’ve now unlocked access to the Advanced Spacecraft Armory, allowing you to:
- Deploy high-speed strike ships
- Customize spacecraft loadouts for different combat scenarios
- Begin targeted raids on alien supply lines and forward bases

This mission will test your combat piloting skills. Strike hard and return safely.

New Spacecraft Unlocked:
- Frigate_2
- Frigate_3

Mission 5: The Final Stand
The alien fleet has launched a full-scale invasion.

Your final mission is to:\n
- Defend Colony 7 from multiple waves of attackers
- Protect key infrastructure: My Home, Warehouse Room, and Spacecraft Terminal
- Take to the skies and eliminate 100 enemy ships in a final showdown to secure humanity’s future

All systems are online. All ships are ready.

This is the last stand. Hold the line.

New Spacecraft Unlocked:
- Frigate_4
- Frigate_5

Project Architecture & Key Patterns

1. Singleton Pattern
- GameService and UIManager both inherit from GenericMonoSingleton<T>, ensuring only one instance exists and providing global access.
- EventService uses a static instance for global event management.

2.Service Locator Pattern
- GameService acts as a central hub, providing access to services like AudioManager, BuildingManager, SpawnPoints, and gameplay services (Player, Drone, Spacecraft, Enemy, Missile, Mission, VFX).
- Other scripts (e.g., controllers, UI) access these services via GameService.Instance.

3.MVC (Model-View-Controller)
-Player, Drone, and Spacecraft systems are structured with clear separation:
-Model: Holds data/state (e.g., PlayerModel, DroneModel, SpacecraftModel).
-View: Handles Unity components and visuals (e.g., PlayerView, DroneView, SpacecraftView).
-Controller: Contains logic and mediates between Model and View (e.g., PlayerController, DroneController, SpacecraftController).

4.State Machine Pattern
-PlayerStateMachine manages player states (Idle, Walk, Run) and transitions.
-DroneController and SpacecraftController use internal state enums and logic for activation, deactivation, and special modes (e.g., Surveillance for drones).

5.Object Pool Pattern
-AudioManager uses a pool of AudioSource objects for efficient sound playback.
-GenericObjectPool<T> is a reusable pool class, inherited by:
 - MissilePool (for missile controllers)
 - EnemySpaceCraftPool (for enemy spacecraft controllers)
 - Pools are used to avoid expensive instantiation/destruction during gameplay.

6.Occlusion Culling
-No direct script-based occlusion culling logic was found. In Unity, occlusion culling is often handled via engine settings and scene setup, not always via code. If you use Unity's built-in occlusion culling, mention it in your 
documentation or scene setup.

7. Minimap for Player and Spacecraft
- MinimapIconFollow: Follows a target (player or spacecraft) on the minimap, updating icon position and rotation.
- UIManager: Holds a reference to MinimapIconFollow and sets the minimap target when switching between player and spacecraft.
- PlayerController and SpacecraftController: On activation, set the minimap to follow their respective transforms.
- MinimapIconFollow.StartBlink: Used for events like enemy spawn, blinking pointers on the minimap.

Key Script Descriptions

GameService.cs
-Central singleton for accessing all major game systems and services.
-Initializes player, drone, spacecraft, enemy, missile, mission, and VFX services.
-Provides a service locator for other scripts.

AudioManager.cs
-Manages all game audio.
-Implements an object pool for 3D audio sources.
-Supports one-shot and looping sounds, with dynamic pool expansion.

PlayerController, PlayerModel, PlayerView, PlayerStateMachine
-Controller: Handles input, state transitions, and player logic.
-Model: Stores player stats, inventory, and state.
-View: Manages Unity components and forwards events to the controller.
-StateMachine: Manages player movement states.

DroneService, DroneController, DroneModel, DroneView
-Service: Manages all drones, switching, and activation.
-Controller/Model/View: Follows MVC, with logic for movement, interaction, and surveillance mode.

SpacecraftService, SpacecraftController, SpacecraftModel, SpacecraftView
-Service: Handles spacecraft creation and removal.
-Controller/Model/View: Follows MVC, with logic for movement, aiming, and missile firing.

EnemySpaceCraftService, EnemySpaceCraftController, EnemySpaceCraftPool
-Manages enemy spacecraft using object pooling for performance.

MissileService, MissilePool, MissileController
-Handles missile creation, configuration, and pooling.

UIManager, MinimapIconFollow, PlayerUIManager, SpaceCraftUIManager, DroneUIManager
-UIManager: Singleton for managing all UI panels and transitions.
-MinimapIconFollow: Updates minimap icon for player/spacecraft, supports blinking for events.
-PlayerUIManager/SpaceCraftUIManager/DroneUIManager: Specialized UI for each entity, updating stats, and handling user actions.
 
Utilities
-GenericMonoSingleton: Base class for singletons.
-GenericObjectPool: Base class for object pools.
-EventService: Singleton event bus for decoupled communication.


🚀 Additional Technical Details for Space Game Architecture
📦 Resource System
- Rock Types: Custom RockType enum (e.g., Volcanis, Xylora, Cryon, Droska, Zentha, Prime) is used as currency/resources in gameplay.
- Rock Storage: Player has a rock inventory managed via PlayerScriptable.rockDatas list. Each entry holds count and rock type.
- Usage & Spending: Rock can be added, checked for availability (CanSpend), and deducted using defined helper methods, supporting gameplay progression like unlocking buildings or upgrading equipment.

🛰️ Building and Room Interaction System
- Buildings/Rooms like DroneControlRoom, SpacecraftTerminal, and ResourceWareHouse act as interactive hubs:
- Interaction logic is handled by player input and collision triggers.
- Selection UIs, drone menus, and spacecraft panels open based on room detection.

🎮 Input Handling
- Custom input-driven interaction via PlayerController.Interact() uses KeyCode.E to toggle interactivity.
- Movement and camera synchronization are handled to always keep the character aligned with the FreeLook camera.

🔊 Sound System (3D Spatial Audio)
- Sounds play using a pooled AudioSource from AudioManager.
- PlayLoopingAt() and StopSound() methods manage movement and ambient sounds with position-based logic.
- Looping sounds for walking/running are tied to PlayerMove() updates, and stopped when player halts.

🌌 Scene Management & Game Start
- Rooms and spawners are organized under a root GameObject (Game).
- Each major gameplay system (Player, Drone, Enemy, etc.) is initialized via GameService at runtime.

🧠 Memory & Performance Optimization
- Profiler-tested frame rate: ~100 FPS with low draw calls and minimal skinned mesh overhead.
- Object pools reduce GC allocations and instantiation costs.
- Scene utilizes Unity's built-in occlusion culling to reduce camera rendering load dynamically.

🧭 Minimap & Navigation
- Dynamic minimap for player and spacecraft, automatically switches via UIManager.
- Minimap icon system supports StartBlink() for alerts and enemy detection events.

🧪 Testing & Debug Tools
- Debug logs integrated to show real-time sound playbacks, rock collection, and interaction triggers.
- LeanTween is used to animate tiredness recovery and handle time-based effects smoothly.


🧱 Core Components & Systems
🧍 Player System (MVC + State Machine)
- Controls player movement, rock collection, room interaction.
- Supports walk, run, interact, and idle states.
- Tracks tiredness, resource inventory, and interaction status.

🛰️ Drone System (Surveillance & Carrier)
- Carrier Drone: Searches terrain and brings rocks to the warehouse.
- Security Drone: Patrols surroundings, provides surveillance of enemy activity.
- Controlled via the Drone Control Room.

🚀 Spacecraft System
- Manages the player’s customizable spacecraft.
- Handles movement, missile targeting, aiming (Cinemachine Aim), and refueling.
- Used for planetary defense and long-range exploration.
- Activated from the Spacecraft Terminal.

🗺️ Minimap System
- Tracks player, drones, spacecraft in real time.
- Blinking icons notify player of events (enemy spawns, alerts).
- Dynamically changes target depending on who is active.

🧠 Game Architecture Patterns
- Singletons: Core managers (GameService, AudioManager, UIManager).
- Service Locator: GameService provides global access to systems.
- MVC: Clear separation for Player, Drone, and Spacecraft.
- Object Pool: Used for missiles, audio sources, and enemy crafts.
- State Machine: Used in player, drone, and spacecraft control logic.

🧱 Rooms and Facilities
🔹 Main Office
Starting point of the player. Access colony overview, missions, and player stats.

🔹 Research Center
Unlock new technologies and analyze alien rock samples for upgrades.

🔹 Spacecraft Terminal
Select, refuel, and upgrade your spacecraft. Displays detailed spacecraft stats and visuals.

🔹 Drone Control Room
Manages the two drones. Allows manual control, AI settings, and surveillance commands.

🔹 Reactor
Generates power required to run buildings, drones, and spacecraft systems.

🔹 Farm
Grows essential life-support materials and food to sustain the colony.

🔹 Machine Building Plant
Produces essential machinery and components used in upgrades and repair.

🔹 Resource Warehouse
Stores all collected rocks. Also used to unlock and build new structures.

🔹 Signal Command Center
Used to intercept enemy signals and trigger planetary-wide alerts.

🔹 My Home
A personal recovery space where the player rests to reduce tiredness and reflect on story logs.

🧬 Gameplay Loop
- Explore Terrain → Collect rocks manually or via drones.
- Deposit in Warehouse → Use rocks as currency to build/upgrade rooms.
- Manage Tiredness → Rest in “My Home” to recover energy.
- Switch Roles → Control drones or spacecraft as needed.
- Defend Base → Use spacecraft to counter enemy attacks triggered randomly.
- Progress Story → Unlock logs, decipher signals, and discover the mystery of the alien threats.

📊 Progression & Resources
- Six Rock Types: Volcanis, Xylora, Cryon, Droska, Zentha, Prime.
- Each unlocks different buildings or technologies.
- Unlock System: Spending rocks from any type (combined logic supported).
- Energy & Fatigue: Strategic rest required for continuous efficiency.

