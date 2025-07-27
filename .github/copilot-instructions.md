# Copilot Instructions for RimWorld Modding Project

## Mod Overview and Purpose
This RimWorld mod, tentatively titled "Rainbeau's Tribal Raiders," aims to enhance the game by introducing new tribal factions and unique raiding events. The mod is designed to expand the player's interaction with the game world by presenting new challenges and storytelling opportunities through dynamic tribal encounters.

## Key Features and Systems
- **New Tribal Factions**: The mod introduces additional tribal factions that have unique characteristics, enhancing the diversity within the game world.
- **Dynamic Raiding Events**: These factions engage in novel raiding scenarios, providing fresh tactical and strategic challenges for players.
- **Customizable Settings**: Implement a settings menu that allows players to tailor the behavior and frequency of these tribal interactions.

## Coding Patterns and Conventions
- **Class Naming**: Follow the PascalCase convention for naming classes, such as `Controller`, `Settings`, and `FactionGenerator_GenerateFactionsIntoWorld`.
- **Method Naming**: Methods should also be in PascalCase, with a clear indication of their purpose. For instance, `DoWindowContents` in the `Settings` class.
- **File Organization**: Keep related classes and methods grouped in coherent files. Utilize distinct files for your `AssemblyInfo`, `Controller`, `Settings`, and faction generation logic.

## XML Integration
- **Mod Configuration**: Use XML to configure various aspects of the mod, such as faction definitions, raid event parameters, and player-facing settings.
- **Error Handling and Parsing**: Ensure all XML files are correctly formatted to prevent parsing errors, as seen in the provided summary. Use robust error checking and documentation to assist in debugging.

## Harmony Patching
- **Static Class for Patching**: Use static classes to manage Harmony patches. For instance, the `FactionGenerator_GenerateFactionsIntoWorld` class appears to handle faction generation; ensure similar structures for other patches.
- **Modular Patches**: Keep patches modular to enhance readability and maintainability. This makes it easier for future developers to understand and modify specific game functions without inadvertently affecting others.

## Suggestions for Copilot
- **Code Completion**: Utilize Copilot for auto-completing repetitive coding patterns such as property definitions or common method signatures.
- **XML Code Generation**: Prompt Copilot to assist in writing and formatting XML files correctly to avoid syntax errors.
- **Harmony Patch Examples**: Request Copilot to suggest examples of Harmony patches, especially in RimWorld contexts, to streamline the integration process.
- **Refactoring Suggestions**: Engage Copilot in suggesting refactoring for code that could be broken down into smaller, more manageable functions or classes.

By adhering to these guidelines and leveraging GitHub Copilot, you can effectively streamline the development process and ensure high-quality contributions to the "Rainbeau's Tribal Raiders" mod.
