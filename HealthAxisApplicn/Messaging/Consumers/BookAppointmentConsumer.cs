using HealthAxisApplicn.Messaging.Contracts;
using MassTransit;
namespace HealthAxisApplicn.Messaging.Consumers
{
    public class BookAppointmentConsumer
        : IConsumer<BookAppointmentEvent>
    {
        public Task Consume(
            ConsumeContext<BookAppointmentEvent> context)
        {
            Console.WriteLine("==== APPOINTMENT RECEIVED ====");

            Console.WriteLine(
                $"Event Type : {context.Message.EventType}");

            Console.WriteLine(
                $"Occurred At : {context.Message.OccurredAt}");

            Console.WriteLine(
                $"Appointment Id : {context.Message.AppointmentId}");

            Console.WriteLine(
                $"Patient Id : {context.Message.PatientId}");

            Console.WriteLine(
                $"Doctor Id : {context.Message.DoctorId}");

            Console.WriteLine("==============================");

            return Task.CompletedTask;
        }
    }

}
