using System;
using VehicleTrackingSystem.CustomObjects.Settings;
using VehicleTrackingSystem.Security.Handlers;
using Xunit;

namespace VehicleTrackingSystem.UnitTest
{
    public class SecurityTest
    {
        [Fact]
        public void GeneratePasswordHashSuccessMethod()
        {
            var password = "123456";
            var _cryptographyHandler = new CryptographyHandler();
            var hashPassword = _cryptographyHandler.GeneratePasswordHash(password);
            Assert.True(!string.IsNullOrEmpty(hashPassword));
        }

        [Fact]
        public void VerifyGeneratedHashSuccessMethod()
        {
            var password = "123456";
            var savedPasswordHash = "9SX59yDbWpfRpbGfTqNnqw2y8AA6E+TEvu5aWCx3fl+bRblA";
            var _cryptographyHandler = new CryptographyHandler();
            Assert.True(_cryptographyHandler.VerifyGeneratedHash(password, savedPasswordHash));
        }

        [Fact]
        public void GenerateJwtSecurityTokenSuccessMethod()
        {
            AppSettings appSettings = new AppSettings();
            appSettings.Secret = "1234567890 a very long word";
            string userId = "1";
            string Role = "Admin";
            var token = new JwtTokenHandler(appSettings).GenerateJwtSecurityToken(userId, Role);
            Assert.True(!string.IsNullOrEmpty(token));
        }

        [Fact]
        public void VerifyJwtSecurityTokenSuccessMethod()
        {
            AppSettings appSettings = new AppSettings();
            appSettings.Secret = "1234567890 a very long word";
            var _jwtTokenHandler = new JwtTokenHandler(appSettings);
            var user = _jwtTokenHandler.VerifyJwtSecurityToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJyb2xlIjoiQWRtaW4iLCJuYmYiOjE2Mjc1NTU1MjQsImV4cCI6MTYyOTI4MzUyNCwiaWF0IjoxNjI3NTU1NTI0fQ.YdsRTTEhpr2aMCCHlUrPcqqbDv3EaKKOP4ncPh2lqjQ");
            Assert.Equal("1", user);
        }
    }
}
