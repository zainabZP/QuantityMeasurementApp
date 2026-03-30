## UC5 — Add Centimeter Unit

### Objective
Add `Centimeter` as a unit and support conversion and comparison with other units.

### Project Structure
```
UC5/
├── QuantityMeasurementApp/
│   ├── Models/
│   │   ├── Feet.cs
│   │   ├── Inch.cs
│   │   ├── Yard.cs
│   │   └── Centimeter.cs
│   └── Program.cs
└── QuantityMeasurementApp.Tests/
```

### What Was Done
- Created `Centimeter` class with conversion: `2.5 cm = 1 Inch`
- All four units can now be compared against each other
- Refactored common conversion logic to avoid duplication

### Conversion Table
| Unit        | In Inches |
|-------------|-----------|
| 1 Feet      | 12 Inches |
| 1 Yard      | 36 Inches |
| 2.5 cm      | 1 Inch    |

---
