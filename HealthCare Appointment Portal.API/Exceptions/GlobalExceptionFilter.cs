using HealthCare_Appointment_Portal.Exceptions;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace HealthCare_Appointment_Portal.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            HttpStatusCode statusCode;
            string message = exception.Message;

            if (exception is PatientNotFoundException
                || exception is DoctorNotFoundException
                || exception is AppointmentNotFoundException
                || exception is HealthRecordNotFoundException
                || exception is InsuranceNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else if (exception is DuplicatePatientException
                     || exception is DuplicatePolicyNumberException
                     || exception is DuplicateHealthRecordException
                     || exception is AppointmentConflictException)
            {
                statusCode = HttpStatusCode.Conflict;
            }
            else if (exception is InvalidAppointmentStatusException
                     || exception is PastDateException
                     || exception is PastTimeSlotException
                     || exception is AdvanceBookingLimitException
                     || exception is DoctorUnavailableException
                     || exception is DoctorDeletionException
                     || exception is PatientDeletionException
                     || exception is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                statusCode = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred.";
            }

            actionExecutedContext.Response =
                actionExecutedContext.Request.CreateResponse(
                    statusCode,
                    new { Message = message });
        }
    }
}

