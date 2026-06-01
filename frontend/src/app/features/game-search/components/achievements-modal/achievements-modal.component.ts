import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { Achievement } from '../../../../core/models/game.models';
import { ImgFallbackDirective } from '../../../../shared/directives/img-fallback.directive';

export interface AchievementsModalData {
  gameName: string;
  achievements: Achievement[];
}

@Component({
  selector: 'app-achievements-modal',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatCardModule, ImgFallbackDirective],
  templateUrl: './achievements-modal.component.html',
  styleUrl: './achievements-modal.component.scss'
})
export class AchievementsModalComponent {
  readonly data = inject<AchievementsModalData>(MAT_DIALOG_DATA);
}
