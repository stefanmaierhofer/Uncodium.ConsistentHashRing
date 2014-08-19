using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Uncodium.Base;

namespace Uncodium.ConsistentHashRing.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void CanConstruct()
        {
            var ring = new ConsistentHashRing<Guid, string>();
        }

        [TestMethod]
        public void NewRingHasNoEntries()
        {
            var ring = new ConsistentHashRing<int, string>();
            Assert.AreEqual(ring.Count, 0);
        }

        [TestMethod]
        public void CanAddElement()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            Assert.AreEqual(ring.Count, 1);
        }

        [TestMethod]
        public void CanRemoveElement()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            ring.Add(93, "Node B");
            ring.Add(17, "Node C");
            ring.Remove(93);
            Assert.AreEqual(ring.Count, 2);
        }

        [TestMethod]
        public void CanFindNearest()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            ring.Add(93, "Node B");
            ring.Add(17, "Node C");
            var nearest = ring.GetNearest(23);
            Assert.AreEqual(nearest.Key, 42);
            Assert.AreEqual(nearest.Value, "Node A");
        }

        [TestMethod]
        public void CanFindNearestBackwards()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            ring.Add(93, "Node B");
            ring.Add(17, "Node C");
            var nearest = ring.GetNearestBackwards(1);
            Assert.AreEqual(nearest.Key, 93);
            Assert.AreEqual(nearest.Value, "Node B");
        }

        [TestMethod]
        public void CanEnumerate()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            ring.Add(93, "Node B");
            ring.Add(17, "Node C");
            var entries = System.Linq.Enumerable.ToArray(ring);
            Assert.AreEqual(entries[0].Key, 17);
            Assert.AreEqual(entries[1].Key, 42);
            Assert.AreEqual(entries[2].Key, 93);
            Assert.AreEqual(entries[0].Value, "Node C");
            Assert.AreEqual(entries[1].Value, "Node A");
            Assert.AreEqual(entries[2].Value, "Node B");
        }

        [TestMethod]
        public void CanEnumerateFromNearest()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            ring.Add(93, "Node B");
            ring.Add(17, "Node C");
            var entries = System.Linq.Enumerable.ToArray(ring.EnumerateFromNearest(23));
            Assert.AreEqual(entries[0].Key, 42);
            Assert.AreEqual(entries[1].Key, 93);
            Assert.AreEqual(entries[2].Key, 17);
            Assert.AreEqual(entries[0].Value, "Node A");
            Assert.AreEqual(entries[1].Value, "Node B");
            Assert.AreEqual(entries[2].Value, "Node C");
        }

        [TestMethod]
        public void CanEnumerateFromNearestBackwards()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            ring.Add(93, "Node B");
            ring.Add(17, "Node C");
            var entries = System.Linq.Enumerable.ToArray(ring.EnumerateFromNearestBackwards(23));
            Assert.AreEqual(entries[0].Key, 17);
            Assert.AreEqual(entries[1].Key, 93);
            Assert.AreEqual(entries[2].Key, 42);
            Assert.AreEqual(entries[0].Value, "Node C");
            Assert.AreEqual(entries[1].Value, "Node B");
            Assert.AreEqual(entries[2].Value, "Node A");
        }

        [TestMethod]
        public void CanEnumerateFromNearestDistinct()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            ring.Add(93, "Node B");
            ring.Add(17, "Node C");
            ring.Add(77, "Node B");
            ring.Add(99, "Node C");
            var entries = System.Linq.Enumerable.ToArray(ring.EnumerateFromNearestDistinct(23));
            Assert.AreEqual(entries.Length, 3);
            Assert.AreEqual(entries[0].Key, 42);
            Assert.AreEqual(entries[1].Key, 77);
            Assert.AreEqual(entries[2].Key, 99);
            Assert.AreEqual(entries[0].Value, "Node A");
            Assert.AreEqual(entries[1].Value, "Node B");
            Assert.AreEqual(entries[2].Value, "Node C");
        }

        [TestMethod]
        public void CanEnumerateFromNearestBackwardsDistinct()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            ring.Add(93, "Node B");
            ring.Add(17, "Node C");
            ring.Add(77, "Node B");
            ring.Add(99, "Node C");
            var entries = System.Linq.Enumerable.ToArray(ring.EnumerateFromNearestBackwardsDistinct(23));
            Assert.AreEqual(entries.Length, 3);
            Assert.AreEqual(entries[0].Key, 17);
            Assert.AreEqual(entries[1].Key, 93);
            Assert.AreEqual(entries[2].Key, 42);
            Assert.AreEqual(entries[0].Value, "Node C");
            Assert.AreEqual(entries[1].Value, "Node B");
            Assert.AreEqual(entries[2].Value, "Node A");
        }

        [TestMethod]
        public void CanGetSuccessor()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            ring.Add(93, "Node B");
            ring.Add(17, "Node C");
            ring.Add(77, "Node B");
            ring.Add(99, "Node C");
            var x = ring.GetSuccessor(17);
            Assert.AreEqual(x.Key, 42);
            Assert.AreEqual(x.Value, "Node A");
        }

        [TestMethod]
        public void CanGetPredecessor()
        {
            var ring = new ConsistentHashRing<int, string>();
            ring.Add(42, "Node A");
            ring.Add(93, "Node B");
            ring.Add(17, "Node C");
            ring.Add(77, "Node B");
            ring.Add(99, "Node C");
            var x = ring.GetPredecessor(17);
            Assert.AreEqual(x.Key, 99);
            Assert.AreEqual(x.Value, "Node C");
        }
    }
}
