using System;
using JsonDiff.Service;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace JsonDiff.Tests.Service
{
    [TestFixture]
    public class EncodeHandlerTest
    {
        private readonly string jsonEncoded = "eyJpZCI6IjUwIn0=";
        private readonly string uncodedString = "GwUePB=DhIWwK=123=;'@h76dlZKM";

        [Test]
        public void Should_Not_Decode_No_Base_64_String()
        {
            // Arrange
            EncodeHandler encoder = new EncodeHandler();

            // Act / Assert
            Assert.Throws<FormatException>(() => encoder.Decode(uncodedString));
        }

        [Test]
        public void Should_Decode_Base_64_String()
        {
            // Arrange
            EncodeHandler encoder = new EncodeHandler();

            // Act
            var result = encoder.Decode(jsonEncoded);

            // Assert
            Assert.AreEqual("{\"id\":\"50\"}", result);
        }
    }
}
