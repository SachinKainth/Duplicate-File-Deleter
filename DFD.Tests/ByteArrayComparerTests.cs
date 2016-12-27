using FluentAssertions;
using NUnit.Framework;
using System;

namespace DFD.UnitTests
{
    [TestFixture]
    class ByteArrayComparerTests
    {
        ByteArrayComparer _comparer;

        [SetUp]
        public void Setup()
        {
            _comparer = new ByteArrayComparer();
        }

        [TestCase(null, null, true)]
        [TestCase(null, new byte[]{}, false)]
        [TestCase(new byte[] { }, null, false)]
        [TestCase(new byte[] { }, new byte[] { }, true)]
        [TestCase(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 }, true)]
        [TestCase(new byte[] { 1, 3, 2 }, new byte[] { 1, 2, 3 }, false)]
        [TestCase(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6 }, false)]
        public void Equals_WhenCalled_ComparesForEquality(byte[] left, byte[] right, bool equal)
        {
            _comparer.Equals(left, right).Should().Be(equal);
        }

        [Test]
        public void GetHashCode_WhenKeyNull_ThrowsException()
        {
            Action action = () => _comparer.GetHashCode(null);
            action
                .ShouldThrow<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: key");
        }

        [Test]
        public void GetHashCode_WhenKeyEmpty_Returns0()
        {
            _comparer.GetHashCode(new byte[] { }).Should().Be(0);
        }

        [Test]
        public void GetHashCode_WhenKeyNotEmpty_ReturnsResult()
        {
            _comparer.GetHashCode(new byte[] { 1, 2, 3 }).Should().Be(6);
        }
    }
}