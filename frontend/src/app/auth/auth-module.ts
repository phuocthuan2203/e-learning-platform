import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing-module';
import { Register } from './register/register';
import { Login } from './login/login';
import { Profile } from './profile/profile';


@NgModule({
  declarations: [
    Register,
    Login,
    Profile
  ],
  imports: [
    CommonModule,
    AuthRoutingModule
  ]
})
export class AuthModule { }
