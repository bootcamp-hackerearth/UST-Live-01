using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
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
    public class AppointmentApiServiceTests
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
                    new Uri("https://localhost/")
            };
        }

        private static StringContent JsonContent(
            object data)
        {
            return new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json");
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ReturnsAppointments()
        {
            var appointments =
                new List<AppointmentDto>
                {
                    new AppointmentDto
                    {
                        AppointmentId = 1
                    },
                    new AppointmentDto
                    {
                        AppointmentId = 2
                    }
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(
                            appointments)
                };

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetAllAppointmentsAsync();

            Assert.Equal(
                2,
                result.Count());
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ReturnsAppointment()
        {
            var appointment =
                new AppointmentDto
                {
                    AppointmentId = 1
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(
                            appointment)
                };

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetAppointmentByIdAsync(
                        1);

            Assert.Equal(
                1,
                result.AppointmentId);
        }

        [Fact]
        public async Task CreateAppointmentAsync_ReturnsAppointmentId()
        {
            var appointment =
                new AppointmentDto
                {
                    AppointmentId = 10
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.Created)
                {
                    Content =
                        JsonContent(
                            appointment)
                };

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var result =
                await service
                    .CreateAppointmentAsync(
                        new CreateAppointmentDto());

            Assert.Equal(
                10,
                result);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_CompletesSuccessfully()
        {
            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NoContent);

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var exception =
                await Record.ExceptionAsync(
                    () => service.UpdateAppointmentAsync(
                        1,
                        new UpdateAppointmentDto()));

            Assert.Null(exception);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_CompletesSuccessfully()
        {
            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NoContent);

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var exception =
                await Record.ExceptionAsync(
                    () => service.DeleteAppointmentAsync(
                        1));

            Assert.Null(exception);
        }


        [Fact]
        public async Task GetAppointmentsByPatientAsync_ReturnsAppointments()
        {
            var appointments =
                new List<AppointmentDto>
                {
                    new AppointmentDto
                    {
                        AppointmentId = 1
                    }
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(
                            appointments)
                };

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetAppointmentsByPatientAsync(
                        1);

            Assert.Single(
                result);
        }

        [Fact]
        public async Task GetNextAppointmentByPatientAsync_ReturnsAppointment()
        {
            var appointment =
                new AppointmentDto
                {
                    AppointmentId = 5
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(
                            appointment)
                };

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetNextAppointmentByPatientAsync(
                        1);

            Assert.Equal(
                5,
                result.AppointmentId);
        }

        [Fact]
        public async Task GetAppointmentsByDoctorAsync_ReturnsAppointments()
        {
            var appointments =
                new List<AppointmentDto>
                {
                    new AppointmentDto()
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(
                            appointments)
                };

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetAppointmentsByDoctorAsync(
                        1);

            Assert.Single(
                result);
        }

        [Fact]
        public async Task GetTodayScheduleAsync_ReturnsAppointments()
        {
            var appointments =
                new List<AppointmentDto>
                {
                    new AppointmentDto()
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(
                            appointments)
                };

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetTodayScheduleAsync(
                        1);

            Assert.Single(
                result);
        }

        [Fact]
        public async Task GetWeeklyScheduleAsync_ReturnsAppointments()
        {
            var appointments =
                new List<AppointmentDto>
                {
                    new AppointmentDto()
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        JsonContent(
                            appointments)
                };

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var result =
                await service
                    .GetWeeklyScheduleAsync(
                        1);

            Assert.Single(
                result);
        }

        [Fact]
        public async Task ConfirmAppointmentAsync_CompletesSuccessfully()
        {
            var service =
                new AppointmentApiService(
                    CreateClient(
                        new HttpResponseMessage(
                            HttpStatusCode.NoContent)));

            var exception =
                await Record.ExceptionAsync(
                    () => service.ConfirmAppointmentAsync(
                        1));

            Assert.Null(exception);
        }

        [Fact]
        public async Task CompleteAppointmentAsync_CompletesSuccessfully()
        {
            var service =
                new AppointmentApiService(
                    CreateClient(
                        new HttpResponseMessage(
                            HttpStatusCode.NoContent)));

            var exception =
                await Record.ExceptionAsync(
                    () => service.CompleteAppointmentAsync(
                        1));

            Assert.Null(exception);
        }

        [Fact]
        public async Task CancelAppointmentAsync_CompletesSuccessfully()
        {
            var service =
                new AppointmentApiService(
                    CreateClient(
                        new HttpResponseMessage(
                            HttpStatusCode.NoContent)));

            var exception =
                await Record.ExceptionAsync(
                    () => service.CancelAppointmentAsync(
                        1,
                        "Doctor unavailable"));

            Assert.Null(exception);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_WhenApiReturnsError_ThrowsException()
        {
            var error =
                new ApiErrorResponse
                {
                    Message =
                        "Appointment not found"
                };

            var response =
                new HttpResponseMessage(
                    HttpStatusCode.NotFound)
                {
                    Content =
                        JsonContent(error)
                };

            var service =
                new AppointmentApiService(
                    CreateClient(response));

            var exception =
                await Assert.ThrowsAsync<
                    HttpRequestException>(
                    () =>
                        service.GetAppointmentByIdAsync(
                            999));

            Assert.Equal(
                "Appointment not found",
                exception.Message);
        }
    }
}