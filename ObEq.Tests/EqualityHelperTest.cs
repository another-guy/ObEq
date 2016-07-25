﻿using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ObEq.Tests
{
    public class EqualityHelperTest
    {
        const string SampleParam = "test";
        private readonly SampleClass<string> SampleObject = new SampleClass<string>(SampleParam);

        [Fact]
        public void ReturnsSameHashCodesForSameObjects()
        {
            // Arrange
            var baseHashCode = EqualityHelper.CalculateHashCode(SampleObject.EqualityMembers);
            foreach (var objectWithSameHashCode in Same)
            {
                // Act
                var sameHashCode = EqualityHelper.CalculateHashCode(objectWithSameHashCode.EqualityMembers);

                // Assert
                Assert.Equal(baseHashCode, sameHashCode);
            }
        }

        [Fact]
        public void ReturnsDifferentHashCodesForDifferentObjects()
        {
            var baseHashCode = EqualityHelper.CalculateHashCode(SampleObject.EqualityMembers);
            foreach (var objectWithNotSameHashCode in NotSame)
            {
                // Act
                var sameHashCode = EqualityHelper.CalculateHashCode(objectWithNotSameHashCode.EqualityMembers);

                // Assert
                Assert.NotEqual(baseHashCode, sameHashCode);
            }
        }

        [Fact]
        public void ReferenceEqualityCorrectForSameObject()
        {
            Assert.True(EqualityHelper.CalculateReferentialEquals(SampleObject, SampleObject));
        }

        [Fact]
        public void ReferenceEqualityCorrectForDifferentObjects()
        {
            Assert.False(EqualityHelper.CalculateReferentialEquals(null, SampleObject));
            Assert.False(EqualityHelper.CalculateReferentialEquals(SampleObject, null));
            Assert.False(EqualityHelper.CalculateReferentialEquals(SampleObject, 123));
            Assert.False(EqualityHelper.CalculateReferentialEquals(123, SampleObject));
        }

        [Fact]
        public void ValueEqualityCorrectForSameObject()
        {
            // Arrange
            foreach (var equalObject in Same)
            {
                // Act
                var areEqual = EqualityHelper.CalculateEquals(SampleObject.EqualityMembers, equalObject.EqualityMembers);

                // Assert
                Assert.True(areEqual);
            }
        }

        [Fact]
        public void ValueEqualityCorrectForDifferentObjects()
        {
            // Arrange
            foreach (var notEqualObject in NotSame)
            {
                // Act
                var areEqual = EqualityHelper.CalculateEquals(SampleObject.EqualityMembers, notEqualObject.EqualityMembers);

                // Assert
                Assert.False(areEqual);
            }
        }

        [Fact]
        public void ValueEqualityCorrectReturnsFalseForObjectsWithDifferentEqualityMember()
        {
            // Arrange
            foreach (var notEqualObject in NotSame)
            {
                // Act
                var areEqual1 = EqualityHelper.CalculateEquals(SampleObject.EqualityMembers, notEqualObject.EqualityMembers.Take(3).ToArray());
                var areEqual2 = EqualityHelper.CalculateEquals(SampleObject.EqualityMembers.Take(3).ToArray(), notEqualObject.EqualityMembers);

                // Assert
                Assert.False(areEqual1);
                Assert.False(areEqual2);
            }
        }

        private List<SampleClass<string>> Same =>
            new List<SampleClass<string>>
            {
                SampleObject,
                new SampleClass<string>(SampleParam)
            };

        private static List<SampleClass<string>> NotSame =>
            new List<SampleClass<string>>
            {
                new SampleClass<string>(SampleParam.Substring(0, 2)),
                new SampleClass<string>(SampleParam) { I = 35 },
                new SampleClass<string>(SampleParam) { DC = 234 },
                new SampleClass<string>(SampleParam) { B = true },
                new SampleClass<string>(SampleParam) { O = new object() },
                new SampleClass<string>(SampleParam) { C = 'z' },
                new SampleClass<string>(SampleParam) { BT = 253 },
                new SampleClass<string>(SampleParam) { DB = 0.32434d },
                new SampleClass<string>(SampleParam) { F = 35.213f },
                new SampleClass<string>(SampleParam) { L = 32345L }
            };
    }

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

        protected bool Equals(SampleClass<T1> other)
        {
            return EqualityHelper.CalculateEquals(this.EqualityMembers, other.EqualityMembers);
        }

        public override bool Equals(object other)
        {
            var referenceEqualityResult = EqualityHelper.CalculateReferentialEquals(this, other);
            return referenceEqualityResult ??
                EqualityHelper.CalculateEquals(this.EqualityMembers, ((SampleClass<T1>)other).EqualityMembers);
        }

        public override int GetHashCode()
        {
            return EqualityHelper.CalculateHashCode(EqualityMembers);
        }

        public object[] EqualityMembers => new[] { this.T, I, DC, B, O, C, BT, DB, F, L };
    }
}