// using QM.Models.DTOs;
// using QM.Models.Entities;
// using QM.Models.Exceptions;
// using QM.Repository.Interface;
// using QM.Repository.Repository;
// using QM.BusinessLogic.Interface;  

// namespace QM.BusinessLogic.Service
// {
//     public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
//     {
//         private readonly IQuantityMeasurementRepository _repository;

//         // Dependency Injection via constructor
//         public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
//         {
//             _repository = repository ?? throw new ArgumentNullException(nameof(repository));
//         }

//         // ══════════════════════════════════════════
//         // COMPARE
//         // ══════════════════════════════════════════
//         public QuantityDTO Compare(QuantityDTO q1, QuantityDTO q2)
//         {
//             try
//             {
//                 ValidateNotNull(q1, q2);

//                 if (!IsSameCategory(q1.MeasurementType, q2.MeasurementType))
//                     throw new QuantityMeasurementException(
//                         $"Cross-category comparison not allowed: {q1.MeasurementType} vs {q2.MeasurementType}");

//                 double base1 = ToBaseValue(q1);
//                 double base2 = ToBaseValue(q2);
//                 bool equal = Math.Abs(base1 - base2) < 0.0001;

//                 var result = new QuantityDTO(equal ? 1 : 0, "SCALAR", "Scalar");
//                 _repository.Save(new QuantityMeasurementEntity("Compare", q1, q2, equal ? 1.0 : 0.0));
//                 return result;
//             }
//             catch (QuantityMeasurementException)
//             {
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 throw new QuantityMeasurementException("Compare failed: " + ex.Message, ex);
//             }
//         }

//         // ══════════════════════════════════════════
//         // CONVERT
//         // ══════════════════════════════════════════
//         public QuantityDTO Convert(QuantityDTO source, string targetUnit)
//         {
//             try
//             {
//                 if (source == null) throw new QuantityMeasurementException("Source quantity cannot be null.");
//                 if (string.IsNullOrWhiteSpace(targetUnit))
//                     throw new QuantityMeasurementException("Target unit cannot be empty.");

//                 double baseValue = ToBaseValue(source);
//                 double converted = FromBaseValue(baseValue, targetUnit, source.MeasurementType);

//                 var result = new QuantityDTO(converted, targetUnit, source.MeasurementType);
//                 _repository.Save(new QuantityMeasurementEntity("Convert", source, result));
//                 return result;
//             }
//             catch (QuantityMeasurementException)
//             {
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 throw new QuantityMeasurementException("Convert failed: " + ex.Message, ex);
//             }
//         }

//         // ══════════════════════════════════════════
//         // ADD
//         // ══════════════════════════════════════════
//         public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2, string targetUnit)
//         {
//             try
//             {
//                 ValidateNotNull(q1, q2);
//                 ValidateSameCategory(q1, q2, "Add");
//                 ValidateArithmeticSupported(q1.MeasurementType, "Add");

//                 double sum = ToBaseValue(q1) + ToBaseValue(q2);
//                 double converted = FromBaseValue(sum, targetUnit, q1.MeasurementType);

//                 var result = new QuantityDTO(converted, targetUnit, q1.MeasurementType);
//                 _repository.Save(new QuantityMeasurementEntity("Add", q1, q2, result));
//                 return result;
//             }
//             catch (QuantityMeasurementException)
//             {
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 throw new QuantityMeasurementException("Add failed: " + ex.Message, ex);
//             }
//         }

//         // ══════════════════════════════════════════
//         // SUBTRACT
//         // ══════════════════════════════════════════
//         public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2, string targetUnit)
//         {
//             try
//             {
//                 ValidateNotNull(q1, q2);
//                 ValidateSameCategory(q1, q2, "Subtract");
//                 ValidateArithmeticSupported(q1.MeasurementType, "Subtract");

//                 double diff = ToBaseValue(q1) - ToBaseValue(q2);
//                 double converted = FromBaseValue(diff, targetUnit, q1.MeasurementType);

