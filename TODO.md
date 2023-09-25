# Todo list

## Misc
- [x] Make the menu page show up when creating a new game
- [x] Add all the menu options we need (and then comment out the ones not in use)
- [ ] Can we make a popup in the corner (after the item get popup) that shows what charms/spell/art/dir was unequipped when we replace stuff? Investigate that.

## Logic

- [ ] Charms
  - [x] Make auto-equipping and unequipping function
  - [x] Make sure the player can look at the charms, but cannot manually equip/unequip them.
  - [ ] Investigate the behaviour of upgradeable charms and treat them accordingly
    - [ ] Grimmchild - do upgrades change its notches? In vanilla I think they do; accomodate. Do they in randomized notch costs as well?
    - [ ] Kingsoul - getting one fragment equips it; we need to treat this as a special case and not add it to history until its completed.
    - [ ] Voidheart - Does the upgrade change the notch cost? If so, we ought to requipcharms on upgrade.
    - [ ] Fragile charms - keep equipped when broken, or don't? do we still consider them to be taking up notch space even if broken? inclined to say yes, they still count... maybe this deserves an option? either they stick to their place in history regardless of breaks/repairs, or they get unequipped on breaks and re-appended to the end of charm history on repair.
    - [ ] Upgrade process for all upgradeable charms - Count it as a new charm. Remove its ID from history, re-add at end of list, equip it. 
  - [ ] Make sure checks with charm-equipped requirements function if you *possess* the charm they need, regardless of equip status
    - [x] Lifeblood door in the abyss
    - [ ] Birthplace
    - [ ] Lore tablets in Fungal Wastes (+ Mr Mushroom lore tablet)
    - [ ] Fragile charms with Divine
      - Requires a patch to locations; we need to create a custom class DivineLocationUwyg and replace the locations for UB heart, greed, strength with instances of them. you will need to reimplement EditSlotSpecificPath so it edits divine's fsm to let you have the check if you *acquired* the prereq charm.
      - https://github.com/homothetyhk/HollowKnight.ItemChanger/blob/48b77ce35ef27be53788b5dfc9bf930e4e2de576/ItemChanger/Resources/locations.json#L915
    - [ ] Grimmchild with Nightmare Troupe progression
      - Grimmkin novices, masters, and nightmares need to appear
      - Grimm needs to appear and upgrade grimmchild1 if you have the flames for it
      - Grimm needs to appear and fight you if you have grimmchild2 and the flames for it
      - Sleeping Grimm needs to let you into the dream if you have grimmchild3 and the flames for it
    - [ ] More?
  - [ ] Make sure Baldurs die automatically if your only in-logic way of killing them is an unequipped charm
  - [ ] Check if any skips or other progression require specific charms to be equipped, and figure out how to accomodate/warn about those cases.
  - [ ] Re-order charms UI so that the charms show up in the order you picked them up in, to improve clairity of what's happening in-game.

- [ ] Spells
  - [ ] Track most recent spell, prevent use of others
  - [ ] Enforce no Fireball/Scream Skip logic when enabled.
  - [ ] Enforce no Shriek Pogo skip logic when enabled.
  - [ ] Enforce no Slopeball skip logic when enabled.
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
  - [x] **Allow Overcharming** *(default: false)* - Auto-equipping no longer cares about the cost of the charm you picked up. If there's one free notch, or one notch can be freed by unequipping old charms, the new charm WILL be equipped. 
  - [x] **Keep Notches Filled** *(default: false)* - If you pick up a notch, and it creates enough room for the most recently unequipped charm(s), they will be re-equipped to fill up the space.
- [ ] **Spells** - Only your most recently obtained spell is usable. Good luck! *(Note: Using this option with Fireball Skips on can hardlock you. Breakable floors will break in response to any spell being used while you're standing on them.)*
  - [ ] **Replace Junk With Extra Spells** *(default: 0)* - Adds the specified number of extra copies of each spell to the pool of junk items, for some extra spice.
- [ ] **Nail Arts** - Only your most recently obtained nail art is usable. Good luck!
  - [ ] **Replace Junk With Extra Arts** *(default: 0)* - Adds the specified number of extra copies of each nail art to the pool of junk items, for some extra spice.
- [ ] **Nail Directions** - When used with Split Nail, then aside from pogoing, only your most recently obtained nail direction is usable. Just... why?
  - [ ] **Replace Junk With Extra Nail Directions** *(default: 0)* - Adds the specified number of extra copies of each nail direction to the pool of junk items, for some extra spice.
