## 🚀 Project Spotlight

I’m beyond excited to share **Dragon Ball Path to Fire**, my Unity passion project where every leap, blast, and transformation is backed by a rock-solid, data-driven codebase—built for fast iteration and epic scale.

---

## 🏃‍♂️ Movement & Exploration

- **Run, Jump & Double-Jump**: Smooth ground controls let you chain aerial moves with precision.  
- **Flight Mode**: Toggle gravity off to soar through the skies like a true Saiyan.  
- **Dynamic Flips**: Direction-based sprite flipping keeps your hero facing the right way at all times.

---

## ⚔️ Combat & Cooldowns

- **Melee Kicks**: Lightning-fast combos with frame-perfect hit detection.  
- **Fireballs on Demand**: Pooled projectiles spawn instantly—no lag between you and your next blast.  
- **Kamehameha Charge**: Hold to power up, release for massive impact—balanced by built-in cooldowns.  
- **Unified Damage Service**: One system handles all damage calculations and ability timers for consistent tuning.

---

## ✨ Special Moves

- **Vanish Teleport**: Blink across the arena with a burst of invulnerability.  
- **Precision Dodge**: Sidestep incoming attacks, then counter before they recover.  
- **Super Saiyan Power-Up**: Collect all seven Dragon Balls to trigger an epic transformation—new stats, animations, and VFX included.

---

## 🛡️ Resources & Progression

- **Health & Respawn**: Take too many hits and trigger a dramatic defeat sequence—then jump right back in.  
- **Stamina Bar**: Abilities draw from a regenerating pool to keep combat balanced.  
- **Collectible Dragon Balls**: Glowing orbs scattered through levels fuel your path to ultimate power.

---

## 🎞️ Animations, VFX & Sound

- **Seamless Sync**: Animator events fire VFX and damage at the perfect frame—no more “out-of-sync” feels.  
- **Pooled Particles**: Energy auras, fireballs, and transformation glows all reuse assets to keep performance rock-steady.  
- **Impact Feedback**: Camera shakes, post-processing flashes, and controller rumble make every hit feel weighty.  
- **Immersive Audio**: A central audio manager handles SFX and BGM, with volumes and spatial cues defined in external assets.

---

## 🏗️ Architecture & Design Patterns

- **MVC + Service Locator**  
  – Models store game state.  
  – Views handle UI presentation.  
  – Controllers manage input and behavior.  
  – Central Registry provides access to core services.  
- **State Machine & Observer**  
  – Modular state classes govern modes (idle, combat, transformed).  
  – Event broadcasts trigger reactions like “transform” or “defeat” without tight coupling.  
- **Factory & Object Pooling**  
  – Creation and recycling systems for bullets, enemies, and effects eliminate GC spikes.  
- **Template & Strategy Patterns**  
  – Base workflows define common logic, overridden by specialized behaviors.  
- **Singleton Services**  
  – Core managers for audio, camera shakes, etc., ensure a single point of control.  
- **ScriptableObjects & Enums**  
  – All tunable values live in external configs; Enums enforce type safety.  
- **SOLID & Dependency Injection**  
  – Interfaces decouple implementations; dependencies are injected for easy testing.  
- **Encapsulation & Namespacing**  
  – Private fields with public accessors and defensive checks; code organized by namespace.  
- **Central String Management**  
  – All UI text and debug messages come from a single string manager to avoid hard-coding.

---

## 📝 A Note on the Journey

Working on **Dragon Ball Path to Fire** has been an incredible learning experience—melding cinematic action with a maintainable, extensible codebase. There’s always room to improve, and I’d love to hear about the complex systems you’re building. Let’s connect and power up our game-dev journeys together! 🔥

---

## 📦 Getting Started

1. **Clone the repo**  
   ```bash
   git clone https://github.com/your-username/Dragon-Ball-Path-to-Fire.git
