# Offline Reward Popup (Unity Case)

This project showcases a clean and testable **Offline Reward system** built with Unity.
It demonstrates both **development-time simulation** and **production-like realtime (UTC) behavior** using the same core architecture.

> 🎥 Showcase Video



https://github.com/user-attachments/assets/0b9b30bf-de2c-4683-b4c6-8b33446c7bf3



---


## Overview

The project is structured around **two scenes**:

- **Development Scene** – fast iteration and testing
- **Realtime UTC Scene** – production-like offline reward logic

Both scenes share the same UI and reward logic.  
Only the **time source** differs.

---

## Core Architecture

The system is designed with clear responsibility separation:

- **Timer**
  - Tracks offline duration
  - Can be simulated (development) or based on real UTC time

- **RewardService**
  - Calculates rewards based on offline duration
  - Adds rewards to the Wallet
  - Handles the timer on collect

- **Wallet**
  - Stores player resources (Coins / Hammers)
  - Loads and saves values via JSON

- **OfflineRewardPopUpUI**
  - Handles all UI updates
  - Displays duration, rewards, buttons and progress bars

- **SaveLoadManager**
  - Centralized JSON save/load utility
  - Stores last collect time and wallet data

---

## Development Scene

**Purpose:**  
Rapid testing without waiting for real time.

**Behavior:**
- Offline duration is simulated using a development timer
- Time speed can be adjusted via debug controls

This scene is intended purely for **debugging and iteration**.

---

## Realtime UTC Scene

**Purpose:**  
Demonstrate a production-ready offline reward flow.

**Behavior:**
- Offline duration is calculated using **UTC Unix time**
- Last collect time and wallet values are persisted via **JSON**
- Rewards accumulate while the app is closed or unfocused.

The system can be tested by **changing the device time**.
External time APIs were **intentionally not used**, despite being a valid alternative.

---


## UI & Feedback

- Clean and scalable layout
- Collect button automatically disables when no rewards are available
- DOTween-based UI burst effect on collect

---

## Persistence

- Data is stored in a single JSON file
- Saved values:
  - Last collect UTC timestamp
  - Wallet coins
  - Wallet hammers
- Save data is easy to inspect during development

---

## Key Design Decisions

- Single RewardService shared across both scenes
- Abstract timer design to support multiple time sources
- Focus on readability and maintainability over over-engineering

---

## Tech Stack

- Unity
- C#
- TextMeshPro
- DOTween
- JSON serialization (local save)

