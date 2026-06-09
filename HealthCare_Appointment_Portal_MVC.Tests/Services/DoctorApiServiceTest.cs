using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal_MVC.Models;
using HealthCare_Appointment_Portal_MVC.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal_MVC.Tests.Services
{
    public class DoctorApiServiceTests
    {
        private static HttpClient CreateClient(
            HttpResponseMessage response)
        {
            var handler =
                new Mock<HttpMessageHandler>();

            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            return new HttpClient(
                handler.Object)
            {
                BaseAddress =
                    new Uri(
                        "https://localhost/")
            };
        }

        [Fact]
        public async Task GetAllDoctorsAsync_ReturnsDoctors()
        {
            var doctors =
                new List<DoctorDto>
                {
                    new DoctorDto
                    {
                        DoctorId = 1
                    },
                    new DoctorDto
                    {
                        DoctorId = 2
                    }
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                doctors),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new DoctorApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetAllDoctorsAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task GetDoctorByIdAsync_ReturnsDoctor()
        {
            var doctor =
                new DoctorDto
                {
                    DoctorId = 1
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                doctor),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new DoctorApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetDoctorByIdAsync(
                        1);

            Assert.Equal(
                1,
                result.DoctorId);
        }

        [Fact]
        public async Task GetDoctorsBySpecialisationAsync_ReturnsDoctors()
        {
            var doctors =
                new List<DoctorDto>
                {
                    new DoctorDto
                    {
                        DoctorId = 1,
                        Specialisation =
                            Specialisation.Cardiology
                    }
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                doctors),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new DoctorApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetDoctorsBySpecialisationAsync(
                        Specialisation.Cardiology);

            Assert.Single(
                result);
        }

        [Fact]
        public async Task CreateDoctorAsync_ReturnsDoctorId()
        {
            var createdDoctor =
                new DoctorDto
                {
                    DoctorId = 10
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.Created)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                createdDoctor),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new DoctorApiService(
                    CreateClient(response));

            var result =
                await service
                    .CreateDoctorAsync(
                        new CreateDoctorDto
                        {
                            FullName =
                                "John Doe",
                            Specialisation =
                                Specialisation.Cardiology,
                            YearsOfExperience =
                                5,
                            ConsultationFee =
                                500
                        });

            Assert.Equal(
                10,
                result);
        }

        [Fact]
        public async Task UpdateDoctorAsync_CompletesSuccessfully()
        {
            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NoContent);

            var service =
                new DoctorApiService(
                    CreateClient(response));

            var exception =
                await Record.ExceptionAsync(
                    () => service.UpdateDoctorAsync(
                        1,
                        new UpdateDoctorDto
                        {
                            FullName = "Updated Doctor",
                            Specialisation = Specialisation.Neurology,
                            YearsOfExperience = 10,
                            ConsultationFee = 1000,
                            IsActive = true
                        }));

            Assert.Null(exception);
        }

        [Fact]
        public async Task DeleteDoctorAsync_CompletesSuccessfully()
        {
            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NoContent);

            var service =
                new DoctorApiService(
                    CreateClient(response));

            var exception =
                await Record.ExceptionAsync(
                    () => service.DeleteDoctorAsync(
                        1));

            Assert.Null(exception);
        }

        [Fact]
        public async Task GetDoctorByIdAsync_WhenApiReturnsNotFound_ThrowsException()
        {
            var error =
                new ApiErrorResponse
                {
                    Message =
                        "Doctor not found"
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NotFound)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                error),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new DoctorApiService(
                    CreateClient(response));

            var exception =
                 await Assert.ThrowsAsync<HttpRequestException>(
                    () => service.GetDoctorByIdAsync(999));

            Assert.Equal(
                "Doctor not found",
                exception.Message);
        }

        [Fact]
        public async Task GetAllDoctorsAsync_WhenApiReturnsServerError_ThrowsException()
        {
            var error =
                new ApiErrorResponse
                {
                    Message =
                        "Internal Server Error"
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.InternalServerError)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                error),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new DoctorApiService(
                    CreateClient(response));

            var exception =
                 await Assert.ThrowsAsync<HttpRequestException>(
                    () => service.GetAllDoctorsAsync());

            Assert.Equal(
                "Internal Server Error",
                exception.Message);
        }

        [Fact]
        public async Task DeleteDoctorAsync_WhenApiReturnsBadRequest_ThrowsException()
        {
            var error =
                new ApiErrorResponse
                {
                    Message =
                        "Doctor cannot be deleted"
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.BadRequest)
                {
                    Content =
                        new StringContent(
                            JsonConvert.SerializeObject(
                                error),
                            Encoding.UTF8,
                            "application/json")
                };

            var service =
                new DoctorApiService(
                    CreateClient(response));

            var exception =
                  await Assert.ThrowsAsync<HttpRequestException>(
                      () => service.DeleteDoctorAsync(1));

            Assert.Equal(
                "Doctor cannot be deleted",
                exception.Message);
        }
    }
}