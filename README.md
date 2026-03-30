## UC6 — Addition of Two Quantities

### Objective
Add two quantities (possibly of different units) and return the result in a target unit.

### Project Structure
```
UC6/
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
- Added an `Add()` method that sums two quantities by converting both to base (inches)
- Supports cross-unit addition: e.g., `1 Feet + 2 Inch = 14 Inch`
- Result returned as `Inch` or base unit value

### Example
```csharp
var result = new Feet(1).Add(new Inch(2)); // 14 Inches
```

---
