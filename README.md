## UC2 — Compare Feet and Inch (Same Type Comparison)

### Objective
Introduce `Inch` class and compare two inch values for equality.

### Project Structure
```
UC2/
├── QuantityMeasurementApp/
│   ├── Models/
│   │   ├── Feet.cs
│   │   └── Inch.cs
│   └── Program.cs
└── QuantityMeasurementApp.Tests/
```

### What Was Done
- Created `Inch` class similar to `Feet` with overridden `Equals()` and `GetHashCode()`
- Compared two `Inch` objects by value
- Both `Feet` and `Inch` remain independent classes at this stage

### Key Concepts
- Same pattern applied to a new unit
- Reinforcing value equality logic

---
