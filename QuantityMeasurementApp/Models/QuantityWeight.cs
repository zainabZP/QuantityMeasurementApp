using System;
namespace QuantityMeasurementApp.Models{
    public class QuantityWeight{
        public double Value{get;}
        public WeightUnit Unit{get;}
        public QuantityWeight(double Value, WeightUnit Unit){
            this.Value=Value;
            this.Unit=Unit;
        }
    }
}