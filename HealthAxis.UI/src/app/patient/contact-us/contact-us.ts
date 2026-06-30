import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-contact-us',
  imports: [RouterLink],
  templateUrl: './contact-us.html',
  styleUrl: './contact-us.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ContactUs {}