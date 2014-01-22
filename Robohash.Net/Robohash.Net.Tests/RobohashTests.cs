﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Robohash.Net.Tests
{
    [TestClass]
    public class RobohashTests
    {
        private readonly string[] _inputs =
        {
            "test"
        };

        private readonly string[] _hexDigests =
        {
            "ee26b0dd4af7e749aa1a8ee3c10ae9923f618980772e473f8819a5d4940e0db27ac185f8a0e1d5f84f88bc887fd67b143732c304cc5fa9ad8e6f57f50028a8ff"
        };

        private readonly long[][] _hashes =
        {
            new []{ 16365621466287L, 8689954724494L, 15651140704547L, 16911593142062L, 4896136993373L, 5033937449594L, 13298821631517L, 6564044389512L, 8784947790707L, 3036622380969L, 11926704062288L}
        };

        [TestMethod]
        public void HexDigestTests()
        {
            for (var i = 0; i < _inputs.Length; ++i)
            {
                var r = Robohash.Create(_inputs[i]);
                Assert.AreEqual(_hexDigests[i], r.HexDigest, "HexDigest for input #{0} does not match.", i);
            }
        }

        [TestMethod]
        public void HashesTests()
        {
            for (var i = 0; i < _inputs.Length; ++i)
            {
                var r = Robohash.Create(_inputs[i]);

                var h = _hashes[i];
                Assert.AreEqual(h.Length, r.Hashes.Length, "Hashes length for input #{0} does not match.", i);
                for (var x = 0; x < h.Length; ++x)
                {
                    Assert.AreEqual(h[i], r.Hashes[i], "Hash at position {1} of input #{0} does not match.", i, x);
                }
            }
        }
    }
}
