import axios from 'axios';
import { AuthResponse, PatternOrExercise, CreatePatternRequest } from '../types';

const API_BASE_URL = process.env.REACT_APP_API_URL || '/api';

// Create axios instance
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add auth token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor for error handling
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('authToken');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth API
export const authAPI = {
  login: async (email: string, password: string): Promise<AuthResponse> => {
    const response = await api.post('/auth/login', { email, password });
    return response.data;
  },

  register: async (name: string, email: string, password: string, role: string = 'instructor'): Promise<AuthResponse> => {
    const response = await api.post('/auth/register', { name, email, password, role });
    return response.data;
  },

  getProfile: async () => {
    const response = await api.get('/auth/profile');
    return response.data;
  },
};

// Patterns API
export const patternsAPI = {
  getAll: async (filters?: {
    type?: string;
    level?: string;
    tags?: string[];
    search?: string;
  }): Promise<PatternOrExercise[]> => {
    const params = new URLSearchParams();
    if (filters?.type) params.append('type', filters.type);
    if (filters?.level) params.append('level', filters.level);
    if (filters?.search) params.append('search', filters.search);
    if (filters?.tags) {
      filters.tags.forEach(tag => params.append('tags', tag));
    }

    const response = await api.get(`/patterns?${params.toString()}`);
    return response.data;
  },

  getById: async (id: string): Promise<PatternOrExercise> => {
    const response = await api.get(`/patterns/${id}`);
    return response.data;
  },

  create: async (pattern: CreatePatternRequest): Promise<PatternOrExercise> => {
    const response = await api.post('/patterns', pattern);
    return response.data;
  },

  update: async (id: string, pattern: Partial<CreatePatternRequest>): Promise<PatternOrExercise> => {
    const response = await api.put(`/patterns/${id}`, pattern);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/patterns/${id}`);
  },
};

export default api;