//                 var result = new QuantityDTO(converted, targetUnit, q1.MeasurementType);
//                 _repository.Save(new QuantityMeasurementEntity("Subtract", q1, q2, result));
//                 return result;
//             }
//             catch (QuantityMeasurementException)
//             {
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 throw new QuantityMeasurementException("Subtract failed: " + ex.Message, ex);
//             }
//         }

//         // ══════════════════════════════════════════
//         // DIVIDE
//         // ══════════════════════════════════════════
//         public QuantityDTO Divide(QuantityDTO q1, QuantityDTO q2)
//         {
//             try
//             {
//                 ValidateNotNull(q1, q2);
//                 ValidateSameCategory(q1, q2, "Divide");
//                 ValidateArithmeticSupported(q1.MeasurementType, "Divide");

//                 double base2 = ToBaseValue(q2);
//                 if (Math.Abs(base2) < 0.0000001)
//                     throw new QuantityMeasurementException("Division by zero is not allowed.");

//                 double quotient = ToBaseValue(q1) / base2;

//                 var result = new QuantityDTO(quotient, "SCALAR", "Scalar");
//                 _repository.Save(new QuantityMeasurementEntity("Divide", q1, q2, quotient));
//                 return result;
//             }
//             catch (QuantityMeasurementException)
//             {
//                 throw;
//             }
//             catch (Exception ex)
//             {
//                 throw new QuantityMeasurementException("Divide failed: " + ex.Message, ex);
//             }
//         }

//         // ══════════════════════════════════════════
//         // PRIVATE HELPERS
//         // ══════════════════════════════════════════

//         private double ToBaseValue(QuantityDTO q)
//         {
//             return q.MeasurementType.ToLower() switch
//             {
//                 "length" => q.Unit.ToUpper() switch
//                 {
//                     "INCHES"      => q.Value,
//                     "FEET"        => q.Value * 12,
//                     "YARDS"       => q.Value * 36,
//                     "YARD"        => q.Value * 36,
//                     "CENTIMETERS" => q.Value / 2.54,
//                     "CM"          => q.Value / 2.54,
//                     _ => throw new QuantityMeasurementException($"Unknown length unit: {q.Unit}")
//                 },
//                 "weight" => q.Unit.ToUpper() switch
//                 {
//                     "GRAM"     => q.Value,
//                     "KILOGRAM" => q.Value * 1000,
//                     "KG"       => q.Value * 1000,
//                     "POUND"    => q.Value * 453.592,
//                     _ => throw new QuantityMeasurementException($"Unknown weight unit: {q.Unit}")
//                 },
//                 "volume" => q.Unit.ToUpper() switch
//                 {
//                     "MILLILITRE" => q.Value,
//                     "ML"         => q.Value,
//                     "LITRE"      => q.Value * 1000,
//                     "GALLON"     => q.Value * 3785.41,
//                     _ => throw new QuantityMeasurementException($"Unknown volume unit: {q.Unit}")
//                 },
//                 "temperature" => q.Unit.ToUpper() switch
//                 {
//                     "KELVIN"     => q.Value,
//                     "CELSIUS"    => q.Value + 273.15,
//                     "FAHRENHEIT" => (q.Value - 32) * 5.0 / 9.0 + 273.15,
//                     _ => throw new QuantityMeasurementException($"Unknown temperature unit: {q.Unit}")
//                 },
//                 _ => throw new QuantityMeasurementException($"Unknown measurement type: {q.MeasurementType}")
//             };
//         }

