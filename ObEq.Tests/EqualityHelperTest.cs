using System;
using System.Collections.Generic;
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
            Assert.True(EqualityHelper.ReferencesEqual(SampleObject, SampleObject));
        }

        [Fact]
        public void ReferenceEqualityCorrectForDifferentObjects()
        {
            Assert.False(EqualityHelper.ReferencesEqual(null, SampleObject));
            Assert.False(EqualityHelper.ReferencesEqual(SampleObject, null));
            Assert.False(EqualityHelper.ReferencesEqual(SampleObject, 123));
            Assert.False(EqualityHelper.ReferencesEqual(123, SampleObject));
        }

        [Fact]
        public void ValueEqualityCorrectForSameObject()
        {
            // Arrange
            foreach (var equalObject in Same)
            {
                // Act
                var areEqual = EqualityHelper.AllMembersEqual(SampleObject.EqualityMembers, equalObject.EqualityMembers);

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
                var areEqual = EqualityHelper.AllMembersEqual(SampleObject.EqualityMembers, notEqualObject.EqualityMembers);

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
                var areEqual1 = EqualityHelper.AllMembersEqual(SampleObject.EqualityMembers, notEqualObject.EqualityMembers.Take(3).ToArray());
                var areEqual2 = EqualityHelper.AllMembersEqual(SampleObject.EqualityMembers.Take(3).ToArray(), notEqualObject.EqualityMembers);

                // Assert
                Assert.False(areEqual1);
                Assert.False(areEqual2);
            }
        }

        [Fact]
        public void ComparingEqualityMembersWithNullThrowsException()
        {
            // Arrange
            // Act
            var caught = Assert.Throws<ArgumentNullException>(() =>
                EqualityHelper.AllMembersEqual(SampleObject.EqualityMembers, null));

            // Assert
            Assert.True(caught.Message.Contains("equalityMembers2"));
        }

        [Fact]
        public void ComparingNullWithEqualityMembersThrowsException()
        {
            // Arrange
            // Act
            var caught = Assert.Throws<ArgumentNullException>(() =>
                EqualityHelper.AllMembersEqual(null, SampleObject.EqualityMembers));

            // Assert
            Assert.True(caught.Message.Contains("equalityMembers1"));
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
}
