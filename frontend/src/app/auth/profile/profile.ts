import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../core/services/auth';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.html',
  styleUrl: './profile.scss',
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  isLoading = true;
  role = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) {
    this.profileForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      email: [{ value: '', disabled: true }], // Read-only
      bio: ['', Validators.maxLength(500)]
    });
  }

  ngOnInit(): void {
    this.authService.getProfile().subscribe({
      next: (profile) => {
        this.role = profile.role;
        this.profileForm.patchValue({
          name: profile.name,
          email: profile.email,
          bio: profile.bio
        });
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.snackBar.open('Failed to load profile', 'Close', { duration: 3000 });
      }
    });
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      this.isLoading = true;
      const updateData = {
        name: this.profileForm.get('name')?.value,
        bio: this.profileForm.get('bio')?.value
      };

      this.authService.updateProfile(updateData).subscribe({
        next: (updated) => {
          this.isLoading = false;
          this.snackBar.open('Profile updated!', 'Close', { duration: 3000 });
        },
        error: () => {
          this.isLoading = false;
          this.snackBar.open('Update failed', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
