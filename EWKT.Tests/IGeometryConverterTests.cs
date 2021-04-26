using EWKT.Models;
using EWKT.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Tests
{
    [TestClass]
    public class IGeometryConverterTests
    {
        [TestMethod]
        public void Test_IGeometryConverter_Polygon_With_Interior_Ring()
        {
            Mock<IGeometryConverter> converterMock = new Mock<IGeometryConverter>(MockBehavior.Strict);
            var sequence = new MockSequence();
            converterMock.InSequence(sequence).Setup(x => x.AddSegmentPoints(It.IsAny<IEnumerable<CoordinateModel>>()));
            converterMock.InSequence(sequence).Setup(x => x.NewInteriorRing());
            converterMock.InSequence(sequence).Setup(x => x.AddSegmentPoints(It.IsAny<IEnumerable<CoordinateModel>>()));
            
            var ewkt = "POLYGON((35 10, 45 45, 15 40, 10 20, 35 10), (20 30, 35 35, 30 20, 20 30))";
            var geometry = EWKTParser.Convert(ewkt);
            
            
            geometry.Convert(converterMock.Object);

            converterMock.Verify(x => x.NewPart(), Times.Never());
            converterMock.Verify(x => x.NewInteriorRing(), Times.Once());
            converterMock.Verify(x => x.AddSegmentPoints(It.IsAny<IEnumerable<CoordinateModel>>()), Times.Exactly(2));
        }


        [TestMethod]
        public void Test_IGeometryConverter_Polygon_No_Interior_Rings()
        {
            Mock<IGeometryConverter> converterMock = new Mock<IGeometryConverter>(MockBehavior.Strict);
            var sequence = new MockSequence();
            converterMock.InSequence(sequence).Setup(x => x.AddSegmentPoints(It.IsAny<IEnumerable<CoordinateModel>>()));

            var ewkt = "POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))";
            var geometry = EWKTParser.Convert(ewkt);


            geometry.Convert(converterMock.Object);

            converterMock.Verify(x => x.NewPart(), Times.Never());
            converterMock.Verify(x => x.NewInteriorRing(), Times.Never());
            converterMock.Verify(x => x.AddSegmentPoints(It.IsAny<IEnumerable<CoordinateModel>>()), Times.Once());
        }


        [TestMethod]
        public void Test_IGeometryConverter_MultiPolygon_One_With_Interior_Ring()
        {
            Mock<IGeometryConverter> converterMock = new Mock<IGeometryConverter>(MockBehavior.Strict);
            var sequence = new MockSequence();
            converterMock.InSequence(sequence).Setup(x => x.AddSegmentPoints(It.IsAny<IEnumerable<CoordinateModel>>()));
            converterMock.InSequence(sequence).Setup(x => x.NewPart());
            converterMock.InSequence(sequence).Setup(x => x.AddSegmentPoints(It.IsAny<IEnumerable<CoordinateModel>>()));
            converterMock.InSequence(sequence).Setup(x => x.NewInteriorRing());
            converterMock.InSequence(sequence).Setup(x => x.AddSegmentPoints(It.IsAny<IEnumerable<CoordinateModel>>()));

            var ewkt = "MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)), ((20 35, 10 30, 10 10, 30 5, 45 20, 20 35), (30 20, 20 15, 20 25, 30 20)))";
            var geometry = EWKTParser.Convert(ewkt);


            geometry.Convert(converterMock.Object);

            converterMock.Verify(x => x.NewPart(), Times.Once());
            converterMock.Verify(x => x.NewInteriorRing(), Times.Once());
            converterMock.Verify(x => x.AddSegmentPoints(It.IsAny<IEnumerable<CoordinateModel>>()), Times.Exactly(3));
        }

    }
}
