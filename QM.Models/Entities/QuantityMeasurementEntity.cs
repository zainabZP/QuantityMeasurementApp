using QM.Models.DTOs;

namespace QM.Models.Entities
{
    [Serializable]
    public class QuantityMeasurementEntity
    {
        public Guid Id { get; set; }
        public string OperationType { get; set; }
        public string? Operand1 { get; set; }
        public string? Operand2 { get; set; }
        public string? Result { get; set; }
        public double? ScalarResult { get; set; }
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }

        // Default constructor for EF Core
        public QuantityMeasurementEntity()
        {
            Id = Guid.NewGuid();
            OperationType = string.Empty;
            HasError = false;
            Timestamp = DateTime.Now;
        }

        // Single operand — Convert
        public QuantityMeasurementEntity(string operationType, QuantityDTO operand1, QuantityDTO result)
        {
            Id = Guid.NewGuid();
            OperationType = operationType;
            Operand1 = operand1.ToString();
            Result = result.ToString();
            HasError = false;
            Timestamp = DateTime.Now;
        }

        // Binary operand — Add / Subtract (returns QuantityDTO)
        public QuantityMeasurementEntity(string operationType, QuantityDTO operand1, QuantityDTO operand2, QuantityDTO result)
        {
            Id = Guid.NewGuid();
            OperationType = operationType;
            Operand1 = operand1.ToString();
            Operand2 = operand2.ToString();
            Result = result.ToString();
            HasError = false;
            Timestamp = DateTime.Now;
        }

        // Binary operand — Divide / Compare (returns scalar)
        public QuantityMeasurementEntity(string operationType, QuantityDTO operand1, QuantityDTO operand2, double scalarResult)
        {
            Id = Guid.NewGuid();
            OperationType = operationType;
            Operand1 = operand1.ToString();
            Operand2 = operand2.ToString();
            ScalarResult = scalarResult;
            HasError = false;
            Timestamp = DateTime.Now;
        }

        // Error constructor
        public QuantityMeasurementEntity(string operationType, QuantityDTO? operand1, QuantityDTO? operand2, string errorMessage)
        {
            Id = Guid.NewGuid();
            OperationType = operationType;
            Operand1 = operand1?.ToString();
            Operand2 = operand2?.ToString();
            HasError = true;
            ErrorMessage = errorMessage;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            if (HasError)
                return $"[{Timestamp:HH:mm:ss}] {OperationType} ERROR: {ErrorMessage}";
            if (ScalarResult.HasValue)
                return $"[{Timestamp:HH:mm:ss}] {OperationType}: {Operand1} , {Operand2} = {ScalarResult.Value}";
            return $"[{Timestamp:HH:mm:ss}] {OperationType}: {Operand1} => {Result}";
        }
    }
}
