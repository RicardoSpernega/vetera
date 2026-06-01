import { Component, DestroyRef, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { catchError, debounceTime, distinctUntilChanged, filter, of, Subject, switchMap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { GameApiService } from '../../../../core/services/game-api.service';
import { LoadingService } from '../../../../core/services/loading.service';
import { GameDetails } from '../../../../core/models/game.models';
import { LoadingSkeletonComponent } from '../../../../shared/components/loading-skeleton/loading-skeleton.component';
import { GameResultCardComponent } from '../../components/game-result-card/game-result-card.component';

@Component({
  selector: 'app-game-search-page',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatProgressSpinnerModule,
    LoadingSkeletonComponent,
    GameResultCardComponent
  ],
  templateUrl: './game-search-page.component.html',
  styleUrl: './game-search-page.component.scss'
})
export class GameSearchPageComponent {
  private readonly manualSearchTrigger = new Subject<string>();

  readonly searchControl = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required, Validators.minLength(2)]
  });

  readonly games = signal<GameDetails[]>([]);
  readonly errorMessage = signal<string | null>(null);
  readonly hasSearched = signal(false);

  constructor(
    private readonly gameApiService: GameApiService,
    readonly loadingService: LoadingService,
    destroyRef: DestroyRef
  ) {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        filter((value) => value.trim().length >= 2),
        switchMap((value) => this.performSearch$(value)),
        takeUntilDestroyed(destroyRef)
      )
      .subscribe((payload) => this.games.set(payload));

    this.manualSearchTrigger
      .pipe(
        switchMap((value) => this.performSearch$(value)),
        takeUntilDestroyed(destroyRef)
      )
      .subscribe((payload) => this.games.set(payload));
  }

  onSearchClick(): void {
    this.manualSearchTrigger.next(this.searchControl.value);
  }

  private performSearch$(value: string) {
    const query = value.trim();
    this.hasSearched.set(true);
    this.errorMessage.set(null);

    if (query.length < 2) {
      this.games.set([]);
      this.errorMessage.set('Digite pelo menos 2 caracteres para pesquisar.');
      return of([] as GameDetails[]);
    }

    return this.gameApiService.searchGames(query).pipe(
      catchError((error: Error) => {
        this.games.set([]);
        this.errorMessage.set(error.message);
        return of([] as GameDetails[]);
      })
    );
  }
}
