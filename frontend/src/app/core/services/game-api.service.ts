import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse, GameDetails } from '../models/game.models';

@Injectable({ providedIn: 'root' })
export class GameApiService {
  private readonly baseUrl = `${environment.apiBaseUrl}/games`;

  constructor(private readonly http: HttpClient) {}

  searchGames(name: string, limit = 10): Observable<GameDetails[]> {
    return this.http
      .get<unknown>(`${this.baseUrl}/search`, {
        params: { name, limit }
      })
      .pipe(
        map((response) => {
          const payload = this.extractPayload(response);

          if (!payload) {
            throw new Error('Failed to load game data.');
          }

          return this.mapGameList(payload);
        })
      );
  }

  private extractPayload(response: unknown): unknown {
    if (!response || typeof response !== 'object') {
      return null;
    }

    const envelope = response as Partial<ApiResponse<unknown>> & {
      Success?: boolean;
      Data?: unknown;
      Error?: string | null;
    };

    const hasEnvelopeShape =
      'success' in envelope ||
      'data' in envelope ||
      'error' in envelope ||
      'Success' in envelope ||
      'Data' in envelope ||
      'Error' in envelope;

    if (!hasEnvelopeShape) {
      return response;
    }

    const success = envelope.success ?? envelope.Success;
    const data = envelope.data ?? envelope.Data;
    const error = envelope.error ?? envelope.Error;

    if (!success || !data) {
      throw new Error(error ?? 'Failed to load game data.');
    }

    return data;
  }

  private mapGameList(payload: unknown): GameDetails[] {
    if (Array.isArray(payload)) {
      return payload.map((item) => this.mapGameDetails(item));
    }

    return [this.mapGameDetails(payload)];
  }

  private mapGameDetails(payload: unknown): GameDetails {
    if (!payload || typeof payload !== 'object') {
      throw new Error('Failed to load game data.');
    }

    const game = payload as {
      name?: string;
      Name?: string;
      description?: string;
      Description?: string;
      backgroundImage?: string;
      BackgroundImage?: string;
      achievements?: unknown[];
      Achievements?: unknown[];
    };

    const achievements = (game.achievements ?? game.Achievements ?? []).map((item) => {
      const achievement = (item ?? {}) as {
        title?: string;
        Title?: string;
        description?: string;
        Description?: string;
        image?: string;
        Image?: string;
      };

      return {
        title: achievement.title ?? achievement.Title ?? '',
        description: achievement.description ?? achievement.Description ?? '',
        image: achievement.image ?? achievement.Image ?? ''
      };
    });

    const name = game.name ?? game.Name;
    const backgroundImage = game.backgroundImage ?? game.BackgroundImage;

    if (!name || !backgroundImage) {
      throw new Error('Failed to load game data.');
    }

    return {
      name,
      description: game.description ?? game.Description ?? '',
      backgroundImage,
      achievements
    };
  }
}
