# Project Overview
- Game Title: Train VR (Unspecified, but it's a train-based experience)
- High-Level Concept: An immersive VR experience on a train where the player interacts with objects (Drill, Book, Tape, Plant) to restore color and progress the story/environment.
- Players: Single player (VR)
- Render Pipeline: URP (based on ColorManager and Project Settings)
- Target Platform: Standalone (VR)

# Game Mechanics
## Core Gameplay Loop
- Find an object in the train.
- Interact with it (pick up, move).
- Place it in a designated area (or on the floor for the plant) to trigger environmental changes (restoring color, stopping storm effects).
- Progression: Drill -> Book -> Tape -> Plant.

## Controls and Input Methods
- VR: OVRGrabber for picking up and dropping objects.
- Desktop: Keyboard/Mouse (Interact Key 'E').

# UI
- Minimalist/Diegetic (based on the scene content).

# Key Asset & Context
- `UniversalPlacementTarget.cs`: A new versatile script to replace `TriggerTarget`, `LecteurTarget`, and `CubeTarget`.
- `Assets/Scripts/TrainPlant.cs`: Reference for the "old" logic.
- `Assets/Prefabs/Plant.prefab`: The plant object.
- `Assets/Scenes/PlaygroundGood.unity`: The main scene.

# Implementation Steps
## 1. Backups
- Create `Assets/_Backups` and copy `TrainPlant.cs`, `TrainBook.cs`, `TrainTape.cs`, `TriggerTarget.cs`, `LecteurTarget.cs`, `CubeTarget.cs`.

## 2. Develop UniversalPlacementTarget
- Create `UniversalPlacementTarget.cs` with the following features:
    - `targetTag`: The tag of the object to detect (e.g., "Livre", "Tape", "Plant").
    - `successEffect`: Particle system to stop upon success.
    - `fogParticles`, `thunderParticles`: Arrays of particles to stop.
    - `fireParticleParents`: Array of GameObjects whose children particles should be stopped.
    - `audioToStop`: AudioSource to stop (e.g., thunderstorm).
    - `objectToShow`: Next object to activate.
    - `colorProgressValue`: Value for `ColorManager`.
    - `trackToPlay`: Enum or index to specify which track in `PlayerManager` to play.
    - `destroyTargetObject`: Boolean (True for Book/Tape, False for Plant if it should stay).
    - `stayInPlace`: Boolean (If true, parent the object to the target and make it kinematic).

## 3. Update the Scene (Logic Adaptation)
- In `PlaygroundGood.unity`:
    - **Book Target**: Update `TableTarget` with `UniversalPlacementTarget`. Configure with Book effects.
    - **Tape Target**: Update `VintageCassettePlayer` with `UniversalPlacementTarget`. Configure with Tape effects.
    - **Drill Target**: Update `Cube` with `UniversalPlacementTarget`.
    - **Plant Target**: Create a new GameObject "FloorPlantTarget" with a large BoxCollider (Trigger) on the floor. Add `UniversalPlacementTarget` configured for the Plant (tag "Plant", color 0.80, Plant track).

## 4. Replicate Plant Logic for VR
- Ensure the "Plant" object has `OVRGrabbable` and is tagged "Plant".
- The "FloorPlantTarget" will detect the plant when released.

## 5. Verification
- Test in VR (if possible) or verify the component configuration and script logic.
- Verify that particles stop and audio plays as expected.

# Verification & Testing
- **Unit Test**: Check if `UniversalPlacementTarget` correctly stops particles in children of `fireParticleParents`.
- **Manual Check**: Verify `colorProgress` updates correctly in `ColorManager`.
- **Logic Check**: Ensure `OVRGrabbable.isGrabbed` is checked before triggering activation to avoid premature triggers while holding the object.
