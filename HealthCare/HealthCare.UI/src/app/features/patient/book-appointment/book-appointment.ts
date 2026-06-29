import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { DoctorService }
from '../../../core/services/doctor.service';

import { AppointmentService }
from '../../../core/services/appointment.service';

@Component({
  selector:'app-book-appointment',
  standalone:true,
  imports:[
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl:'./book-appointment.html',
  styleUrl:'./book-appointment.css'
})
export class BookAppointmentComponent {

  form:FormGroup;

  doctors:any[]=[];
  slots:string[]=[];

  specializations=[
    'Cardiology',
    'Dermatology',
    'Neurology',
    'Orthopedics',
    'Pediatrics',
    'General Medicine'
  ];

  constructor(
    private fb:FormBuilder,
    private doctorService:DoctorService,
    private appointmentService:AppointmentService
  ){

    this.form=this.fb.group({

      scheduledDate:['',Validators.required],

      specialization:['',Validators.required],

      doctorId:['',Validators.required],

      timeSlot:['',Validators.required]

    });
  }

  loadDoctors(){

    const spec =
      this.form.value.specialization;

    const date =
      this.form.value.scheduledDate;

    if(!spec || !date)
      return;

    this.doctorService
      .getAvailableDoctors(spec,date)
      .subscribe(res=>{

        this.doctors = res;
      });
  }

  loadSlots(){

    const doctorId =
      this.form.value.doctorId;

    const date =
      this.form.value.scheduledDate;

    if(!doctorId)
      return;

    this.appointmentService
      .getAvailableSlots(
        doctorId,
        date
      )
      .subscribe(res=>{

        this.slots = res;
      });
  }

  bookAppointment(){

    if(this.form.invalid)
      return;

    const appointment = {

      doctorId:
        this.form.value.doctorId,

      scheduledDate:
        this.form.value.scheduledDate,

      timeSlot:
        this.form.value.timeSlot
    };

    this.appointmentService
      .bookAppointment(appointment)
      .subscribe({

        next:()=>{

          alert(
            'Appointment booked successfully'
          );

          this.form.reset();

          this.doctors=[];
          this.slots=[];
        },

        error:()=>{

          alert(
            'Unable to book appointment'
          );
        }

      });

  }

}