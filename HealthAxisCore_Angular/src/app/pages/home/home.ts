import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  features = [
    {
      title: 'Find trusted doctors',
      description: 'Search specialists, compare experience, and choose the right care for your needs.'
    },
    {
      title: 'Book smarter appointments',
      description: 'Pick available slots, avoid unavailable times, and manage visits from one place.'
    },
    {
      title: 'Secure health records',
      description: 'Patients and doctors access the right health information with role-protected security.'
    }
  ];

  journeySteps = [
    'Patients register and find the right doctor.',
    'Appointments are booked with live availability.',
    'Doctors manage consultations and add health records.',
    'Admins monitor doctors, users, and reports.'
  ];
}
