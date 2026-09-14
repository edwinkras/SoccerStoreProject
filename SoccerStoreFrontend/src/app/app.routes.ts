import { Routes } from '@angular/router';
import { Jerseys } from './jerseys/jerseys';
import { Home } from './home/home';
import { Cleats } from './cleats/cleats';

export const routes: Routes = [
  {
    path: '',
    component: Home,
  },
  {
    path: 'jerseys',
    component: Jerseys,
  },
  {
    path: 'cleats',
    component: Cleats,
  },

];
