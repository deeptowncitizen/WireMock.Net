﻿using System;
using System.Net.Http;
using NFluent;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace WireMock.Net.Tests
{
    public class ObservableLogEntriesTest: IDisposable
    {
        private FluentMockServer _server;

        [Fact]
        public async void Test()
        {
            // Assign
            _server = FluentMockServer.Start();

            _server
                .Given(Request.Create()
                    .WithPath("/foo")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithBody(@"{ msg: ""Hello world!""}"));

            int count = 0;
            _server.LogEntriesChanged += (sender, args) => count++;

            // Act
            await new HttpClient().GetAsync("http://localhost:" + _server.Ports[0] + "/foo");

            // Assert
            Check.That(count).Equals(1);
        }

        public void Dispose()
        {
            _server?.Dispose();
        }
    }
}
