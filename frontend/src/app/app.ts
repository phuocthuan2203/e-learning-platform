import { Component, OnInit, signal } from '@angular/core';
import { AuthService } from './core/services/auth';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.scss'
})
export class AppComponent implements OnInit {
  title = 'elearning-platform-frontend';

  // Inject the service
  constructor(private authService: AuthService) {}

  // Add this lifecycle hook
  ngOnInit(): void {
    this.authService.getUsers().subscribe({
      next: (users) => {
        console.log('Successfully connected to backend and DB!', users);
      },
      error: (err) => {
        console.error('Failed to connect to backend!', err);
      }
    });
  }
}
