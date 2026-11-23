# AI Usage Declaration – HydroRush

## 1. Introduction

During the development of HydroRush, generative Artificial Intelligence (AI) tools were used as support for programming, code debugging, gameplay mechanics implementation, accessibility features, and visual asset creation.

The purpose of using these tools was to assist the development team in solving programming problems, generating initial code structures, improving existing implementations, and exploring solutions for different gameplay systems.

AI-generated content was reviewed and adapted by the team before being integrated into the project. The final implementation, modifications, testing, and validation were the responsibility of the developers.

## 2. AI Tools and Versions

The following generative AI tools were reported as part of the development process:

|          Tool          |                   Version                   |                        Main Purpose                        |
|------------------------|---------------------------------------------|------------------------------------------------------------|
|         ChatGPT        |             To be confirmed                 | C# code generation, debugging, and programming assistance. |
|         Claude         | Sonnet 5.5 (reported; verification pending) |           Code generation, review, and debugging.          |
| Image generation tool  |             To be confirmed                 |     Creation of the main character's sprite sheet (Aqua).  |

The exact versions and tools used for each task must be confirmed with the corresponding team members.

## 3. AI Usage in Development

AI tools were used to support the development of several systems in HydroRush.

The documented areas include:

- MenuManager: scene transition debugging.
- EnemySpawn: random enemy spawn position debugging.
- PlayerController: implementation of the Dash ability.
- AccessibilitySettings: implementation of an automatic shooting accessibility option.
- Aqua Sprite Sheet: AI-assisted visual asset creation.

### 3.1. MenuManager – Scene Transition Debugging

**Objective:** Identify and solve an issue where the Play method was not being executed correctly when interacting with a UI button.

**AI-generated prompt:**

"I have a menumanager but is not reading the void play in order to change scene on the editor what shout i change, since the script is already on the button"

**AI assistance:** Debugging and identifying possible problems with the connection between the UI button and the scene transition method.

**Generated code:** To be documented after reviewing the AI response and the final script.

**Manual modifications:** To be confirmed with the developer.

**Functional validation:** To be documented with evidence from Unity.

### 3.2. EnemySpawn – Random Position Debugging

**Objective:** Solve an error related to the use of Random.Range for defining enemy spawn positions.

**AI-generated prompt:**

"On the following script i have an issue with the random.range to define were the enemies should spawn, how should i fixed it and also have the error code Error: 'Random' is an ambiguous reference between 'UnityEngine.Random' and 'System.Random'"

**AI assistance:** Identification of the ambiguous reference between UnityEngine.Random and System.Random and suggestions for correcting the random position generation.

**Generated code:** To be documented after reviewing the AI response and the final script.

**Manual modifications:** To be confirmed with the developer.

**Functional validation:** To be documented with evidence from Unity.

### 3.3. PlayerController – Dash Ability

**Objective:** Implement a dash ability with temporary invulnerability while preserving the existing player movement, aiming, and shooting systems.

**AI-generated prompt:**

"I have a 2D top-down shooter in Unity (new Input System, Rigidbody2D). My PlayerController already handles movement, mouse aiming and shooting, and my PlayerHealth already has SetInvulnerable(bool).

Add a dash with minimal changes:
1. Input: 'Player/Dash' action (Left Shift, Space, gamepad East) found with InputSystem.actions.FindAction. If missing, LogWarning and disable only the dash.
2. Serialized fields: dashSpeed, dashDuration, dashCooldown (counted from the start of the dash).
3. Direction: move input if moving, otherwise the aim direction.
4. While dashing, the dash velocity overrides normal movement in FixedUpdate.
5. Invulnerable during the dash through SetInvulnerable(true/false).

If I paste a Console error, explain the cause, give the exact fix, and say whether it comes from my scripts or the Editor."

**AI assistance:** Generation of an initial implementation for the dash mechanic, including input handling, movement direction, cooldown, and temporary invulnerability.

**Generated code:** To be documented after reviewing the AI response and the final PlayerController script.

**Manual modifications:** To be confirmed with the developer.

**Functional validation:** To be documented with evidence from Unity.

### 3.4. AccessibilitySettings – Automatic Shooting

**Objective:** Implement an accessibility option that allows the player to activate automatic shooting while preserving manual shooting functionality.

**AI-generated prompt:**

"Add an auto-fire accessibility option with minimal changes:
1. Static class `AccessibilitySettings`: each option is a property saved with PlayerPrefs (off by default) that raises a C# event when it changes.
2. MonoBehaviour `AutoFireToggle`: toggles with the F key and supports an optional UI Toggle and TMP status text.
3. Change the shooting condition in PlayerController so manual shooting still works.
4. Edge cases: Time.timeScale = 0, UI clicks counting as shots, and resetting the saved PlayerPrefs value."


---

## AI-assisted Development and Debugging

AI tools were used as support during the development of HydroRush, particularly for generating, reviewing, and debugging C# scripts in Unity.

The main areas where AI assistance was used included:

- **MenuManager:** Assistance with scene transitions and menu functionality.
- **EnemySpawn:** Identifying and resolving an ambiguity involving the `Random` class in C#.
- **PlayerController:** Reviewing and implementing the dash mechanic according to the project's requirements.
- **AccessibilitySettings:** Exploring an auto-fire accessibility option and its integration with the player controller and user interface.
- **Game assets:** Assistance with the creation of visual resources, including sprite-sheet concepts.

## Example of AI-assisted Debugging

During development, an issue involving the `Random` class in the `EnemySpawn` script was identified.

The problem was related to an ambiguity between the `System.Random` and `UnityEngine.Random` classes.

AI assistance was used to understand the cause of the error and identify the appropriate reference to resolve the conflict.

The proposed solution was reviewed and adapted to the existing project code.

This process helped improve understanding of the error and reinforced the importance of reviewing AI-generated suggestions before integrating them into the project.

## Verification and Human Oversight

All AI-generated suggestions were treated as development assistance rather than final or automatically accepted solutions.

The development team remained responsible for:

- Reviewing generated code and proposed changes.
- Adapting solutions to the existing project architecture.
- Testing scripts and functionalities within Unity.
- Identifying and correcting errors during implementation.
- Making final decisions regarding the project's design and functionality.

AI tools did not replace the development team's decision-making or responsibility for the final implementation.

## AI Usage Statement

This declaration documents the use of artificial intelligence as a support tool during the development of HydroRush.

The purpose of using these tools was to facilitate programming tasks, understand technical errors, explore possible solutions, and support the creation of certain visual resources.

The final implementation, integration, testing, and decisions regarding the project remained under the responsibility of the development team.
