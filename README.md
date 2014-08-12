LeagueSharp
===========
A repo by Dibes from L#

### Support

> Easy framework to integrate all support champions under one script.

| Champions  | Status | Features |
| :----------: | :------: | :----: |
| Thresh     | Everything but W logic implemented | [Link](#thresh) |
| Morgana    | Finished | [Link](#morgana) |
| Blitzcrank | Finished | [Link](#blitzcrank) |
| Sona       | Finished  | [Link](#Sona) |


#### Thresh

  - Combo
    - Death Sentence (Q)
      - Pulls at very end of Q
      - Option to use second Q or not
    - Flay (E)
      - Flays towards thresh when enemy is in range
    - The Box (R)
      - Ult when X enemies are in range
  - Harass
    - Death Sentence (Q)
      - Same options as above
    - Flay (E)
      - Flays away from thresh when enemy is in range.
  - Anti-Gapclose
    - Flays on gapcloser
  - Interrupter
    - Death Sentence
      - Does this is out of range of E
    - Flays
  
#### Morgana

  - Combo
    - Dark Binding (Q)
      - Using basic prediction
    - Tormented Soil (W)
      - Casts if target is bound by Q
    - Soul Shackles (R)
      - Casts if X amount of Champs in Range
  - Harass
    - Dark Binding (Q)
      - Using basic prediction
    - Tormented Soil (W)
      - Casts if target is bound by Q

#### Blitzcrank

  - Finished but TODO

#### Sona

  - Combo (SBTW mode)
    - Hymn of Valor
      - Can Autocast | Casts when enemy is in range
    - Aria of Perseverance
      - Can autocast | Casts when ally is below hp threshold
    - Crescendo
      - Casts when can hit X amount of enemies
      - This may be tempermental, using an exerimental way to get amount of champs.
  - Harass (Mixed mode)
    - Hymn of Valor
      - Can Autocast | Casts when enemy is in range
    - Aria of Perseverance
      - Can autocast | Casts when ally is below hp threshold
