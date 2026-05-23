# Project Overview
- Game Title: Train VR Experience
- High-Level Concept: Unifier la logique de progression environnementale (sons/particules) entre Desktop et VR pour assurer que les effets visuels et sonores de la tempête s'arrêtent correctement lors de la pose des objets.
- Players: Single player VR/PC
- Render Pipeline: PC_RP (Built-in/Custom)

# Game Mechanics
## Core Gameplay Loop
Le joueur déplace des objets clés (Drill, Book, Tape, Plant) vers des emplacements spécifiques. Chaque pose réussie réduit le chaos environnemental (arrête le brouillard, la foudre, le feu) et fait progresser la musique.

## Controls and Input Methods
- **VR** : OVRGrabbable (Grab/Release). La détection de pose ne se fait que si l'objet est relâché (`isGrabbed == false`).
- **PC** : Key 'E' (PickUp/Place) via les scripts `TrainItem`.

# UI
- N/A (Interaction physique uniquement).

# Key Asset & Context
- `EnvironmentManager.cs` (Nouveau) : Centralise les références aux systèmes de particules (Fog 1-4, Thunder 1-5, Fire) et à l'audio de la tempête.
- `TriggerTarget.cs`, `LecteurTarget.cs`, `CubeTarget.cs` : Modifiés pour appeler le gestionnaire central au lieu de gérer leurs propres références locales.
- `OVRGrabbable` : Utilisé pour synchroniser le déclenchement VR avec le relâchement de l'objet.

# Implementation Steps
1. **Créer `EnvironmentManager.cs`**
   - Stocker les tableaux de `ParticleSystem` (Fog, Thunder).
   - Stocker le tableau `fireParticleParents`.
   - Implémenter des méthodes comme `StopStage1()`, `StopStage2()`, etc., ou une méthode générique `StopEnvironmentForObject(string tag)`.
   - *Fichiers* : `Assets/Scripts/EnvironmentManager.cs`

2. **Mettre à jour `TriggerTarget.cs`, `LecteurTarget.cs`, `CubeTarget.cs`**
   - Ajouter la logique de vérification `isGrabbed` (via `GetComponent<OVRGrabbable>()`) pour empêcher le déclenchement tant que l'objet est en main.
   - Appeler `EnvironmentManager.Instance` pour arrêter les particules et sons associés.
   - *Fichiers* : `Assets/Scripts/TriggerTarget.cs`, `Assets/Scripts/LecteurTarget.cs`, `Assets/Scripts/CubeTarget.cs`

3. **Mettre à jour `TrainPlant.cs` et autres scripts d'objets**
   - S'assurer qu'ils utilisent le `GameAudioManager` central pour la musique.
   - Nettoyer les anciennes références de particules devenues inutiles (déportées vers le Manager).
   - *Fichiers* : `Assets/Scripts/TrainPlant.cs`, `Assets/Scripts/TrainBook.cs`, `Assets/Scripts/TrainDrill.cs`, `Assets/Scripts/TrainTape.cs`

4. **Configuration de la Scène**
   - Créer le GameObject `EnvironmentManager` dans la scène.
   - Assigner les particules `FogParticleSystem (1-4)`, `LightningParticleSystem (1-5)`, `RainSound` et les parents de feu.
   - Re-connecter les cibles dans la scène pour qu'elles n'aient plus besoin de références locales.

# Verification & Testing
- **Test de Pose (PC/VR)** : Vérifier que l'objet disparaît (ou se pose) et que le brouillard/feu correspondant s'arrête immédiatement.
- **Vérification Audio** : Confirmer que la piste musicale suivante se lance sans conflit avec la tempête.
- **Edge Case** : Vérifier que si on rentre et sort de la zone VR sans lâcher l'objet, rien ne se passe.
