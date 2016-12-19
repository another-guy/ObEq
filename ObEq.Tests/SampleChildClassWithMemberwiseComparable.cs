namespace ObEq.Tests
{
    public class SampleChildClassWithMemberwiseComparable<T1> :
        SampleClassWithMemberwiseComparable<T1>
    {
        public int NewField { get; set; }

        public SampleChildClassWithMemberwiseComparable(T1 genericTypeObject, int newField)
            : base(genericTypeObject)
        {
            NewField = newField;
        }
    }
}
