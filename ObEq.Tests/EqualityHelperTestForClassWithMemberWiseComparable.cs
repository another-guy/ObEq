using System.Collections.Generic;
using Xunit;

namespace ObEq.Tests
{
    public class EqualityHelperTestForClassWithMemberWiseComparable
    {
        const string SampleParam = "test";
        private readonly IMemberwiseComparable SampleObject =
            new SampleClassWithMemberwiseComparable<string>(SampleParam);

        [Fact]
        public void ReturnsSameHashCodesForSameObjects()
        {
            // Arrange
            var baseHashCode = EqualityHelper.CalculateHashCode(SampleObject);
            foreach (var objectWithSameHashCode in Same)
            {
                // Act
                var sameHashCode = EqualityHelper.CalculateHashCode(objectWithSameHashCode);

                // Assert
                Assert.Equal(baseHashCode, sameHashCode);
            }
        }

        [Fact]
        public void ReturnsDifferentHashCodesForDifferentObjects()
        {
            var baseHashCode = EqualityHelper.CalculateHashCode(SampleObject);
            foreach (var objectWithNotSameHashCode in NotSame)
            {
                // Act
                var sameHashCode = EqualityHelper.CalculateHashCode(objectWithNotSameHashCode);

                // Assert
                Assert.NotEqual(baseHashCode, sameHashCode);
            }
        }

        [Fact]
        public void EqualityCalculatedCorrectlyForSameObjects()
        {
            // Arrange
            foreach (var otherObject in Same)
            {
                // Act
                var areSame = EqualityHelper.CalculateEquals(SampleObject, otherObject);
                
                // Assert
                Assert.True(areSame);
            }
        }

        [Fact]
        public void EqualityCalculatedCorrectlyForNotSameObjects()
        {
            // Arrange
            foreach (var otherObject in NotSame)
            {
                // Act
                var areSame = EqualityHelper.CalculateEquals(SampleObject, otherObject);

                // Assert
                Assert.False(areSame);
            }
        }

        [Fact]
        public void NullObjectsAreConsideredSame()
        {
            // Arrange
            SampleClassWithMemberwiseComparable<string> someNull = null;
            SampleClassWithMemberwiseComparable<string> otherNull = null;
            
            // Act
            var areSame = EqualityHelper.CalculateEquals(someNull, otherNull);
            
            // Assert
            Assert.True(areSame);
        }

        [Fact]
        public void CompatibleButNotDifferentTypeObjectsAreNotSameForBothNull()
        {
            // Arrange
            SampleClassWithMemberwiseComparable<string> someNull = null;
            SampleChildClassWithMemberwiseComparable<string> otherNull = null;

            // Act
            var areSame = EqualityHelper.CalculateEquals(someNull, otherNull);

            // Assert
            Assert.False(areSame);
        }

        [Fact]
        public void CompatibleButNotDifferentTypeObjectsAreNotSame()
        {
            // Arrange
            var var1 = new SampleClassWithMemberwiseComparable<string>(SampleParam) { I = 35 };
            var var2 = new SampleChildClassWithMemberwiseComparable<string>(SampleParam, newField: 123) { I = 35 };

            // Act
            var areSame = EqualityHelper.CalculateEquals(var1, var2);

            // Assert
            Assert.False(areSame);
        }

        private List<IMemberwiseComparable> Same =>
            new List<IMemberwiseComparable>
            {
                SampleObject,
                new SampleClassWithMemberwiseComparable<string>(SampleParam)
            };

        private static List<IMemberwiseComparable> NotSame =>
            new List<IMemberwiseComparable>
            {
                new SampleClassWithMemberwiseComparable<string>(SampleParam.Substring(0, 2)),
                new SampleClassWithMemberwiseComparable<string>(SampleParam) { I = 35 },
                new SampleClassWithMemberwiseComparable<string>(SampleParam) { DC = 234 },
                new SampleClassWithMemberwiseComparable<string>(SampleParam) { B = true },
                new SampleClassWithMemberwiseComparable<string>(SampleParam) { O = new object() },
                new SampleClassWithMemberwiseComparable<string>(SampleParam) { C = 'z' },
                new SampleClassWithMemberwiseComparable<string>(SampleParam) { BT = 253 },
                new SampleClassWithMemberwiseComparable<string>(SampleParam) { DB = 0.32434d },
                new SampleClassWithMemberwiseComparable<string>(SampleParam) { F = 35.213f },
                new SampleClassWithMemberwiseComparable<string>(SampleParam) { L = 32345L }
            };
    }
}
