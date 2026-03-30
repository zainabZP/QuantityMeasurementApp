using System;
namespace QM.Models.Models
{
    public class Feet{
        private readonly double value;
        public Feet(double value){
            this.value=value;
        }
        public override bool Equals(object? obj){ // object can store any type of of value bcz every class in c# is derived from object class.
            if(ReferenceEquals(this,obj)) return true; // ReferenceEquals is a static method of object class which checks whether the two objects are same or not i.e their address.
            if(obj==null || obj.GetType()!=typeof(Feet)) return false; // GetType is a method of object class which returns the type class object (here type is feet, GetType is used when we dont know the type of obj) while typeof also returs type class obj but we use it when we already know type of obj.
            Feet other=(Feet)obj;
            return Math.Abs(this.value-other.value)<0.0001;
        }
        public override int GetHashCode(){
            return value.GetHashCode();
        }
    }
}
