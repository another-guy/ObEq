namespace ObEq.Tests
{
    public class SampleClass<T1>
    {
        public T1 T { get; set; }
        public int I { get; set; }
        public decimal DC { get; set; }
        public bool B { get; set; }
        public object O { get; set; }
        public char C { get; set; }
        public byte BT { get; set; }
        public double DB { get; set; }
        public float F { get; set; }
        public long L { get; set; }

        public SampleClass(T1 t)
        {
            T = t;
            I = int.MaxValue;
            DC = decimal.MaxValue;
            B = false;
            O = null;
            C = 'a';
            BT = byte.MaxValue;
            DB = double.MaxValue;
            F = float.MaxValue;
            L = long.MaxValue;
        }

        public bool Equals(SampleClass<T1> other)
        {
            return EqualityHelper.AllMembersEqual(this.EqualityMembers, other.EqualityMembers);
        }

        public override bool Equals(object other)
        {
            var referenceEqualityResult = EqualityHelper.ReferencesEqual(this, other);
            return referenceEqualityResult ??
                EqualityHelper.AllMembersEqual(this.EqualityMembers, ((SampleClass<T1>)other).EqualityMembers);
        }

        public override int GetHashCode()
        {
            return EqualityHelper.CalculateHashCode(EqualityMembers);
        }

        public object[] EqualityMembers => new[] { T, I, DC, B, O, C, BT, DB, F, L };
    }
}
