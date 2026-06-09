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
    public class DoctorApiServiceTests
    {
        private const string BaseUrl = "https://localhost:44364/api/";

        private static DoctorApiService CreateService(
            HttpResponseMessage response,
            out FakeHttpMessageHandler handler)
        {
            handler = new FakeHttpMessageHandler(response);

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            return new DoctorApiService(httpClient);
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

        private static string SingleDoctorJson(
            int doctorId = 1,
            string fullName = "Anil Kumar",
            int specialisation = 0,
            string practiceStartDate = "2015-01-01T00:00:00",
            int yearsOfExperience = 11,
            decimal consultationFee = 500,
            bool isActive = true)
        {
            return $@"{{
                ""doctorId"": {doctorId},
                ""fullName"": ""{fullName}"",
                ""specialisation"": {specialisation},
                ""practiceStartDate"": ""{practiceStartDate}"",
                ""yearsOfExperience"": {yearsOfExperience},
                ""consultationFee"": {consultationFee},
                ""isActive"": {isActive.ToString().ToLower()}
            }}";
        }

        [Fact]
        public async Task GetAllAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                {
                    ""doctorId"": 1,
                    ""fullName"": ""Anil Kumar"",
                    ""specialisation"": 0,
                    ""practiceStartDate"": ""2015-01-01T00:00:00"",
                    ""yearsOfExperience"": 11,
                    ""consultationFee"": 500.00,
                    ""isActive"": true
                },
                {
                    ""doctorId"": 2,
                    ""fullName"": ""Meera Nair"",
                    ""specialisation"": 1,
                    ""practiceStartDate"": ""2018-01-01T00:00:00"",
                    ""yearsOfExperience"": 8,
                    ""consultationFee"": 650.00,
                    ""isActive"": false
                }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result[0].DoctorId.Should().Be(1);
            result[0].FullName.Should().Be("Anil Kumar");
            result[1].DoctorId.Should().Be(2);
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "doctors");
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
        public async Task GetAllActiveAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                {
                    ""doctorId"": 1,
                    ""fullName"": ""Anil Kumar"",
                    ""specialisation"": 0,
                    ""practiceStartDate"": ""2015-01-01T00:00:00"",
                    ""yearsOfExperience"": 11,
                    ""consultationFee"": 500.00,
                    ""isActive"": true
                }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.GetAllActiveAsync();

            // Assert
            result.Should().HaveCount(1);
            result[0].IsActive.Should().BeTrue();
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "doctors/active");
        }

        [Fact]
        public async Task GetAllActiveAsync_ThrowsException_WithCleanMessage_OnFailure()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.InternalServerError,
                    @"{ ""message"": ""Server error."" }"),
                out _);

            // Act
            Func<Task> action = async () => await service.GetAllActiveAsync();

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Server error.");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDto_AndGetsCorrectUrl()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.OK, SingleDoctorJson()),
                out var handler);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Anil Kumar");
            result.IsActive.Should().BeTrue();
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "doctors/1");
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WithCleanMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound,
                    @"{ ""message"": ""Doctor with ID 999 was not found."" }"),
                out _);

            // Act
            Func<Task> action = async () => await service.GetByIdAsync(999);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Doctor with ID 999 was not found.");
        }

        [Fact]
        public async Task GetByIdAsync_ThrowsException_WithCleanMessage_UsingCapitalMessageKey()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound,
                    @"{ ""Message"": ""Doctor not found."" }"),
                out _);

            // Act
            Func<Task> action = async () => await service.GetByIdAsync(999);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Doctor not found.");
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
                .WithMessage("The request could not be completed.");
        }

        [Fact]
        public async Task SearchBySpecialisationAsync_ReturnsList_AndGetsCorrectUrl()
        {
            // Arrange
            var json = @"[
                {
                    ""doctorId"": 1,
                    ""fullName"": ""Anil Kumar"",
                    ""specialisation"": 0,
                    ""practiceStartDate"": ""2015-01-01T00:00:00"",
                    ""yearsOfExperience"": 11,
                    ""consultationFee"": 500.00,
                    ""isActive"": true
                }
            ]";

            var service = CreateService(JsonResponse(HttpStatusCode.OK, json), out var handler);

            // Act
            var result = await service.SearchBySpecialisationAsync((Specialisation)0);

            // Assert
            result.Should().HaveCount(1);
            result[0].Specialisation.Should().Be((Specialisation)0);
            handler.Request.Method.Should().Be(HttpMethod.Get);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "doctors/specialisation/0");
        }

        [Fact]
        public async Task SearchBySpecialisationAsync_ThrowsException_WithCleanMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound,
                    @"{ ""message"": ""No doctors found for this specialisation."" }"),
                out _);

            // Act
            Func<Task> action = async () =>
                await service.SearchBySpecialisationAsync((Specialisation)0);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("No doctors found for this specialisation.");
        }

        [Fact]
        public async Task AddAsync_ReturnsDto_AndPostsToCorrectUrl()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.Created, SingleDoctorJson()),
                out var handler);

            var dto = new CreateDoctorDto
            {
                FullName = "Anil Kumar",
                Specialisation = (Specialisation)0,
                PracticeStartDate = new DateTime(2015, 1, 1),
                ConsultationFee = 500
            };

            // Act
            var result = await service.AddAsync(dto);

            // Assert
            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Anil Kumar");
            handler.Request.Method.Should().Be(HttpMethod.Post);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "doctors");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);
            body["FullName"].Value<string>().Should().Be("Anil Kumar");
            body["ConsultationFee"].Value<decimal>().Should().Be(500);
        }

        [Fact]
        public async Task AddAsync_ThrowsException_WithCleanMessage_OnBadRequest()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.BadRequest,
                    @"{ ""message"": ""Practice start date cannot be in the future."" }"),
                out _);

            var dto = new CreateDoctorDto
            {
                FullName = "Invalid Doctor",
                Specialisation = (Specialisation)0,
                PracticeStartDate = DateTime.Today.AddYears(1),
                ConsultationFee = 500
            };

            // Act
            Func<Task> action = async () => await service.AddAsync(dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Practice start date cannot be in the future.");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsDto_AndPutsToCorrectUrl()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.OK,
                    SingleDoctorJson(fullName: "Anil Kumar Updated", consultationFee: 700)),
                out var handler);

            var dto = new UpdateDoctorDto
            {
                FullName = "Anil Kumar Updated",
                Specialisation = (Specialisation)0,
                PracticeStartDate = new DateTime(2015, 1, 1),
                ConsultationFee = 700,
                IsActive = true
            };

            // Act
            var result = await service.UpdateAsync(1, dto);

            // Assert
            result.DoctorId.Should().Be(1);
            result.FullName.Should().Be("Anil Kumar Updated");
            result.ConsultationFee.Should().Be(700);
            handler.Request.Method.Should().Be(HttpMethod.Put);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "doctors/1");

            string requestBody = await handler.Request.Content.ReadAsStringAsync();
            JObject body = JObject.Parse(requestBody);
            body["FullName"].Value<string>().Should().Be("Anil Kumar Updated");
            body["ConsultationFee"].Value<decimal>().Should().Be(700);
            body["IsActive"].Value<bool>().Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WithCleanMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound,
                    @"{ ""message"": ""Doctor not found."" }"),
                out _);

            var dto = new UpdateDoctorDto
            {
                FullName = "Anil Kumar Updated",
                Specialisation = (Specialisation)0,
                PracticeStartDate = new DateTime(2015, 1, 1),
                ConsultationFee = 700,
                IsActive = true
            };

            // Act
            Func<Task> action = async () => await service.UpdateAsync(99, dto);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Doctor not found.");
        }

        [Fact]
        public async Task DeactivateAsync_ReturnsDto_AndSendsDeleteToCorrectUrl()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.OK, SingleDoctorJson(isActive: false)),
                out var handler);

            // Act
            var result = await service.DeactivateAsync(1);

            // Assert
            result.DoctorId.Should().Be(1);
            result.IsActive.Should().BeFalse();
            handler.Request.Method.Should().Be(HttpMethod.Delete);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "doctors/1");
            handler.Request.Content.Should().BeNull();
        }

        [Fact]
        public async Task DeactivateAsync_ThrowsException_WithCleanMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound,
                    @"{ ""message"": ""Doctor not found."" }"),
                out _);

            // Act
            Func<Task> action = async () => await service.DeactivateAsync(99);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Doctor not found.");
        }

        [Fact]
        public async Task ReactivateAsync_ReturnsDto_AndPostsToCorrectUrl()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.OK, SingleDoctorJson(isActive: true)),
                out var handler);

            // Act
            var result = await service.ReactivateAsync(1);

            // Assert
            result.DoctorId.Should().Be(1);
            result.IsActive.Should().BeTrue();
            handler.Request.Method.Should().Be(HttpMethod.Post);
            handler.Request.RequestUri.ToString()
                .Should().Be(BaseUrl + "doctors/1/reactivate");
            handler.Request.Content.Should().BeNull();
        }

        [Fact]
        public async Task ReactivateAsync_ThrowsException_WithCleanMessage_OnNotFound()
        {
            // Arrange
            var service = CreateService(
                JsonResponse(HttpStatusCode.NotFound,
                    @"{ ""message"": ""Doctor not found."" }"),
                out _);

            // Act
            Func<Task> action = async () => await service.ReactivateAsync(99);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Doctor not found.");
        }
    }
}