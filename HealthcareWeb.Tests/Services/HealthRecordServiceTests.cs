using HealthcareWeb.Services;
using Newtonsoft.Json;
using System.Net.Http;
using SharedClasses.Dtos;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Collections.Generic;
using System.Net;

namespace HealthcareWeb.Tests.Services
{
    public class HealthRecordApiServiceTests
    {
        [Fact]
        public async Task GetAllAsync_WhenApiReturnsSuccess_ShouldReturnHealthRecords()
        {
            // Arrange
            List<HealthRecordDto> healthRecords = new List<HealthRecordDto>
            {
                CreateHealthRecordDto()
            };

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, healthRecords));

            HealthRecordApiService service = CreateService(handler);

            // Act
            List<HealthRecordDto> result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(System.Net.Http.HttpMethod.Get, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/healthrecords", handler.LastRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task GetByIdAsync_WhenApiReturnsSuccess_ShouldReturnHealthRecord()
        {
            // Arrange
            HealthRecordDto healthRecord = CreateHealthRecordDto();

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, healthRecord));

            HealthRecordApiService service = CreateService(handler);

            // Act
            HealthRecordDto result = await service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(System.Net.Http.HttpMethod.Get, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/healthrecords/1", handler.LastRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task GetByPatientAsync_WhenApiReturnsSuccess_ShouldReturnHealthRecords()
        {
            // Arrange
            List<HealthRecordDto> healthRecords = new List<HealthRecordDto>
            {
                CreateHealthRecordDto()
            };

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, healthRecords));

            HealthRecordApiService service = CreateService(handler);

            // Act
            List<HealthRecordDto> result = await service.GetByPatientAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(System.Net.Http.HttpMethod.Get, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/healthrecords/patient/10", handler.LastRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task GetByDoctorAsync_WhenApiReturnsSuccess_ShouldReturnHealthRecords()
        {
            // Arrange
            List<HealthRecordDto> healthRecords = new List<HealthRecordDto>
            {
                CreateHealthRecordDto()
            };

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, healthRecords));

            HealthRecordApiService service = CreateService(handler);

            // Act
            List<HealthRecordDto> result = await service.GetByDoctorAsync(20);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(System.Net.Http.HttpMethod.Get, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/healthrecords/doctor/20", handler.LastRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task GetByAppointmentAsync_WhenApiReturnsSuccess_ShouldReturnHealthRecords()
        {
            // Arrange
            List<HealthRecordDto> healthRecords = new List<HealthRecordDto>
            {
                CreateHealthRecordDto()
            };

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, healthRecords));

            HealthRecordApiService service = CreateService(handler);

            // Act
            List<HealthRecordDto> result = await service.GetByAppointmentAsync(30);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(System.Net.Http.HttpMethod.Get, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/healthrecords/appointment/30", handler.LastRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task AddAsync_WhenApiReturnsSuccess_ShouldPostHealthRecordAndReturnHealthRecord()
        {
            // Arrange
            AddHealthRecordDto addHealthRecordDto = CreateAddHealthRecordDto();

            HealthRecordDto healthRecord = CreateHealthRecordDto();

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, healthRecord));

            HealthRecordApiService service = CreateService(handler);

            // Act
            HealthRecordDto result = await service.AddAsync(addHealthRecordDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(System.Net.Http.HttpMethod.Post, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/healthrecords", handler.LastRequest.RequestUri.ToString());
            Assert.False(string.IsNullOrWhiteSpace(handler.LastRequestBody));
            Assert.Contains("{", handler.LastRequestBody);
        }

        [Fact]
        public async Task UpdateAsync_WhenApiReturnsSuccess_ShouldPutHealthRecordAndReturnHealthRecord()
        {
            // Arrange
            UpdateHealthRecordDto updateHealthRecordDto = CreateUpdateHealthRecordDto();

            HealthRecordDto healthRecord = CreateHealthRecordDto();

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, healthRecord));

            HealthRecordApiService service = CreateService(handler);

            // Act
            HealthRecordDto result = await service.UpdateAsync(1, updateHealthRecordDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(System.Net.Http.HttpMethod.Put, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/healthrecords/1", handler.LastRequest.RequestUri.ToString());
            Assert.False(string.IsNullOrWhiteSpace(handler.LastRequestBody));
            Assert.Contains("{", handler.LastRequestBody);
        }

        [Fact]
        public async Task DeleteAsync_WhenApiReturnsSuccess_ShouldDeleteHealthRecordAndReturnHealthRecord()
        {
            // Arrange
            HealthRecordDto healthRecord = CreateHealthRecordDto();

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, healthRecord));

            HealthRecordApiService service = CreateService(handler);

            // Act
            HealthRecordDto result = await service.DeleteAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(System.Net.Http.HttpMethod.Delete, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/healthrecords/1", handler.LastRequest.RequestUri.ToString());
        }

        [Theory]
        [InlineData("Health record not found.", "Health record not found.")]
        [InlineData("Invalid patient id.", "Invalid patient id.")]
        [InlineData("", "API request failed.")]
        public async Task GetAllAsync_WhenApiReturnsError_ShouldThrowApiMessage(
            string responseContent,
            string expectedMessage)
        {
            // Arrange
            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            HealthRecordApiService service = CreateService(handler);

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(() =>
                service.GetAllAsync());

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        private static HealthRecordApiService CreateService(FakeHttpMessageHandler handler)
        {
            HttpClient httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost/api/")
            };

            return new HealthRecordApiService(httpClient);
        }

        private static HttpResponseMessage CreateJsonResponse<T>(
            HttpStatusCode statusCode,
            T value)
        {
            string json = JsonConvert.SerializeObject(value);

            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private static HealthRecordDto CreateHealthRecordDto()
        {
            return new HealthRecordDto();
        }

        private static AddHealthRecordDto CreateAddHealthRecordDto()
        {
            return new AddHealthRecordDto();
        }

        private static UpdateHealthRecordDto CreateUpdateHealthRecordDto()
        {
            return new UpdateHealthRecordDto();
        }

        private class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public HttpRequestMessage LastRequest { get; private set; }

            public string LastRequestBody { get; private set; }

            public FakeHttpMessageHandler(HttpResponseMessage response)
            {
                _response = response;
            }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                LastRequest = request;

                if (request.Content != null)
                {
                    LastRequestBody = await request.Content.ReadAsStringAsync();
                }
                else
                {
                    LastRequestBody = string.Empty;
                }

                return _response;
            }
        }
    }
}
