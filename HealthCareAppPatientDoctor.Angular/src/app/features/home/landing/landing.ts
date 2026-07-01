import { Component } from '@angular/core';
import {NavbarComponent} from '../../../shared/components/navbar/navbar';
import {Footer} from '../../../shared/components/footer/footer';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-landing',
  imports: [NavbarComponent,Footer,RouterLink],
  templateUrl: './landing.html',
  styleUrl: './landing.css',
})
export class Landing {}
