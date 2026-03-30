## UC4 — Add Yard Unit

### Objective
Add `Yard` as a new measurement unit and support its comparison with `Feet` and `Inch`.

### Project Structure
```
UC4/
├── QuantityMeasurementApp/
│   ├── Models/
│   │   ├── Feet.cs
│   │   ├── Inch.cs
│   │   └── Yard.cs
│   └── Program.cs
└── QuantityMeasurementApp.Tests/
```

### What Was Done
- Created `Yard` class with conversion: `1 Yard = 3 Feet = 36 Inches`
- Cross-unit comparison now works between Feet, Inch, and Yard
- All units convert to a base (inch) before comparison

### Conversion Table
| Unit  | In Inches |
|-------|-----------|
| 1 Feet | 12 Inches |
| 1 Yard | 36 Inches |
| 1 Inch | 1 Inch    |

---
