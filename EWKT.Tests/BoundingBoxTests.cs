using EWKT.Models;
using NUnit.Framework;

namespace EWKT.Tests
{
    public class Tests
    {
        [Test]
        public void Test_BoundingBox_Inside_Expected()
        {
            var box = new BoundingBox(0, 0, 2, 2);

            Assert.IsTrue(box.Inside(new CoordinateModel { X = 1, Y = 1 }));
            Assert.IsTrue(box.Inside(new CoordinateModel { X = 1.99, Y = 1.99 }));
        }

        [Test]
        public void Test_BoundingBox_Inside_Not_Expected()
        {
            var box = new BoundingBox(0, 0, 2, 2);

            Assert.IsFalse(box.Inside(new CoordinateModel { X = 1, Y = 3 }));
            Assert.IsFalse(box.Inside(new CoordinateModel { X = 2, Y = 2 })); //op de rand

        }

        [Test]
        public void Test_BoundingBox_OnEdge_Expected()
        {
            var box = new BoundingBox(0, 0, 2, 2);

            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 0, Y = 0 }));
            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 1, Y = 0 }));
            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 2, Y = 0 }));

            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 2, Y = 0 }));
            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 2, Y = 1 }));
            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 2, Y = 2 }));

            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 0, Y = 2 }));
            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 1, Y = 2 }));
            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 2, Y = 2 }));

            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 0, Y = 0 }));
            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 0, Y = 1 }));
            Assert.IsTrue(box.OnEgde(new CoordinateModel { X = 0, Y = 2 }));
        }

        [Test]
        public void Test_BoundingBox_OnEdge_Not_Expected()
        {
            var box = new BoundingBox(0, 0, 2, 2);

            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 0.1, Y = 0.1 }));
            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 1, Y = 1 }));
            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 2, Y = 2.1 }));

            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = -0.1, Y = 0.1 }));
            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 1, Y = 1 }));
            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 2, Y = -0.1 }));


            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 2, Y = 2.1 }));
            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 2, Y = 3 }));
            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 2, Y = -0.1 }));

            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 0, Y = 2.1 }));
            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 0, Y = 3 }));
            Assert.IsFalse(box.OnEgde(new CoordinateModel { X = 0, Y = -0.1 }));
        }

        [Test]
        public void Test_BoundingBox_Intersection_Expected()
        {
            var box = new BoundingBox(0, 0, 2, 2);

            Assert.IsTrue(box.IntersectsWith(new BoundingBox(-2, -2, 0, 0)));
            Assert.IsTrue(box.IntersectsWith(new BoundingBox(2, 0, 4, 2)));
        }

        [Test]
        public void Test_BoundingBox_Intersection_Not_Expected()
        {
            var box = new BoundingBox(0, 0, 2, 2);

            Assert.IsFalse(box.IntersectsWith(new BoundingBox(-2, -2, -0.000001, 0)));
            Assert.IsFalse(box.IntersectsWith(new BoundingBox(2.0000001, 0, 4, 2)));
        }


        [Test]
        public void Test_BoundingBox_Merge_Other_Smaller()
        {
            var box = new BoundingBox(0, 0, 4, 4);
            var other = new BoundingBox(1, 1, 2, 2);

            var merge = box.Merge(other);
            Assert.IsNotNull(merge);
            Assert.AreEqual(box.MaxX, merge.MaxX);
            Assert.AreEqual(box.MinX, merge.MinX);
            Assert.AreEqual(box.MaxY, merge.MaxY);
            Assert.AreEqual(box.MinY, merge.MinY);
        }


        [Test]
        public void Test_BoundingBox_Merge_Other_Null()
        {
            var box = new BoundingBox(0, 0, 4, 4);
            var other = (BoundingBox)null;

            var merge = box.Merge(other);
            Assert.IsNotNull(merge);
            Assert.AreEqual(box.MaxX, merge.MaxX);
            Assert.AreEqual(box.MinX, merge.MinX);
            Assert.AreEqual(box.MaxY, merge.MaxY);
            Assert.AreEqual(box.MinY, merge.MinY);
        }
    }
}