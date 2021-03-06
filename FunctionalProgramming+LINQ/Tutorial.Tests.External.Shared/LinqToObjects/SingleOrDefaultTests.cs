// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Xunit;

using Enumerable = Tutorial.LinqToObjects.EnumerableExtensions;
using static Tutorial.LinqToObjects.EnumerableExtensions;

// namespace System.Linq.Tests
namespace Tutorial.Tests.LinqToObjects
{
    public class SingleOrDefaultTests : EnumerableTests
    {
        [Fact]
        public void SameResultsRepeatCallsIntQuery()
        {
            var q = from x in new[] { 0.12335f }
                    select x;

            Assert.Equal(q.SingleOrDefault(), q.SingleOrDefault());
        }

        [Fact]
        public void SameResultsRepeatCallsStringQuery()
        {
            var q = from x in new[] { "" }
                    select x;

            Assert.Equal(q.SingleOrDefault(string.IsNullOrEmpty), q.SingleOrDefault(string.IsNullOrEmpty));
        }

        [Fact]
        public void EmptyIList()
        {
            int?[] source = { };
            int? expected = null;

            Assert.Equal(expected, source.SingleOrDefault());
        }

        [Fact]
        public void SingleElementIList()
        {
            int[] source = { 4 };
            int expected = 4;

            Assert.Equal(expected, source.SingleOrDefault());
        }

        [Fact]
        public void ManyElementIList()
        {
            int[] source = { 4, 4, 4, 4, 4 };

            Assert.Throws<InvalidOperationException>(() => source.SingleOrDefault());
        }

        [Fact]
        public void EmptyNotIList()
        {
            IEnumerable<int> source = RepeatedNumberGuaranteedNotCollectionType(0, 0);
            int expected = default(int);

            Assert.Equal(expected, source.SingleOrDefault());
        }

        [Fact]
        public void SingleElementNotIList()
        {
            IEnumerable<int> source = RepeatedNumberGuaranteedNotCollectionType(-5, 1);
            int expected = -5;

            Assert.Equal(expected, source.SingleOrDefault());
        }

        [Fact]
        public void ManyElementNotIList()
        {
            IEnumerable<int> source = RepeatedNumberGuaranteedNotCollectionType(3, 5);

            Assert.Throws<InvalidOperationException>(() => source.SingleOrDefault());
        }

        [Fact]
        public void EmptySourceWithPredicate()
        {
            int[] source = { };
            int expected = default(int);

            Assert.Equal(expected, source.SingleOrDefault(i => i % 2 == 0));
        }

        [Fact]
        public void SingleElementPredicateTrue()
        {
            int[] source = { 4 };
            int expected = 4;
            
            Assert.Equal(expected, source.SingleOrDefault(i => i % 2 == 0));
        }

        [Fact]
        public void SingleElementPredicateFalse()
        {
            int[] source = { 3 };
            int expected = default(int);

            Assert.Equal(expected, source.SingleOrDefault(i => i % 2 == 0));
        }

        [Fact]
        public void ManyElementsPredicateFalseForAll()
        {
            int[] source = { 3, 1, 7, 9, 13, 19 };
            int expected = default(int);

            Assert.Equal(expected, source.SingleOrDefault(i => i % 2 == 0));
        }

        [Fact]
        public void ManyElementsPredicateTrueForLast()
        {
            int[] source = { 3, 1, 7, 9, 13, 19, 20 };
            int expected = 20;

            Assert.Equal(expected, source.SingleOrDefault(i => i % 2 == 0));
        }

        [Fact]
        public void ManyElementsPredicateTrueForFirstAndFifth()
        {
            int[] source = { 2, 3, 1, 7, 10, 13, 19, 9 };

            Assert.Throws<InvalidOperationException>(() => source.SingleOrDefault(i => i % 2 == 0));
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(42, 100)]
        public void FindSingleMatch(int target, int range)
        {
            Assert.Equal(target, Enumerable.Range(0, range).SingleOrDefault(i => i == target));
        }
        
        // [Fact]
        public void ThrowsOnNullSource()
        {
            int[] source = null;
            Assert.Throws<ArgumentNullException>("source", () => source.SingleOrDefault());
            Assert.Throws<ArgumentNullException>("source", () => source.SingleOrDefault(i => i % 2 == 0));
        }

        // [Fact]
        public void ThrowsOnNullPredicate()
        {
            int[] source = { };
            Func<int, bool> nullPredicate = null;
            Assert.Throws<ArgumentNullException>("predicate", () => source.SingleOrDefault(nullPredicate));
        }
    }
}
