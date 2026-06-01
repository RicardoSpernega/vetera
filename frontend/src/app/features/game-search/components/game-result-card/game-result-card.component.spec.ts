import { TestBed } from '@angular/core/testing';
import { MatDialog } from '@angular/material/dialog';
import { Achievement, GameDetails } from '../../../../core/models/game.models';
import { AchievementsModalComponent } from '../achievements-modal/achievements-modal.component';
import { GameResultCardComponent } from './game-result-card.component';

describe('GameResultCardComponent', () => {
  let openSpy: jasmine.Spy;

  const achievement: Achievement = {
    title: 'Primeira conquista',
    description: 'Descricao da conquista',
    image: 'achievement-image'
  };

  const buildGame = (achievements: Achievement[]): GameDetails => ({
    name: 'Elden Ring',
    description: 'Action RPG',
    backgroundImage: 'game-image',
    achievements
  });

  beforeEach(async () => {
    openSpy = spyOn(MatDialog.prototype, 'open').and.returnValue({} as never);

    await TestBed.configureTestingModule({
      imports: [GameResultCardComponent]
    }).compileComponents();
  });

  it('should disable achievements button when game has no achievements', () => {
    const fixture = TestBed.createComponent(GameResultCardComponent);

    fixture.componentRef.setInput('game', buildGame([]));
    fixture.detectChanges();

    const button = fixture.nativeElement.querySelector('button') as HTMLButtonElement;
    expect(button.disabled).toBeTrue();
  });

  it('should open achievements modal when clicking the button', () => {
    const fixture = TestBed.createComponent(GameResultCardComponent);

    fixture.componentRef.setInput('game', buildGame([achievement]));
    fixture.detectChanges();

    const button = fixture.nativeElement.querySelector('button') as HTMLButtonElement;
    button.click();

    expect(openSpy).toHaveBeenCalledWith(
      AchievementsModalComponent,
      jasmine.objectContaining({
        data: {
          gameName: 'Elden Ring',
          achievements: [achievement]
        }
      })
    );
  });
});
