# Project Overview
- Game Title: Train VR Migration - Interaction System
- High-Level Concept: Transitioning from gaze-based interaction (Keyboard 'E') to VR controller interaction (Trigger/Grip) for picking up and placing objects inside a moving train.
- Interaction Style: VR Controller pointing + Trigger.

# Game Mechanics
## Core Gameplay Loop
- Aim at objects tagged with `canPickUpX` using the VR controller.
- Grab objects using the controller trigger.
- Carry objects and drop them into target zones (`CubeTarget`, `LecteurTarget`).
- Dropping an object in a target zone triggers visual and audio progression.

## Controls and Input Methods
- Aiming: Raycast from the VR Controller (Left or Right hand).
- Action: Grab/Drop via `OVRInput.Button.PrimaryIndexTrigger` or `SecondaryIndexTrigger`.
- Movement: Left Joystick (already implemented).

# UI
- Optional: Visual feedback (Laser/Reticle) coming from the hand when pointing at an interactable.

# Key Asset & Context
- **PickUpScript.cs**: The original camera-based script. Its logic will be migrated to the OVRGrabber system.
- **OVRGrabber**: Meta XR component to be added to the Right Hand Anchor.
- **OVRGrabbable**: Meta XR component to be added to interactable objects.
- **CubeTarget.cs / LecteurTarget.cs**: Existing target zone scripts using `OnTriggerEnter`.
- **RightHandAnchor**: The specific hand used for interaction.

# Implementation Steps
1. **Right Hand Setup**
   - Locate `RightHandAnchor` in the `OVRCameraRig`.
   - Attach the `OVRGrabber` component.
   - Configure the `Grab Volume` (SphereCollider as trigger) to define the "proximity" area.
   - Add a `LineRenderer` to act as the "Laser" visual for aiming.

2. **Interactable Objects Configuration**
   - Identify all objects tagged with `canPickUpX` and `Tape`.
   - Attach `OVRGrabbable` to each.
   - Ensure they have a `Rigidbody` and a `Collider`.
   - Implement a custom `VRGrabbableExtension` script to handle `originalParent` restoration (Train) upon release.

3. **Laser & Aiming Logic**
   - Create a small helper script for the Laser visualizer.
   - The laser will change color or activate when pointing at an `OVRGrabbable` within range.
   - Interaction will be triggered by `OVRInput.Button.SecondaryIndexTrigger` (Right Hand).

4. **Integration with Target Zones**
   - Verify that when an object is released from the `OVRGrabber`, it is correctly detected by `CubeTarget` and `LecteurTarget`.
   - Ensure the objects are parented back to `TrainRoot` if they don't hit a target, so they don't fall out of the moving train.

5. **Haptic Feedback**
   - Implement a short vibration on the right controller when the laser hits a grabbable object.

# Verification & Testing
- **Aiming Test**: Confirm the raycast follows the controller direction.
- **Grab Test**: Squeeze trigger to pick up; object should snap to hand.
- **Hold Test**: Walk around the train; object should stay parented and follow correctly.
- **Drop Test**: Release trigger; object should fall according to gravity.
- **Progression Test**: Drop object into a target zone; verify music plays and colors shift.
