# Todo list

## Misc
- [ ] Make the menu page show up when creating a new game
- [ ] Add all the menu options we need (and then comment out the ones not in use)
- [ ] Can we make a popup in the corner (after the item get popup) that shows what charms/spell/art/dir was unequipped when we replace stuff? Investigate that.

## Logic

- [ ] Charms
  - [x] Make auto-equipping and unequipping function
  - [ ] Make sure the player can look at the charms, but cannot manually equip/unequip them.
  - [ ] Make sure checks with charm-equipped requirements function if you *possess* the charm they need, regardless of equip status
    - [ ] Lifeblood door in the abyss
    - [ ] Birthplace
    - [ ] Lore tablets in Fungal Wastes (+ Mr Mushroom lore tablet)
    - [ ] Fragile charms with Divine
    - [ ] Grimmchild with Nightmare Troupe progression
    - [ ] More?
  - [ ] Make sure Baldurs die automatically if your only in-logic way of killing them is an unequipped charm
  - [ ] Check if any skips or other progression require specific charms to be equipped, and figure out how to accomodate/warn about those cases.
- [ ] Spells
  - [ ] Track most recent spell, prevent use of others
  - [ ] If possible, enforce no Fireball/Scream Skip logic when enabled. If not, make sure to warn that the combo can create unbeatable games.
  - [ ] Make floors break in response to any spell, provided ddive or ddark have been picked up. (for better coverage, maybe just test if a location only accessible by ddive/ddark is in logic)
  - [ ] Check for other progression issues that removal of access to spells can cause and decide how to accomodate.
  - [ ] Make sure Baldur auto-kill logic includes "unequipped" spells
  - [ ] Determine what the rando thinks is junk, and replace some junk with copies. If there's not enough junk to cover the extras requested, then some junk locations get multiple shinies now, if that's possible.
- [ ] Nail Arts
  - [ ] Track most recent art, prevent use of others.
  - [ ] Ensure there's no skips/etc that are part of rando logic that use arts; if there are, we must plan how to accomodate or warn about them.
  - [ ] Make sure Baldur auto-kill logic includes "unequipped" arts
  - [ ] Determine what the rando thinks is junk, and replace some junk with copies. If there's not enough junk to cover the extras requested, then some junk locations get multiple shinies now, if that's possible.
- [ ] Nail Directions
  - [ ] Track most recent direction, prevent use of others (except down)
  - [ ] Ensure there's no skips/etc that are part of rando logic that use specific nail directions other than down; if there are, we must plan how to accomodate or warn about them.
  - [ ] Make sure Baldur auto-kill logic includes "unequipped" directions
  - [ ] 
  - [ ] 
  - [ ] Determine what the rando thinks is junk, and replace some junk with copies. If there's not enough junk to cover the extras requested, then some junk locations get multiple shinies now, if that's possible.
  - [ ] 
  - [ ] 
  - [ ] 

## Options

- [ ] **Charms** - Automatically keeps your most recently picked up charms equipped. You have to use what you get!
  - [ ] **Allow Overcharming** *(default: false)* - Auto-equipping no longer cares about the cost of the charm you picked up. If there's one free notch, or one notch can be freed by unequipping old charms, the new charm WILL be equipped. 
  - [ ] **Keep Notches Filled** *(default: false)* - If you pick up a notch, and it creates enough room for the most recently unequipped charm(s), they will be re-equipped to fill up the space.
- [ ] **Spells** - Only your most recently obtained spell is usable. Good luck! *(Note: Using this option with Fireball Skips on can hardlock you. Breakable floors will break in response to any spell being used while you're standing on them.)*
  - [ ] **Replace Junk With Extra Spells** *(default: 0)* - Adds the specified number of extra copies of each spell to the pool of junk items, for some extra spice.
- [ ] **Nail Arts** - Only your most recently obtained nail art is usable. Good luck!
  - [ ] **Replace Junk With Extra Arts** *(default: 0)* - Adds the specified number of extra copies of each nail art to the pool of junk items, for some extra spice.
- [ ] **Nail Directions** - When used with Split Nail, then aside from pogoing, only your most recently obtained nail direction is usable. Just... why?
  - [ ] **Replace Junk With Extra Nail Directions** *(default: 0)* - Adds the specified number of extra copies of each nail direction to the pool of junk items, for some extra spice.
