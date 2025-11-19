import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { RegisterRequest, LoginRequest, AuthResponse } from '../models/auth.models';
import { UserProfile, UpdateProfileRequest } from '../models/auth.models';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl;
  private readonly TOKEN_KEY = 'jwt_token';
  private readonly USER_KEY = 'user_info';

  constructor(private http: HttpClient) { }

  // 1. Registration
  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, request).pipe(
      tap(response => this.handleAuthSuccess(response))
    );
  }

  // 2. Login
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, request).pipe(
      tap(response => this.handleAuthSuccess(response))
    );
  }

  // 3. Logout
  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
  }

  // 4. Helper to save token and user info
  private handleAuthSuccess(response: AuthResponse): void {
    localStorage.setItem(this.TOKEN_KEY, response.token);
    // Store basic user info for easy access in UI
    localStorage.setItem(this.USER_KEY, JSON.stringify({
      userId: response.userId,
      name: response.name,
      role: response.role
    }));
  }

  // 5. Helper to get token
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  // 6. Check if logged in
  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  // Add this test method
  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/test/users`);
  }

  getProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.apiUrl}/auth/profile`);
  }

  updateProfile(data: UpdateProfileRequest): Observable<UserProfile> {
    return this.http.put<UserProfile>(`${this.apiUrl}/auth/profile`, data);
  }
}
