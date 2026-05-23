# Project Overview
- Game Title: Train VR Migration
- High-Level Concept: First-person experience inside a moving train on a spline, transitioning from Flat to VR.
- Players: Single player VR.
- Inspiration / Reference Games: Immersive train sims / narrative experiences.
- Tone / Art Direction: Realistic/Vintage (based on ColorManager).
- Target Platform: Meta Quest / PCVR (Meta XR SDK).
- Render Pipeline: URP (based on UniversalAdditionalCameraData).

# Game Mechanics
## Core Gameplay Loop
- Exploration of the train interior while it moves along a spline.
- Progression through "color switching" (handled by ColorManager).
- Physical interaction (to be implemented later).

## Controls and Input Methods
- Locomotion: Left Joystick for Move/Strafe.
- Orientation: Movement direction is based on the HMD (Head-Mounted Display) forward vector.
- Rotation: Physical rotation only (Right Joystick rotation disabled).
- Interaction: (Postponed).

# UI
- N/A for this phase.

# Key Asset & Context
- **OVRPlayerController**: The main VR rig from Meta XR SDK.
- **PlayerManager.cs**: Previous script using Kinematic movement.
- **CharacterController**: New physics-based component on OVRPlayerController.
- **TrainRoot**: The parent object moving along the spline.

# Implementation Steps
1. **Prepare Scene Hierarchy** (Completed)
2. **Adapt PlayerManager for VR** (Completed)
3. **Configure VR Locomotion** (Completed)
4. **Robust Collision & Physical Anti-Cheat** (Completed)
5. **Fix Falling & Positioning at Launch** (Completed)
6. **Migrate ColorManager** (Completed)
21. **Done**
   - Update `ColorManager` references: Ensure the `PostProcessVolume` affects the `CenterEyeAnchor` camera (URP Volume should work globally, but check if any camera-specific overrides exist).
   - If `ColorManager` uses `Camera.main`, ensure the `CenterEyeAnchor` is tagged as `MainCamera`.

# Verification & Testing
- **Movement Test**: Move with left stick, confirm direction changes when looking around.
- **Rotation Test**: Right stick should do nothing. Turning 180° physically should work.
- **Collision Test**: Walk into a train wall with the joystick; the player should stop.
- **Physical Collision Test**: Try to walk physically through a wall; check if the "push back" logic keeps the virtual body inside.
- **Visuals Test**: Verify `ColorManager` still shifts colors correctly in the headset.
