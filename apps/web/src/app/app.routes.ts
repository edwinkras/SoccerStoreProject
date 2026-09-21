import { Route } from '@angular/router';
import { Home } from './home/home';
import { Cleats } from './cleats/cleats';
import { Jerseys } from './jerseys/jerseys';
import { Balls } from './balls/balls';
import { Checkout } from './checkout/checkout';

export const appRoutes: Route[] = [
  {
    path: '',
    component: Home,
  },
  {
    path: 'cleats',
    component: Cleats,
  },
  {
    path: 'jerseys',
    component: Jerseys,
  },
  {
    path: 'balls',
    component: Balls,
  },
  {
    path: 'checkout',
    component: Checkout
  }
];
