# Get A Life! 

In the current version of **PlateUp!**, the **Extra Life** appliance is notoriously absent from the standard shop pool. This mod restores it, allowing players to enjoy their runs as long as they'd like while maintaining the flexibility to fall back into the roguelike challenge by simply choosing not to use it.

Whether you're aiming for a casual "endless" experience or just want a safety net for a high-tier restaurant, *Get A Life!* puts that choice back in your hands.

---

## 🛠 Features

- **Store Restoration:** Re-introduces the Extra Life appliance into the daily blueprint pool.
- **Master Toggle:** Easily enable or disable the mod without needing to unsubscribe. If disabled, the appliance is removed from future shop pools.
- **Configurable Frequency:** Control how often the blueprint appears using standard rarity weights.
- **Custom Pricing:** Map the gold cost to a tier that fits your run's economy.
- **In-Game Settings:** Access configuration via **Options > Mods** from both the Main Menu and Pause Menu.

---

## ⚙️ Configuration Tiers

### Frequency (Rarity)
This setting determines how often the Extra Life appears compared to other items in the deck.

| Tier | Comparison | Real-world Feel |
| :--- | :--- | :--- |
| **Common** | Plunger / Floor Buffer | Shows up almost every few days. |
| **Uncommon** | Prep Station / Dining Table | A regular sight in most runs. |
| **Rare** | Robot Arm / Dishwasher | A lucky find for a prepared kitchen. |
| **Special** | **Vanilla Default** | Restores the original rare spawn rate. |

### Pricing (Price Tiers)
Gold costs are determined by tiers. Below are the specific mappings used by this mod:

| Price Tier | Cost (Gold) | Notes |
| :--- | :--- | :--- |
| **Free** | 0 | Pure emergency aid. |
| **Very Cheap** | 5 | Practically a gift. |
| **Cheap** | 20 | Similar to a Plate stack. |
| **Medium Cheap** | 40 | Standard starting appliance cost. |
| **Medium** | 60 | Similar to a basic Hob. |
| **Expensive** | 120 | **Vanilla Default**. |
| **Very Expensive** | 250 | A significant investment. |
| **Extremely Expensive** | 1250 | High-stakes survival choice. |

---

## 🛰 Requirements

This mod **requires** [KitchenLib](https://steamcommunity.com/sharedfiles/filedetails/?id=2898069883) to function. Please ensure it is installed and enabled via the Steam Workshop.

---

## ❓ Troubleshooting

### The Mod Menu is Red
A solid red box indicates a UI initialization failure.
1.  Ensure **KitchenLib** is updated to the latest version.
2.  Restart the game completely.
3.  If the problem persists, delete your local configuration file: 
    `%USERPROFILE%\AppData\LocalLow\It's Happening\PlateUp\ModData\KitchenLib\Preferences\com.redundantsemicolonmods.getalife.json`

### Submenu won't open
If the "Get A Life!" button is visible but clicking it does nothing, ensure you are running PlateUp! version **1.1.6 or higher**.

---

## 🐛 Reporting Bugs

Technical feedback is highly valued. If you encounter a crash or a bug, please raise an issue on the [GitHub Repository](https://github.com/PnModen/Get-A-Life/issues):

1.  **Locate your Log:** Find your `Player.log` file. You can find it by copy-pasting this into your Windows Explorer address bar:
    `%USERPROFILE%\AppData\LocalLow\It's Happening\PlateUp\`
2.  **Open a New Issue:** Provide a brief description of what you were doing when the bug occurred (e.g., "Changing rarity in the Main Menu").
3.  **Attach the Log:** Paste the relevant errors or upload the `Player.log` file directly to the issue.

---

*Made with ❤️ for the PlateUp! community by RedundantSemicolonMods.*