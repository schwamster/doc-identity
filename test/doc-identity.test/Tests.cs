using System;
using Xunit;
using doc_identity;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Configuration;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Test1() 
        {
            var config = new Mock<IConfiguration>();
            var users =  doc_identity.Config.GetUsers(config.Object);
            users.Count.Should().Be(1);
        }
    }
}