//         private double FromBaseValue(double baseValue, string targetUnit, string measurementType)
//         {
//             return measurementType.ToLower() switch
//             {
//                 "length" => targetUnit.ToUpper() switch
//                 {
//                     "INCHES"      => baseValue,
//                     "FEET"        => baseValue / 12,
//                     "YARDS"       => baseValue / 36,
//                     "YARD"        => baseValue / 36,
//                     "CENTIMETERS" => baseValue * 2.54,
//                     "CM"          => baseValue * 2.54,
//                     _ => throw new QuantityMeasurementException($"Unknown length unit: {targetUnit}")
//                 },
//                 "weight" => targetUnit.ToUpper() switch
//                 {
//                     "GRAM"     => baseValue,
//                     "KILOGRAM" => baseValue / 1000,
//                     "KG"       => baseValue / 1000,
//                     "POUND"    => baseValue / 453.592,
//                     _ => throw new QuantityMeasurementException($"Unknown weight unit: {targetUnit}")
//                 },
//                 "volume" => targetUnit.ToUpper() switch
//                 {
//                     "MILLILITRE" => baseValue,
//                     "ML"         => baseValue,
//                     "LITRE"      => baseValue / 1000,
//                     "GALLON"     => baseValue / 3785.41,
//                     _ => throw new QuantityMeasurementException($"Unknown volume unit: {targetUnit}")
//                 },
//                 "temperature" => targetUnit.ToUpper() switch
//                 {
//                     "KELVIN"     => baseValue,
//                     "CELSIUS"    => baseValue - 273.15,
//                     "FAHRENHEIT" => (baseValue - 273.15) * 9.0 / 5.0 + 32,
//                     _ => throw new QuantityMeasurementException($"Unknown temperature unit: {targetUnit}")
//                 },
//                 _ => throw new QuantityMeasurementException($"Unknown measurement type: {measurementType}")
//             };
//         }

//         private void ValidateNotNull(QuantityDTO q1, QuantityDTO q2)
//         {
//             if (q1 == null) throw new QuantityMeasurementException("First quantity cannot be null.");
//             if (q2 == null) throw new QuantityMeasurementException("Second quantity cannot be null.");
//         }

//         private bool IsSameCategory(string type1, string type2)
//         {
//             return string.Equals(type1, type2, StringComparison.OrdinalIgnoreCase);
//         }

//         private void ValidateSameCategory(QuantityDTO q1, QuantityDTO q2, string operation)
//         {
//             if (!IsSameCategory(q1.MeasurementType, q2.MeasurementType))
//                 throw new QuantityMeasurementException(
//                     $"{operation} not allowed between {q1.MeasurementType} and {q2.MeasurementType}.");
//         }

//         private void ValidateArithmeticSupported(string measurementType, string operation)
//         {
//             if (string.Equals(measurementType, "temperature", StringComparison.OrdinalIgnoreCase))
//                 throw new QuantityMeasurementException(
//                     $"Temperature does not support {operation} operation.");
//         }
//     }
// }


using QM.Models.DTOs;
using QM.Models.Entities;
using QM.Models.Exceptions;
using QM.Repository.Interface;
using QM.BusinessLogic.Interface;  

