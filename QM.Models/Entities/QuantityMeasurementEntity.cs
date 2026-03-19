using QM.Models.DTOs;
namespace QM.Models.Entities
{
    [Serializable]
    public class QuantityMeasurementEntity
    {
        public Guid Id { get; private set; }
        public string OperationType { get; private set; }
        public QuantityDTO? Operand1 { get; private set; }
        public QuantityDTO? Operand2 { get; private set; }
        public QuantityDTO? Result { get; private set; }
        public double? ScalarResult { get; private set; }
        public bool HasError { get; private set; }
        public string? ErrorMessage { get; private set; }
        public DateTime Timestamp { get; private set; }

        // Single operand — Convert
        public QuantityMeasurementEntity(string operationType,
                                         QuantityDTO operand1,
                                         QuantityDTO result)
        {
            Id = Guid.NewGuid();
            OperationType = operationType;
            Operand1 = operand1;
            Result = result;
            HasError = false;
            Timestamp = DateTime.Now;
        }

        // Binary operand — Add / Subtract (returns QuantityDTO)
        public QuantityMeasurementEntity(string operationType,
                                         QuantityDTO operand1,
                                         QuantityDTO operand2,
                                         QuantityDTO result)
        {
            Id = Guid.NewGuid();
            OperationType = operationType;
            Operand1 = operand1;
            Operand2 = operand2;
            Result = result;
            HasError = false;
            Timestamp = DateTime.Now;
        }

        // Binary operand — Divide / Compare (returns scalar)
        public QuantityMeasurementEntity(string operationType,
                                         QuantityDTO operand1,
                                         QuantityDTO operand2,
                                         double scalarResult)
        {
            Id = Guid.NewGuid();
            OperationType = operationType;
            Operand1 = operand1;
            Operand2 = operand2;
            ScalarResult = scalarResult;
            HasError = false;
            Timestamp = DateTime.Now;
        }

        // Error constructor
        public QuantityMeasurementEntity(string operationType,
                                         QuantityDTO? operand1,
                                         QuantityDTO? operand2,
                                         string errorMessage)
        {
            Id = Guid.NewGuid();
            OperationType = operationType;
            Operand1 = operand1;
            Operand2 = operand2;
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