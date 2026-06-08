using HealthCare_Appointment_Portal.Exceptions;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace HealthCare_Appointment_Portal.Filters
{
    public class GlobalExceptionFilter
        : ExceptionFilterAttribute
    {
        public override void OnException(
            HttpActionExecutedContext context)
        {
            HttpStatusCode statusCode =
                HttpStatusCode.BadRequest;

            string message =
                context.Exception.Message;

            if (context.Exception is PatientNotFoundException
                || context.Exception is DoctorNotFoundException
                || context.Exception is AppointmentNotFoundException
                || context.Exception is HealthRecordNotFoundException
                || context.Exception is InsuranceNotFoundException)
            {
                statusCode =
                    HttpStatusCode.NotFound;
            }
            else if (context.Exception is DuplicatePatientException
                     || context.Exception is DuplicatePolicyNumberException
                     || context.Exception is DuplicateHealthRecordException
                     || context.Exception is AppointmentConflictException)
            {
                statusCode =
                    HttpStatusCode.Conflict;
            }
            else if (context.Exception is InvalidAppointmentStatusException
                     || context.Exception is PastDateException
                     || context.Exception is PastTimeSlotException
                     || context.Exception is AdvanceBookingLimitException
                     || context.Exception is DoctorUnavailableException
                     || context.Exception is DoctorDeletionException
                     || context.Exception is PatientDeletionException
                     || context.Exception is ValidationException)
            {
                statusCode =
                    HttpStatusCode.BadRequest;
            }
            else
            {
                statusCode =
                    HttpStatusCode.InternalServerError;

                message =
                    "An unexpected error occurred.";
            }

            context.Response =
                context.Request.CreateResponse(
                    statusCode,
                    new
                    {
                        Message = message
                    });
        }
    }
}