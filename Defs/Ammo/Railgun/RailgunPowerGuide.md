# RailgunPowerGuide

## Description

To maintain a more realistic concept of railguns and avoid “space magic,” the following is an example of railgun penetration analysis.

A railgun is a kinetic weapon powered by electricity, so understanding the actual feasibility of firing one is critical.

- **Energy per Shot (E₆, E₈)**  
  - 레일건을 해당 속도로 투사하기 위한 발당 에너지 요구량
  - 에너지 효율 100% 기준으로 최소한 이 에너지가 있어야 투사가 가능하다는 의미
  - 30~50%를 가정하고 실제 필요 에너지를 계산하는 것이 적절함
  - 에너지 무기의 효율은 20%
- 마하 6과 마하 8의 차이
  - 20 mm 탄 기준 Mach 6 → Mach 8로 변경 시 관통력은 약 37% 증가하지만, 에너지 요구량은 약 78% 증가.
  - 단일 폰이 30mm 레일건을 마하 8 속도로 지속투사하는 것은 명백한 울트라/아르코테크 레벨의 무기
  - 일반적인 Spacer 수준에서는 Mach 6 정도의 투사를 한다고 가정하는 것이 적절함.
- 탄두의 크기
  - Caliber * (2x of Caliber)mm


### Mach 6 (2 040 m/s)

| Caliber d (mm) | Penetration Depth P₆ (mm RHA) | Energy per Shot E₆ (Wh) | Total Energy for 300 Shots (Wh) |
|----------------|-------------------------------|-------------------------|----------------------------------|
| 2.0            | 17.9                          | 0.70                    | 210                              |
| 5.6 (.22)      | 50.1                          | 15.39                   | 4 616                            |
| 7.62           | 68.2                          | 38.77                   | 11 630                           |
| 12.7 (.50)     | 113.6                         | 179.47                  | 53 840                           |
| 20.0           | 178.9                         | 700.91                  | 210 274                          |
| 30.0           | 268.4                         | 2 365.59                 | 709 676                          |
| 32.0           | 286.2                         | 2 870.95                 | 861 284                          |

---

### Mach 8 (2 720 m/s)

| Caliber d (mm) | Penetration Depth P₈ (mm RHA) | Energy per Shot E₈ (Wh) | Total Energy for 300 Shots (Wh) |
|----------------|-------------------------------|-------------------------|----------------------------------|
| 2.0            | 24.6                          | 1.25                    | 374                              |
| 5.6 (.22)      | 68.7                          | 27.35                   | 8 206                            |
| 7.62           | 93.5                          | 68.92                   | 20 675                           |
| 12.7 (.50)     | 155.9                         | 319.05                   | 95 716                           |
| 20.0           | 245.5                         | 1 246.07                 | 373 821                          |
| 30.0           | 368.3                         | 4 205.49                 | 1 261 646                        |
| 32.0           | 392.8                         | 5 103.90                 | 1 531 171                        |
