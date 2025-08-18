# RailgunPowerGuide

## Description

To maintain a more realistic concept of railguns and avoid **"space magic"** The following is an example of railgun penetration analysis.

A railgun is a kinetic weapon powered by electricity, so understanding the actual feasibility of firing one is helpful to make more realistic.

- Energy per Shot (E6, E8)
  - The required energy per shot to launch the projectile at the given velocity.
  - Values assume 100% efficiency; this represents the **minimum energy needed for launch**.
  - In practical terms, calculating with 30–50% efficiency is more realistic.
  - For typical energy(laser) weapons, an efficiency of around 20% is assumed.

- Difference between `Mach 6` and `Mach 8`
  - For a 20 mm projectile, increasing from Mach 6 to Mach 8 improves penetration by roughly 37%, but increases energy requirements by about 78%.
  - A single pawn continuously firing a 30 mm railgun at Mach 8 would be clearly in the Ultra/Archotech weapon tier.
  - For typical Spacer-level technology, assuming around Mach 6 is more appropriate
- Projectile size: Caliber × (10 × Caliber) mm
  - If High damage railgun, increase overall caliber is good choice. Shooting railgun is needed pure energy. You increase length of projectile but needed more energy and also increase Penetration.
  - 50% increase length means 50% increase energy and Penetration.
  - Typically, long length projectile needed more Rail length 
- Energy Cell is based on following material : `Defs\Ammo\Lasers\H2_Fuel_Cell_Guide.md`

- **Making Railgun Projectiles**
  - You can choose whether the projectile is **cell-infused** or **non-cell-infused**.
  - **Cell-infused** means the projectile itself contains the required H₂ fuel cells, so the railgun does not need a dedicated onboard power source.
  - **Non-cell-infused** means the projectile relies entirely on the weapon’s own power supply.
  - Performance between cell-infused and non-cell-infused projectiles is almost identical, but non-cell versions are generally cheaper.
  - However, consider the practicality within RimWorld: **hand-held 20 mm railgun** without cell-infused. This weapon has an extremely advanced portable power source (something on the level where FTL travel would be easier), or the weapon has very limited shots available.
  - For this reason, non-cell railgun projectiles are classified as **Glitterworld/Archotech-tier** technology.

- Remember, this is only guide. You can freely set the power, recipe, and other thing. But this can suggest mod lessly considered as cheat something.

### Mach 6 (2040 m/s, length = 10 d)

| Caliber d (mm) | Penetration P6 (mm RHA) | Energy/Shot E6 (Wh) | Cells @100% (min) | Cells @50% | Cells @30% | Projectile Mass (g) |
| -------------: | ----------------------: | ------------------: | ----------------: | ---------: | ---------: | ------------------: |
|            2.0 |                    17.9 |                0.70 |                 1 |          1 |          1 |            **1.21** |
|      5.6 (.22) |                    50.1 |               15.39 |                 1 |          1 |          1 |           **26.62** |
|           7.62 |                    68.2 |               38.77 |                 1 |          2 |          2 |           **67.07** |
|     12.7 (.50) |                   113.6 |              179.47 |                 3 |          6 |          9 |          **310.50** |
|           20.0 |                   178.9 |              700.91 |                11 |         21 |         34 |        **1,212.65** |
|           30.0 |                   268.4 |            2,365.59 |                34 |         68 |        113 |        **4,092.71** |
|           32.0 |                   286.2 |            2,870.95 |                42 |         83 |        137 |        **4,967.03** |

### Mach 8 (2720 m/s, length = 10 d)

