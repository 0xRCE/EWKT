using EWKT.Parsers;
using EWKT.Primitives;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWKT.Tests.Parsers
{
    public class EWKTParserTests
    {
        [Test]
        public void Test_EWKTParser_RootGeometry()
        {
            var ewkt = "POINT (10 20)";

            var root = EWKTParser.Convert(ewkt);

            Assert.IsNotNull(root);
            Assert.IsInstanceOf<PointZ>(root);
        }

        [Test]
        public void Test_EWKTParser_Point_Negative_Coordinate()
        {
            var ewkt = "POINT(-1 2.0)";
            var parser = EWKTParser.CreateParser(ewkt);

            var geom = parser.Parse();
            Assert.AreEqual("POINT", geom.Name);
            Assert.IsNotNull(geom);

            var coordinates = geom.Coordinates.ToList();
            Assert.AreEqual(1, coordinates.Count);
            Assert.AreEqual("-1 2.0", coordinates[0].Set);
        }

        [Test]
        public void Test_EWKTParser_PointGeometry_Negative_Coordinate()
        {
            var ewkt = "POINT(-1 2.0)";
            var geom = EWKTParser.Convert(ewkt) as PointZ;

            Assert.IsNotNull(geom);
            var point = geom.Coordinate;
            Assert.AreEqual(-1, point.X);
            Assert.AreEqual(2.0d, point.Y);
        }

        [Test]
        public void Test_EWKTParser_Null()
        {
            var ewkt = (string)null;
            var geom = EWKTParser.Convert(ewkt);

            Assert.IsNull(geom);
        }

        [Test]
        public void Test_EWKTParser_Simple_Polygon()
        {
            var ewkt = "POLYGON Z((30 10 1,40 40 1,20 40 1,10 20 1,30 10 1))";
            var parser = EWKTParser.CreateParser(ewkt);

            var geom = parser.Parse();
            Assert.AreEqual("POLYGON Z", geom.Name);
            Assert.IsNotNull(geom);

            var coordinates = geom.Children.First().Coordinates.ToList();
            Assert.AreEqual(1, coordinates.Count);
            Assert.AreEqual("30 10 1, 40 40 1, 20 40 1, 10 20 1, 30 10 1", coordinates[0].Set);
        }

        [Test]
        public void Test_EWKTParser_Complex_CurvePolygon()
        {
            var ewkt = "CURVEPOLYGON(CIRCULARSTRING(1 3, 3 5, 4 7, 7 3, 1 3))";
            var parser = EWKTParser.CreateParser(ewkt);

            var geom = parser.Parse();
            Assert.AreEqual("CURVEPOLYGON", geom.Name);
            Assert.IsNotNull(geom);

            var coordinates = geom.Coordinates.ToList();
            Assert.AreEqual(0, coordinates.Count);

            var child = geom.Children.FirstOrDefault();
            Assert.IsNotNull(child);
            coordinates = child.Coordinates.ToList();
            Assert.AreEqual(1, coordinates.Count);
            Assert.AreEqual("1 3, 3 5, 4 7, 7 3, 1 3", coordinates[0].Set);
        }


    }
}