namespace QM.BusinessLogic.Service
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // ══════════════════════════════════════════
        // COMPARE
        // ══════════════════════════════════════════
        public QuantityDTO Compare(QuantityDTO q1, QuantityDTO q2)
        {
            try
            {
                ValidateNotNull(q1, q2);

                if (!IsSameCategory(q1.MeasurementType, q2.MeasurementType))
                    throw new QuantityMeasurementException(
                        $"Cross-category comparison not allowed: {q1.MeasurementType} vs {q2.MeasurementType}");

                double base1 = ToBaseValue(q1);
                double base2 = ToBaseValue(q2);
                bool equal = Math.Abs(base1 - base2) < 0.0001;

                var result = new QuantityDTO(equal ? 1 : 0, "SCALAR", "Scalar");

                _repository.Save(new QuantityMeasurementEntity
                {
                    OperationType = "Compare",
                    Operand1      = q1.ToString(),
                    Operand2      = q2.ToString(),
                    ScalarResult  = equal ? 1.0 : 0.0,
                    HasError      = false,
                    Timestamp     = DateTime.UtcNow
                });

                return result;
            }
            catch (QuantityMeasurementException) { throw; }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException("Compare failed: " + ex.Message, ex);
            }
        }

        // ══════════════════════════════════════════
        // CONVERT
        // ══════════════════════════════════════════
        public QuantityDTO Convert(QuantityDTO source, string targetUnit)
        {
            try
            {
                if (source == null)
                    throw new QuantityMeasurementException("Source quantity cannot be null.");
                if (string.IsNullOrWhiteSpace(targetUnit))
                    throw new QuantityMeasurementException("Target unit cannot be empty.");

                double baseValue  = ToBaseValue(source);
                double converted  = FromBaseValue(baseValue, targetUnit, source.MeasurementType);

                var result = new QuantityDTO(converted, targetUnit, source.MeasurementType);

                _repository.Save(new QuantityMeasurementEntity
                {
                    OperationType = "Convert",
                    Operand1      = source.ToString(),
                    Result        = result.ToString(),
                    HasError      = false,
                    Timestamp     = DateTime.UtcNow
                });

                return result;
            }
            catch (QuantityMeasurementException) { throw; }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException("Convert failed: " + ex.Message, ex);
            }
        }

        // ══════════════════════════════════════════
        // ADD
        // ══════════════════════════════════════════
        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            try
            {
                ValidateNotNull(q1, q2);
                ValidateSameCategory(q1, q2, "Add");
                ValidateArithmeticSupported(q1.MeasurementType, "Add");

                double sum       = ToBaseValue(q1) + ToBaseValue(q2);
                double converted = FromBaseValue(sum, targetUnit, q1.MeasurementType);

                var result = new QuantityDTO(converted, targetUnit, q1.MeasurementType);

                _repository.Save(new QuantityMeasurementEntity
                {
                    OperationType = "Add",
                    Operand1      = q1.ToString(),
                    Operand2      = q2.ToString(),
                    Result        = result.ToString(),
                    HasError      = false,
                    Timestamp     = DateTime.UtcNow
                });

                return result;
            }
            catch (QuantityMeasurementException) { throw; }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException("Add failed: " + ex.Message, ex);
            }
        }

        // ══════════════════════════════════════════
        // SUBTRACT
        // ══════════════════════════════════════════
        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2, string targetUnit)
        {
            try
            {
                ValidateNotNull(q1, q2);
                ValidateSameCategory(q1, q2, "Subtract");
                ValidateArithmeticSupported(q1.MeasurementType, "Subtract");

                double diff      = ToBaseValue(q1) - ToBaseValue(q2);
                double converted = FromBaseValue(diff, targetUnit, q1.MeasurementType);

                var result = new QuantityDTO(converted, targetUnit, q1.MeasurementType);

                _repository.Save(new QuantityMeasurementEntity
                {
                    OperationType = "Subtract",
                    Operand1      = q1.ToString(),
                    Operand2      = q2.ToString(),
                    Result        = result.ToString(),
                    HasError      = false,
                    Timestamp     = DateTime.UtcNow
                });

                return result;
            }
            catch (QuantityMeasurementException) { throw; }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException("Subtract failed: " + ex.Message, ex);
            }
        }

        // ══════════════════════════════════════════
        // DIVIDE
        // ══════════════════════════════════════════
        public QuantityDTO Divide(QuantityDTO q1, QuantityDTO q2)
        {
            try
            {
                ValidateNotNull(q1, q2);
                ValidateSameCategory(q1, q2, "Divide");
                ValidateArithmeticSupported(q1.MeasurementType, "Divide");

                double base2 = ToBaseValue(q2);
                if (Math.Abs(base2) < 0.0000001)
                    throw new QuantityMeasurementException("Division by zero is not allowed.");

                double quotient = ToBaseValue(q1) / base2;

                var result = new QuantityDTO(quotient, "SCALAR", "Scalar");

                _repository.Save(new QuantityMeasurementEntity
                {
                    OperationType = "Divide",
                    Operand1      = q1.ToString(),
                    Operand2      = q2.ToString(),
                    ScalarResult  = quotient,
                    HasError      = false,
                    Timestamp     = DateTime.UtcNow
                });

                return result;
            }
            catch (QuantityMeasurementException) { throw; }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException("Divide failed: " + ex.Message, ex);
            }
        }

        // ══════════════════════════════════════════
        // PRIVATE HELPERS
        // ══════════════════════════════════════════

        private double ToBaseValue(QuantityDTO q)
        {
            return q.MeasurementType.ToLower() switch
            {
                "length" => q.Unit.ToUpper() switch
                {
                    "INCHES"      => q.Value,
                    "FEET"        => q.Value * 12,
                    "YARDS"       => q.Value * 36,
                    "YARD"        => q.Value * 36,
                    "CENTIMETERS" => q.Value / 2.54,
                    "CM"          => q.Value / 2.54,
                    _ => throw new QuantityMeasurementException($"Unknown length unit: {q.Unit}")
                },
                "weight" => q.Unit.ToUpper() switch
                {
                    "GRAM"     => q.Value,
                    "KILOGRAM" => q.Value * 1000,
                    "KG"       => q.Value * 1000,
                    "POUND"    => q.Value * 453.592,
                    _ => throw new QuantityMeasurementException($"Unknown weight unit: {q.Unit}")
                },
                "volume" => q.Unit.ToUpper() switch
                {
                    "MILLILITRE" => q.Value,
                    "ML"         => q.Value,
                    "LITRE"      => q.Value * 1000,
                    "GALLON"     => q.Value * 3785.41,
                    _ => throw new QuantityMeasurementException($"Unknown volume unit: {q.Unit}")
                },
                "temperature" => q.Unit.ToUpper() switch
                {
                    "KELVIN"     => q.Value,
                    "CELSIUS"    => q.Value + 273.15,
                    "FAHRENHEIT" => (q.Value - 32) * 5.0 / 9.0 + 273.15,
                    _ => throw new QuantityMeasurementException($"Unknown temperature unit: {q.Unit}")
                },
                _ => throw new QuantityMeasurementException($"Unknown measurement type: {q.MeasurementType}")
            };
        }

        private double FromBaseValue(double baseValue, string targetUnit, string measurementType)
        {
            return measurementType.ToLower() switch
            {
                "length" => targetUnit.ToUpper() switch
                {
                    "INCHES"      => baseValue,
                    "FEET"        => baseValue / 12,
                    "YARDS"       => baseValue / 36,
                    "YARD"        => baseValue / 36,
                    "CENTIMETERS" => baseValue * 2.54,
                    "CM"          => baseValue * 2.54,
                    _ => throw new QuantityMeasurementException($"Unknown length unit: {targetUnit}")
                },
                "weight" => targetUnit.ToUpper() switch
                {
                    "GRAM"     => baseValue,
                    "KILOGRAM" => baseValue / 1000,
                    "KG"       => baseValue / 1000,
                    "POUND"    => baseValue / 453.592,
                    _ => throw new QuantityMeasurementException($"Unknown weight unit: {targetUnit}")
                },
                "volume" => targetUnit.ToUpper() switch
                {
                    "MILLILITRE" => baseValue,
                    "ML"         => baseValue,
                    "LITRE"      => baseValue / 1000,
                    "GALLON"     => baseValue / 3785.41,
                    _ => throw new QuantityMeasurementException($"Unknown volume unit: {targetUnit}")
                },
                "temperature" => targetUnit.ToUpper() switch
                {
                    "KELVIN"     => baseValue,
                    "CELSIUS"    => baseValue - 273.15,
                    "FAHRENHEIT" => (baseValue - 273.15) * 9.0 / 5.0 + 32,
                    _ => throw new QuantityMeasurementException($"Unknown temperature unit: {targetUnit}")
                },
                _ => throw new QuantityMeasurementException($"Unknown measurement type: {measurementType}")
            };
        }

        private void ValidateNotNull(QuantityDTO q1, QuantityDTO q2)
        {
            if (q1 == null) throw new QuantityMeasurementException("First quantity cannot be null.");
            if (q2 == null) throw new QuantityMeasurementException("Second quantity cannot be null.");
        }

        private bool IsSameCategory(string type1, string type2)
            => string.Equals(type1, type2, StringComparison.OrdinalIgnoreCase);

        private void ValidateSameCategory(QuantityDTO q1, QuantityDTO q2, string operation)
        {
            if (!IsSameCategory(q1.MeasurementType, q2.MeasurementType))
                throw new QuantityMeasurementException(
                    $"{operation} not allowed between {q1.MeasurementType} and {q2.MeasurementType}.");
        }

        private void ValidateArithmeticSupported(string measurementType, string operation)
        {
            if (string.Equals(measurementType, "temperature", StringComparison.OrdinalIgnoreCase))
                throw new QuantityMeasurementException(
                    $"Temperature does not support {operation} operation.");
        }
    }
}