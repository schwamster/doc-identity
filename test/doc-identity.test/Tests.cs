using System;
using Xunit;
using doc_identity;
using FluentAssertions;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Test1() 
        {
            var users =  doc_identity.Config.GetUsers();
            users.Count.Should().Be(2);
        }
    }
}