| Caliber d (mm) | Penetration P8 (mm RHA) | Energy/Shot E8 (Wh) | Cells @100% (min) | Cells @50% | Cells @30% | Projectile Mass (g) |
| -------------: | ----------------------: | ------------------: | ----------------: | ---------: | ---------: | ------------------: |
|            2.0 |                    24.6 |                1.25 |                 1 |          1 |          1 |            **1.21** |
|      5.6 (.22) |                    68.7 |               27.35 |                 1 |          1 |          2 |           **26.62** |
|           7.62 |                    93.5 |               68.92 |                 1 |          2 |          4 |           **67.07** |
|     12.7 (.50) |                   155.9 |              319.05 |                 5 |         10 |         16 |          **310.50** |
|           20.0 |                   245.5 |            1,246.07 |                18 |         36 |         60 |        **1,212.65** |
|           30.0 |                   368.3 |            4,205.49 |                61 |        121 |        201 |        **4,092.71** |
|           32.0 |                   392.8 |            5,103.90 |                73 |        146 |        244 |        **4,967.03** |

## Typical length of Railgun

`a` is allowable acceleration, meaning how much acceleration the gun's structure can withstand. Higher a means the weapon can accelerate the projectile faster within a shorter barrel.

| Ammunition / Weapon          | Muzzle Velocity v (m/s) | Barrel Length L (m) | Acceleration a (m/s²) | Acceleration a (g) |
| ---------------------------- | ----------------------- | ------------------- | --------------------- | ------------------ |
| 5.56×45mm NATO (M16A4)       | 930                     | 0.508               | \~851,000             | \~86,700 g         |
| 7.62×51mm NATO (FN FAL)      | 840                     | 0.533               | \~662,000             | \~67,500 g         |
| 9×19mm Parabellum (Glock 17) | 370                     | 0.114               | \~600,000             | \~61,000 g         |
| .50 BMG (M2 Browning)        | 890                     | 1.143               | \~346,000             | \~35,300 g         |
| 120mm APFSDS (M1 Abrams)     | 1,750                   | 6.6                 | \~232,000             | \~23,600 g         |

In most cases, longer projectiles require lower allowable acceleration because building a barrel strong enough to handle the higher acceleration is more difficult.

### Mach 6 (2,040 m/s)

> 10mm and 20mm have same rail length, it means acceleration is not effected in projectile.
> **important value is `Assumed a (g)` This decide Real Rail Length**
> Overall Gun Length is only recommend, Heavy firearm have more extra size

| Projectile Length L (mm) | Assumed a (g) | Rail Length (m) | Overall Gun Length (m) |
| -----------------------: | ------------: | --------------: | ---------------------: |
|                       10 |       300,000 |           0.707 |             **\~1.11** |
|                       20 |       300,000 |           0.707 |             **\~1.11** |
|                       40 |       200,000 |           1.061 |             **\~1.46** |
|                       60 |       150,000 |           1.414 |             **\~2.01** |
|                      100 |       100,000 |           2.121 |             **\~2.72** |
|                      150 |        75,000 |           2.828 |             **\~3.83** |
|                      200 |        60,000 |           3.535 |             **\~4.54** |
|                      250 |        55,000 |           3.858 |             **\~4.86** |
|                  **300** |    **50,000** |       **4.242** |             **\~5.24** |
|                      400 |        40,000 |           5.303 |             **\~6.31** |
|                      500 |        30,000 |           7.070 |             **\~8.07** |

### Mach 8 (2,720 m/s)

| Projectile Length L (mm) | Assumed a (g) | Rail Length (m) | Overall Gun Length (m) |
| -----------------------: | ------------: | --------------: | ---------------------: |
|                       10 |       300,000 |           1.257 |             **\~1.66** |
|                       20 |       300,000 |           1.257 |             **\~1.66** |
|                       40 |       200,000 |           1.885 |             **\~2.29** |
|                       60 |       150,000 |           2.514 |             **\~3.11** |
|                      100 |       100,000 |           3.771 |             **\~4.37** |
|                      150 |        75,000 |           5.028 |             **\~6.03** |
|                      200 |        60,000 |           6.283 |             **\~7.29** |
|                      250 |        55,000 |           6.754 |             **\~7.76** |
|                  **300** |    **50,000** |       **7.542** |             **\~8.54** |
|                      400 |        40,000 |           9.427 |            **\~10.43** |
|                      500 |        30,000 |          12.569 |            **\~13.57** |

- 155mm M107 projectile length is 605.3mm, if want to make 155mm railgun. it will be very long length of rail(above 20M) required.
