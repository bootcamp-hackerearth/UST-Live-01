import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Landing } from './landing/landing';
import { Login } from './login/login';
import { Register } from './register/register';

const routes: Routes = [
  { path: '', component: Landing },
  { path: 'login', component: Login },
  { path: 'register', component: Register }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
