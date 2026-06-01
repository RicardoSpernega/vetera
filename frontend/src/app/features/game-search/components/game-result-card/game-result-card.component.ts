import { Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatExpansionModule } from '@angular/material/expansion';
import { GameDetails } from '../../../../core/models/game.models';
import { ImgFallbackDirective } from '../../../../shared/directives/img-fallback.directive';

@Component({
  selector: 'app-game-result-card',
  standalone: true,
  imports: [MatCardModule, MatExpansionModule, ImgFallbackDirective],
  templateUrl: './game-result-card.component.html',
  styleUrl: './game-result-card.component.scss'
})
export class GameResultCardComponent {
  readonly game = input.required<GameDetails>();
}
