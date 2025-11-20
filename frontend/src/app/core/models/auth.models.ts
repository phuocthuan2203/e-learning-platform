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

export interface UserProfile {
  userId: number;
  name: string;
  email: string;
  bio?: string;
  role: string;
}

export interface UpdateProfileRequest {
  name: string;
  bio?: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}
