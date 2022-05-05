# Elysium.Items

## Overview

This package aims to facilitate the process of receiving and handling game items. In order to do this, it defines 3 main entities:

1- Item: the objects to be held in an inventory.
2- Inventory: a finite or infinite collection of items.
3- Crate: a random item generator, based on a list of available items and probabilities.

There is also support for generating a user interface for an inventory, using Unity's UGUI.

#### Item

Items are the objects that will be held by the user. 

Every item has an ItemID (which is shared amongst all items of the same type), as well as an InstanceID (which is unique to each item). As an example, lets say we create two types of items, an 'Apple' and a 'Banana'. All apples will share the same ItemID, and all bananas will share a different ItemID. However, each apple and each banana will have a unique InstanceID.

Items also have a Max Stack property, which will define the maximum amount of items that can be held in the same Item Stack. As an example, each item that has a Max Stack value of 1 will require a separate inventory slot to store, while items with a Max Stack value of 10 can have 10 copies stored in each inventory slot.

Lastly, items can be used by an Item User. Items will be used from a specific Item Stack, and Item Users can be casted to their subclass to implement specific behaviours. You can find a practical example in the 'Inventory' demo scene.

#### Inventory

Inventories are a collection of Item Stacks, each of which can hold a determined amount of items. There are 2 main types of inventories: limited and unlimited. Unlimited inventories will hold an infinite amount of items, while limited inventories only have a certain number of item stacks. That being said, limited inventories can be expanded, in case the user is intended to increase their inventory size throughout their game.

There are currently 4 default types of inventories currently implemented:

- LimitedInventory
- UnlimitedInventory
- LimitedInventorySO (supports persistency)
- UnlimitedInventorySO (supports persistency)

Scriptable Object inventories support data persistency (Save/Load). They can be saved and/or loaded by accessing their IPersistent methods: *void Load(ILoader _loader)* and *void Save(ISaver _saver)*. This package includes a default ISaver and ILoader class of type SaveSystem, which currently saves the data as json or binary to Unity's PlayerPrefs. You can find a practical example in the 'Inventory' demo scene.

#### Crates

This package also includes 'crates', which generate items based on a random item pool. There are currently 2 types of crates:

- UniqueCrate
- MultiCrate

Unique crates will only return one item result from the pool, while multi crates handle each item's probability individually, therefore being able to return multiple items. You can find a practical example in the 'Crates' demo scene.

#### User Interface

An interface can be generated for an inventory by using a VisualInventory. Visual inventories can be opened with filters (to show only specific types of items - such as weapons), and can have item use events associated to them (to trigger specific behaviours when a user clicks on an item in the UI). 

This package also contains a class called VisualInventoryOverride, which can be used to open a user's inventory while overriding the items' default "use" behaviour. An example could be when you need the user to select an item to use as material for crafting. This way, an override action (add the potion as crafting material) can be used instead of the item's default action (healing health). You can find a practical example in the 'Inventory' demo scene.