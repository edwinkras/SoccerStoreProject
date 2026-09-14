import { Routes } from '@angular/router';
import { Jerseys } from './jerseys/jerseys';
import { Home } from './home/home';

export const routes: Routes = [
  {
    path: '',
    component: Home,
  },
  {
    path: 'jerseys',
    component: Jerseys,
  }

];
