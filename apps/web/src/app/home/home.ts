import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  imports: [CommonModule, RouterLink],
  selector: 'app-home',
  styleUrl: './home.scss',
  templateUrl: './home.html',
})
export class Home {
  protected readonly sections = [
    {
      label: 'Cleats',
      path: '/cleats',
      note: 'Boots for grass, turf, and everything between.',
      image: '/cleats.jpg',
    },
    {
      label: 'Jerseys',
      path: '/jerseys',
      note: 'Match and training kits, club and country.',
      image: '/jersey.jpg',
    },
    {
      label: 'Balls',
      path: '/balls',
      note: 'Match balls in every regulation size.',
      image: null,
    },
  ];
}
