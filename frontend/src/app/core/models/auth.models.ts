export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  role: 'Student' | 'Instructor';
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  userId: number;
  role: string;
  name: string;
  expiresAt: string;
}
