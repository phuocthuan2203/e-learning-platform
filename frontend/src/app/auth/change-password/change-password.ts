import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth';

@Component({
  selector: 'app-change-password',
  standalone: false,
  templateUrl: './change-password.html',
  styleUrl: './change-password.scss',
})
export class ChangePassword {
  passwordForm: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {
    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmNewPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  // Custom validator for matching passwords
  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('newPassword');
    const confirm = control.get('confirmNewPassword');
    return password && confirm && password.value !== confirm.value 
      ? { mismatch: true } 
      : null;
  }

  onSubmit(): void {
    if (this.passwordForm.valid) {
      this.isLoading = true;
      this.authService.changePassword(this.passwordForm.value).subscribe({
        next: () => {
          this.isLoading = false;
          this.snackBar.open('Password changed successfully!', 'Close', { duration: 3000 });
          this.router.navigate(['/auth/profile']);
        },
        error: (err) => {
          this.isLoading = false;
          const msg = err.error?.message || 'Failed to change password';
          this.snackBar.open(msg, 'Close', { duration: 5000 });
        }
      });
    }
  }
}

