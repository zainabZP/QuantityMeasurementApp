using System;
namespace QM.Models.Models
{
    public class QuantityLength{
        public double Value{get;}
        public LengthUnit Unit{get;}
        public QuantityLength(double Value, LengthUnit Unit){
            this.Value=Value;
            this.Unit=Unit;
        }
        // Added for previous ucs testcases

        // convert to target unit -
        public QuantityLength ConvertTo(LengthUnit targetUnit){
            double baseValue=this.Unit.ConvertToBaseUnit(this.Value);
            double converted=targetUnit.ConvertFromBaseUnit(baseValue);
            return new QuantityLength(converted, targetUnit);
        }
        // add in first unit -
        public QuantityLength Add(QuantityLength other){
            double base1=this.Unit.ConvertToBaseUnit(this.Value);
            double base2=other.Unit.ConvertToBaseUnit(other.Value);
            double sumBase=base1+base2;
            double result=this.Unit.ConvertFromBaseUnit(sumBase);
            return new QuantityLength(result, this.Unit);
        }

        // override equals to compare lengths -
        public override bool Equals(object? obj){
            if(ReferenceEquals(this,obj)) return true;
            if(obj==null || obj.GetType()!=typeof(QuantityLength)) return false;
            QuantityLength other=(QuantityLength)obj;
            double base1=this.Unit.ConvertToBaseUnit(this.Value);
            double base2=other.Unit.ConvertToBaseUnit(other.Value);
            return Math.Abs(base1-base2)<0.0001;
        }

        public override int GetHashCode(){
            return HashCode.Combine(Value, Unit); // return this obj combined hash for value and unit
        }
    }
}