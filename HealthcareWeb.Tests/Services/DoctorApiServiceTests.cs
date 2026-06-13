using HealthcareWeb.Services;
using Newtonsoft.Json;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HealthcareWeb.Tests.Services
{
    public class DoctorApiServiceTests
    {
        [Fact]
        public async Task GetAllAsync_WhenApiReturnsSuccess_ShouldReturnDoctors()
        {
            // Arrange
            List<DoctorDto> doctors = new List<DoctorDto>
            {
                CreateDoctorDto()
            };

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, doctors));

            DoctorApiService service = CreateService(handler);

            // Act
            List<DoctorDto> result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(2, result[0].DoctorId);
            Assert.Equal("Rahul Sharma", result[0].FullName);
            Assert.Equal("http://localhost/api/doctors", handler.LastRequest.RequestUri.ToString());
            Assert.Equal(System.Net.Http.HttpMethod.Get, handler.LastRequest.Method);
        }

        [Fact]
        public async Task GetAllActiveAsync_WhenApiReturnsSuccess_ShouldReturnActiveDoctors()
        {
            // Arrange
            List<DoctorDto> doctors = new List<DoctorDto>
            {
                CreateDoctorDto()
            };

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, doctors));

            DoctorApiService service = CreateService(handler);

            // Act
            List<DoctorDto> result = await service.GetAllActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.True(result[0].IsActive);
            Assert.Equal("http://localhost/api/doctors/active", handler.LastRequest.RequestUri.ToString());
            Assert.Equal(System.Net.Http.HttpMethod.Get, handler.LastRequest.Method);
        }

        [Fact]
        public async Task GetByIdAsync_WhenApiReturnsSuccess_ShouldReturnDoctor()
        {
            // Arrange
            DoctorDto doctor = CreateDoctorDto();

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, doctor));

            DoctorApiService service = CreateService(handler);

            // Act
            DoctorDto result = await service.GetByIdAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.DoctorId);
            Assert.Equal("Rahul Sharma", result.FullName);
            Assert.Equal("http://localhost/api/doctors/2", handler.LastRequest.RequestUri.ToString());
            Assert.Equal(System.Net.Http.HttpMethod.Get, handler.LastRequest.Method);
        }

        [Fact]
        public async Task SearchBySpecialisationAsync_WhenApiReturnsSuccess_ShouldReturnDoctors()
        {
            // Arrange
            Specialisation specialisation = GetAnySpecialisation();

            List<DoctorDto> doctors = new List<DoctorDto>
            {
                CreateDoctorDto(specialisation)
            };

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, doctors));

            DoctorApiService service = CreateService(handler);

            // Act
            List<DoctorDto> result = await service.SearchBySpecialisationAsync(specialisation);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(specialisation, result[0].Specialisation);
            Assert.Equal(
                "http://localhost/api/doctors/specialisation/" + (int)specialisation,
                handler.LastRequest.RequestUri.ToString());
            Assert.Equal(System.Net.Http.HttpMethod.Get, handler.LastRequest.Method);
        }

        [Fact]
        public async Task AddAsync_WhenApiReturnsSuccess_ShouldPostDoctorAndReturnDoctor()
        {
            // Arrange
            Specialisation specialisation = GetAnySpecialisation();

            CreateDoctorDto createDoctorDto = new CreateDoctorDto
            {
                FullName = "Rahul Sharma",
                Specialisation = specialisation,
                PracticeStartDate = DateTime.Today.AddYears(-5),
                ConsultationFee = 500
            };

            DoctorDto doctor = CreateDoctorDto(specialisation);

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, doctor));

            DoctorApiService service = CreateService(handler);

            // Act
            DoctorDto result = await service.AddAsync(createDoctorDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.DoctorId);
            Assert.Equal("Rahul Sharma", result.FullName);
            Assert.Equal(System.Net.Http.HttpMethod.Post, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/doctors", handler.LastRequest.RequestUri.ToString());
            Assert.Contains("\"FullName\":\"Rahul Sharma\"", handler.LastRequestBody);
            Assert.Contains("\"ConsultationFee\":500", handler.LastRequestBody);
        }

        [Fact]
        public async Task UpdateAsync_WhenApiReturnsSuccess_ShouldPutDoctorAndReturnDoctor()
        {
            // Arrange
            Specialisation specialisation = GetAnySpecialisation();

            UpdateDoctorDto updateDoctorDto = new UpdateDoctorDto
            {
                FullName = "Rahul Sharma",
                Specialisation = specialisation,
                PracticeStartDate = DateTime.Today.AddYears(-5),
                ConsultationFee = 700,
                IsActive = true
            };

            DoctorDto doctor = CreateDoctorDto(specialisation);

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, doctor));

            DoctorApiService service = CreateService(handler);

            // Act
            DoctorDto result = await service.UpdateAsync(2, updateDoctorDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.DoctorId);
            Assert.Equal("Rahul Sharma", result.FullName);
            Assert.Equal(System.Net.Http.HttpMethod.Put, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/doctors/2", handler.LastRequest.RequestUri.ToString());
            Assert.Contains("\"FullName\":\"Rahul Sharma\"", handler.LastRequestBody);
            Assert.Contains("\"ConsultationFee\":700", handler.LastRequestBody);
            Assert.Contains("\"IsActive\":true", handler.LastRequestBody);
        }

        [Fact]
        public async Task DeactivateAsync_WhenApiReturnsSuccess_ShouldDeleteDoctorAndReturnDoctor()
        {
            // Arrange
            DoctorDto doctor = CreateDoctorDto();
            doctor.IsActive = false;

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, doctor));

            DoctorApiService service = CreateService(handler);

            // Act
            DoctorDto result = await service.DeactivateAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.DoctorId);
            Assert.False(result.IsActive);
            Assert.Equal(System.Net.Http.HttpMethod.Delete, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/doctors/2", handler.LastRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task ReactivateAsync_WhenApiReturnsSuccess_ShouldPostAndReturnDoctor()
        {
            // Arrange
            DoctorDto doctor = CreateDoctorDto();
            doctor.IsActive = true;

            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                CreateJsonResponse(HttpStatusCode.OK, doctor));

            DoctorApiService service = CreateService(handler);

            // Act
            DoctorDto result = await service.ReactivateAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.DoctorId);
            Assert.True(result.IsActive);
            Assert.Equal(System.Net.Http.HttpMethod.Post, handler.LastRequest.Method);
            Assert.Equal("http://localhost/api/doctors/2/reactivate", handler.LastRequest.RequestUri.ToString());
        }

        [Theory]
        [InlineData("{\"message\":\"Doctor name should contain only letters and spaces.\"}", "Doctor name should contain only letters and spaces.")]
        [InlineData("{\"Message\":\"Doctor not found.\"}", "Doctor not found.")]
        [InlineData("Plain doctor error", "Plain doctor error")]
        [InlineData("", "The request could not be completed.")]
        public async Task GetAllAsync_WhenApiReturnsError_ShouldThrowExtractedMessage(
            string responseContent,
            string expectedMessage)
        {
            // Arrange
            FakeHttpMessageHandler handler = new FakeHttpMessageHandler(
                new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                });

            DoctorApiService service = CreateService(handler);

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(() =>
                service.GetAllAsync());

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        private static DoctorApiService CreateService(FakeHttpMessageHandler handler)
        {
            HttpClient httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost/api/")
            };

            return new DoctorApiService(httpClient);
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

        private static DoctorDto CreateDoctorDto()
        {
            return CreateDoctorDto(GetAnySpecialisation());
        }

        private static DoctorDto CreateDoctorDto(Specialisation specialisation)
        {
            return new DoctorDto
            {
                DoctorId = 2,
                FullName = "Rahul Sharma",
                Specialisation = specialisation,
                PracticeStartDate = DateTime.Today.AddYears(-5),
                YearsOfExperience = 5,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        private static Specialisation GetAnySpecialisation()
        {
            Array values = Enum.GetValues(typeof(Specialisation));
            return (Specialisation)values.GetValue(0);
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