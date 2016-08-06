namespace ObEq.Tests
{
    public class SampleClassWithMemberwiseComparable<T1> :
        IMemberwiseComparable
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

        public SampleClassWithMemberwiseComparable(T1 t)
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

        public override bool Equals(object other)
        {
            return EqualityHelper.CalculateEquals(this, other as IMemberwiseComparable);
        }

        public override int GetHashCode()
        {
            return EqualityHelper.CalculateHashCode(EqualityMembers);
        }

        public object[] EqualityMembers => new[] { T, I, DC, B, O, C, BT, DB, F, L };
    }
}
