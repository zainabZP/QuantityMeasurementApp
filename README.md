## UC1 — Compare Two Feet Values

### Objective
Create a `Feet` class that compares two feet values for equality.

### Project Structure
```
UC1/
├── QuantityMeasurementApp/
│   ├── Models/
│   │   └── Feet.cs
│   └── Program.cs
└── QuantityMeasurementApp.Tests/
```

### What Was Done
- Created `Feet` class with a `value` field
- Overrode `Equals()` to compare two `Feet` objects by their numeric value
- Used `Math.Abs` with a tolerance of `0.0001` for floating-point comparison
- Overrode `GetHashCode()` for consistency
- Console program accepts two feet values and prints whether they are equal

### Key Concepts
- Value equality vs reference equality
- Overriding `Equals()` and `GetHashCode()`
- TDD with NUnit

---
