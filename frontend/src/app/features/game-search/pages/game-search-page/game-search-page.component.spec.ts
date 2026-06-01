import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { GameSearchPageComponent } from './game-search-page.component';
import { GameApiService } from '../../../../core/services/game-api.service';

describe('GameSearchPageComponent', () => {
  const gameApiServiceMock = {
    searchGames: jasmine.createSpy('searchGames')
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GameSearchPageComponent],
      providers: [{ provide: GameApiService, useValue: gameApiServiceMock }]
    }).compileComponents();
  });

  it('should load game data on valid search', () => {
    gameApiServiceMock.searchGames.and.returnValue(
      of([
        {
          name: 'Elden Ring',
          description: 'Action RPG',
          backgroundImage: 'image',
          achievements: []
        }
      ])
    );

    const fixture = TestBed.createComponent(GameSearchPageComponent);
    const component = fixture.componentInstance;

    component.searchControl.setValue('elden ring');
    component.onSearchClick();

    expect(component.games()[0].name).toBe('Elden Ring');
  });

  it('should expose error message when API fails', () => {
    gameApiServiceMock.searchGames.and.returnValue(throwError(() => new Error('Falha no backend')));

    const fixture = TestBed.createComponent(GameSearchPageComponent);
    const component = fixture.componentInstance;

    component.searchControl.setValue('zelda');
    component.onSearchClick();

    expect(component.errorMessage()).toBe('Falha no backend');
  });
});
