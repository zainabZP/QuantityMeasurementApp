## UC3 — Feet to Inch Conversion

### Objective
Compare one `Feet` value with one `Inch` value by converting both to a common base unit.

### Project Structure
```
UC3/
├── QuantityMeasurementApp/
│   ├── Models/
│   │   ├── Feet.cs
│   │   ├── Inch.cs
│   │   └── UnitConverter.cs (or conversion logic inside models)
│   └── Program.cs
└── QuantityMeasurementApp.Tests/
```

### What Was Done
- Added conversion factor: `1 Feet = 12 Inches`
- `Equals()` in `Feet` now converts both to a base unit before comparing
- Cross-type comparison supported: `new Feet(1).Equals(new Inch(12))` returns `true`

### Key Concepts
- Unit conversion logic
- Cross-type equality using a common base unit

---
