# RailgunPowerGuide

## Description

To maintain a more realistic concept of railguns and avoid “space magic,” the following is an example of railgun penetration analysis.

A railgun is a kinetic weapon powered by electricity, so understanding the actual feasibility of firing one is helpful to make more realistic.

- Energy per Shot (E₆, E₈)
  - The required energy per shot to launch the projectile at the given velocity.
  - Values assume 100% efficiency; this represents the minimum energy needed for launch.
  - In practical terms, calculating with 30–50% efficiency is more realistic.
  - For typical energy(laser) weapons, an efficiency of around 20% is assumed.

- Difference between Mach 6 and Mach 8
  - For a 20 mm projectile, increasing from Mach 6 to Mach 8 improves penetration by roughly 37%, but increases energy requirements by about 78%.
  - A single pawn continuously firing a 30 mm railgun at Mach 8 would be clearly in the Ultra/Archotech weapon tier.
  - For typical Spacer-level technology, assuming around Mach 6 is more appropriate
- Projectile size: Caliber × (2 × Caliber) mm
- Energy Cell is based on following material : `Defs\Ammo\Lasers\H2_Fuel_Cell_Guide.md`

- Making RailGun projectile
  - You can choice cell infused one or not
  - Cell infused mean Railgun is not powered by something electric reactor or power source
  - Non-Cell infused mean Railgun is powered of weapon's source
  - Non-Cell and Cell infused power is almostly same. price is cheaper in Non-Cell infused one
  - But consider your power source is acceptable in Rimworld. Hand-held 20mm Railgun without cell means, this weapon have superial energy source (may be FTL is much easier) or limited shot is available
  - So, this reason. Non-cell Railgun projectile is treated as "Gliter/Archotech" thing

### Mach 6 (2 040 m/s)

| Caliber d (mm) | Penetration P₆ (mm RHA) | Energy/Shot E₆ (Wh) | Cells @100% (min) | Cells @50% | Cells @30% |
|----------------|--------------------------|----------------------|-------------------|------------|------------|
| 2.0            | 17.9                     | 0.70                 | 1                 | 1          | 1          |
| 5.6 (.22)      | 50.1                     | 15.39                | 1                 | 1          | 1          |
| 7.62           | 68.2                     | 38.77                | 1                 | 2          | 2          |
| 12.7 (.50)     | 113.6                    | 179.47               | 3                 | 6          | 9          |
| 20.0           | 178.9                    | 700.91               | 11                | 21         | 34         |
| 30.0           | 268.4                    | 2 365.59             | 34                | 68         | 113        |
| 32.0           | 286.2                    | 2 870.95             | 42                | 83         | 137        |

### Mach 8 (2 720 m/s)

| Caliber d (mm) | Penetration P₈ (mm RHA) | Energy/Shot E₈ (Wh) | Cells @100% (min) | Cells @50% | Cells @30% |
|----------------|--------------------------|----------------------|-------------------|------------|------------|
| 2.0            | 24.6                     | 1.25                 | 1                 | 1          | 1          |
| 5.6 (.22)      | 68.7                     | 27.35                | 1                 | 1          | 2          |
| 7.62           | 93.5                     | 68.92                | 1                 | 2          | 4          |
| 12.7 (.50)     | 155.9                    | 319.05               | 5                 | 10         | 16         |
| 20.0           | 245.5                    | 1 246.07             | 18                | 36         | 60         |
| 30.0           | 368.3                    | 4 205.49             | 61                | 121        | 201        |
| 32.0           | 392.8                    | 5 103.90             | 73                | 146        | 244        |
