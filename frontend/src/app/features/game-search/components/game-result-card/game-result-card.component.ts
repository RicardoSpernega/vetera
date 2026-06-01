import { Component, inject, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { GameDetails } from '../../../../core/models/game.models';
import { AchievementsModalComponent } from '../achievements-modal/achievements-modal.component';
import { ImgFallbackDirective } from '../../../../shared/directives/img-fallback.directive';

@Component({
  selector: 'app-game-result-card',
  standalone: true,
  imports: [MatButtonModule, MatCardModule, MatDialogModule, ImgFallbackDirective],
  templateUrl: './game-result-card.component.html',
  styleUrl: './game-result-card.component.scss'
})
export class GameResultCardComponent {
  private readonly dialog = inject(MatDialog);

  readonly game = input.required<GameDetails>();

  openAchievements(): void {
    if (this.game().achievements.length === 0) {
      return;
    }

    this.dialog.open(AchievementsModalComponent, {
      width: 'min(920px, 92vw)',
      maxWidth: '92vw',
      autoFocus: false,
      data: {
        gameName: this.game().name,
        achievements: this.game().achievements
      }
    });
  }
}
