import { Routes } from '@angular/router';
import { GameSearchPageComponent } from './features/game-search/pages/game-search-page/game-search-page.component';

export const routes: Routes = [
  {
    path: '',
    component: GameSearchPageComponent
  },
  {
    path: '**',
    redirectTo: ''
  }
];
