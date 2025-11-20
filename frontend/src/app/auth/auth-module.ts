import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms'; // Critical for forms

// Material Imports
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSnackBarModule } from '@angular/material/snack-bar'; // For error messages
import { MatIconModule } from '@angular/material/icon'; // For icons
import { MatRadioModule } from '@angular/material/radio'; // For radio buttons
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'; // For loading spinner

import { AuthRoutingModule } from './auth-routing-module';
import { Register } from './register/register';
import { Login } from './login/login';
import { ProfileComponent } from './profile/profile';
import { ChangePassword } from './change-password/change-password';


@NgModule({
  declarations: [
    Register,
    Login,
    ProfileComponent,
    ChangePassword
  ],
  imports: [
    CommonModule,
    AuthRoutingModule,
    ReactiveFormsModule, // <--- Don't forget this
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatFormFieldModule,
    MatSnackBarModule,
    MatIconModule,
    MatRadioModule,
    MatProgressSpinnerModule
  ]
})
export class AuthModule { }
