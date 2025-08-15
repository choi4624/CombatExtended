# H2 Fuel Cell Energy-to-Penetration Reference

Because the H2 fuel cell stores **pure energy**, its penetration capability can be directly estimated from energy output.

**Base assumption:** 1 H2 Fuel Cell (3.5 g) ≈ 20 mm RHA penetration @ Ø1 mm impact area.  
Each cell stores **70 Wh ≈ 252 kJ** of usable energy.

- Modern 50 BMG AP can penetrate 20mm RHA in 15~18kJ
- Laser weapons are generally **less efficient** than kinetic penetrators.
- In this mod, laser penetration efficiency is assumed to be **~20%**.
- For heavy weapons, the beam diameter is often **increased** to handle higher power(damage) output.
- The table below suggests the **minimum** cell usage for a given penetration.  
  Same penetration with **larger beam diameter or higher damage** is acceptable and may require **more cells**.

This document provides a **balancing reference** for weapon profiles using `Ammo_H2_Fuel_Cell` as the base ammunition.

---

## Penetration Reference Table

| Penetration (mm RHA) | Approx. Cells Required | Intended Role / Example |
|----------------------|------------------------|--------------------------|
| 20                   | 1                      | Rifle-level, modern infantry small arms penetration |
| 44                   | 3–9                    | Squad support / limited anti-armor (light vehicles, shields) |
| 100                  | 16–27                  | Light vehicle armor penetration (APC/IFV side armor) |
| 150                  | 24–40                  | Medium armor penetration (IFV/APC frontal) |
| 225                  | 36–60                  | Heavy armor penetration (modern MBT weak points) |
| 300                  | 48–80                  | Modern MBT side or armored bunker |
| 400+                 | 64–107                 | Frontal MBT composite |
| 900+                 | 324–540                | ~120 mm APFSDS equivalent, but beam diameter ~1 cm |

---

## Notes

- Values assume **ideal armor interaction**
- ±1–2 cell difference in balancing is acceptable.
- Intended for **Combat Extended** mod balance discussions and PR documentation.
