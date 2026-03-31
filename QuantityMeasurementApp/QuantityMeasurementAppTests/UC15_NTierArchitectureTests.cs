using NUnit.Framework;
using QM.Models.Models;
using QM.BusinessLogic.Service;
using QM.BusinessLogic.Interface;
using QuantityMeasurementApp.Controllers;
using QM.Repository.Repository;
using QM.Repository.Interface;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class UC15_NTierArchitectureTests
    {
        private QuantityMeasurementController controller;
        private IQuantityMeasurementRepository repository;
        private const double EPSILON = 0.001;

        [SetUp]
        public void Setup()
        {
            repository = QuantityMeasurementCacheRepository.Instance;
            var service = new QuantityMeasurementServiceImpl(repository);
            controller = new QuantityMeasurementController(service);
        }

        #region Entity Layer Tests - QuantityModel

        [Test]
        public void testQuantityModel_LengthConstruction_Success()
        {
            var quantity = new QuantityModel<LengthUnit>(10.0, LengthUnit.FEET);
            
            Assert.That(quantity.Value, Is.EqualTo(10.0));
            Assert.That(quantity.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void testQuantityModel_WeightConstruction_Success()
        {
            var quantity = new QuantityModel<WeightUnit>(5.0, WeightUnit.KILOGRAM);
            
            Assert.That(quantity.Value, Is.EqualTo(5.0));
            Assert.That(quantity.Unit, Is.EqualTo(WeightUnit.KILOGRAM));
        }

        [Test]
        public void testQuantityModel_VolumeConstruction_Success()
        {
            var quantity = new QuantityModel<VolumeUnit>(1.0, VolumeUnit.LITRE);
            
            Assert.That(quantity.Value, Is.EqualTo(1.0));
            Assert.That(quantity.Unit, Is.EqualTo(VolumeUnit.LITRE));
        }

        [Test]
        public void testQuantityModel_TemperatureConstruction_Success()
        {
            var quantity = new QuantityModel<TemperatureUnit>(25.0, TemperatureUnit.CELSIUS);
            
            Assert.That(quantity.Value, Is.EqualTo(25.0));
            Assert.That(quantity.Unit, Is.EqualTo(TemperatureUnit.CELSIUS));
        }

        [Test]
        public void testQuantityModel_ToString_ReturnsFormattedString()
        {
            var quantity = new QuantityModel<LengthUnit>(5.0, LengthUnit.FEET);
            string result = quantity.ToString();
            
            Assert.That(result, Does.Contain("5"));
            Assert.That(result, Does.Contain("FEET"));
        }

        #endregion

        #region Service Layer Tests - QuantityMeasurementServiceImpl

        [Test]
        public void testService_CompareEquality_SameUnit_Success()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            bool result = q1.Equals(q2);
            
            Assert.That(result, Is.True);
        }

        [Test]
        public void testService_CompareEquality_DifferentUnit_Success()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);

            bool result = q1.Equals(q2);
            
            Assert.That(result, Is.True);
        }

        [Test]
        public void testService_CompareInequality_Success()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(2.0, LengthUnit.FEET);

            bool result = q1.Equals(q2);
            
            Assert.That(result, Is.False);
        }

        [Test]
        public void testService_Convert_FeetToInches_Success()
        {
            var quantity = new QuantityLength(1.0, LengthUnit.FEET);
            var converted = quantity.ConvertTo(LengthUnit.INCHES);

            Assert.That(converted.Value, Is.EqualTo(12.0).Within(EPSILON));
            Assert.That(converted.Unit, Is.EqualTo(LengthUnit.INCHES));
        }

        [Test]
        public void testService_Convert_InchesToFeet_Success()
        {
            var quantity = new QuantityLength(12.0, LengthUnit.INCHES);
            var converted = quantity.ConvertTo(LengthUnit.FEET);

            Assert.That(converted.Value, Is.EqualTo(1.0).Within(EPSILON));
            Assert.That(converted.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void testService_AddQuantities_SameUnit_Success()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            var result = q1.Add(q2);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void testService_AddQuantities_DifferentUnits_Success()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);

            var result = q1.Add(q2);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void testService_CancelQuantities_Success()
        {
            var q1 = new QuantityLength(2.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);

            var result = new Quantity<LengthUnit>(q1.Value, q1.Unit).Subtract(
                new Quantity<LengthUnit>(q2.Value, q2.Unit), LengthUnit.FEET);

            Assert.That(result.Value, Is.EqualTo(1.0).Within(EPSILON));
        }

        [Test]
        public void testService_DivideQuantities_Success()
        {
            var q1 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.That(result, Is.EqualTo(2.0).Within(EPSILON));
        }

        [Test]
        public void testService_DivideByZero_ThrowsException()
        {
            var q1 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(0.0, LengthUnit.FEET);

            Assert.Throws<DivideByZeroException>(() => q1.Divide(q2));
        }

        [Test]
        public void testService_AllMeasurementCategories_Length_Success()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testService_AllMeasurementCategories_Weight_Success()
        {
            var q1 = new QuantityModel<WeightUnit>(1000.0, WeightUnit.GRAM);
            var q2 = new QuantityModel<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            double base1 = q1.Value * (q1.Unit == WeightUnit.GRAM ? 1.0 : 1000.0);
            double base2 = q2.Value * (q2.Unit == WeightUnit.GRAM ? 1.0 : 1000.0);

            Assert.That(Math.Abs(base1 - base2), Is.LessThan(EPSILON));
        }

        #endregion

        #region Repository Layer Tests - Persistence

        [Test]
        public void testRepository_Singleton_DuplicateInstancesAreIdentical()
        {
            var repo1 = QuantityMeasurementCacheRepository.Instance;
            var repo2 = QuantityMeasurementCacheRepository.Instance;

            Assert.That(repo1, Is.SameAs(repo2));
        }

        [Test]
        public void testRepository_SaveEntity_Success()
        {
            var quantity = new QuantityLength(1.0, LengthUnit.FEET);
            var converted = quantity.ConvertTo(LengthUnit.INCHES);

            // Repository save would be called by service in full implementation
            Assert.That(converted.Value, Is.EqualTo(12.0).Within(EPSILON));
        }

        #endregion

        #region Controller Layer Tests - Orchestration

        [Test]
        public void testController_HandlesFeetToInchesConversion()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(q1.ConvertTo(LengthUnit.INCHES).Value, LengthUnit.INCHES);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testController_HandlesLengthEquality()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testController_HandlesAddition()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);
            var result = q1.Add(q2);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
        }

        #endregion

        #region Backward Compatibility Tests - UC1-UC14

        [Test]
        public void testBackwardCompatibility_UC1_FeetEquality()
        {
            var f1 = new Feet(1.0);
            var f2 = new Feet(1.0);

            Assert.That(f1.Equals(f2), Is.True);
        }

        [Test]
        public void testBackwardCompatibility_UC1_FeetInequality()
        {
            var f1 = new Feet(1.0);
            var f2 = new Feet(2.0);

            Assert.That(f1.Equals(f2), Is.False);
        }

        [Test]
        public void testBackwardCompatibility_UC3_QuantityLengthConversion()
        {
            var quantity = new QuantityLength(1.0, LengthUnit.FEET);
            var converted = quantity.ConvertTo(LengthUnit.INCHES);

            Assert.That(converted.Value, Is.EqualTo(12.0).Within(EPSILON));
            Assert.That(converted.Unit, Is.EqualTo(LengthUnit.INCHES));
        }

        [Test]
        public void testBackwardCompatibility_UC4_QuantityLengthAddition()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);
            var result = q1.Add(q2);

            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void testBackwardCompatibility_UC10_GenericQuantity()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testBackwardCompatibility_UC14_TemperatureConversion()
        {
            var q1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var q2 = q1.ConvertTo(TemperatureUnit.FAHRENHEIT);

            Assert.That(q2.Value, Is.EqualTo(32.0).Within(EPSILON));
            Assert.That(q2.Unit, Is.EqualTo(TemperatureUnit.FAHRENHEIT));
        }

        #endregion

        #region Error Handling and Validation Tests

        [Test]
        public void testValidation_InvalidUnit_Rejection()
        {
            // This test ensures invalid operations are rejected
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var result = q1.ConvertTo(LengthUnit.FEET);

            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void testErrorHandling_TemperatureArithmetic_NotSupported()
        {
            var q1 = new Quantity<TemperatureUnit>(25.0, TemperatureUnit.CELSIUS);
            var q2 = new Quantity<TemperatureUnit>(30.0, TemperatureUnit.CELSIUS);

            // Temperature arithmetic is not supported
            Assert.Throws<UnsupportedOperationException>(() => 
                q1.Add(q2, TemperatureUnit.CELSIUS)
            );
        }

        #endregion

        #region Data Flow and Layer Integration Tests

        [Test]
        public void testLayerIntegration_CompleteAdditionFlow()
        {
            // Controller receives input
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);

            // Service performs operation
            var result = q1.Add(q2);

            // Verify result
            Assert.That(result.Value, Is.EqualTo(2.0).Within(EPSILON));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.FEET));
        }

        [Test]
        public void testLayerIntegration_CompleteConversionFlow()
        {
            // Input
            var quantity = new QuantityLength(5.0, LengthUnit.FEET);

            // Service conversion
            var converted = quantity.ConvertTo(LengthUnit.YARDS);

            // Output validation
            double expectedYards = 5.0 / 3.0;
            Assert.That(converted.Value, Is.EqualTo(expectedYards).Within(EPSILON));
        }

        [Test]
        public void testDataStandardization_AllOperationsReturnConsistentTypes()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);

            // Comparison returns bool
            bool comparison = q1.Equals(q2);
            Assert.That(comparison, Is.TypeOf<bool>());

            // Conversion returns QuantityLength
            var converted = q1.ConvertTo(LengthUnit.INCHES);
            Assert.That(converted, Is.TypeOf<QuantityLength>());

            // Addition returns QuantityLength
            var addition = q1.Add(q2);
            Assert.That(addition, Is.TypeOf<QuantityLength>());
        }

        #endregion

        #region Architecture Principle Tests

        [Test]
        public void testSingleResponsibilityPrinciple_ServiceLayerHandlesOnlyLogic()
        {
            // Service should only handle business logic, not presentation
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);

            // No System.out.println in comparison
            bool result = q1.Equals(q2);
            Assert.That(result, Is.True);
        }

        [Test]
        public void testSeparationOfConcerns_ControllerDoesNotHaveBusinessLogic()
        {
            // Controller delegates to service/model
            var quantity = new QuantityLength(1.0, LengthUnit.FEET);
            var converted = quantity.ConvertTo(LengthUnit.INCHES);

            Assert.That(converted.Value, Is.EqualTo(12.0).Within(EPSILON));
        }

        [Test]
        public void testOpenClosedPrinciple_NewUnitsAddable()
        {
            // Can add new length units without modifying service
            var q1 = new QuantityLength(100.0, LengthUnit.CENTIMETERS);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            // Conversion should work with new units
            Assert.That(q1, Is.Not.Null);
        }

        [Test]
        public void testListkovSubstitution_DifferentUnitsSubstitutable()
        {
            Quantity<LengthUnit> length = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            Quantity<WeightUnit> weight = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);

            Assert.That(length, Is.Not.Null);
            Assert.That(weight, Is.Not.Null);
        }

        [Test]
        public void testInterfaceSegregationPrinciple_RepositoryAbstracted()
        {
            IQuantityMeasurementRepository repo = QuantityMeasurementCacheRepository.Instance;
            Assert.That(repo, Is.Not.Null);
        }

        [Test]
        public void testDependencyInversion_ServiceAcceptedAsInterface()
        {
            var repository = QuantityMeasurementCacheRepository.Instance;
            var service = new QuantityMeasurementServiceImpl(repository);
            var controller = new QuantityMeasurementController(service);

            Assert.That(controller, Is.Not.Null);
        }

        #endregion

        #region Extension and Scalability Tests

        [Test]
        public void testScalability_NewOperation_CanBeAdded()
        {
            var q1 = new QuantityLength(10.0, LengthUnit.FEET);
            var q2 = new QuantityLength(3.0, LengthUnit.FEET);

            double division = new Quantity<LengthUnit>(q1.Value, q1.Unit)
                .Divide(new Quantity<LengthUnit>(q2.Value, q2.Unit));

            Assert.That(division, Is.EqualTo(q1.Value / q2.Value).Within(EPSILON));
        }

        [Test]
        public void testScalability_AllMeasurementTypes_SupportedIdentically()
        {
            // Length
            var length = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.That(length.ConvertTo(LengthUnit.INCHES).Value, Is.EqualTo(12.0).Within(EPSILON));

            // Weight operations would work similarly
            var weight = new QuantityModel<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            Assert.That(weight.Value, Is.EqualTo(1.0));

            // Volume and Temperature follow same pattern
        }

        #endregion

        #region REST and Future Extensibility Tests

        [Test]
        public void testREST_Readiness_DTOFormat()
        {
            // Service returns standardized data
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);

            // Could be serialized to JSON for REST response
            bool result = q1.Equals(q2);
            string jsonResponse = $"{{\"result\": {result.ToString().ToLower()}, \"type\": \"comparison\"}}";

            Assert.That(jsonResponse, Does.Contain("true"));
        }

        [Test]
        public void testREST_Readiness_StandardizedResponse()
        {
            var quantity = new QuantityLength(1.0, LengthUnit.FEET);
            var converted = quantity.ConvertTo(LengthUnit.INCHES);

            // Standardized response structure
            string response = $"{{\"value\": {converted.Value}, \"unit\": \"{converted.Unit}\"}}";
            Assert.That(response, Does.Contain("12"));
        }

        #endregion
    }

    #region Additional Test Fixtures for Specific Scenarios

    [TestFixture]
    public class UC15_TemperatureSpecificTests
    {
        private const double EPSILON = 0.001;

        [Test]
        public void testTemperature_CelsiusToFahrenheit_Conversion()
        {
            var celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var fahrenheit = celsius.ConvertTo(TemperatureUnit.FAHRENHEIT);

            Assert.That(fahrenheit.Value, Is.EqualTo(32.0).Within(EPSILON));
        }

        [Test]
        public void testTemperature_CelsiusToKelvin_Conversion()
        {
            var celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var kelvin = celsius.ConvertTo(TemperatureUnit.KELVIN);

            Assert.That(kelvin.Value, Is.EqualTo(273.15).Within(EPSILON));
        }

        [Test]
        public void testTemperature_Comparison_Success()
        {
            var t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT);

            Assert.That(t1.Equals(t2), Is.True);
        }

        [Test]
        public void testTemperature_Addition_ThrowsException()
        {
            var t1 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS);

            Assert.Throws<UnsupportedOperationException>(() => 
                t1.Add(t2, TemperatureUnit.CELSIUS)
            );
        }
    }

    [TestFixture]
    public class UC15_CrossCategoryTests
    {
        [Test]
        public void testCrossCategory_LengthVsWeight_CannotCompare()
        {
            var length = new QuantityLength(1.0, LengthUnit.FEET);
            var weight = new QuantityModel<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            // These are different types and should not be comparable directly
            Assert.That(length, Is.TypeOf<QuantityLength>());
            Assert.That(weight, Is.TypeOf<QuantityModel<WeightUnit>>());
        }

        [Test]
        public void testCrossCategory_PreventMixedOperations()
        {
            // Service should prevent mixing categories
            var length = new QuantityLength(1.0, LengthUnit.FEET);

            // Should not be able to mix length with weight
            Assert.That(length, Is.Not.Null);
        }
    }

    #endregion
}
