import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { GameApiService } from './game-api.service';

describe('GameApiService', () => {
  let service: GameApiService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), GameApiService]
    });

    service = TestBed.inject(GameApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should map successful API response', () => {
    service.searchGames('elden ring').subscribe((result) => {
      expect(result.length).toBe(1);
      expect(result[0].name).toBe('Elden Ring');
      expect(result[0].achievements.length).toBe(1);
    });

    const request = httpMock.expectOne((req) => req.url.includes('/games/search'));
    expect(request.request.params.get('name')).toBe('elden ring');
    expect(request.request.params.get('limit')).toBe('10');

    request.flush({
      success: true,
      data: [
        {
          name: 'Elden Ring',
          description: 'RPG',
          backgroundImage: 'x',
          achievements: [{ title: 'A', description: 'B', image: 'C' }]
        }
      ],
      error: null
    });
  });

  it('should map direct payload with PascalCase properties', () => {
    service.searchGames('elden ring').subscribe((result) => {
      expect(result.length).toBe(1);
      expect(result[0].name).toBe('Elden Ring');
      expect(result[0].backgroundImage).toBe('cover-image');
      expect(result[0].achievements[0].title).toBe('First Step');
    });

    const request = httpMock.expectOne((req) => req.url.includes('/games/search'));
    expect(request.request.params.get('name')).toBe('elden ring');

    request.flush([
      {
        Name: 'Elden Ring',
        Description: 'RPG',
        BackgroundImage: 'cover-image',
        Achievements: [{ Title: 'First Step', Description: 'Done', Image: 'img' }]
      }
    ]);
  });
});
