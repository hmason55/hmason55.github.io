---
# This page uses Hydejack's `about` layout, which shows the primary author's picture and about text at the top.
# You can change it to the regular `page` layout if you want.
layout: page

# The title of the page.
title: Play Guide

# Write a short (~150 characters) description of each blog post.
# This description is used to preview the page on search engines, social media, etc.
description: >
  **Here you can learn the gameplay basics.**

hide_description: false

---

# Table of Contents

1. [**Movement**](/guide/#movement)
2. [**User Interface**](/guide/#user-interface)
	* [Resources](/guide/#resources)
	* [Menu Shortcuts](/guide/#menu-shortcuts)
	* [Profile](/guide/#profile)
	* [Bag](/guide/#bag)
	* [Spells](/guide/#spells)
3. [**Items**](/guide/#bag)
	* [Equipment](/guide/#equipment)
	* [Consumables](/guide/#consumables)
4. [**Combat**](/guide/#combat)
	* [Status Effects](/guide/#status-effects)
	* [Telegraphs](/guide/#telegraphs)
5. [**Objects**](/guide/#objects)
	* [Containers](/guide/#containers)
	* [Loot](/guide/#loot)
	* [Exits](/guide/#exits)

# Movement
Units can move in 4 directions but not through other units, walls, or other objects. Movement is available both in and out of combat but only on the unit's turn. Moving while in combat costs 1 [**Essence**](/guide/#resources) for each tile moved.

![Directional Movement](/assets/img/guide/directional_movement.png)

<br/>
The default key bindings for movement actions are:

| Direction | Key |
|-----------|-----|
| Up        | W   |
| Down      | S   |
| Left      | A   |
| Right     | D   |

<br/>
# User Interface
The user interface displays relevant information to the player about their current status.

## Resources
Your character's Health and Essence are displayed here at the bottom of the screen. Health is displayed as a red bar and can be restored by returning to the Hub, or by using healing [**Items**](/guide/#items). If your character's Health reaches 0, they will be defeated and taken back to the Hub after dropping all Gold they are carrying. Essence is displayed as solid blue spheres above the Health bar, and it is restored at the beginning of each turn. Essence 

![Resources](/assets/img/guide/resources.png)

<br/>
## Menu Shortcuts
The menu shortcuts can be found at the top of the screen, and can be used to open the Menu, [**Profile**](/guide/#profile), and [**Bag**](/guide/#bag) interfaces. You can also end your turn here.

![Shortcuts](/assets/img/guide/shortcuts.png)

<br/>
The default key bindings for movement actions are:

| Interface | Key |
|-----------|-----|
| Menu      | Esc |
| Profile   | C   |
| Bag       | B   |
| End Turn  | T   |

<br/>
The **Help** shortcut can be used to open this Play Guide at any time while playing the game.

![Help Shortcut](/assets/img/guide/help_shortcut.png)

<br/>
## Profile
The profile interface displays your character's current attributes.

![Profile](/assets/img/guide/profile.png)

<br/>
## Bag
The bag interface displays all the items your character is carrying, and equipment they are wearing.

![Bag](/assets/img/guide/bag.png)

<br/>
Interface panels such as the [**Bag**](/guide/#bag) or [**Profile**](/guide/#profile) can either be closed by clicking the red button at the upper right corner, or by pressing the **Escape** key.

![Close UI](/assets/img/guide/close_ui.png)

<br/>
## Spells
Spells are acquired from items that the player equips throughout their adventure. The currently available spells will appear at the bottom of the screen.

![Spell Hotbar](/assets/img/guide/spell_hotbar.png)

<br/>
Clicking on spells or pressing the corresponding key binding will ready the spell, showing an area in which the spell can be cast.

![Ready Tiles](/assets/img/guide/ready_cast.png)

<br/>
Clicking one of the available tiles will display an area where that the spell will affect.

![Confirm Tiles](/assets/img/guide/confirm_cast.png)

<br/>
When a tile in this area is clicked, the spell will be cast and corresponding [**Essence**](/guide/#resources) will be spent.

![Cast Spell](/assets/img/guide/cast_spell.png)

<br/>
Spell information can be displayed by hovering over the `+` icon in the upper right corner of each spell button.

![Spell Info](/assets/img/guide/spell_tooltip.png)

<br/>
The default key bindings for spell actions are:

| Spell | Key |
|-------|-----|
| 1     | 1   |
| 2     | 2   |
| 3     | 3   |
| 4     | 4   |
| 5     | 5   |
| 6     | 6   |
| 7     | 7   |
| 8     | 8   |
| 9     | 9   |
| 10    | 10  |

<br/>
# Items
Items can be found in [**Containers**](/guide/#containers) and dropped from enemies inside [**Loot Bags**](/guide/#loot). Hovering the mouse of items will display a tool tip containing more information about that item.

## Equipment
Equipment items grant the wearer spells and bonus attributes. Players can view their equipment in the [**Bag**](/guide/#bag) interface. Clicking on equipment will equip or unequip the item.

![Equipment](/assets/img/guide/equipment.png)

<br/>
## Consumables
Consumable items have various effects and can be used while in or out of combat during the player's turn.

<br/>
# Combat
Combat begins when a unit moves too close to, or deals damage to another unit. Both units involved will enter combat, and units near those involved will be drawn into combat as well. Combat will conclude if the player dies, all enemies involved in combat are defeated, or the player exits the level.

## Status Effects
Units can gain status effects through the use of [**Spells**](/guide/#spells). There are a variety of status effects, some of which can be removed through the use of [**Consumable Items**](/guide/#consumables).

![Status Effects](/assets/img/guide/status_effects.png)

<br/>
## Telegraphs
Enemies will display an icon above themselves after each turn. This icon represents what the enemy will attempt to do on their next turn. For instance, this skeleton will attempt to attack for 4 damage assuming it can reach its target.

![Telegraph](/assets/img/guide/enemy_telegraph.png)

<br/>
# Objects
Objects such as [**Containers**](/guide/#containers), [**Loot Bags**](/guide/#loot), and Exits can be found inside every dungeon. They can be interacted with by hovering over with the mouse and clicking.

## Containers
Containers often hold treasures such as gold, items, and keys.

![Container](/assets/img/guide/container.png)

<br/>
## Loot
Loot is occasionally dropped by enemy units when defeated. Loot bags will usually contain gold or [**Items**](/guide/#items).

![Loot Bag](/assets/img/guide/loot.png)

<br/>
## Exits
Exits are initially locked, and require a specific key to open.

![Exit Sealed](/assets/img/guide/exit_sealed.png)

<br/>
Clicking on the exit with the corresponding key in your inventory will unlock the exit and allow you to proceed to the next level.

![Exit Activated](/assets/img/guide/exit_activated.png)


