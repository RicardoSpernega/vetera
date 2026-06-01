export interface Achievement {
  title: string;
  description: string;
  image: string;
}

export interface GameDetails {
  name: string;
  description: string;
  backgroundImage: string;
  achievements: Achievement[];
}

export interface ApiResponse<T> {
  success: boolean;
  data: T | null;
  error: string | null;
}
