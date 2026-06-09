using FluentAssertions;
using HealthcareMvcApiTests.Fakes;
using HealthcareWeb.Services;
using Newtonsoft.Json.Linq;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HealthcareMvcApiTests.Services.Web
{
    public class PatientApiServiceTests
    {
        private const string BaseUrl = "https://localhost:44364/api/";

        private static PatientApiService CreateService(
            HttpResponseMessage response,
            out FakeHttpMessageHandler handler)
        {
            handler = new FakeHttpMessageHandler(response);

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            return new PatientApiService(httpClient);
        }

        private static HttpResponseMessage JsonResponse(
            HttpStatusCode statusCode,
            string json)
        {
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                {
                    ""patientId"": 1,
                    ""fullName"": ""John Mathew"",
                    ""dateOfBirth"": ""1995-01-01T00:00:00"",
                    ""gender"": 0,
                    ""phoneNumber"": ""9876543210"",
                    ""email"": ""john@example.com"",
                    ""insuranceId"": ""INS001""
                },
                {
                    ""patientId"": 2,
                    ""fullName"": ""Mary Thomas"",
                    ""dateOfBirth"": ""1998-02-02T00:00:00"",
                    ""gender"": 1,
                    ""phoneNumber"": ""9876543211"",
                    ""email"": ""mary@example.com"",
                    ""insuranceId"": ""INS002""
                }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result[0].PatientId.Should().Be(1);
            result[0].FullName.Should().Be("John Mathew");
            result[1].PatientId.Should().Be(2);
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "patients");
        }

        [Fact]
        public async Task GetAllAsync_ThrowsException_WithCleanMessage_OnFailure()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.InternalServerError,
                    @"{ ""message"": ""Unexpected server error."" }"),
                out _);

            // Act
            Func<Task> action = async () => await service.GetAllAsync();

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Unexpected server error.");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDto_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"{
                ""patientId"": 1,
                ""fullName"": ""John Mathew"",
                ""dateOfBirth"": ""1995-01-01T00:00:00"",
                ""gender"": 0,
                ""phoneNumber"": ""9876543210"",
                ""email"": ""john@example.com"",
                ""insuranceId"": ""INS001""
            }";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            result.PatientId.Should().Be(1);
            result.FullName.Should().Be("John Mathew");
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "patients/1");
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WithCleanMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound,
                    @"{ ""message"": ""Patient with ID 999 was not found."" }"),
                out _);

            // Act
            Func<Task> action = async () => await service.GetByIdAsync(999);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Patient with ID 999 was not found.");
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WithCleanMessage_UsingCapitalMessageKey()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound,
                    @"{ ""Message"": ""Patient not found."" }"),
                out _);

            // Act
            Func<Task> action = async () => await service.GetByIdAsync(999);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Patient not found.");
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WithRawContent_WhenResponseIsNotJson()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.InternalServerError, "Internal Server Error"),
                out _);

            // Act
            Func<Task> action = async () => await service.GetByIdAsync(1);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Internal Server Error");
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WithFallbackMessage_WhenErrorBodyIsEmpty()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var service = CreateService(response, out _);

            // Act
            Func<Task> action = async () => await service.GetByIdAsync(1);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("API request failed.");
        }

        [Fact]
        public async Task AddAsync_ReturnsDto_AndPostsToCorrectUrl()
        {
            // Arrange
            var responseJson = @"{
                ""patientId"": 1,
                ""fullName"": ""John Mathew"",
                ""dateOfBirth"": ""1995-01-01T00:00:00"",
                ""gender"": 0,
                ""phoneNumber"": ""9876543210"",
                ""email"": ""john@example.com"",
                ""insuranceId"": ""INS001""
            }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.Created, responseJson),
                out var handler);

            var dto = new CreatePatientDto
            {
                FullName = "John Mathew",
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "john@example.com",
                InsuranceId = "INS001"
            };

            // Act
            var result = await service.AddAsync(dto);

            // Assert
            result.PatientId.Should().Be(1);
            result.FullName.Should().Be("John Mathew");
            handler.Request.Method.Should().Be(HttpMethod.Post);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "patients");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);
            body["FullName"].Value<string>().Should().Be("John Mathew");
            body["Email"].Value<string>().Should().Be("john@example.com");
            body["InsuranceId"].Value<string>().Should().Be("INS001");
        }

        [Fact]
        public async Task AddAsync_ThrowsException_WithCleanMessage_OnBadRequest()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.BadRequest,
                    @"{ ""message"": ""Email already in use."" }"),
                out _);

            var dto = new CreatePatientDto
            {
                FullName = "John Mathew",
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9876543210",
                Email = "john@example.com",
                InsuranceId = "INS001"
            };

            // Act
            Func<Task> action = async () => await service.AddAsync(dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Email already in use.");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsDto_AndPutsToCorrectUrl()
        {
            // Arrange
            var responseJson = @"{
                ""patientId"": 1,
                ""fullName"": ""John Updated"",
                ""dateOfBirth"": ""1995-01-01T00:00:00"",
                ""gender"": 0,
                ""phoneNumber"": ""9999999999"",
                ""email"": ""john.updated@example.com"",
                ""insuranceId"": ""INS001""
            }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.OK, responseJson),
                out var handler);

            var dto = new UpdatePatientDto
            {
                FullName = "John Updated",
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "john.updated@example.com",
                InsuranceId = "INS001"
            };

            // Act
            var result = await service.UpdateAsync(1, dto);

            // Assert
            result.PatientId.Should().Be(1);
            result.FullName.Should().Be("John Updated");
            handler.Request.Method.Should().Be(HttpMethod.Put);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "patients/1");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);
            body["FullName"].Value<string>().Should().Be("John Updated");
            body["Email"].Value<string>().Should().Be("john.updated@example.com");
            body["PhoneNumber"].Value<string>().Should().Be("9999999999");
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WithCleanMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound,
                    @"{ ""message"": ""Patient not found."" }"),
                out _);

            var dto = new UpdatePatientDto
            {
                FullName = "John Updated",
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "9999999999",
                Email = "john.updated@example.com",
                InsuranceId = "INS001"
            };

            // Act
            Func<Task> action = async () => await service.UpdateAsync(99, dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Patient not found.");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsDto_AndSendsDeleteToCorrectUrl()
        {
            // Arrange
            var responseJson = @"{
                ""patientId"": 1,
                ""fullName"": ""John Mathew"",
                ""dateOfBirth"": ""1995-01-01T00:00:00"",
                ""gender"": 0,
                ""phoneNumber"": ""9876543210"",
                ""email"": ""john@example.com"",
                ""insuranceId"": ""INS001""
            }";

            var service = CreateService(
                JsonResponse(HttpStatusCode.OK, responseJson),
                out var handler);

            // Act
            var result = await service.DeleteAsync(1);

            // Assert
            result.PatientId.Should().Be(1);
            result.FullName.Should().Be("John Mathew");
            handler.Request.Method.Should().Be(HttpMethod.Delete);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "patients/1");
            handler.Request.Content.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WithCleanMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound,
                    @"{ ""message"": ""Patient not found."" }"),
                out _);

            // Act
            Func<Task> action = async () => await service.DeleteAsync(99);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Patient not found.");
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WithCleanMessage_UsingModelStateKey()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.BadRequest,
                    @"{ ""modelState"": ""Validation failed."" }"),
                out _);

            // Act
            Func<Task> action = async () => await service.DeleteAsync(1);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Validation failed.");
        }
    }
}